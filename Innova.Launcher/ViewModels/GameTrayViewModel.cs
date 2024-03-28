// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.GameTrayViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;

namespace Innova.Launcher.ViewModels
{
  public class GameTrayViewModel : ReactiveObject
  {
    private string _name;
    private double _progress;
    private GameTrayState _state;

    public ReactiveCommand<Unit, Unit> OpenCommand { get; }

    public string Key { get; }

    public Type Type => typeof (GameTrayViewModel);

    public string Name
    {
      get => this._name;
      private set => this.RaiseAndSetIfChanged<GameTrayViewModel, string>(ref this._name, value, nameof (Name));
    }

    public double Progress
    {
      get => this._progress;
      private set => this.RaiseAndSetIfChanged<GameTrayViewModel, double>(ref this._progress, value, nameof (Progress));
    }

    public GameTrayState State
    {
      get => this._state;
      private set
      {
        int num = (int) this.RaiseAndSetIfChanged<GameTrayViewModel, GameTrayState>(ref this._state, value, nameof (State));
      }
    }

    public GameTrayViewModel(
      ServiceStatus status,
      IDictionary<string, string> keyNameMapper,
      ReactiveCommand<Unit, Unit> openCommand)
    {
      status = status ?? throw new ArgumentNullException(nameof (status));
      keyNameMapper = keyNameMapper ?? throw new ArgumentNullException(nameof (keyNameMapper));
      openCommand = openCommand ?? throw new ArgumentNullException(nameof (openCommand));
      this.OpenCommand = openCommand;
      this.Key = status.GameKey;
      string str;
      this.Name = keyNameMapper.TryGetValue(status.GameKey, out str) ? str : status.GameKey;
      this.SetFromGameStatus(status);
    }

    public void SetFromGameStatus(ServiceStatus status)
    {
      if (status.Info is ProgressInfo info)
        this.Progress = (double) info.Progress;
      if (status.Status == GameStatus.InstallationPaused || status.Status == GameStatus.UpdatePaused)
        this.State = GameTrayState.Paused;
      else if (status.Status == GameStatus.InstallProgress || status.Status == GameStatus.UpdateProgress || status.Status == GameStatus.RepairProgress)
        this.State = GameTrayState.InProgress;
      else
        this.State = GameTrayState.Installed;
    }
  }
}
