// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.SwitchGamesEnvironmentViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.UI.Extensions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace Innova.Launcher.ViewModels
{
  public class SwitchGamesEnvironmentViewModel : ReactiveObject
  {
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;
    private List<GamesEnvironmentViewModel> _gamesEnvironments;
    private GamesEnvironmentViewModel _selectedGamesEnvironment;

    public List<GamesEnvironmentViewModel> GamesEnvironments
    {
      get => this._gamesEnvironments;
      set => this.RaiseAndSetIfChanged<SwitchGamesEnvironmentViewModel, List<GamesEnvironmentViewModel>>(ref this._gamesEnvironments, value, nameof (GamesEnvironments));
    }

    public GamesEnvironmentViewModel SelectedGamesEnvironment
    {
      get => this._selectedGamesEnvironment;
      set => this.RaiseAndSetIfChanged<SwitchGamesEnvironmentViewModel, GamesEnvironmentViewModel>(ref this._selectedGamesEnvironment, value, nameof (SelectedGamesEnvironment));
    }

    public ReactiveCommand<Unit, Unit> SwitchCommand { get; }

    public SwitchGamesEnvironmentViewModel(IGamesEnvironmentProvider gamesEnvironmentProvider)
    {
      this._gamesEnvironmentProvider = gamesEnvironmentProvider;
      this.SwitchCommand = ReactiveCommand.Create<Unit>((Action<Unit>) (v =>
      {
        if (this.SelectedGamesEnvironment == null)
          return;
        this.Gui((Action) (() => this._gamesEnvironmentProvider.UpdateGamesEnvironment(this.SelectedGamesEnvironment.Name)));
      }));
    }

    public void Init() => Task.Factory.StartNew((Action) (() =>
    {
      Innova.Launcher.Core.Models.GamesEnvironments environments = this._gamesEnvironmentProvider.GetAvailableGamesEnvironments();
      this.GuiNoWait((Action) (() =>
      {
        this.GamesEnvironments = new List<GamesEnvironmentViewModel>(environments.Environments.Select<Innova.Launcher.Core.Models.GamesEnvironments.GamesEnvironment, GamesEnvironmentViewModel>((Func<Innova.Launcher.Core.Models.GamesEnvironments.GamesEnvironment, GamesEnvironmentViewModel>) (e => new GamesEnvironmentViewModel(e))));
        this.SelectedGamesEnvironment = this.GamesEnvironments.FirstOrDefault<GamesEnvironmentViewModel>((Func<GamesEnvironmentViewModel, bool>) (v => v.Name == this._gamesEnvironmentProvider.CurrentGamesEnvironment));
      }));
    }));
  }
}
