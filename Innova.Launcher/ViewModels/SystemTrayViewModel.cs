// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.SystemTrayViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using DynamicData;
using DynamicData.Aggregation;
using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.UI.Extensions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace Innova.Launcher.ViewModels
{
  public class SystemTrayViewModel : ReactiveObject
  {
    private readonly ISystemTrayService _systemTrayService;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly SourceList<GameTrayViewModel> _gamesSource;
    private Dictionary<string, string> _gamesKeyNameMapper;
    private readonly ReadOnlyObservableCollection<GameTrayViewModel> _games;
    private string _tooltipText;
    private readonly ObservableAsPropertyHelper<bool> _noGames;

    public ReadOnlyObservableCollection<GameTrayViewModel> Games => this._games;

    public string TooltipText
    {
      get => this._tooltipText;
      set => this.RaiseAndSetIfChanged<SystemTrayViewModel, string>(ref this._tooltipText, value, nameof (TooltipText));
    }

    public bool NoGames => this._noGames.Value;

    public ReactiveCommand<Unit, Unit> ExitCommand { get; }

    public ReactiveCommand<Unit, Unit> RiseMainWindowCommand { get; }

    public ReactiveCommand<Unit, Unit> RiseOrHideMainWindowCommand { get; }

    public ReactiveCommand<Unit, Unit> OpenForgameCommand { get; }

    public SystemTrayViewModel(
      ISystemTrayService systemTrayService,
      IGamesConfigProvider gamesConfigProvider,
      IInternetConnectionChecker internetConnectionChecker,
      IGameStatusExtractor gameStatusExtractor)
    {
      this._gamesSource = new SourceList<GameTrayViewModel>((IObservable<IChangeSet<GameTrayViewModel>>) null);
      this._systemTrayService = systemTrayService;
      this._gamesConfigProvider = gamesConfigProvider;
      this._internetConnectionChecker = internetConnectionChecker;
      this._gameStatusExtractor = gameStatusExtractor;
      this.ExitCommand = ReactiveCommand.Create(new Action(systemTrayService.Exit));
      this.RiseMainWindowCommand = ReactiveCommand.Create(new Action(systemTrayService.RiseMainWindow));
      this.RiseOrHideMainWindowCommand = ReactiveCommand.Create(new Action(systemTrayService.RiseOrHideMainWindow));
      this.OpenForgameCommand = ReactiveCommand.Create(new Action(systemTrayService.OpenMainPage));
      ObservableListEx.DisposeMany<GameTrayViewModel>(ObservableListEx.Bind<GameTrayViewModel>(this._gamesSource.Connect((Func<GameTrayViewModel, bool>) null), ref this._games, 25)).Subscribe<IChangeSet<GameTrayViewModel>>();
      CountEx.IsEmpty<GameTrayViewModel>(this._gamesSource.Connect((Func<GameTrayViewModel, bool>) null)).ToProperty<SystemTrayViewModel, bool>(this, (Expression<Func<SystemTrayViewModel, bool>>) (v => v.NoGames), out this._noGames, true);
      this.TooltipText = "4game";
      this.RefreshGames();
      MessageBus.Current.Listen<GameStatusChangingEvent>().ObserveOnDispatcher<GameStatusChangingEvent>().Subscribe<GameStatusChangingEvent>((Action<GameStatusChangingEvent>) (evnt =>
      {
        GameTrayViewModel gameTrayViewModel = this.Games.FirstOrDefault<GameTrayViewModel>((Func<GameTrayViewModel, bool>) (e => e.Key == evnt.Status.GameKey));
        if (evnt.Status.Status == GameStatus.NotInstalled)
          this._gamesSource.Remove<GameTrayViewModel>(gameTrayViewModel);
        else if (gameTrayViewModel == null)
          this._gamesSource.Add<GameTrayViewModel>(this.CreateGameViewModel(evnt.Status));
        else
          gameTrayViewModel.SetFromGameStatus(evnt.Status);
      }));
    }

    public void RefreshGames() => this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() =>
    {
      this._gamesKeyNameMapper = this._gamesConfigProvider.RefreshAndGetAll().Games.ToDictionary<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string, string>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string>) (e => e.Key), (Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string>) (e => e.DisplayName));
      this.GuiNoWait((Action) (() =>
      {
        this._gamesSource.Clear<GameTrayViewModel>();
        foreach (GameTrayViewModel gameTrayViewModel in this._gameStatusExtractor.GetAllStatuses().Where<ServiceStatus>((Func<ServiceStatus, bool>) (e => e.Status != 0)).Select<ServiceStatus, GameTrayViewModel>(new Func<ServiceStatus, GameTrayViewModel>(this.CreateGameViewModel)))
          this._gamesSource.Add<GameTrayViewModel>(gameTrayViewModel);
      }));
    }));

    private GameTrayViewModel CreateGameViewModel(ServiceStatus status) => new GameTrayViewModel(status, (IDictionary<string, string>) this._gamesKeyNameMapper, this.CreateGameOpenCommand(status.GameKey));

    private ReactiveCommand<Unit, Unit> CreateGameOpenCommand(string gameKey) => ReactiveCommand.Create((Action) (() => this._systemTrayService.OpenGame(gameKey)));
  }
}
