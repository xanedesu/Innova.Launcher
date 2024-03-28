// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.LauncherUpdateServiceFactory
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Unity;

namespace Innova.Launcher.Services
{
  public class LauncherUpdateServiceFactory : ILauncherUpdateServiceFactory
  {
    private readonly ILauncherStateService _launcherStateService;
    private readonly ILauncherUpdateService _networkUpdateService;
    private readonly ILauncherUpdateService _emptyUpdateService;

    public LauncherUpdateServiceFactory(
      ILauncherStateService launcherStateService,
      [Dependency("network")] ILauncherUpdateService networkUpdateService,
      [Dependency("empty")] ILauncherUpdateService emptyUpdateService)
    {
      this._launcherStateService = launcherStateService;
      this._networkUpdateService = networkUpdateService;
      this._emptyUpdateService = emptyUpdateService;
    }

    public ILauncherUpdateService GetLauncherUpdateService() => this._launcherStateService.IsLocalVersion ? this._emptyUpdateService : this._networkUpdateService;
  }
}
