// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameStatusExtractor
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Infrastructure.Mapping.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services
{
  public class GameStatusExtractor : IGameStatusExtractor
  {
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IMapper _mapper;
    private readonly IOperatingSystemProvider _operatingSystemProvider;

    public GameStatusExtractor(
      IGamesConfigProvider gamesConfigProvider,
      IMapper mapper,
      IRegionalGamesService regionalGamesService,
      IOperatingSystemProvider operatingSystemProvider)
    {
      this._gamesConfigProvider = gamesConfigProvider ?? throw new ArgumentNullException(nameof (gamesConfigProvider));
      this._mapper = mapper ?? throw new ArgumentNullException(nameof (mapper));
      this._regionalGamesService = regionalGamesService ?? throw new ArgumentNullException(nameof (regionalGamesService));
      this._operatingSystemProvider = operatingSystemProvider ?? throw new ArgumentNullException(nameof (operatingSystemProvider));
    }

    public ServiceStatus GetStatus(Game game)
    {
      ServiceStatus status = this._mapper.Map<Game, ServiceStatus>(game);
      status.Culture = game.Culture;
      if (game.NeedUpdate())
        status.Status = GameStatus.OutOfDate;
      Innova.Launcher.Shared.Models.GameConfig.GameConfig game1 = this._gamesConfigProvider.GetGame(game.Key);
      if (game1 != null)
      {
        if (game1.OnlyX64)
        {
          int? architecture = this._operatingSystemProvider.Get().Architecture;
          int num = 64;
          if (!(architecture.GetValueOrDefault() == num & architecture.HasValue))
            status.Status = GameStatus.NotAvailableBecauseOsVersion;
        }
        status.AvailableEvents = game1.Events.Select<GameEventConfig, AvailableEvent>((Func<GameEventConfig, AvailableEvent>) (e => new AvailableEvent()
        {
          EventKey = e.EventKey,
          EventName = e.EventName
        })).ToList<AvailableEvent>();
        ServiceStatus serviceStatus = status;
        GameLanguage[] languages = game1.Languages;
        string[] array = languages != null ? ((IEnumerable<GameLanguage>) languages).Select<GameLanguage, string>((Func<GameLanguage, string>) (p => p.Culture)).ToArray<string>() : (string[]) null;
        serviceStatus.Languages = array;
      }
      return status;
    }

    public ServiceStatus GetStatus(string gameKey)
    {
      ServiceStatus status = new ServiceStatus()
      {
        GameKey = gameKey,
        Status = GameStatus.NotAvailable,
        Info = (object) "info"
      };
      Innova.Launcher.Shared.Models.GameConfig.GameConfig game = this._gamesConfigProvider.GetGame(gameKey);
      if (game != null)
      {
        ServiceStatus serviceStatus = status;
        GameLanguage[] languages = game.Languages;
        string[] array = languages != null ? ((IEnumerable<GameLanguage>) languages).Select<GameLanguage, string>((Func<GameLanguage, string>) (p => p.Culture)).ToArray<string>() : (string[]) null;
        serviceStatus.Languages = array;
        if (game.OnlyX64)
        {
          int? architecture = this._operatingSystemProvider.Get().Architecture;
          int num = 64;
          if (!(architecture.GetValueOrDefault() == num & architecture.HasValue))
          {
            status.Status = GameStatus.NotAvailableBecauseOsVersion;
            return status;
          }
        }
        Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
        if (gameOrDefault != null)
        {
          if (gameOrDefault.Status == GameStatus.NotAvailable)
          {
            gameOrDefault.Status = GameStatus.NotInstalled;
            this._regionalGamesService.Save(gameOrDefault);
          }
          status = this.GetStatus(gameOrDefault);
        }
        else
          status.Status = GameStatus.NotInstalled;
      }
      return status;
    }

    public List<ServiceStatus> GetAllStatuses() => this._regionalGamesService.GetGames().Select<Game, ServiceStatus>(new Func<Game, ServiceStatus>(this.GetStatus)).ToList<ServiceStatus>();
  }
}
