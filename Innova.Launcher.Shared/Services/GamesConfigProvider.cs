// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.GamesConfigProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Extensions;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Services
{
  public class GamesConfigProvider : IGamesConfigProvider, IDisposable
  {
    private readonly IGamesConfigCdnDataProvider _cdnDataProvider;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IGamesConfigParser _configParser;
    private readonly IGameActualVersionProvider _gameActualVersionProvider;
    private readonly ILogger _logger;
    private readonly object _sharedLock = new object();
    private readonly ConcurrentDictionary<string, string> _gameVersions;
    private Task _worker;
    private GamesConfig _sharedConfig;

    public GamesConfigProvider(
      IGamesConfigCdnDataProvider cdnDataProvider,
      IGamesConfigParser configParser,
      IGameActualVersionProvider gameActualVersionProvider,
      ILoggerFactory loggerFactory,
      ILifetimeManager lifetimeManager)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GamesConfigProvider>();
      this._cdnDataProvider = cdnDataProvider;
      this._configParser = configParser;
      this._gameActualVersionProvider = gameActualVersionProvider;
      this._gameVersions = new ConcurrentDictionary<string, string>();
      this._lifetimeManager = lifetimeManager ?? throw new ArgumentNullException(nameof (lifetimeManager));
    }

    public Innova.Launcher.Shared.Models.GameConfig.GameConfig GetGame(string gameKey) => this.GetAll().GetGameConfig(gameKey);

    public GamesConfig RefreshAndGetAll()
    {
      GamesConfig mainConfig = this.GetMainConfig();
      if (mainConfig == null)
        return this.GetMergedConfig();
      lock (this._sharedLock)
        this._sharedConfig = mainConfig;
      return this.GetMergedConfig();
    }

    public Innova.Launcher.Shared.Models.GameConfig.GameConfig RefreshAndGetGame(string gameKey) => this.RefreshAndGetAll().GetGameConfig(gameKey);

    private GamesConfig GetAll()
    {
      if (this._sharedConfig == null)
        throw new GamesConfigNotLoadedException();
      return this.GetMergedConfig();
    }

    private GamesConfig GetMergedConfig()
    {
      if (this._sharedConfig == null)
        return this._sharedConfig;
      lock (this._sharedLock)
      {
        if (this._sharedConfig != null)
        {
          foreach (Innova.Launcher.Shared.Models.GameConfig.GameConfig game in this._sharedConfig.Games)
          {
            string str;
            if (this._gameVersions.TryGetValue(game.Key, out str))
              game.Version = str;
          }
        }
        return this._sharedConfig;
      }
    }

    private GamesConfig GetMainConfig()
    {
      string[] configs = this._cdnDataProvider.GetConfigs();
      GamesConfig mainConfig = this._configParser.Parse(configs[0]);
      if (configs.Length > 1)
      {
        HashSet<string> existingKeys = mainConfig.Games.Select<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string>) (g => g.Key)).ToHashSet<string>();
        GamesConfig gamesConfig = this._configParser.Parse(configs[1]);
        foreach (Innova.Launcher.Shared.Models.GameConfig.GameConfig game in gamesConfig.Games)
          game.IsSingle = true;
        mainConfig.Games.AddRange(gamesConfig.Games.Where<Innova.Launcher.Shared.Models.GameConfig.GameConfig>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, bool>) (gc => !existingKeys.Contains(gc.Key))));
        mainConfig.SinglesIcons = gamesConfig.Icons;
      }
      mainConfig.Games = mainConfig.Games.Where<Innova.Launcher.Shared.Models.GameConfig.GameConfig>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, bool>) (e => !string.IsNullOrWhiteSpace(e.Key))).ToList<Innova.Launcher.Shared.Models.GameConfig.GameConfig>();
      return mainConfig;
    }

    public void RunSyncRemoteGameVersions()
    {
      this.RefreshAndGetAll();
      this._worker = (Task) Task.Factory.StartNew<Task>((Func<object, Task>) (async _ =>
      {
        GamesConfigProvider gamesConfigProvider = this;
        while (gamesConfigProvider._lifetimeManager.IsAlive)
        {
          if (gamesConfigProvider._sharedConfig?.Games != null)
          {
            // ISSUE: reference to a compiler-generated method
            await gamesConfigProvider._sharedConfig.Games.Where<Innova.Launcher.Shared.Models.GameConfig.GameConfig>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, bool>) (e => !string.IsNullOrWhiteSpace(e.VersionCheckUrl))).ParallelAsync<Innova.Launcher.Shared.Models.GameConfig.GameConfig>(4, new Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, Task>(gamesConfigProvider.\u003CRunSyncRemoteGameVersions\u003Eb__16_2));
          }
          await Task.Delay(TimeSpan.FromMinutes(10.0), gamesConfigProvider._lifetimeManager.CancellationToken);
        }
      }), (object) TaskCreationOptions.LongRunning, this._lifetimeManager.CancellationToken);
    }

    public void Dispose()
    {
    }
  }
}
