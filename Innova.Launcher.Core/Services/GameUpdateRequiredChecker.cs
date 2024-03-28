// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameUpdateRequiredChecker
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Exceptions.GamesUpdateChecker;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services
{
  public class GameUpdateRequiredChecker : IGameUpdateRequiredChecker
  {
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IGameManager _gameManager;

    public GameUpdateRequiredChecker(
      ILoggerFactory loggerFactory,
      IGamesConfigProvider gamesConfigProvider,
      IGameManager gameManager)
    {
      this._gamesConfigProvider = gamesConfigProvider;
      this._gameManager = gameManager;
    }

    public ICollection<ServiceStatus> CheckGamesUpdateRequiredStatus(ICollection<Game> games)
    {
      try
      {
        GamesConfig gamesConfig = this._gamesConfigProvider.RefreshAndGetAll();
        return (ICollection<ServiceStatus>) games.Select(game => new
        {
          Game = game,
          Config = gamesConfig.GetGameConfig(game.Key),
          Status = game.Status
        }).Where(v => v.Config != null).Select(v => new
        {
          NewStatus = this._gameManager.CheckGameNeedToUpdate(v.Game, false),
          OldStatus = v.Status
        }).Where(e => e.NewStatus.Status != e.OldStatus).Select(e => e.NewStatus).ToList<ServiceStatus>();
      }
      catch (GamesConfigDataLoadingException ex)
      {
        throw new GamesUpdateCheckException("Games config load problem", (Exception) ex);
      }
    }
  }
}
