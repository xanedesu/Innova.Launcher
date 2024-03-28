// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.RegionalGamesService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services
{
  public class RegionalGamesService : IRegionalGamesService
  {
    private readonly IGameRepositoryFactory _gameRepositoryFactory;
    private readonly ILauncherStateService _launcherStateService;

    public RegionalGamesService(
      ILauncherStateService launcherStateService,
      IGameRepositoryFactory gameRepositoryFactory)
    {
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
      this._gameRepositoryFactory = gameRepositoryFactory ?? throw new ArgumentNullException(nameof (gameRepositoryFactory));
    }

    public List<Game> GetGames() => this._gameRepositoryFactory.Get().GetAll().Where<Game>(new Func<Game, bool>(this.GameFilter)).ToList<Game>();

    private bool GameFilter(Game game)
    {
      string launcherRegion = this._launcherStateService.LauncherRegion;
      if (!(launcherRegion == "br") && !(launcherRegion == "la"))
        return game.Key.EndsWith("-" + launcherRegion);
      return game.Key.EndsWith("-br") || game.Key.EndsWith("-la");
    }

    public Game GetGameOrDefault(string key) => this.GetGames().FirstOrDefault<Game>((Func<Game, bool>) (game => game.Key == key));

    public void Save(Game game) => this._gameRepositoryFactory.Get().Save(game);
  }
}
