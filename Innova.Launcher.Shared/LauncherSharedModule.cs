// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.LauncherSharedModule
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Configuration;
using Innova.Launcher.Shared.Infrastructure.Configuration.Interfaces;
using Innova.Launcher.Shared.Infrastructure.Internet;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Infrastructure.IPC;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Services;
using Innova.Launcher.Shared.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher.Shared
{
  public sealed class LauncherSharedModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<ICommandLineArgsProvider, CommandLineArgsProvider>();
      containerRegistry.RegisterSingleton<IComputerNameProvider, ComputerNameProvider>();
      containerRegistry.RegisterSingleton<IExistingLauncherChecker, ExistingLauncherChecker>();
      containerRegistry.RegisterSingleton<IGameActualVersionProvider, GameActualVersionProvider>();
      containerRegistry.RegisterSingleton<IGameInSystemRegistrator, GameInWindowsRegistrator>();
      containerRegistry.RegisterSingleton<IGamesConfigCdnDataProvider, GamesConfigCdnDataProvider>();
      containerRegistry.RegisterSingleton<IGamesConfigProvider, GamesConfigProvider>();
      containerRegistry.RegisterSingleton<IGamesConfigParser, GamesConfigXmlParser>();
      containerRegistry.RegisterSingleton<IHardwareIdProvider, HardwareIdProvider>();
      containerRegistry.RegisterSingleton<IHardwareProvider, HardwareProvider>();
      containerRegistry.RegisterSingleton<ILauncherIdGenerator, LauncherIdGenerator>();
      containerRegistry.RegisterSingleton<ILauncherIdProvider, LauncherIdProvider>();
      containerRegistry.RegisterSingleton<ILauncherInSystemRegistrator, LauncherInWindowsRegistrator>();
      containerRegistry.RegisterSingleton<ILauncherStructureProvider, LauncherStructureProvider>();
      containerRegistry.RegisterSingleton<ILauncherUpdatesRollStrategy, LauncherUpdatesRollStrategy>();
      containerRegistry.RegisterSingleton<ILifetimeManager, LifetimeManager>();
      containerRegistry.RegisterSingleton<IOldForgameGameFinder, OldForgameGameFinder>();
      containerRegistry.RegisterSingleton<IOperatingSystemProvider, OperatingSystemProvider>();
      containerRegistry.RegisterSingleton<IRunningLauncherRepairer, RunningLauncherRepairer>();
      containerRegistry.RegisterSingleton<IStartupParametersParser, StartupParametersParser>();
      containerRegistry.RegisterSingleton<ITimeProvider, TimeProvider>();
      containerRegistry.RegisterSingleton<IIpcServer<PipesIpcConnection>, PipesIpcServer>();
      containerRegistry.RegisterSingleton<IIpcServer<IIpcConnection>, TcpServer>();
      containerRegistry.RegisterSingleton<IInternetConnectionCheckerConfigProvider, DefaultInternetConnectionConfigProvider>();
      containerRegistry.RegisterSingleton<IInternetConnectionChecker, InternetConnectionChecker>();
      containerRegistry.RegisterSingleton<ILauncherStructureConfigurationProvider, LauncherStructureConfiguration>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
