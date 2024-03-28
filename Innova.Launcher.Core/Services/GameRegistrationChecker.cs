// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameRegistrationChecker
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class GameRegistrationChecker
  {
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IGameManager _gameManager;
    private readonly GameStateMachineFactory _gameStateMachineFactory;
    private readonly IGameInSystemRegistrator _gameInSystemRegistrator;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly object _gamesRegistrationLock = new object();
    private Task _worker;

    public GameRegistrationChecker(
      ILifetimeManager lifetimeManager,
      IGameManager gameManager,
      GameStateMachineFactory gameStateMachineFactory,
      IGameInSystemRegistrator gameInSystemRegistrator,
      ILauncherStateService launcherStateService,
      IRegionalGamesService regionalGamesService)
    {
      this._lifetimeManager = lifetimeManager ?? throw new ArgumentNullException(nameof (lifetimeManager));
      this._gameManager = gameManager ?? throw new ArgumentNullException(nameof (gameManager));
      this._gameStateMachineFactory = gameStateMachineFactory ?? throw new ArgumentNullException(nameof (gameStateMachineFactory));
      this._gameInSystemRegistrator = gameInSystemRegistrator ?? throw new ArgumentNullException(nameof (gameInSystemRegistrator));
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
      this._regionalGamesService = regionalGamesService ?? throw new ArgumentNullException(nameof (regionalGamesService));
    }

    public void StartPollVersions() => this._worker = Task.Factory.StartNew((Action) (() =>
    {
      while (this._lifetimeManager.IsAlive)
      {
        lock (this._gamesRegistrationLock)
          this._regionalGamesService.GetGames().Where<Game>((Func<Game, bool>) (g => g.Status == GameStatus.Installed && !this._gameInSystemRegistrator.IsRegistered(this._launcherStateService.LauncherKey, g.EnvKey))).ToList<Game>().ForEach((Action<Game>) (g =>
          {
            this._gameManager.CleanLaunchData(g.Key);
            this._gameStateMachineFactory.Get(g.Key).Uninstall();
          }));
        TaskHelper.Delay(TimeSpan.FromSeconds(5.0), this._lifetimeManager.CancellationToken);
      }
    }), this._lifetimeManager.CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
  }
}
