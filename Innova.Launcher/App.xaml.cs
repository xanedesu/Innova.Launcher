// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.App
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Properties;
using Innova.Launcher.Services;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared;
using Innova.Launcher.Shared.Localization;
using Innova.Launcher.Shared.Localization.Interfaces;
using Innova.Launcher.Shared.Localization.Services;
using Innova.Launcher.Shared.Logging;
using Innova.Launcher.Shared.Services;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Tracking;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.UI.Themes;
using Innova.Launcher.Updater.Core;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Prism.Unity.Ioc;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Unity;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Innova.Launcher
{
  public partial class App : PrismApplication
  {
    private Mutex _applicationMutex;
    private LauncherTrackingService _errorHandler;
    private bool _contentLoaded;

    private event EventHandler _loaded = (s, e) => { };

    [Dependency]
    public IRunningLauncherRepairer RunningLauncherRepairer { get; set; }

    protected override void OnStartup(StartupEventArgs e)
    {
      this._errorHandler = new LauncherTrackingService();
      if (!this.CanApplicationContinue(e))
      {
        Environment.Exit(1);
      }
      else
      {
        ResourceLocalizationProvider localizationProvider = new ResourceLocalizationProvider();
        localizationProvider.LoadFromApplicationResources();
        LocalizeDictionary.SetDefaultProvider((DependencyObject) null, (ILocalizationProvider) localizationProvider);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.AppDomainUnhandledException);
        this.Dispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(this.HandleDispatcherUnhandledException);
        TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(this.TaskSchedulerUnobservedTaskException);
        this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        this.CheckSettings();
        Thread thread = new Thread(new ParameterizedThreadStart(this.ThreadStartingSplashScreen));
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = false;
        thread.Start();
        this.UpgradeSettings();
        this.ShutdownMode = ShutdownMode.OnMainWindowClose;
        base.OnStartup(e);
        MainShell mainShell = this.Container.Resolve<MainShell>();
        Application.Current.MainWindow = (Window) mainShell;
        this.MainWindow = (Window) mainShell;
        if (this.Container.Resolve<IWindowsService>() is WindowsService windowsService)
          windowsService.RegisterMainWindow((IMainShellPresenter) this.MainWindow);
        EventHandler loaded = this._loaded;
        if (loaded != null)
          loaded((object) this, EventArgs.Empty);
        mainShell.Show();
        this.Container.Resolve<ILauncherLocalizationUpdater>();
        ILauncherApplicationMediator applicationMediator = this.Container.Resolve<ILauncherApplicationMediator>();
        applicationMediator.ApplicationStarted(Environment.CurrentDirectory);
        applicationMediator.MainWindowShowed();
        GC.KeepAlive((object) this.Container.Resolve<PushNotificatorViewModel>());
        this.SetupGlobalMouseHook();
        this.StartInternetConnectionNotifier();
        this.SetLocalization();
        this.MigrateCookies();
      }
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
    }

    private void MigrateCookies()
    {
      try
      {
        this.Container.Resolve<ICookieMigrator>().Import();
      }
      catch (Exception ex)
      {
        this.LogUnhandledException("UpgradeSettings", ex);
      }
    }

    private void SetLocalization()
    {
      try
      {
        ILocalizationService localizationService = this.Container.Resolve<ILocalizationService>();
        ILauncherStateService launcherStateService = this.Container.Resolve<ILauncherStateService>();
        string launcherRegion = launcherStateService.LauncherRegion;
        string culture = launcherStateService.Culture;
        localizationService.UpdateLanguageByRegion(launcherRegion, culture);
      }
      catch (Exception ex)
      {
        this.LogUnhandledException("UpgradeSettings", ex);
      }
    }

    protected override Window CreateShell() => (Window) null;

    private void SetupGlobalMouseHook() => ServiceLocator.Current.GetInstance<IGlobalMouseHook>().Setup();

    private void CheckSettings()
    {
      try
      {
        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
        if (ConfigurationManager.AppSettings.Count != 0)
          return;
        this.ResetSettings();
      }
      catch (ConfigurationErrorsException ex)
      {
        File.Delete(ex.Filename);
        this.ResetSettings();
      }
    }

    private void ResetSettings()
    {
      Settings.Default.Reset();
      Settings.Default.Save();
      Settings.Default.Reload();
    }

    private void UpgradeSettings()
    {
      try
      {
        if (!Settings.Default.UpgradeRequired)
          return;
        Settings.Default.Upgrade();
        Settings.Default.UpgradeRequired = false;
        Settings.Default.Save();
      }
      catch (Exception ex)
      {
        this.LogUnhandledException(nameof (UpgradeSettings), ex);
      }
    }

    private void StartInternetConnectionNotifier() => ServiceLocator.Current.GetInstance<IInternetConnectionStateNotifier>().Start();

    private void ClosingHandler(object sender, CancelEventArgs e)
    {
      Window window = sender as Window;
      if (window == null)
        return;
      window.Closing -= new CancelEventHandler(this.ClosingHandler);
      e.Cancel = true;
      DoubleAnimation animation = new DoubleAnimation(0.0, Duration.op_Implicit(TimeSpan.FromSeconds(0.5)));
      animation.Completed += (EventHandler) ((s, _) =>
      {
        ((object) this).Gui<bool>((Func<bool>) (() => Application.Current.MainWindow.Activate()));
        window.Close();
        window.Dispatcher.InvokeShutdown();
      });
      window.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
    }

    private void ThreadStartingSplashScreen(object viewModel)
    {
      SplashWindow splashWindow = new SplashWindow();
      splashWindow.Closing += new CancelEventHandler(this.ClosingHandler);
      this._loaded += (EventHandler) ((s, e) => splashWindow.Dispatcher.Invoke((Action) (() => splashWindow.Close())));
      splashWindow.Show();
      Dispatcher.Run();
    }

    protected override void OnExit(ExitEventArgs e)
    {
      this._errorHandler.UpdateLastErrorCheckDate();
      ServiceLocator.Current.GetInstance<ILifetimeManager>()?.Die();
      this._applicationMutex.Dispose();
      base.OnExit(e);
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
      moduleCatalog.AddModule<LauncherSharedModule>();
      moduleCatalog.AddModule<LauncherLoggingModule>();
      moduleCatalog.AddModule<LauncherUpdaterCoreModule>();
      moduleCatalog.AddModule<LauncherTrackingModule>();
      moduleCatalog.AddModule<LauncherLocalizationModule>();
      moduleCatalog.AddModule<LauncherCoreModule>();
      moduleCatalog.AddModule<LauncherModule>();
    }

    private void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) => this.LogUnhandledException("AppDomain", (Exception) e.ExceptionObject);

    private void HandleDispatcherUnhandledException(
      object sender,
      DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
      this.LogUnhandledException("Dispatcher", e.Exception);
    }

    private void TaskSchedulerUnobservedTaskException(
      object sender,
      UnobservedTaskExceptionEventArgs e)
    {
      e.SetObserved();
    }

    private void LogUnhandledException(string type, Exception ex) => this.LogException(ex, type + ". Unhandled exception in application");

    private void LogException(Exception exception, string message) => this._errorHandler.LogException(exception, message);

    private bool CanApplicationContinue(StartupEventArgs args)
    {
      bool createdNew;
      this._applicationMutex = new Mutex(true, "1C2E8132-9D83-4948-8494-65768772425A", out createdNew);
      if (!createdNew && this.TryToRepairHangingProcess(args))
      {
        this._applicationMutex = (Mutex) null;
        return false;
      }
      GC.KeepAlive((object) this._applicationMutex);
      return true;
    }

    private bool TryToRepairHangingProcess(StartupEventArgs args)
    {
      try
      {
        IRunningLauncherRepairer launcherRepairer = this.ConfigureServicesContainer().TryResolve<IRunningLauncherRepairer>();
        return launcherRepairer == null || launcherRepairer.TryWakeUp(args.Args);
      }
      catch (Exception ex)
      {
        this.LogException(ex, nameof (TryToRepairHangingProcess));
        return true;
      }
    }

    private IUnityContainer ConfigureServicesContainer()
    {
      IContainerRegistry containerRegistry = (IContainerRegistry) new UnityContainerExtension();
      new LauncherLoggingModule().RegisterTypes(containerRegistry);
      new LauncherCoreModule().RegisterTypes(containerRegistry);
      new LauncherSharedModule().RegisterTypes(containerRegistry);
      return containerRegistry.GetContainer();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/app.xaml", UriKind.Relative));
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
