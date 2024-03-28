// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.LauncherCoreModule
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Infrastructure.Crypto;
using Innova.Launcher.Core.Infrastructure.Crypto.Interfaces;
using Innova.Launcher.Core.Infrastructure.Data;
using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Core.Infrastructure.Installing;
using Innova.Launcher.Core.Infrastructure.Installing.Interfaces;
using Innova.Launcher.Core.Infrastructure.IPC;
using Innova.Launcher.Core.Infrastructure.Mapping;
using Innova.Launcher.Core.Infrastructure.Mapping.Interfaces;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Accounts;
using Innova.Launcher.Core.Services.GameUpdateHandlers;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers;
using Innova.Launcher.Core.Services.MessageHandlers.Accounts;
using Innova.Launcher.Core.Services.MessageHandlers.AppIdentity;
using Innova.Launcher.Core.Services.MessageHandlers.GameEvent;
using Innova.Launcher.Core.Services.MessageHandlers.Install;
using Innova.Launcher.Core.Services.MessageHandlers.InstallDir;
using Innova.Launcher.Core.Services.MessageHandlers.LauncherUpdate;
using Innova.Launcher.Core.Services.MessageHandlers.Play;
using Innova.Launcher.Core.Services.MessageHandlers.Session;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Core.Services.MessageHandlers.Tracking;
using Innova.Launcher.Core.Services.MessageHandlers.UserData;
using Innova.Launcher.Core.Services.MessageHandlers.Version;
using Innova.Launcher.Core.Services.MessageHandlers.Windows;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher.Core
{
  public sealed class LauncherCoreModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<IAccountRepositoryFactory, AccountRepositoryFactory>();
      containerRegistry.RegisterSingleton<IWebMessageHandler, DeleteAccountHandler>("DeleteAccountHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, GetAccountsHandler>("VersionComparer");
      containerRegistry.RegisterSingleton<IWebMessageHandler, NormalizeMainWindowOnAccountLoggedOutHandler>("NormalizeMainWindowOnAccountLoggedOutHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SaveAccountHandler>("SaveAccountHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, GetAppIdentityHandler>("GetAppIdentityHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, LauncherSettingsUpdatedMessageHandler>("LauncherSettingsUpdatedMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SendLauncherLaunchedOnFirstAppIdentityReceivedHandler>("SendLauncherLaunchedOnFirstAppIdentityReceivedHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SetGameEventMessageHandler>("SetGameEventMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, InstallMessageHandler>("InstallMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, InstallDirMessageHandler>("InstallDirMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, LauncherAutoUpdateMessageHandler>("LauncherAutoUpdateMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, LauncherUpdateMessageHandler>("LauncherUpdateMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, PlayMessageHandler>("PlayMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, StatusMessageHandler>("StatusMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, UserDataMessageHandler>("UserDataMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, VersionMessageHandler>("VersionMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SendWindowsInfoOnGetOpenedWindowsReceivedHandler>("SendWindowsInfoOnGetOpenedWindowsReceivedHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, ConfigureMessageHandler>("ConfigureMessageHandler");
      containerRegistry.RegisterSingleton<IWebMessageHandler, SetGameLanguageHandler>("SetGameLanguageHandler");
      containerRegistry.RegisterSingleton<SessionEndedMessageHandler>();
      containerRegistry.RegisterSingleton<TrackingEventMessageHandler>();
      containerRegistry.RegisterSingleton<BrowserInputMessagesQueue>();
      containerRegistry.RegisterSingleton<ILauncherUpdateService, EmptyLauncherUpdateService>("empty");
      containerRegistry.Register<IFrostUpdater, FrostUpdater>();
      containerRegistry.RegisterSingleton<FrostUpdaterFactory>();
      containerRegistry.RegisterSingleton<GameBrokenInstallProgressChecker>();
      containerRegistry.RegisterSingleton<IGameInstaller, GameInstaller>();
      containerRegistry.Register<IGameUpdateHandler, BdoGameUpdateHandler>();
      containerRegistry.Register<IGameManager, GameManager>();
      containerRegistry.Register<GameRegistrationChecker>();
      containerRegistry.RegisterSingleton<IGameRepository, GameRepository>();
      containerRegistry.RegisterSingleton<IGameRepositoryFactory, GameRepositoryFactory>();
      containerRegistry.RegisterSingleton<IGamesEnvironmentProvider, GamesEnvironmentProvider>();
      containerRegistry.RegisterSingleton<GameStateMachine>();
      containerRegistry.RegisterSingleton<GameStateMachineFactory>();
      containerRegistry.RegisterSingleton<IGameStatusExtractor, GameStatusExtractor>();
      containerRegistry.RegisterSingleton<IGameUpdateRequiredChecker, GameUpdateRequiredChecker>();
      containerRegistry.RegisterSingleton<GameVersionChecker>();
      containerRegistry.RegisterSingleton<ILauncherApplicationMediator, LauncherApplicationMediator>();
      containerRegistry.RegisterSingleton<ILauncherConfigurationProvider, LauncherConfigurationProvider>();
      containerRegistry.RegisterSingleton<ILauncherIpcService, LauncherIpcService>();
      containerRegistry.RegisterSingleton<ILauncherManager, LauncherManager>();
      containerRegistry.RegisterSingleton<ILauncherStartupParametersParser, LauncherStartupParametersParser>();
      containerRegistry.RegisterSingleton<ILauncherStateService, LauncherStateService>();
      containerRegistry.RegisterSingleton<ILoggerConfigurationProvider, LoggerConfigurationProvider>();
      containerRegistry.RegisterSingleton<MessageProcessor>();
      containerRegistry.RegisterSingleton<IRegionalGamesService, RegionalGamesService>();
      containerRegistry.RegisterSingleton<IRuntimeErrorsHandler, RuntimeErrorsHandler>();
      containerRegistry.RegisterSingleton<IStatusSender, StatusSender>();
      containerRegistry.RegisterSingleton<IUserDataProvider, UserDataProvider>();
      containerRegistry.RegisterSingleton<IWindowsStorageManager, WindowsStorageManager>();
      containerRegistry.RegisterSingleton<IGameUninstaller, GameUninstaller>();
      containerRegistry.RegisterSingleton<IMapper, AutomapperWrapper>();
      containerRegistry.RegisterSingleton<IPipesIpcServerConfigProvider, PipesIpcServerConfigProvider>();
      containerRegistry.RegisterSingleton<ITcpServerConfigProvider, TcpServerConfig>();
      containerRegistry.RegisterSingleton<ICdnGamesConfigAddressProvider, CdnConfigAddressProvider>();
      containerRegistry.RegisterSingleton<IGameComponentsInstaller, GameComponentsInstaller>();
      containerRegistry.RegisterSingleton<IDatabaseByEnvironmentLocationStrategy, DatabaseByEnvironmentLocationStrategy>();
      containerRegistry.RegisterSingleton<IDatabaseInProgramDataLocationStrategy, DatabaseInProgramDataLocationStrategy>();
      containerRegistry.RegisterSingleton<ICryptoManager, CryptoManager>();
      containerRegistry.RegisterSingleton<IRsaStorage, RsaStorage>();
      containerRegistry.RegisterSingleton<IEnvironmentProvider, EnvironmentProvider>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
