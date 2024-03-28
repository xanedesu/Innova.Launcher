// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherApplicationMediator
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Helpers;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Localization.Helpers;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class LauncherApplicationMediator : ILauncherApplicationMediator
  {
    private readonly IGameInstaller _gameInstaller;
    private readonly ILauncherStateService _launcherStateService;
    private readonly GameBrokenInstallProgressChecker _brokenInstallProgressChecker;
    private readonly GameVersionChecker _gameVersionChecker;
    private readonly GameRegistrationChecker _gameRegistrationChecker;
    private readonly ILauncherStructureProvider _launcherStructureProvider;
    private readonly ILauncherManager _launcherManager;
    private readonly ILauncherConfigurationProvider _launcherConfigurationProvider;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly ISystemTrayService _systemTrayService;
    private readonly IGameManager _gameManager;
    private readonly ILauncherIpcService _launcherIpcService;
    private readonly ILauncherTrackingService _launcherTrackingService;
    private readonly IWindowsService _windowsService;
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;
    private readonly ILogger _logger;

    public string StartupUrl { get; private set; }

    public LauncherApplicationMediator(
      ILoggerFactory loggerFactory,
      IGameInstaller gameInstaller,
      ILauncherStateService launcherStateService,
      MessageProcessor messageProcessor,
      GameVersionChecker gameVersionChecker,
      GameRegistrationChecker gameRegistrationChecker,
      GameBrokenInstallProgressChecker brokenInstallProgressChecker,
      ILauncherStructureProvider launcherStructureProvider,
      ILauncherManager launcherManager,
      ILauncherConfigurationProvider launcherConfigurationProvider,
      IGamesConfigProvider gamesConfigProvider,
      IInternetConnectionChecker internetConnectionChecker,
      ISystemTrayService systemTrayService,
      IGameManager gameManager,
      ILauncherIpcService launcherIpcService,
      ILauncherTrackingService launcherTrackingService,
      IWindowsService windowsService,
      IGamesEnvironmentProvider gamesEnvironmentProvider)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherApplicationMediator>();
      this._gameInstaller = gameInstaller ?? throw new ArgumentNullException(nameof (gameInstaller));
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
      this._brokenInstallProgressChecker = brokenInstallProgressChecker ?? throw new ArgumentNullException(nameof (brokenInstallProgressChecker));
      this._gameVersionChecker = gameVersionChecker ?? throw new ArgumentNullException(nameof (gameVersionChecker));
      this._gameRegistrationChecker = gameRegistrationChecker ?? throw new ArgumentNullException(nameof (gameRegistrationChecker));
      this._launcherStructureProvider = launcherStructureProvider ?? throw new ArgumentNullException(nameof (launcherStructureProvider));
      this._launcherManager = launcherManager ?? throw new ArgumentNullException(nameof (launcherManager));
      this._launcherConfigurationProvider = launcherConfigurationProvider ?? throw new ArgumentNullException(nameof (launcherConfigurationProvider));
      this._gamesConfigProvider = gamesConfigProvider ?? throw new ArgumentNullException(nameof (gamesConfigProvider));
      this._internetConnectionChecker = internetConnectionChecker ?? throw new ArgumentNullException(nameof (internetConnectionChecker));
      this._systemTrayService = systemTrayService ?? throw new ArgumentNullException(nameof (systemTrayService));
      this._gameManager = gameManager ?? throw new ArgumentNullException(nameof (gameManager));
      this._launcherIpcService = launcherIpcService ?? throw new ArgumentNullException(nameof (launcherIpcService));
      this._launcherTrackingService = launcherTrackingService ?? throw new ArgumentNullException(nameof (launcherTrackingService));
      this._windowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));
      this._gamesEnvironmentProvider = gamesEnvironmentProvider ?? throw new ArgumentNullException(nameof (gamesEnvironmentProvider));
    }

    public void ApplicationStarted(string startupPath)
    {
      this._logger.Trace("Application started");
      this._launcherIpcService.StartListen();
      this._systemTrayService.CreateTrayIcon();
      this.RunConnectionSensitiveServicesAsync();
      this._logger.Trace("RunConnectionSensitiveServicesAsync");
      try
      {
        this._launcherManager.StartPollLauncherUpdates();
        this._logger.Trace("StartPollLauncherUpdates");
        this._launcherStructureProvider.BaseDirectory = Path.GetDirectoryName(startupPath);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "ApplicationStarted exception");
      }
      this._launcherTrackingService.SendHardware();
      this.StartupUrl = this._launcherStateService.CurrentStartPage;
      this._launcherStateService.RegionUpdated += new EventHandler<RegionUpdatedEventArgs>(this.LauncherRegionUpdated);
      this._gamesEnvironmentProvider.GamesEnvironmentChanged += new EventHandler(this.GamesEnvironmentChanged);
      if (!string.IsNullOrWhiteSpace(this._launcherConfigurationProvider.StartupGameKey))
      {
        string startupGameKey = this._launcherConfigurationProvider.StartupGameKey;
        this._logger.Trace("Start with game " + startupGameKey);
        try
        {
          Innova.Launcher.Shared.Models.GameConfig.GameConfig game = this._gamesConfigProvider.RefreshAndGetGame(startupGameKey);
          if (game == null)
          {
            this._logger.Error("Bad gameKey parameter value " + startupGameKey);
          }
          else
          {
            this.StartupUrl = this._gameManager.GetGameWindowUrl(startupGameKey, false);
            GameLanguage[] languages = game.Languages;
            string culture = languages != null ? ((IEnumerable<GameLanguage>) languages).Where<GameLanguage>((Func<GameLanguage, bool>) (p => p.Culture == LocalizationHelper.CurrentCulture.Name)).Select<GameLanguage, string>((Func<GameLanguage, string>) (p => p.Culture)).FirstOrDefault<string>() : (string) null;
            if (this.ShouldInstallGame())
              Task.Run<bool>((Func<bool>) (() => this._gameInstaller.InstallAsync(this._launcherConfigurationProvider.StartupGameKey, this._launcherConfigurationProvider.StartupGameInstallPath, string.Empty, culture).Wait(TimeSpan.FromHours(5.0))));
          }
        }
        catch (GamesConfigDataLoadingException ex)
        {
          this._logger.Warn<GamesConfigDataLoadingException>(ex);
        }
        catch (GamesConfigNotLoadedException ex)
        {
          this._logger.Warn<GamesConfigNotLoadedException>(ex);
        }
      }
      if (!string.IsNullOrWhiteSpace(this._launcherConfigurationProvider.RelativePath))
      {
        try
        {
          this.StartupUrl = UrlHelper.ConcatPathWithParams(this._launcherStateService.CurrentStartPage, this._launcherConfigurationProvider.RelativePath);
        }
        catch (Exception ex)
        {
          this._logger.Log<Exception>(LogLevel.Error, ex);
        }
      }
      this._logger.Trace("ApplicationStarted end");
    }

    public void MainWindowShowed() => this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() =>
    {
      this._windowsService.OpenUrlInMainWindow(this.StartupUrl);
      this._gameRegistrationChecker.StartPollVersions();
      this._gamesConfigProvider.RunSyncRemoteGameVersions();
      this._gameVersionChecker.StartPollVersions();
    }));

    private void RunConnectionSensitiveServicesAsync() => this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() =>
    {
      this._gamesConfigProvider.RefreshAndGetAll();
      this._brokenInstallProgressChecker.RestoreBrokenProgresses();
      this._systemTrayService.RefreshGames();
      this._launcherTrackingService.SendUntrackedErrors();
    }));

    private bool ShouldInstallGame() => !string.IsNullOrWhiteSpace(this._launcherConfigurationProvider.StartupGameInstallPath);

    private void LauncherRegionUpdated(object sender, RegionUpdatedEventArgs args)
    {
      this.StartupUrl = this._launcherStateService.CurrentStartPage;
      this._windowsService.CloseExceptMain();
      this._windowsService.OpenUrlInMainWindow(args.UrlAfterUpdate);
      this._systemTrayService.RefreshGames();
      this._brokenInstallProgressChecker.RestoreBrokenProgresses();
    }

    private void GamesEnvironmentChanged(object sender, EventArgs e) => this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() =>
    {
      this._gamesConfigProvider.RefreshAndGetAll();
      this._windowsService.OpenUrlInMainWindow(this._launcherStateService.CurrentStartPage);
      this._systemTrayService.RefreshGames();
      this._brokenInstallProgressChecker.RestoreBrokenProgresses();
    }));
  }
}
