// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.InstallDir.InstallDirMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Newtonsoft.Json;
using System.IO;

namespace Innova.Launcher.Core.Services.MessageHandlers.InstallDir
{
  [WebMessageHandlerFilter(new string[] {"browseDir", "getInstallDir"})]
  public class InstallDirMessageHandler : IWebMessageHandler
  {
    private readonly IGameManager _gameManager;
    private readonly IWindowsService _dialogService;
    private readonly IOutputMessageDispatcherProvider _messageDispatcherProvider;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly ILauncherStateService _launcherStateService;

    public InstallDirMessageHandler(
      ILoggerFactory loggerFactory,
      IGameManager gameManager,
      IWindowsService dialogService,
      IOutputMessageDispatcherProvider messageDispatcherProvider,
      IGamesConfigProvider gamesConfigProvider,
      ILauncherStateService launcherStateService)
    {
      this._gameManager = gameManager;
      this._dialogService = dialogService;
      this._messageDispatcherProvider = messageDispatcherProvider;
      this._gamesConfigProvider = gamesConfigProvider;
      this._launcherStateService = launcherStateService;
    }

    public void Handle(WebMessage webMessage)
    {
      bool flag1 = true;
      string gameKey;
      string path1;
      if (webMessage.Type == "getInstallDir")
      {
        gameKey = webMessage.Data.ToObject<InstallDirMessageHandler.GetInstallDirInput>().GameKey;
        path1 = this.GetExistingGamePathOrDefault(gameKey);
      }
      else
      {
        InstallDirMessageHandler.BrowseDirInput browseDirInput = webMessage.Data.ToObject<InstallDirMessageHandler.BrowseDirInput>();
        bool? checkFreeSpace = browseDirInput.CheckFreeSpace;
        bool flag2 = false;
        flag1 = !(checkFreeSpace.GetValueOrDefault() == flag2 & checkFreeSpace.HasValue);
        string path2 = browseDirInput.Path;
        gameKey = browseDirInput.GameKey;
        if (string.IsNullOrWhiteSpace(path2))
          path2 = this.GetExistingGamePathOrDefault(gameKey);
        string str = this._dialogService.OpenFolderSelectDialog(FileSystemHelper.FindExistingDirectoryInPath(path2));
        if (string.IsNullOrWhiteSpace(str))
          return;
        path1 = str;
      }
      GamePathValidationInfo pathValidationInfo = this._gameManager.ValidateGamePath(gameKey, path1);
      if (pathValidationInfo.HasErrors && !flag1)
        pathValidationInfo.RemoveErrors<GameNotEnoughSpaceError>();
      GetInstallDirLauncherMessageData data = new GetInstallDirLauncherMessageData()
      {
        Path = path1,
        DriveLetter = pathValidationInfo.DriveLetter,
        FreeSpace = pathValidationInfo.FreeSpace,
        GameSize = pathValidationInfo.GameSize,
        Errors = pathValidationInfo.Errors
      };
      if (webMessage.Type == "browseDir" && !pathValidationInfo.HasErrors)
        this._gameManager.SetLaunchData(gameKey, path1);
      this._messageDispatcherProvider.Get(webMessage.WindowId).Dispatch((LauncherMessage) new GetInstallDirLauncherMessage(webMessage.Id, data));
    }

    private string GetExistingGamePathOrDefault(string gameKey)
    {
      string path = this._gameManager.GetPath(gameKey);
      if (path != null)
        return path;
      string name = this._gamesConfigProvider.GetGame(gameKey)?.Name;
      string lastGamesFolder = this._launcherStateService.GetLastGamesFolder();
      return string.IsNullOrWhiteSpace(lastGamesFolder) ? FileSystemHelper.GetDefaultGameInstallPath(name) : Path.Combine(lastGamesFolder, name ?? "game");
    }

    private class GetInstallDirInput
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }
    }

    private class BrowseDirInput
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }

      [JsonProperty("path")]
      public string Path { get; set; }

      [JsonProperty("checkFreeSpace")]
      public bool? CheckFreeSpace { get; set; }
    }
  }
}
