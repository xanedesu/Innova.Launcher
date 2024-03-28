// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.IGameManager
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.MessageHandlers.Status;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface IGameManager
  {
    void Launch(string gameKey, GameLaunchData data);

    Game CreateGame(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig, string path, string culture);

    void SaveGame(Game game);

    void SetLaunchData(string gameKey, string path);

    void CleanLaunchData(string gameKey);

    string GetPath(string gameKey);

    void UpdateGame(Game game, Innova.Launcher.Shared.Models.GameConfig.GameConfig actualConfig);

    void RevertGameVersion(Game game);

    byte[] GetGameIconData(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig);

    string GetGameWindowUrl(string gameKey, bool withAuth);

    void ClearError(Game game);

    ServiceStatus CheckGameNeedToUpdate(Game game, bool refreshConfig = true);

    string GetFrostFullPath(Game game, Innova.Launcher.Shared.Models.GameConfig.GameConfig config);

    GamePathValidationInfo ValidateGamePath(string gameKey, string path);

    void SelectEventForGame(string dataGameKey, string dataGameEventKey);
  }
}
