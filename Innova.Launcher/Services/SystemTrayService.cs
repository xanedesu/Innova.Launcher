// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.SystemTrayService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using System;

namespace Innova.Launcher.Services
{
  public class SystemTrayService : ISystemTrayService
  {
    private readonly IWindowsService _dialogService;
    private readonly IGameManager _gameManager;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IGameStatusExtractor _gameStatusExtractor;
    private SystemTrayControl _trayControl;

    public SystemTrayService(
      ILoggerFactory loggerFactory,
      IWindowsService dialogService,
      ILifetimeManager lifetimeManager,
      IGameManager gameManager,
      IGamesConfigProvider gamesConfigProvider,
      IInternetConnectionChecker internetConnectionChecker,
      ILauncherStateService launcherStateService,
      IGameStatusExtractor gameStatusExtractor)
    {
      this._internetConnectionChecker = internetConnectionChecker;
      this._launcherStateService = launcherStateService;
      this._gameStatusExtractor = gameStatusExtractor;
      this._dialogService = dialogService;
      this._gameManager = gameManager;
      this._gamesConfigProvider = gamesConfigProvider;
      lifetimeManager.TimeToDie += new EventHandler(this.ApplicationDying);
    }

    public void CreateTrayIcon() => this.Gui((Action) (() => this._trayControl = new SystemTrayControl(new SystemTrayViewModel((ISystemTrayService) this, this._gamesConfigProvider, this._internetConnectionChecker, this._gameStatusExtractor))));

    public void RefreshGames() => this.Gui((Action) (() => this._trayControl?.ViewModel.RefreshGames()));

    public void Exit()
    {
      if (this._trayControl.ContextMenu != null)
        this._trayControl.ContextMenu.IsOpen = false;
      this._dialogService.CloseWindow("main", true);
    }

    public void RiseMainWindow() => this._dialogService.RiseAllWindows();

    public void RiseOrHideMainWindow()
    {
      if (this._dialogService.IsMainWindowVisible())
        this._dialogService.HideToTrayAllWindows();
      else
        this._dialogService.RiseAllWindows();
    }

    public void OpenGame(string key)
    {
      this._dialogService.RiseAllWindows();
      this._dialogService.OpenUrlInMainWindow(this._gameManager.GetGameWindowUrl(key, false));
    }

    public void OpenMainPage()
    {
      this._dialogService.RiseAllWindows();
      this._dialogService.OpenUrlInMainWindow(this._launcherStateService.CurrentStartPage);
    }

    private void ApplicationDying(object sender, EventArgs e) => this._trayControl?.Dispose();
  }
}
