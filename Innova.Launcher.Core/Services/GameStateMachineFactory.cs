// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameStateMachineFactory
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services
{
  public class GameStateMachineFactory
  {
    private readonly ILoggerFactory _loggerFactory;
    private readonly IStatusSender _statusSender;
    private readonly IGameManager _gameManager;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly Dictionary<string, GameStateMachine> _stateMachines = new Dictionary<string, GameStateMachine>();

    public GameStateMachineFactory(
      ILoggerFactory loggerFactory,
      IStatusSender statusSender,
      IGameManager gameManager,
      IRegionalGamesService regionalGamesService,
      IGameStatusExtractor gameStatusExtractor)
    {
      this._loggerFactory = loggerFactory;
      this._statusSender = statusSender;
      this._gameManager = gameManager;
      this._regionalGamesService = regionalGamesService;
      this._gameStatusExtractor = gameStatusExtractor;
    }

    public GameStateMachine Get(string gameKey)
    {
      lock (this._stateMachines)
      {
        if (this._stateMachines.ContainsKey(gameKey))
          return this._stateMachines[gameKey];
        GameStateMachine gameStateMachine = new GameStateMachine(gameKey, this._statusSender, this._gameManager, this._loggerFactory.GetCurrentClassLogger<GameStateMachine>(), this._regionalGamesService, this._gameStatusExtractor);
        this._stateMachines[gameKey] = gameStateMachine;
        return gameStateMachine;
      }
    }
  }
}
