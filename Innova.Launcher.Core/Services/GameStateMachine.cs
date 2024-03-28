// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameStateMachine
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using NLog;
using Stateless;
using System;

namespace Innova.Launcher.Core.Services
{
  public class GameStateMachine
  {
    private readonly StateMachine<GameStatus, Trigger> _gameStateMachine;

    public GameStateMachine(
      string gameKey,
      IStatusSender statusSender,
      IGameManager gameManager,
      ILogger logger,
      IRegionalGamesService regionalGamesService,
      IGameStatusExtractor gameStatusExtractor)
    {
      Game game = regionalGamesService.GetGameOrDefault(gameKey);
      if (game == null)
      {
        game = new Game()
        {
          Key = gameKey,
          Status = GameStatus.NotInstalled
        };
        gameManager.SaveGame(game);
      }
      this._gameStateMachine = new StateMachine<GameStatus, Trigger>(game.Status);
      this._gameStateMachine.OnTransitioned((Action<StateMachine<GameStatus, Trigger>.Transition>) (transition =>
      {
        Game gameOrDefault = regionalGamesService.GetGameOrDefault(gameKey);
        logger.Trace(string.Format("Game {0} transited to {1} from {2}", (object) gameKey, (object) transition.Destination, (object) gameOrDefault.Status));
        gameOrDefault.Status = transition.Destination;
        gameOrDefault.LastAction = transition.Trigger;
        if (transition.Source == GameStatus.UpdateProgress && transition.Trigger == Trigger.Finish)
          gameOrDefault.UpdateType = UpdateType.No;
        gameManager.SaveGame(gameOrDefault);
        statusSender.Send(gameStatusExtractor.GetStatus(gameOrDefault));
      }));
      this._gameStateMachine.Configure(GameStatus.NotInstalled).PermitReentry(Trigger.Cancel).Permit(Trigger.Install, GameStatus.InstallProgress);
      this._gameStateMachine.Configure(GameStatus.Installed).PermitReentry(Trigger.Cancel).Permit(Trigger.Uninstall, GameStatus.NotInstalled);
      this._gameStateMachine.Configure(GameStatus.InstallProgress).PermitReentry(Trigger.Resume).Permit(Trigger.Pause, GameStatus.InstallationPaused).Permit(Trigger.Cancel, GameStatus.NotInstalled).Permit(Trigger.Finish, GameStatus.Installed);
      this._gameStateMachine.Configure(GameStatus.InstallationPaused).Permit(Trigger.Resume, GameStatus.InstallProgress).Permit(Trigger.Cancel, GameStatus.NotInstalled);
      this._gameStateMachine.Configure(GameStatus.Installed).PermitIf(Trigger.Update, GameStatus.UpdateProgress, (Func<bool>) (() =>
      {
        Game gameOrDefault = regionalGamesService.GetGameOrDefault(gameKey);
        return gameOrDefault.UpdateType == UpdateType.UpdateRequired || gameOrDefault.UpdateType == UpdateType.FullUpdateRequired;
      }), (string) null);
      this._gameStateMachine.Configure(GameStatus.UpdateProgress).PermitReentry(Trigger.Resume).Permit(Trigger.Finish, GameStatus.Installed).Permit(Trigger.Pause, GameStatus.UpdatePaused).Permit(Trigger.Cancel, GameStatus.Installed);
      this._gameStateMachine.Configure(GameStatus.UpdatePaused).Permit(Trigger.Resume, GameStatus.UpdateProgress).Permit(Trigger.Cancel, GameStatus.Installed);
      this._gameStateMachine.Configure(GameStatus.Installed).Permit(Trigger.Repair, GameStatus.RepairProgress);
      this._gameStateMachine.Configure(GameStatus.RepairProgress).PermitReentry(Trigger.Resume).Permit(Trigger.Finish, GameStatus.Installed).Permit(Trigger.Pause, GameStatus.RepairPaused).Permit(Trigger.Cancel, GameStatus.Installed);
      this._gameStateMachine.Configure(GameStatus.RepairPaused).Permit(Trigger.Resume, GameStatus.RepairProgress).Permit(Trigger.Cancel, GameStatus.Installed);
    }

    public bool CanInstall() => this.CanFire(Trigger.Install);

    public bool CanUninstall() => this.CanFire(Trigger.Uninstall);

    public bool CanRepair() => this.CanFire(Trigger.Repair);

    public bool CanResume() => this.CanFire(Trigger.Resume);

    public bool CanPause() => this.CanFire(Trigger.Pause);

    public bool CanUpdate() => this.CanFire(Trigger.Update);

    public bool CanCancel() => this.CanFire(Trigger.Cancel);

    public bool CanFinishProgress() => this.CanFire(Trigger.Finish);

    public void Install() => this.DoTransition(Trigger.Install);

    public void Repair() => this.DoTransition(Trigger.Repair);

    public void Update() => this.DoTransition(Trigger.Update);

    public void Pause() => this.DoTransition(Trigger.Pause);

    public void Resume() => this.DoTransition(Trigger.Resume);

    public void Cancel() => this.DoTransition(Trigger.Cancel);

    public void Uninstall() => this.DoTransition(Trigger.Uninstall);

    public void FinishProgress() => this.DoTransition(Trigger.Finish);

    public bool IsInInstallationProgress()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.InstallProgress);
    }

    public bool IsInUpdateProgress()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.UpdateProgress);
    }

    public bool IsInRepairProgress()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.RepairProgress);
    }

    public bool IsInInstallationPause()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.InstallationPaused);
    }

    public bool IsInUpdatePause()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.UpdatePaused);
    }

    public bool IsInRepairPause()
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.IsInState(GameStatus.RepairPaused);
    }

    private bool CanFire(Trigger trigger)
    {
      lock (this._gameStateMachine)
        return this._gameStateMachine.CanFire(trigger);
    }

    private void DoTransition(Trigger trigger)
    {
      lock (this._gameStateMachine)
      {
        if (!this._gameStateMachine.CanFire(trigger))
          throw new GameStateMachineTransitionException(this._gameStateMachine.State, trigger);
        this._gameStateMachine.Fire(trigger);
      }
    }
  }
}
