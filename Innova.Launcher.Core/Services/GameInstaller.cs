// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameInstaller
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Exceptions.GameComponent;
using Innova.Launcher.Core.Infrastructure.Installing.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.Updater.Core.Models;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class GameInstaller : IGameInstaller
  {
    private const string GameManagerInLauncherFolder = "gameManager";
    private const string GameManagerInGameFolder = "gameManager";
    private const string GameManagerExeName = "gameManager.exe";
    private readonly HashSet<GameStatus> _gameStatusesToSendProgressOn = new HashSet<GameStatus>()
    {
      GameStatus.InstallProgress,
      GameStatus.UpdateProgress,
      GameStatus.RepairProgress
    };
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IGameManager _gameManager;
    private readonly GameStateMachineFactory _gameStateMachineFactory;
    private readonly IExternalGameInstaller _externalInstaller;
    private readonly IFrostUpdater _frostUpdater;
    private readonly ILogger _logger;
    private readonly object _progressLocker = new object();
    private readonly IGameInSystemRegistrator _gameInSystemRegistrator;
    private readonly IGameComponentsInstaller _gameComponentsInstaller;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IRuntimeErrorsHandler _runtimeErrorsHandler;
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly IStatusSender _statusSender;

    public GameInstaller(
      IGamesConfigProvider gamesConfigProvider,
      IGameManager gameManager,
      ILoggerFactory loggerFactory,
      GameStateMachineFactory gameStateMachineFactory,
      IExternalGameInstaller externalInstaller,
      IGameInSystemRegistrator gameInSystemRegistrator,
      IFrostUpdater frostUpdater,
      IGameComponentsInstaller gameComponentsInstaller,
      ILauncherStateService launcherStateService,
      IRuntimeErrorsHandler runtimeErrorsHandler,
      IRegionalGamesService regionalGamesService,
      IGameStatusExtractor gameStatusExtractor,
      IStatusSender statusSender)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameInstaller>();
      this._gamesConfigProvider = gamesConfigProvider;
      this._gameManager = gameManager;
      this._gameStateMachineFactory = gameStateMachineFactory;
      this._externalInstaller = externalInstaller;
      this._frostUpdater = frostUpdater;
      this._gameComponentsInstaller = gameComponentsInstaller;
      this._launcherStateService = launcherStateService;
      this._logger = loggerFactory.GetCurrentClassLogger<GameInstaller>();
      this._gameInSystemRegistrator = gameInSystemRegistrator;
      this._runtimeErrorsHandler = runtimeErrorsHandler;
      this._regionalGamesService = regionalGamesService;
      this._gameStatusExtractor = gameStatusExtractor;
      this._statusSender = statusSender;
      this._externalInstaller.InstallCompleted += new EventHandler<GameInstallCompletedEventArgs>(this.InstallCompleted);
      this._externalInstaller.InstallProgressReceived += new EventHandler<GameInstallProgressInfoEventArgs>(this.InstallProgressReceived);
      this._externalInstaller.InstallError += new EventHandler<GameInstallErrorEventArgs>(this.InstallProgressError);
    }

    public async Task InstallAsync(
      string gameKey,
      string path,
      string extendedStatus,
      string culture)
    {
      Game game = (Game) null;
      try
      {
        this._logger.Trace("Install with params (" + gameKey + ", " + path + ", " + extendedStatus + ")");
        GameStateMachine stateMachine = this._gameStateMachineFactory.Get(gameKey);
        if (stateMachine.CanInstall())
        {
          Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.RefreshAndGetAll().GetGameConfig(gameKey);
          game = this._gameManager.CreateGame(gameConfig, path, culture);
          if (!this.IsSelectedGameFolderValid(game, path, "start_install_error", "Installation path validation error"))
          {
            game = (Game) null;
            return;
          }
          await this.UpdateFrostAndCleanStatus(game, gameConfig);
          this._launcherStateService.SetLastGamesFolder(Path.GetDirectoryName(path));
          await this._externalInstaller.InstallAsync(gameKey, path, game.Url, gameConfig.UpdaterExeName).ConfigureAwait(false);
          stateMachine.Install();
          gameConfig = (Innova.Launcher.Shared.Models.GameConfig.GameConfig) null;
        }
        stateMachine = (GameStateMachine) null;
        game = (Game) null;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Error while game start installing");
        if (game == null)
        {
          game = (Game) null;
        }
        else
        {
          this.ReportErrorAndCancel(game, "start_install_error", "Starting installation error" + System.Environment.NewLine + ex?.ToString());
          game = (Game) null;
        }
      }
    }

    public async Task UpdateAsync(string gameKey)
    {
      this._logger.Trace("Update with params (" + gameKey + ")");
      GameStateMachine stateMachine = this._gameStateMachineFactory.Get(gameKey);
      if (!stateMachine.CanUpdate())
      {
        stateMachine = (GameStateMachine) null;
      }
      else
      {
        Game game = this._regionalGamesService.GetGameOrDefault(gameKey);
        Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameKey).WithEvent(game.CurrentGameEvent);
        if (!this.IsSelectedGameFolderValid(game, game.Path, "start_update_error", "Update path validation error", true))
        {
          stateMachine = (GameStateMachine) null;
        }
        else
        {
          try
          {
            await this.UpdateFrostAndCleanStatus(game, gameConfig);
            this._gameManager.UpdateGame(game, gameConfig);
            await this._externalInstaller.UpdateAsync(gameKey, game.Path, game.Url, gameConfig.UpdaterExeName);
            stateMachine.Update();
          }
          catch (Exception ex)
          {
            this._logger.Error(ex, "Error while game start updating");
            this.ReportErrorAndCancel(game, "start_update_error", "Starting game update error" + System.Environment.NewLine + ex?.ToString());
          }
          game = (Game) null;
          gameConfig = (Innova.Launcher.Shared.Models.GameConfig.GameConfig) null;
          stateMachine = (GameStateMachine) null;
        }
      }
    }

    public async Task RepairAsync(string gameKey)
    {
      this._logger.Trace("Repair with params (" + gameKey + ")");
      GameStateMachine stateMachine = this._gameStateMachineFactory.Get(gameKey);
      if (!stateMachine.CanRepair())
      {
        stateMachine = (GameStateMachine) null;
      }
      else
      {
        Game game = this._regionalGamesService.GetGameOrDefault(gameKey);
        Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameKey).WithEvent(game.CurrentGameEvent);
        if (!this.IsSelectedGameFolderValid(game, game.Path, "start_repair_error", "Repair path validation error", true))
        {
          stateMachine = (GameStateMachine) null;
        }
        else
        {
          try
          {
            await this.UpdateFrostAndCleanStatus(game, gameConfig);
            await this._externalInstaller.InstallAsync(gameKey, game.Path, game.Url, gameConfig.UpdaterExeName);
            stateMachine.Repair();
          }
          catch (Exception ex)
          {
            this._logger.Error(ex, "Error while game start repairing");
            this.ReportErrorAndCancel(game, "start_repair_error", "Game repair error" + System.Environment.NewLine + ex?.ToString());
          }
          game = (Game) null;
          gameConfig = (Innova.Launcher.Shared.Models.GameConfig.GameConfig) null;
          stateMachine = (GameStateMachine) null;
        }
      }
    }

    public void Pause(string gameKey)
    {
      this._logger.Trace("Pause with params " + gameKey);
      GameStateMachine gameStateMachine = this._gameStateMachineFactory.Get(gameKey);
      if (!gameStateMachine.CanPause())
        return;
      lock (this._progressLocker)
      {
        if (gameStateMachine.IsInInstallationProgress())
          this._externalInstaller.PauseInstall(gameKey);
        else if (gameStateMachine.IsInUpdateProgress())
          this._externalInstaller.PauseUpdate(gameKey);
        else if (gameStateMachine.IsInRepairProgress())
          this._externalInstaller.PauseInstall(gameKey);
        gameStateMachine.Pause();
      }
    }

    public async Task ResumeAsync(string gameKey)
    {
      this._logger.Trace("Resume with params " + gameKey);
      GameStateMachine stateMachine = this._gameStateMachineFactory.Get(gameKey);
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameKey).WithEvent(gameOrDefault.CurrentGameEvent);
      if (!stateMachine.CanResume())
      {
        stateMachine = (GameStateMachine) null;
      }
      else
      {
        if (stateMachine.IsInInstallationPause() || stateMachine.IsInInstallationProgress())
          await this._externalInstaller.ResumeInstallAsync(gameKey, gameOrDefault.Path, gameOrDefault.Url, gameConfig.UpdaterExeName);
        else if (stateMachine.IsInUpdatePause() || stateMachine.IsInUpdateProgress())
          await this._externalInstaller.ResumeUpdateAsync(gameKey, gameOrDefault.Path, gameOrDefault.Url, gameConfig.UpdaterExeName);
        else if (stateMachine.IsInRepairPause() || stateMachine.IsInRepairProgress())
          await this._externalInstaller.ResumeInstallAsync(gameKey, gameOrDefault.Path, gameOrDefault.Url, gameConfig.UpdaterExeName);
        stateMachine.Resume();
        stateMachine = (GameStateMachine) null;
      }
    }

    public void Cancel(string gameKey)
    {
      this._logger.Trace("Cancel with params " + gameKey);
      GameStateMachine gameStateMachine = this._gameStateMachineFactory.Get(gameKey);
      if (!gameStateMachine.CanCancel())
        return;
      lock (this._progressLocker)
      {
        Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
        if (gameStateMachine.IsInInstallationPause() || gameStateMachine.IsInInstallationProgress())
          this._externalInstaller.CancelInstall(gameKey);
        else if (gameStateMachine.IsInUpdatePause() || gameStateMachine.IsInUpdateProgress())
        {
          this._externalInstaller.CancelUpdate(gameKey);
          this._gameManager.RevertGameVersion(gameOrDefault);
        }
        else if (gameStateMachine.IsInRepairPause() || gameStateMachine.IsInRepairProgress())
          this._externalInstaller.CancelInstall(gameKey);
        gameOrDefault.ProgressInfo = (ProgressInfo) null;
        this._gameManager.SaveGame(gameOrDefault);
        gameStateMachine.Cancel();
      }
    }

    public void Register(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameConfig.Key);
      if (gameOrDefault == null || string.IsNullOrEmpty(gameOrDefault.Path))
        return;
      byte[] gameIconData = this._gameManager.GetGameIconData(gameConfig);
      string fullPath = Path.GetFullPath(gameOrDefault.Path);
      string path2_1 = "gameManager";
      string path2_2 = "gameManager.exe";
      string str = Path.Combine(fullPath, "gameManager");
      string gameManagerPath = Path.Combine(str, path2_2);
      string sourcePath = Path.Combine(System.Environment.CurrentDirectory, path2_1);
      FileSystemHelper.DeleteDirectoryIfExists(str, true);
      string destinationPath = str;
      FileSystemHelper.CopyDirectory(sourcePath, destinationPath);
      string uninstallString = this.GetUninstallString(gameConfig, gameOrDefault, fullPath, gameManagerPath, str);
      string runCommand = this.GetRunCommand(gameConfig);
      this._gameInSystemRegistrator.Register(new GameRegistrationData()
      {
        LauncherKey = this._launcherStateService.LauncherKey,
        Name = gameConfig.DisplayName,
        ShotcutTitle = gameConfig.ShortcutTitle ?? gameConfig.DisplayName,
        IconData = gameIconData,
        UninstallCommand = uninstallString,
        Key = gameOrDefault.EnvKey,
        Version = gameOrDefault.Version,
        RunnerPath = gameManagerPath,
        RunnerArgs = runCommand,
        InstallationPath = gameOrDefault.Path,
        InstallationDate = DateTime.UtcNow,
        Size = gameConfig.Size
      });
    }

    private bool IsSelectedGameFolderValid(
      Game game,
      string path,
      string errorCode,
      string errorMessage,
      bool ignoreFreeSpace = false)
    {
      GamePathValidationInfo pathValidationInfo = this._gameManager.ValidateGamePath(game.Key, path);
      if (pathValidationInfo.HasErrors & ignoreFreeSpace)
        pathValidationInfo.RemoveErrors<GameNotEnoughSpaceError>();
      List<BaseError> errors = pathValidationInfo.Errors;
      BaseError data = errors != null ? errors.FirstOrDefault<BaseError>() : (BaseError) null;
      if (data == null)
        return true;
      this.ReportErrorAndCancel(game, errorCode, errorMessage, data);
      return false;
    }

    private async Task UpdateFrostAndCleanStatus(Game game, Innova.Launcher.Shared.Models.GameConfig.GameConfig config)
    {
      if (config.LaunchType == LaunchType.Frost)
      {
        string frostKey = config.FrostKey;
        await this._frostUpdater.UpdateAsync(this._gameManager.GetFrostFullPath(game, config), frostKey).ConfigureAwait(false);
      }
      this._gameManager.ClearError(game);
      game.ProgressInfo = new ProgressInfo();
      game.InstallationStartDate = new DateTime?(DateTime.UtcNow);
      this._gameManager.SaveGame(game);
    }

    private string GetRunCommand(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig) => "run -l \"" + this._launcherStateService.LauncherKey + "\" -k \"" + gameConfig.Key.EscapeCmdInQuotesPart() + "\" -u \"" + gameConfig.GameUrl.EscapeCmdInQuotesPart() + "\"  ";

    private string GetUninstallString(
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig,
      Game game,
      string gameFullPath,
      string gameManagerPath,
      string gameManagerFolderPath)
    {
      string str1 = (string) null;
      if (gameConfig.Uninstall != null)
      {
        string str2 = gameConfig.Uninstall.Exe;
        if (str2.Contains("%"))
          str2 = str2.Replace("%PROG_FILES_X86%", FileSystemHelper.GetProgramFilesX86Folder()).Replace("%GAME_DIR%", gameFullPath);
        str1 = "\"" + str2 + "\" " + gameConfig.Uninstall.Args;
      }
      string[] strArray = new string[23];
      strArray[0] = "\"C:\\Windows\\System32\\cmd.exe\" /c \"\"";
      strArray[1] = gameManagerPath.EscapeCmdInQuotesPart();
      strArray[2] = "\" uninstall -p \"";
      strArray[3] = game.Path.EscapeCmdInQuotesPart();
      strArray[4] = "\" -u \"";
      string baseUrl = gameConfig.BaseUrl;
      strArray[5] = baseUrl != null ? baseUrl.EscapeCmdInQuotesPart() : (string) null;
      strArray[6] = "\" -k \"";
      strArray[7] = game.EnvKey.EscapeCmdInQuotesPart();
      strArray[8] = "\" -n \"";
      strArray[9] = game.Name.EscapeCmdInQuotesPart();
      strArray[10] = "\" -l \"";
      strArray[11] = this._launcherStateService.LauncherKey;
      strArray[12] = "\"  -t \"";
      string updaterExeName = gameConfig.UpdaterExeName;
      strArray[13] = updaterExeName != null ? updaterExeName.EscapeCmdInQuotesPart() : (string) null;
      strArray[14] = "\"  -s \"";
      string shortcutTitle = gameConfig.ShortcutTitle;
      strArray[15] = (shortcutTitle != null ? shortcutTitle.EscapeCmdInQuotesPart() : (string) null) ?? game.Name.EscapeCmdInQuotesPart();
      strArray[16] = "\"  && rd /s/q \"";
      strArray[17] = gameManagerFolderPath.EscapeCmdInQuotesPart();
      strArray[18] = "\" && rd /q \"";
      strArray[19] = game.Path.EscapeCmdInQuotesPart();
      strArray[20] = "\"";
      strArray[21] = string.IsNullOrWhiteSpace(str1) ? "" : " & " + str1;
      strArray[22] = " \"";
      return string.Concat(strArray);
    }

    private void InstallCompleted(object sender, GameInstallCompletedEventArgs args)
    {
      lock (this._progressLocker)
      {
        string gameKey = args.GameKey;
        Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
        if (gameOrDefault == null)
          return;
        Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this._gamesConfigProvider.GetGame(gameKey).WithEvent(gameOrDefault.CurrentGameEvent);
        ServiceStatus update = this._gameManager.CheckGameNeedToUpdate(gameOrDefault, false);
        if (update.Status == GameStatus.OutOfDate)
        {
          this._statusSender.Send(update);
        }
        else
        {
          GameStateMachine gameStateMachine = this._gameStateMachineFactory.Get(gameKey);
          if (gameStateMachine.CanFinishProgress() && (gameOrDefault.ProgressInfo == null || gameOrDefault.ProgressInfo.Progress >= 100M || gameOrDefault.ProgressInfo.Progress == 0M))
          {
            try
            {
              this._gameComponentsInstaller.Install(gameConfig, gameOrDefault);
            }
            catch (GameComponentInstallException ex)
            {
              this._logger.Error((Exception) ex, "Game components install error");
              this.ReportInstallFinishingError(gameOrDefault, "additional_component_install_error", string.Format("Component installation error {0}: {1} {2}", (object) ex.ComponentConfig.Name, (object) ex.ErrorCode, (object) ex.Message), (BaseError) new AdditionalComponentInstallError(ex.ErrorCode.ToString(), ex.ComponentConfig.Name));
              return;
            }
            catch (GameComponentInstallerStartException ex)
            {
              this._logger.Error((Exception) ex, "Game components install error");
              this.ReportInstallFinishingError(gameOrDefault, "additional_component_start_install_error", "Starting component installation error " + ex.ComponentConfig.Name + ": " + ex.Message, (BaseError) new AdditionalComponentInstallError(ex.ComponentConfig.Name));
              return;
            }
            catch (GameComponentException ex)
            {
              this._logger.Error((Exception) ex, "Game components install error");
              this.ReportInstallFinishingError(gameOrDefault, "additional_component_install_error", "Component installation error " + ex.ComponentConfig.Name + ": " + ex.Message, (BaseError) new AdditionalComponentInstallError(ex.ComponentConfig.Name));
              return;
            }
            catch (Exception ex)
            {
              this._logger.Error(ex, "Error while components install");
              this.ReportInstallFinishingError(gameOrDefault, "additional_component_install_error", "Components installation error");
              return;
            }
            try
            {
              this.Register(gameConfig);
            }
            catch (Exception ex)
            {
              this._logger.Error(ex, "Error while game " + gameKey + " registration");
              this.ReportInstallFinishingError(gameOrDefault, "game_registration_error", "Game registration in system error " + ex.Message + " " + gameKey);
              return;
            }
            gameOrDefault.ExtendedStatus = "full";
            gameOrDefault.ProgressInfo = (ProgressInfo) null;
            this._gameManager.SaveGame(gameOrDefault);
            gameStateMachine.FinishProgress();
          }
          else
          {
            gameOrDefault.ProgressInfo = (ProgressInfo) null;
            this._gameManager.SaveGame(gameOrDefault);
            this._statusSender.Send(this._gameStatusExtractor.GetStatus(gameOrDefault));
          }
        }
      }
    }

    private void InstallProgressError(object sender, GameInstallErrorEventArgs e) => this.ReportInstallFinishingError(this._regionalGamesService.GetGameOrDefault(e.GameKey), "game_install_progress_error", e.ErrorMessage ?? "", (BaseError) new GameInstallProgressError(e.ErrorCode.ToString()));

    private void ReportInstallFinishingError(
      Game game,
      string code,
      string errorMessage,
      BaseError errorData = null)
    {
      this._logger.Error("Error while game (key=" + game.Key + ", env_key=" + game.EnvKey + ") installing: " + code + " " + errorMessage);
      game.ProgressInfo = (ProgressInfo) null;
      this._regionalGamesService.Save(game);
      this._runtimeErrorsHandler.HandleError(new RuntimeErrorInfo("users.game.installation.error", game.Key, code, errorMessage)
      {
        ErrorData = errorData
      });
      GameStateMachine gameStateMachine = this._gameStateMachineFactory.Get(game.Key);
      if (!gameStateMachine.CanCancel())
        return;
      if (gameStateMachine.IsInUpdateProgress())
        this._gameManager.RevertGameVersion(game);
      gameStateMachine.Cancel();
    }

    private void ReportErrorAndCancel(Game game, string code, string errorMessage, BaseError data = null)
    {
      this._logger.Error("Error while game (key=" + game.Key + ", env_key=" + game.EnvKey + ") start installing: " + code + " " + errorMessage + " " + data?.Code);
      this._runtimeErrorsHandler.HandleError(new RuntimeErrorInfo("users.game.installation.error", game.Key, code, errorMessage)
      {
        ErrorData = data
      });
      GameStateMachine gameStateMachine = this._gameStateMachineFactory.Get(game.Key);
      if (!gameStateMachine.CanCancel())
        return;
      gameStateMachine.Cancel();
    }

    private void InstallProgressReceived(
      object sender,
      GameInstallProgressInfoEventArgs serviceProgressInfo)
    {
      lock (this._progressLocker)
      {
        Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(serviceProgressInfo.GameKey);
        if (gameOrDefault == null || !this._gameStatusesToSendProgressOn.Contains(gameOrDefault.Status))
          return;
        UpdateProgressInfo progressInfo = serviceProgressInfo.ProgressInfo;
        if ((progressInfo != null ? (progressInfo.Progress > 0M ? 1 : 0) : 0) != 0 || gameOrDefault.ProgressInfo == null)
          gameOrDefault.ProgressInfo = new ProgressInfo(serviceProgressInfo.ProgressInfo);
        this._gameManager.SaveGame(gameOrDefault);
        ServiceStatus status = this._gameStatusExtractor.GetStatus(gameOrDefault);
        status.Action = "install";
        status.Info = (object) gameOrDefault.ProgressInfo;
        this._statusSender.Send(status);
      }
    }
  }
}
