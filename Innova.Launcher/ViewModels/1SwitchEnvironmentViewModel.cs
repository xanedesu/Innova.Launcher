// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.GamesEnvironmentViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;

namespace Innova.Launcher.ViewModels
{
  public class GamesEnvironmentViewModel
  {
    private readonly GamesEnvironments.GamesEnvironment _gamesEnvironment;

    public string Name => this._gamesEnvironment.Name;

    public string DisplayName => this._gamesEnvironment.DisplayName;

    public GamesEnvironmentViewModel(
      GamesEnvironments.GamesEnvironment gamesEnvironment)
    {
      this._gamesEnvironment = gamesEnvironment;
    }
  }
}
