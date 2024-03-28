// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameVersionChecker
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Exceptions.GamesUpdateChecker;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class GameVersionChecker
  {
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IStatusSender _statusSender;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IGameUpdateRequiredChecker _gameUpdateRequiredChecker;
    private readonly ILogger _logger;
    private readonly int _gameVersionPollingInterval;
    private Task _worker;

    public GameVersionChecker(
      ILifetimeManager lifetimeManager,
      ILoggerFactory loggerFactory,
      ILauncherConfigurationProvider configurationProvider,
      IStatusSender statusSender,
      IGameUpdateRequiredChecker gameUpdateRequiredChecker,
      IRegionalGamesService regionalGamesService)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameVersionChecker>();
      this._lifetimeManager = lifetimeManager ?? throw new ArgumentNullException(nameof (lifetimeManager));
      this._statusSender = statusSender ?? throw new ArgumentNullException(nameof (statusSender));
      this._gameUpdateRequiredChecker = gameUpdateRequiredChecker ?? throw new ArgumentNullException(nameof (gameUpdateRequiredChecker));
      this._regionalGamesService = regionalGamesService ?? throw new ArgumentNullException(nameof (regionalGamesService));
      this._gameVersionPollingInterval = configurationProvider.GameVersionPollingInterval;
    }

    public void StartPollVersions() => this._worker = Task.Factory.StartNew((Action) (() =>
    {
      while (this._lifetimeManager.IsAlive)
      {
        try
        {
          ICollection<ServiceStatus> serviceStatuses = this._gameUpdateRequiredChecker.CheckGamesUpdateRequiredStatus((ICollection<Game>) this._regionalGamesService.GetGames());
          if (serviceStatuses.Any<ServiceStatus>())
          {
            this._logger.Trace(string.Format("Send {0} out_of_date status changes..", (object) serviceStatuses.Count));
            this._statusSender.Send((IEnumerable<ServiceStatus>) serviceStatuses);
          }
        }
        catch (GamesUpdateCheckException ex)
        {
          this._logger.Error<GamesUpdateCheckException>(string.Format("Can't check games versions. Retry after {0}", (object) this._gameVersionPollingInterval), ex);
        }
        TaskHelper.Delay(TimeSpan.FromMilliseconds((double) this._gameVersionPollingInterval), this._lifetimeManager.CancellationToken);
      }
    }), this._lifetimeManager.CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
  }
}
