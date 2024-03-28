// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Status.StatusMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services.MessageHandlers.Status
{
  [WebMessageHandlerFilter(new string[] {"getStatuses"})]
  public class StatusMessageHandler : IWebMessageHandler
  {
    private readonly IGameUpdateRequiredChecker _gameUpdateRequiredChecker;
    private readonly IGameManager _gameManager;
    private readonly IOutputMessageDispatcher _outputDispatcher;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly IRegionalGamesService _regionalGamesService;

    public StatusMessageHandler(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      IGameManager gameManager,
      IGameUpdateRequiredChecker gameUpdateRequiredChecker,
      IGameStatusExtractor gameStatusExtractor,
      IRegionalGamesService regionalGamesService)
    {
      this._gameUpdateRequiredChecker = gameUpdateRequiredChecker;
      this._gameStatusExtractor = gameStatusExtractor;
      this._regionalGamesService = regionalGamesService;
      this._gameManager = gameManager;
      this._outputDispatcher = outputMessageDispatcherProvider.GetMain();
    }

    public void Handle(WebMessage webMessage)
    {
      StatusMessageHandler.GetStatusRequestData statusRequestData = webMessage.Data.ToObject<StatusMessageHandler.GetStatusRequestData>();
      IEnumerable<ServiceStatus> source;
      if (statusRequestData?.GameKeys == null)
      {
        this._gameUpdateRequiredChecker.CheckGamesUpdateRequiredStatus((ICollection<Game>) this._regionalGamesService.GetGames());
        source = (IEnumerable<ServiceStatus>) this._gameStatusExtractor.GetAllStatuses();
      }
      else
      {
        List<Game> list = statusRequestData.GameKeys.Select<string, Game>((Func<string, Game>) (key => this._regionalGamesService.GetGameOrDefault(key))).Where<Game>((Func<Game, bool>) (e => e != null)).ToList<Game>();
        list.ForEach((Action<Game>) (g => this._gameManager.ClearError(g)));
        this._gameUpdateRequiredChecker.CheckGamesUpdateRequiredStatus((ICollection<Game>) list);
        List<ServiceStatus> serviceStatusList = new List<ServiceStatus>();
        foreach (string gameKey in statusRequestData.GameKeys)
        {
          ServiceStatus status = this._gameStatusExtractor.GetStatus(gameKey);
          serviceStatusList.Add(status);
        }
        source = (IEnumerable<ServiceStatus>) serviceStatusList;
      }
      StatusesLauncherMessageData data = new StatusesLauncherMessageData()
      {
        ServiceStatuses = (IEnumerable<ServiceStatus>) source.ToArray<ServiceStatus>()
      };
      this._outputDispatcher.Dispatch((LauncherMessage) new StatusesLauncherMessage(webMessage.Id, data));
    }

    private class GetStatusRequestData
    {
      [JsonProperty("serviceId")]
      public IEnumerable<string> GameKeys { get; set; }
    }
  }
}
