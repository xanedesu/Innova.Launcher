// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.RuntimeErrorsHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;

namespace Innova.Launcher.Core.Services
{
  public class RuntimeErrorsHandler : IRuntimeErrorsHandler
  {
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly IStatusSender _statusSender;
    private readonly ILogger _logger;

    public RuntimeErrorsHandler(
      ILoggerFactory loggerFactory,
      IRegionalGamesService regionalGamesService,
      IGameStatusExtractor gameStatusExtractor,
      IStatusSender statusSender)
    {
      this._regionalGamesService = regionalGamesService ?? throw new ArgumentNullException(nameof (regionalGamesService));
      this._gameStatusExtractor = gameStatusExtractor ?? throw new ArgumentNullException(nameof (gameStatusExtractor));
      this._statusSender = statusSender ?? throw new ArgumentNullException(nameof (statusSender));
      this._logger = loggerFactory.GetCurrentClassLogger<RuntimeErrorsHandler>();
    }

    public void HandleError(RuntimeErrorInfo errorInfo)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(errorInfo.GameKey);
      if (gameOrDefault == null)
      {
        this._logger.Error("Unable to get game '" + errorInfo.GameKey + "'");
      }
      else
      {
        gameOrDefault.ErrorType = errorInfo.ErrorType;
        gameOrDefault.Error = errorInfo.ErrorCode;
        gameOrDefault.ErrorDescription = errorInfo.ErrorMessage;
        gameOrDefault.ErrorData = errorInfo.ErrorData;
        this._logger.Error("Game got error: " + JsonConvert.SerializeObject((object) gameOrDefault));
        this._regionalGamesService.Save(gameOrDefault);
        this._statusSender.Send(this._gameStatusExtractor.GetStatus(gameOrDefault));
        this._logger.Info("Game " + gameOrDefault.Key + ": new status sent");
      }
    }
  }
}
