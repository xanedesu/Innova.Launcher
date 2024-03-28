// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameBrokenInstallProgressChecker
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class GameBrokenInstallProgressChecker
  {
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IGameInstaller _gameInstaller;
    private readonly ILogger _logger;

    public GameBrokenInstallProgressChecker(
      ILoggerFactory loggerFactory,
      IGameInstaller gameInstaller,
      IRegionalGamesService regionalGamesService)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameBrokenInstallProgressChecker>();
      this._gameInstaller = gameInstaller;
      this._regionalGamesService = regionalGamesService;
    }

    public void RestoreBrokenProgresses()
    {
      List<Game> brokenGames = this._regionalGamesService.GetGames().Where<Game>((Func<Game, bool>) (e => e.Status == GameStatus.InstallProgress || e.Status == GameStatus.UpdateProgress || e.Status == GameStatus.RepairProgress)).ToList<Game>();
      Task.Factory.StartNew<Task>((Func<Task>) (async () =>
      {
        foreach (Game brokenGame in brokenGames)
        {
          try
          {
            this._logger.Trace("The game " + brokenGame.Key + " was broken. Restart progress..");
            await this._gameInstaller.ResumeAsync(brokenGame.Key);
          }
          catch (Exception ex)
          {
            this._logger.Error(ex, "Fail to resume progress of game " + brokenGame.Key);
          }
        }
      }), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
    }
  }
}
