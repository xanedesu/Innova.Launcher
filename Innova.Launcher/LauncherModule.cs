// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.LauncherModule
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Handlers;
using Innova.Launcher.Services;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Services.MessageHandlers;
using Innova.Launcher.Shared.Tracking.Interfaces;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher
{
  public class LauncherModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<IWebMessageHandler, CloseExceptMainWebMessageHandler>("CloseExceptMainWebMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, FocusWindowWebMessageHandler>("FocusWindowWebMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, GetMainWindowSizeMessageHandler>("GetMainWindowSizeMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, LocalStorageWebMessageHandler>("LocalStorageWebMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, OpenWindowWebMessageHandler>("OpenWindowWebMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, PushNotificationWebMessageHandler>("PushNotificationWebMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SendWindowEventWebMessageHandler>("SendWindowEventWebMessageHandler");
      containerRegistry.RegisterSingleton<IApplicationUrlLoadService, ApplicationUrlLoadService>();
      containerRegistry.Register<BackendApi>();
      containerRegistry.Register<IBrowserDispatcher, BrowserDispatcher>();
      containerRegistry.Register<IOutputMessageDispatcher, ChromiumOutputMessageDispatcher>();
      containerRegistry.Register<IScriptExecutor, ChromiumScriptExecutor>();
      containerRegistry.Register<IScriptExecutor<ChromiumWebBrowser>, ChromiumScriptExecutor>();
      containerRegistry.Register<IDefaultBrowserService, DefaultBrowserService>();
      containerRegistry.Register<IEnvironmentConfigurationProvider, EnvironmentConfigurationProvider>();
      containerRegistry.RegisterSingleton<IGlobalMouseHook, GlobalMouseHook>();
      containerRegistry.RegisterSingleton<IInternetConnectionStateNotifier, InternetConnectionStateNotifier>();
      containerRegistry.RegisterSingleton<ILauncherVersionProvider, LauncherDeployVersionProvider>();
      containerRegistry.RegisterSingleton<ILauncherLocalizationUpdater, LauncherLocalizationUpdater>();
      containerRegistry.RegisterSingleton<ILauncherTrackingService, LauncherTrackingService>();
      containerRegistry.Register<ILauncherUpdateService, LauncherUpdateService>("network");
      containerRegistry.RegisterSingleton<ILauncherUpdateServiceFactory, LauncherUpdateServiceFactory>();
      containerRegistry.RegisterSingleton<ICookieMigrator, CookieMigrator>();
      containerRegistry.RegisterSingleton<IMainShellFactory, MainShellFactory>();
      containerRegistry.RegisterSingleton<IMessageBus, MessageBusWrapper>();
      containerRegistry.RegisterSingleton<INotificationUrlService, NotificationUrlService>();
      containerRegistry.RegisterSingleton<IOutputMessageDispatcherProvider, OutputMessageDispatcherProvider>();
      containerRegistry.RegisterSingleton<IPushNotificationCoordinator, PushNotificationCoordinator>();
      containerRegistry.RegisterSingleton<IScriptExecutorProvider, ScriptExecutorProvider>();
      containerRegistry.RegisterSingleton<ISwitchGamesEnvironmentService, SwitchGamesEnvironmentService>();
      containerRegistry.RegisterSingleton<ISystemTrayService, SystemTrayService>();
      containerRegistry.RegisterSingleton<IWindowsService, WindowsService>();
      containerRegistry.RegisterSingleton<ITrackingConfiguration, TrackingConfigurationProvider>();
      containerRegistry.RegisterSingleton<IWhiteListService, WhiteListService>();
      containerRegistry.Register<InternalBrowserLifeSpanHandler>();
      containerRegistry.Register<ForgameContextMenuHandler>();
      containerRegistry.Register<ForgameLoadHandler>();
      containerRegistry.Register<ExternalBrowserLifeSpanHandler>();
      containerRegistry.Register<SystemTrayControl>();
      containerRegistry.Register<BrowserViewModel>();
      containerRegistry.RegisterSingleton<PushNotificatorViewModel>();
      containerRegistry.Register<SystemTrayViewModel>();
      containerRegistry.Register<MainViewModel>();
      containerRegistry.RegisterSingleton<MainShell>();
      containerRegistry.RegisterSingleton<InternalBrowser>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
