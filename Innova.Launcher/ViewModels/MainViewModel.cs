// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.MainViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using CefSharp.Event;
using CefSharp.WinForms;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Events;
using Innova.Launcher.Handlers;
using Innova.Launcher.Properties;
using Innova.Launcher.Services;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Services.MessageHandlers;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.UI.Behaviors;
using Innova.Launcher.UI.Extensions;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Innova.Launcher.ViewModels
{
  public class MainViewModel : ReactiveObject
  {
    private readonly IScriptExecutorProvider _scriptExecutorProvider;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private readonly ILauncherTrackingService _trackingService;
    private readonly ForgameLoadHandler _forgameLoadHandler;
    private readonly ForgameContextMenuHandler _forgameContextMenuHandler;
    private readonly ExternalBrowserLifeSpanHandler _singleBrowserAwareLifeSpanHandler;
    private readonly ISwitchGamesEnvironmentService _switchGamesEnvironmentService;
    private WindowState _previousWindowState;
    private string _lastRequestedUrl;
    private ChromiumWebBrowser _browser;
    private readonly ObservableAsPropertyHelper<bool> _hasProblem;
    private bool _internetConnectionExists;
    private bool _refreshProcessing;
    private readonly ObservableAsPropertyHelper<bool> _showSomethingGoWrong;
    private string _lastLoadError;
    private bool _saveWindowPosition;
    private bool _servicePaused;
    private bool _windowActive;
    private readonly ObservableAsPropertyHelper<bool> _showWindowTitle;

    public string Id { get; set; } = "main";

    public IWindowsService WindowsService { get; }

    public BackendApi BackendApi { get; }

    public bool HasProblem => this._hasProblem.Value;

    public bool InternetConnectionExists
    {
      get => this._internetConnectionExists;
      set => this.RaiseAndSetIfChanged<MainViewModel, bool>(ref this._internetConnectionExists, value, nameof (InternetConnectionExists));
    }

    public bool RefreshProcessing
    {
      get => this._refreshProcessing;
      set => this.RaiseAndSetIfChanged<MainViewModel, bool>(ref this._refreshProcessing, value, nameof (RefreshProcessing));
    }

    public bool ShowSomethingGoWrong => this._showSomethingGoWrong.Value;

    public string LastLoadError
    {
      get => this._lastLoadError;
      set => this.RaiseAndSetIfChanged<MainViewModel, string>(ref this._lastLoadError, value, nameof (LastLoadError));
    }

    public bool SaveWindowPosition
    {
      get => this._saveWindowPosition;
      set => this.RaiseAndSetIfChanged<MainViewModel, bool>(ref this._saveWindowPosition, value, nameof (SaveWindowPosition));
    }

    public bool ServicePaused
    {
      get => this._servicePaused;
      set => this.RaiseAndSetIfChanged<MainViewModel, bool>(ref this._servicePaused, value, nameof (ServicePaused));
    }

    public bool WindowActive
    {
      get => this._windowActive;
      set => this.RaiseAndSetIfChanged<MainViewModel, bool>(ref this._windowActive, value, nameof (WindowActive));
    }

    public bool ShowWindowTitle => this._showWindowTitle.Value;

    public ReactiveCommand<Unit, Unit> RefreshErrorLoadedPageCommand { get; }

    public ReactiveCommand<SenderEventArgsCommandParameter, Unit> HandleWindowStateChangeCommand { get; }

    public MainViewModel(
      IWindowsService windowsService,
      IScriptExecutorProvider scriptExecutorProvider,
      ILauncherStateService launcherStateService,
      IInternetConnectionChecker internetConnectionChecker,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      ISwitchGamesEnvironmentService switchGamesEnvironmentService,
      ILauncherTrackingService trackingService,
      BackendApi backendApi,
      ForgameLoadHandler forgameLoadHandler,
      ForgameContextMenuHandler forgameContextMenuHandler,
      ExternalBrowserLifeSpanHandler singleBrowserAwareLifeSpanHandler)
    {
      this._forgameLoadHandler = forgameLoadHandler ?? throw new ArgumentNullException(nameof (forgameLoadHandler));
      this._forgameContextMenuHandler = forgameContextMenuHandler ?? throw new ArgumentNullException(nameof (forgameContextMenuHandler));
      this._singleBrowserAwareLifeSpanHandler = singleBrowserAwareLifeSpanHandler ?? throw new ArgumentNullException(nameof (singleBrowserAwareLifeSpanHandler));
      this._internetConnectionChecker = internetConnectionChecker ?? throw new ArgumentNullException(nameof (internetConnectionChecker));
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider ?? throw new ArgumentNullException(nameof (outputMessageDispatcherProvider));
      this._trackingService = trackingService ?? throw new ArgumentNullException(nameof (trackingService));
      this._scriptExecutorProvider = scriptExecutorProvider ?? throw new ArgumentNullException(nameof (scriptExecutorProvider));
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
      this._switchGamesEnvironmentService = switchGamesEnvironmentService ?? throw new ArgumentNullException(nameof (switchGamesEnvironmentService));
      this.WindowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));
      this.BackendApi = backendApi ?? throw new ArgumentNullException(nameof (backendApi));
      this.InternetConnectionExists = true;
      this.SaveWindowPosition = true;
      this.WindowActive = true;
      this.HandleWindowStateChangeCommand = ReactiveCommand.Create<SenderEventArgsCommandParameter>((Action<SenderEventArgsCommandParameter>) (v =>
      {
        if (!(v.Sender is Window sender2))
          return;
        if (this.Id == "main")
        {
          if (sender2.WindowState == WindowState.Minimized)
            this._trackingService.SendMainWindowMinimized();
          else if (this._previousWindowState == WindowState.Minimized)
            this._trackingService.SendMainWindowRestored();
        }
        this._outputMessageDispatcherProvider.Get(this.Id).Dispatch(new LauncherMessage("windowStateChanged", (object) new WindowStateChangedData(this.Id, sender2.WindowState)));
        this._previousWindowState = sender2.WindowState;
      }));
      this.WhenAnyValue<MainViewModel, bool, string, bool>((Expression<Func<MainViewModel, string>>) (v => v.LastLoadError), (Expression<Func<MainViewModel, bool>>) (v => v.RefreshProcessing), (Func<string, bool, bool>) ((le, rp) => !string.IsNullOrWhiteSpace(le) | rp)).ToProperty<MainViewModel, bool>(this, (Expression<Func<MainViewModel, bool>>) (v => v.ShowSomethingGoWrong), out this._showSomethingGoWrong);
      this.WhenAnyValue<MainViewModel, bool, bool, bool>((Expression<Func<MainViewModel, bool>>) (v => v.InternetConnectionExists), (Expression<Func<MainViewModel, bool>>) (v => v.ShowSomethingGoWrong), (Func<bool, bool, bool>) ((ie, he) => !ie | he)).ToProperty<MainViewModel, bool>(this, (Expression<Func<MainViewModel, bool>>) (v => v.HasProblem), out this._hasProblem);
      this.WhenAny<MainViewModel, bool, bool>((Expression<Func<MainViewModel, bool>>) (v => v.ShowSomethingGoWrong), (Func<IObservedChange<MainViewModel, bool>, bool>) (v => v.Value)).Where<bool>((Func<bool, bool>) (v => v)).Subscribe<bool>((Action<bool>) (v => this.SendTrackingError("SomethingWentWrong", this.LastLoadError)));
      this.WhenAny<MainViewModel, bool, bool>((Expression<Func<MainViewModel, bool>>) (v => v.InternetConnectionExists), (Func<IObservedChange<MainViewModel, bool>, bool>) (v => v.Value)).Where<bool>((Func<bool, bool>) (v => !v)).Subscribe<bool>((Action<bool>) (v => this.SendTrackingError("InternetConnectionLost")));
      this.WhenAnyValue<MainViewModel, bool, bool, bool>((Expression<Func<MainViewModel, bool>>) (v => v.HasProblem), (Expression<Func<MainViewModel, bool>>) (v => v.ServicePaused), (Func<bool, bool, bool>) ((hp, sp) => hp | sp)).ToProperty<MainViewModel, bool>(this, (Expression<Func<MainViewModel, bool>>) (v => v.ShowWindowTitle), out this._showWindowTitle);
      this.WhenAnyValue<MainViewModel, bool>((Expression<Func<MainViewModel, bool>>) (v => v.WindowActive)).Subscribe<bool>(new Action<bool>(this.WindowActivatedChanged));
      MessageBus.Current.Listen<NavigateMainWindowEvent>().Where<NavigateMainWindowEvent>((Func<NavigateMainWindowEvent, bool>) (_ => this.Id == "main")).Subscribe<NavigateMainWindowEvent>((Action<NavigateMainWindowEvent>) (v => this.Gui((Action) (() =>
      {
        try
        {
          if (Uri.TryCreate(v.Url, UriKind.Absolute, out Uri _))
            this._browser.Load(v.Url);
          else
            this._outputMessageDispatcherProvider.Get(this.Id).Dispatch(new LauncherMessage("redirect", (object) new
            {
              path = v.Url
            })
            {
              Async = false
            });
        }
        catch (Exception ex)
        {
        }
      }))));
      this.RefreshErrorLoadedPageCommand = ReactiveCommand.CreateFromTask((Func<Task>) (() => Task.Run(new Action(this.RefreshErrorLoadedPage))));
    }

    public void Hide()
    {
    }

    public void Reload() => this._browser.Reload();

    private void RefreshErrorLoadedPage()
    {
      this.RefreshProcessing = true;
      this.LastLoadError = string.Empty;
      this.ServicePaused = false;
      // ISSUE: method pointer
      this._browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((object) this, __methodptr(\u003CRefreshErrorLoadedPage\u003Eg__PageLoaded\u007C64_0));
      this._browser.Load(this._lastRequestedUrl);
    }

    public void Init(IMainShellPresenter shell, ChromiumWebBrowser browser)
    {
      this._browser = browser;
      ChromiumWebBrowser browser1 = this._browser;
      if (browser1.BrowserSettings == null)
      {
        IBrowserSettings browserSettings;
        browser1.BrowserSettings = browserSettings = (IBrowserSettings) new BrowserSettings();
      }
      this._browser.BrowserSettings.WindowlessFrameRate = 60;
      this._browser.BrowserSettings.Javascript = CefState.Enabled;
      this._browser.BrowserSettings.JavascriptAccessClipboard = CefState.Enabled;
      this._browser.BrowserSettings.JavascriptDomPaste = CefState.Enabled;
      this._browser.BrowserSettings.WebGl = CefState.Enabled;
      this._browser.BrowserSettings.BackgroundColor = Cef.ColorSetARGB((uint) byte.MaxValue, 18U, 18U, 28U);
      bool isShellClosing = false;
      shell.Closing += (CancelEventHandler) ((sender, args) =>
      {
        if (args.Cancel)
          return;
        if (this.Id == "main")
        {
          this._trackingService.SendLauncherClosed();
          this._outputMessageDispatcherProvider.Get(this.Id).Dispatch(new LauncherMessage("closed", (object) new
          {
            WindowId = this.Id
          })
          {
            Async = false
          });
        }
        try
        {
          isShellClosing = true;
          if (browser.IsDisposed)
            return;
          ((Component) browser).Dispose();
        }
        catch
        {
        }
      });
      ((Component) browser).Disposed += (EventHandler) ((_param1, _param2) =>
      {
        try
        {
          if (isShellClosing)
            return;
          shell.HideAndClose();
        }
        catch
        {
        }
      });
      this.BackendApi.InitShell(shell);
      this._scriptExecutorProvider.Add(this.Id, browser);
      browser.RequestHandler = (IRequestHandler) this._forgameLoadHandler;
      browser.MenuHandler = (IContextMenuHandler) this._forgameContextMenuHandler;
      browser.KeyboardHandler = (IKeyboardHandler) new ForgameKeyboardHandler(this.WindowsService, this._switchGamesEnvironmentService, this.Id, this._outputMessageDispatcherProvider);
      browser.LifeSpanHandler = (ILifeSpanHandler) this._singleBrowserAwareLifeSpanHandler;
      browser.LoadError += new EventHandler<LoadErrorEventArgs>(this.BrowserFailLoadingFrameEvent);
      browser.JavascriptObjectRepository.ResolveObject += (EventHandler<JavascriptBindingEventArgs>) ((_, ea) =>
      {
        if (!ea.ObjectName.Equals("LAUNCHER", StringComparison.OrdinalIgnoreCase))
          return;
        browser.JavascriptObjectRepository.Register(ea.ObjectName, (object) this.BackendApi, true, BindingOptions.DefaultBinder);
      });
      browser.FrameLoadStart += (EventHandler<FrameLoadStartEventArgs>) ((_, e) =>
      {
        if (!e.Frame.IsMain)
          return;
        this.ServicePaused = false;
      });
      browser.FrameLoadEnd += (EventHandler<FrameLoadEndEventArgs>) ((_, e) =>
      {
        if (!e.Frame.IsMain)
          return;
        if (e.HttpStatusCode >= 400 || e.HttpStatusCode < 200)
        {
          if (!this.InternetConnectionExists || !(e.Url == this._launcherStateService.CurrentStartPage))
            return;
          this.LastLoadError = string.Format("Url: {0}, Error: {1}", (object) e.Url, (object) e.HttpStatusCode);
        }
        else
          browser.SetZoomLevel(Settings.Default.ZoomLevel);
      });
      this._internetConnectionChecker.CheckAsync().ContinueWith((Action<Task<bool>>) (connectionExistsTask =>
      {
        if (connectionExistsTask.Result)
          return;
        this._internetConnectionChecker.WaitConnectionAsync().ContinueWith((Action<Task>) (t => this.GuiNoWait(new Action(this.InternetConnectionRestored))));
        this.InternetConnectionExists = false;
      }), TaskContinuationOptions.OnlyOnRanToCompletion);
      this._previousWindowState = shell.WindowState;
    }

    private void BrowserFailLoadingFrameEvent(object sender, LoadErrorEventArgs e)
    {
      if (!this.InternetConnectionExists || !e.Frame.IsMain || !(e.FailedUrl == this._launcherStateService.CurrentStartPage))
        return;
      if (e.ErrorCode == CefErrorCode.Aborted)
        this.ServicePaused = true;
      else
        this.LastLoadError = string.Format("Url: {0}, Error: {1:G}", (object) e.FailedUrl, (object) e.ErrorCode);
    }

    private void InternetConnectionRestored()
    {
      this._browser.Reload();
      // ISSUE: method pointer
      this._browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>((object) this, __methodptr(\u003CInternetConnectionRestored\u003Eg__PageLoaded\u007C67_0));
    }

    public void OpenUrl(string url)
    {
      this._lastRequestedUrl = url;
      if (!(url != this._browser.Address))
        return;
      this._browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(PageLoaded);
      this._browser.Load(url);

      static bool IsAuthUrl(string uri) => new Uri(uri).AbsolutePath.EndsWith("auth");

      void PageLoaded(object s, FrameLoadEndEventArgs args)
      {
        if (!args.Frame.IsMain)
          return;
        if (new Uri(this._launcherStateService.CurrentStartPage).Host == new Uri(url).Host)
          this.WindowsService.RestoreWindowLocalStorage(this.Id, true);
        this._browser.FrameLoadEnd -= new EventHandler<FrameLoadEndEventArgs>(PageLoaded);
        if (!this._lastRequestedUrl.Equals(this._launcherStateService.CurrentStartPage) || this._lastRequestedUrl.Equals(args.Url) || !IsAuthUrl(args.Url))
          return;
        this._browser.AddressChanged += new EventHandler<AddressChangedEventArgs>(RedirectBackToStartPage);
      }

      void RedirectBackToStartPage(object sender, AddressChangedEventArgs args)
      {
        this._browser.AddressChanged -= new EventHandler<AddressChangedEventArgs>(RedirectBackToStartPage);
        if (args.Address.Equals(this._lastRequestedUrl))
          return;
        this._browser.Load(this._lastRequestedUrl);
      }
    }

    private void SendTrackingError(string error, string description = "")
    {
      DateTime time = DateTime.UtcNow;
      this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() => this._trackingService.SendTrackingError(new LauncherError()
      {
        Error = error,
        Description = description,
        Time = time
      })));
    }

    public WindowStorage GetWindowStorage() => this._browser != null && !this._browser.IsDisposed ? new WindowStorage() : (WindowStorage) null;

    public void RestoreWindowStorage(WindowStorage storage)
    {
    }

    private void WindowActivatedChanged(bool windowActive)
    {
      if (!this._launcherStateService.IsAppIdentityReceived)
        return;
      string name = windowActive ? "windowActivated" : "windowDeactivated";
      this._outputMessageDispatcherProvider.GetMain().Dispatch(new LauncherMessage(name));
    }
  }
}
