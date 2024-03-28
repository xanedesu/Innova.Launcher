// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Interfaces.IGamesConfigProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Models.GameConfig;

namespace Innova.Launcher.Shared.Services.Interfaces
{
  public interface IGamesConfigProvider
  {
    Innova.Launcher.Shared.Models.GameConfig.GameConfig GetGame(string gameKey);

    GamesConfig RefreshAndGetAll();

    Innova.Launcher.Shared.Models.GameConfig.GameConfig RefreshAndGetGame(string gameKey);

    void RunSyncRemoteGameVersions();
  }
}
