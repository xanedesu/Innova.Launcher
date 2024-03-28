// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameManager
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Exceptions.GameStart;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Infrastructure.Mapping.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services.GameUpdateHandlers;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Infrastructure.Internet;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.Updater.Core.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class GameManager : IGameManager
  {
    private readonly IGameRepositoryFactory _gameRepositoryFactory;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly ILauncherStateService _launcherStateService;
    private readonly FrostUpdaterFactory _frostUpdaterFactory;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly IStatusSender _statusSender;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IRuntimeErrorsHandler _runtimeErrorsHandler;
    private readonly ILauncherConfigurationProvider _launcherConfigurationProvider;
    private readonly IEnumerable<IGameUpdateHandler> _gameUpdateHandlers;

    public GameManager(
      ILoggerFactory loggerFactory,
      IGameRepositoryFactory gameRepositoryFactory,
      IGamesConfigProvider gamesConfigProvider,
      ILauncherStateService launcherStateService,
      FrostUpdaterFactory frostUpdaterFactory,
      IStatusSender statusSender,
      IMapper mapper,
      IGameStatusExtractor gameStatusExtractor,
      IRegionalGamesService regionalGamesService,
      IRuntimeErrorsHandler runtimeErrorsHandler,
      ILauncherConfigurationProvider launcherConfigurationProvider,
      IEnumerable<IGameUpdateHandler> gameUpdateHandlers)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameManager>();
      this._gameRepositoryFactory = gameRepositoryFactory ?? throw new ArgumentNullException(nameof (gameRepositoryFactory));
      this._gamesConfigProvider = gamesConfigProvider ?? throw new ArgumentNullException(nameof (gamesConfigProvider));
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
      this._frostUpdaterFactory = frostUpdaterFactory ?? throw new ArgumentNullException(nameof (frostUpdaterFactory));
      this._statusSender = statusSender ?? throw new ArgumentNullException(nameof (statusSender));
      this._mapper = mapper ?? throw new ArgumentNullException(nameof (mapper));
      this._gameStatusExtractor = gameStatusExtractor ?? throw new ArgumentNullException(nameof (gameStatusExtractor));
      this._regionalGamesService = regionalGamesService ?? throw new ArgumentNullException(nameof (regionalGamesService));
      this._runtimeErrorsHandler = runtimeErrorsHandler ?? throw new ArgumentNullException(nameof (runtimeErrorsHandler));
      this._launcherConfigurationProvider = launcherConfigurationProvider ?? throw new ArgumentNullException(nameof (launcherConfigurationProvider));
      this._gameUpdateHandlers = (IEnumerable<IGameUpdateHandler>) ((gameUpdateHandlers != null ? gameUpdateHandlers.ToList<IGameUpdateHandler>() : (List<IGameUpdateHandler>) null) ?? throw new ArgumentNullException(nameof (gameUpdateHandlers)));
      this._logger = loggerFactory.GetCurrentClassLogger<GameManager>();
    }

    public void Launch(string gameKey, GameLaunchData data)
    {
      try
      {
        Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
        Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameOrDefault.Key).WithEvent(gameOrDefault.CurrentGameEvent);
        int length = gameConfig.StageSizesArray.Length;
        ServiceStatus update = this.CheckGameNeedToUpdate(gameOrDefault, true);
        if (update.Status == GameStatus.OutOfDate)
        {
          this.SendGameStatus(update);
        }
        else
        {
          string launchParams = gameConfig.LaunchParams.Replace("%ACCOUNT_ID%", data.Login).Replace("%LOGIN%", data.Login).Replace("%PASS%", data.Password).Replace("%STAGE%", length.ToString()).Replace("%AUTH_URL%", this._launcherConfigurationProvider.SingleGamesValidationUrl);
          if (!string.IsNullOrWhiteSpace(data.Culture))
          {
            GameLanguage[] languages = gameConfig.Languages;
            GameLanguage gameLanguage = languages != null ? ((IEnumerable<GameLanguage>) languages).FirstOrDefault<GameLanguage>((Func<GameLanguage, bool>) (p => p.Culture == data.Culture)) : (GameLanguage) null;
            if (gameLanguage != null && gameLanguage.BeforeLaunch != null && gameLanguage.BeforeLaunch.Length != 0)
            {
              string currentDirectory = Directory.GetCurrentDirectory();
              try
              {
                Directory.SetCurrentDirectory(gameOrDefault.Path);
                foreach (Operation operation in gameLanguage.BeforeLaunch)
                {
                  try
                  {
                    operation.Execute();
                  }
                  catch (Exception ex)
                  {
                    this._logger.Error(ex, "On before command execution");
                  }
                }
              }
              finally
              {
                Directory.SetCurrentDirectory(currentDirectory);
              }
            }
            if (gameLanguage != null && gameLanguage.LaunchParam != null)
              launchParams = launchParams + " " + gameLanguage.LaunchParam;
          }
          if (gameConfig.LaunchType == LaunchType.Frost)
          {
            string frostLaunchPath = this.GetFrostFullPath(gameOrDefault, gameConfig);
            IFrostUpdater frostUpdater = this._frostUpdaterFactory.Get();
            TaskCompletionSource<bool> frostUpdateResult = new TaskCompletionSource<bool>();
            string frostKey = gameConfig.FrostKey;
            frostUpdater.FrostUpdateCompleted += (EventHandler) ((s, e) =>
            {
              try
              {
                string str = System.IO.File.Exists(frostLaunchPath) ? Path.GetDirectoryName(frostLaunchPath) : throw new GameStartException(gameKey, "frost_exe_not_found", "Not found frost exe by path " + frostLaunchPath);
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                  FileName = frostLaunchPath,
                  UseShellExecute = true,
                  WorkingDirectory = str,
                  Arguments = "-frostGameNameType " + frostKey + " -frostLauncher " + gameConfig.FrostLauncher + " -frostGame " + gameConfig.FrostGame + " " + launchParams
                };
                this._logger.Trace("Run game by frost. (dir: " + startInfo.WorkingDirectory + ", file: " + startInfo.FileName + ", args: " + startInfo.Arguments + ")");
                Process process = Process.Start(startInfo);
                if (process == null || process.HasExited)
                  throw new GameStartException(gameKey, "frost_exe_not_found", "Not started frost exe " + frostLaunchPath);
                frostUpdateResult.SetResult(true);
              }
              catch (Exception ex)
              {
                this._logger.Error(ex, "Error while start game after frost update");
                frostUpdateResult.SetException(ex);
              }
            });
            try
            {
              try
              {
                frostUpdater.UpdateAsync(frostLaunchPath, frostKey).Wait(TimeSpan.FromMinutes(15.0));
              }
              catch (AggregateException ex)
              {
                if (ex.InnerExceptions.Count == 1)
                  ExceptionDispatchInfo.Capture(ex.InnerExceptions[0]).Throw();
                throw;
              }
            }
            catch (ForgameUpdaterStartException ex)
            {
              throw new GameStartException(gameKey, "frost_update_error", "Not started frost update on path " + frostLaunchPath);
            }
          }
          else
          {
            if (gameConfig.LaunchType != LaunchType.Exe)
              return;
            string str1 = gameConfig.FrostGame.Replace("%PROG_FILES_X86%", FileSystemHelper.GetProgramFilesX86Folder());
            if (!Path.IsPathRooted(str1))
              str1 = Path.Combine(gameOrDefault.Path, str1);
            string str2 = System.IO.File.Exists(str1) ? new FileInfo(str1).DirectoryName : throw new GameStartException(gameKey, "game_exe_not_found", "Not found exe by path " + str1);
            if (!string.IsNullOrEmpty(gameConfig.Workdir))
              str2 = gameConfig.Workdir.Replace("%GAME_PATH%", gameOrDefault.Path);
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
              WorkingDirectory = str2,
              UseShellExecute = true,
              FileName = str1,
              Arguments = launchParams
            };
            this._logger.Trace("Run game by exe. (dir: " + startInfo.WorkingDirectory + ", file: " + startInfo.FileName + ", args: " + startInfo.Arguments + ")");
            Process process = Process.Start(startInfo);
            if (process == null || process.HasExited)
              throw new GameStartException(gameKey, "game_exe_not_started", "Not started exe " + str1);
          }
        }
      }
      catch (Exception ex)
      {
        string errorCode = "game_launcher_unknown_error";
        if (ex is GameStartException gameStartException)
          errorCode = gameStartException.Code;
        this._runtimeErrorsHandler.HandleError(new RuntimeErrorInfo("users.game.launch.error", gameKey, errorCode, ex.Message));
        this._logger.Error(ex, "Error while start game");
        throw;
      }
    }

    public string GetPath(string gameKey)
    {
      string path = this._regionalGamesService.GetGameOrDefault(gameKey)?.Path;
      return !string.IsNullOrEmpty(path) ? Path.GetFullPath(path) : (string) null;
    }

    public void SaveGame(Game game) => this._gameRepositoryFactory.Get().Save(game);

    public void SetLaunchData(string gameKey, string path)
    {
      Game game1 = this._regionalGamesService.GetGameOrDefault(gameKey);
      if (game1 == null)
        game1 = new Game() { Key = gameKey };
      Game game2 = game1;
      game2.Path = path;
      this.SaveGame(game2);
    }

    public void CleanLaunchData(string gameKey)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
      if (gameOrDefault == null)
        return;
      gameOrDefault.Path = (string) null;
      this.SaveGame(gameOrDefault);
    }

    public Game CreateGame(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig, string path, string culture)
    {
      Game game = this._mapper.Map<Innova.Launcher.Shared.Models.GameConfig.GameConfig, Game>(gameConfig);
      game.Path = path;
      game.Culture = culture;
      this.SaveGame(game);
      return game;
    }

    public void UpdateGame(Game game, Innova.Launcher.Shared.Models.GameConfig.GameConfig actualConfig)
    {
      game.PreviousVersion = game.Version;
      game.PreviousUrl = game.Url;
      game.Version = actualConfig.Version;
      game.Url = actualConfig.Url;
      game.OptionsExe = actualConfig.OptionsExe;
      this.SaveGame(game);
    }

    public void RevertGameVersion(Game game)
    {
      game.Version = game.PreviousVersion;
      game.Url = game.PreviousUrl;
      this.SaveGame(game);
    }

    public byte[] GetGameIconData(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig) => this.GetIconData(this._gamesConfigProvider.RefreshAndGetAll().GetGameIconUrl(gameConfig.Key));

    public string GetGameWindowUrl(string gameKey, bool withAuth)
    {
      UriBuilder uriBuilder = new UriBuilder(this._launcherStateService.CurrentStartPage);
      string path = uriBuilder.Path;
      uriBuilder.Path = !withAuth ? Path.Combine(path, "splash", gameKey) : Path.Combine(path, "auth", gameKey);
      return uriBuilder.Uri.ToString();
    }

    public void SendGameStatus(ServiceStatus status) => this._statusSender.Send(status);

    public void ClearError(Game game)
    {
      game.Error = (string) null;
      game.ErrorDescription = (string) null;
      game.ErrorData = (BaseError) null;
      this.SaveGame(game);
    }

    public ServiceStatus CheckGameNeedToUpdate(Game game, bool refreshConfig = true)
    {
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = GetGameConfig();
      string version = gameConfig.Version;
      if (game.CurrentGameEvent != null)
      {
        GameEventConfigs events = gameConfig.Events;
        GameEventConfig gameEventConfig = events != null ? events.FirstOrDefault<GameEventConfig>((Func<GameEventConfig, bool>) (e => e.EventKey == game.CurrentGameEvent)) : (GameEventConfig) null;
        if (gameEventConfig == null)
        {
          version = gameConfig.Version;
          game.CurrentGameEvent = (string) null;
          this.SaveGame(game);
        }
        else
          version = gameEventConfig.Version;
      }
      if (!string.IsNullOrWhiteSpace(version) && version != game.Version)
      {
        if (game.UpdateType == UpdateType.No || game.UpdateType == UpdateType.UpdateRequired)
        {
          this._logger?.Trace("There is update version " + version + " of game key=" + game.Key + " (current version = " + game.Version + ")");
          game.UpdateType = UpdateType.UpdateRequired;
          game.AvailableVersion = version;
          this.SaveGame(game);
        }
      }
      else
      {
        if (game.Status == GameStatus.Installed && game.UpdateType != UpdateType.No)
        {
          this._logger.Trace("There was not-nessesary update status on game key=" + game.Key + " version " + version + " (current version = " + game.Version + ")");
          game.UpdateType = UpdateType.No;
          game.AvailableVersion = version;
          this.SaveGame(game);
        }
        if (this.ShouldUpdateGame(game))
        {
          game.UpdateType = UpdateType.UpdateRequired;
          game.AvailableVersion = version;
          this.SaveGame(game);
        }
      }
      return this._gameStatusExtractor.GetStatus(game);

      Innova.Launcher.Shared.Models.GameConfig.GameConfig GetGameConfig() => refreshConfig ? this._gamesConfigProvider.RefreshAndGetAll().GetGameConfig(game.Key) : this._gamesConfigProvider.GetGame(game.Key);
    }

    private bool ShouldUpdateGame(Game game)
    {
      try
      {
        IGameUpdateHandler gameUpdateHandler = this._gameUpdateHandlers.FirstOrDefault<IGameUpdateHandler>((Func<IGameUpdateHandler, bool>) (h => h.GameKey == game.Key));
        if (gameUpdateHandler != null)
        {
          this._logger.Trace("[" + game.Key + "] found update handler updateHandler");
          return gameUpdateHandler.ShouldUpdateGame(game);
        }
      }
      catch (Exception ex)
      {
        this._logger.Warn(string.Format("Failed to check {0} update requirement. {1}", (object) game.Key, (object) ex));
      }
      return false;
    }

    public string GetFrostFullPath(Game game, Innova.Launcher.Shared.Models.GameConfig.GameConfig config) => !string.IsNullOrWhiteSpace(game?.Path) && !string.IsNullOrWhiteSpace(config.FrostPath) ? Path.GetFullPath(Path.Combine(game.Path, config.FrostPath)) : (string) null;

    public GamePathValidationInfo ValidateGamePath(string gameKey, string path)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameKey).WithEvent(gameOrDefault?.CurrentGameEvent);
      bool flag = FileSystemHelper.IsValidPath(path, false);
      long? nullable1 = flag ? new long?(FileSystemHelper.GetFreeSpace(path)) : new long?();
      long result;
      long? nullable2 = long.TryParse(gameConfig?.Size, out result) ? new long?(result) : new long?();
      string str = flag ? Path.GetPathRoot(path)?.Substring(0, 1) : (string) null;
      GamePathValidationInfo pathValidationInfo = new GamePathValidationInfo()
      {
        Path = path,
        DriveLetter = str,
        FreeSpace = nullable1,
        GameSize = nullable2
      };
      if (!flag)
        pathValidationInfo.AddError((BaseError) new GameInvalidPathError());
      else if (nullable2.HasValue)
      {
        long? nullable3 = nullable1;
        long? nullable4 = nullable2;
        if (nullable3.GetValueOrDefault() < nullable4.GetValueOrDefault() & nullable3.HasValue & nullable4.HasValue)
          pathValidationInfo.AddError((BaseError) new GameNotEnoughSpaceError(nullable1.Value, nullable2.Value));
      }
      return pathValidationInfo;
    }

    public void SelectEventForGame(string gameKey, string gameEventKey)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
      if (gameOrDefault == null)
        return;
      gameOrDefault.CurrentGameEvent = gameEventKey;
      this.SaveGame(gameOrDefault);
      this.SendGameStatus(this.CheckGameNeedToUpdate(gameOrDefault, true));
    }

    private byte[] GetIconData(string url)
    {
      using (HeaderedWebClient headeredWebClient = new HeaderedWebClient())
      {
        try
        {
          return headeredWebClient.DownloadData(url);
        }
        catch (WebException ex)
        {
          this._logger.Error((Exception) ex, "Problem to download icon data by url=" + url);
          return new byte[0];
        }
      }
    }
  }
}
