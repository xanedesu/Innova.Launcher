// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.SwitchGamesEnvironmentService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using System;

namespace Innova.Launcher.Services
{
  public class SwitchGamesEnvironmentService : ISwitchGamesEnvironmentService
  {
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;

    public SwitchGamesEnvironmentService(IGamesEnvironmentProvider gamesEnvironmentProvider) => this._gamesEnvironmentProvider = gamesEnvironmentProvider;

    public void SwitchEnvironment() => this.Gui((Action) (() =>
    {
      SwitchGamesEnvironmentViewModel viewModel = new SwitchGamesEnvironmentViewModel(this._gamesEnvironmentProvider);
      viewModel.Init();
      new SwitchGamesEnvironmentWindow(viewModel).ShowDialog();
    }));
  }
}
