// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameUninstaller
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public sealed class GameUninstaller : IGameUninstaller
  {
    private readonly IExternalGameInstaller _externalGameInstaller;
    private readonly IGameInSystemRegistrator _gameInSystemRegister;
    private readonly ILogger _logger;

    public GameUninstaller(
      ILoggerFactory loggerFactory,
      IExternalGameInstaller externalGameInstaller,
      IGameInSystemRegistrator gameInSystemRegister)
    {
      this._externalGameInstaller = externalGameInstaller ?? throw new ArgumentNullException(nameof (externalGameInstaller));
      this._gameInSystemRegister = gameInSystemRegister ?? throw new ArgumentNullException(nameof (gameInSystemRegister));
      this._logger = loggerFactory?.GetCurrentClassLogger<GameUninstaller>() ?? throw new ArgumentNullException(nameof (loggerFactory));
    }

    public async Task Uninstall(
      string gameDirectory,
      string gameUrl,
      string gameKey,
      string gameName,
      string launcherKey,
      string updaterName,
      string shortcutTitle)
    {
      this._logger.Trace("Uninstall game in directory " + gameDirectory);
      await this._externalGameInstaller.UninstallAsync(gameDirectory, gameUrl, updaterName);
      this.DeleteDirectories(gameDirectory);
      this._gameInSystemRegister.Unregister(new GameUnregistrationData()
      {
        Key = gameKey,
        InstallationPath = gameDirectory,
        Name = gameName,
        ShotcutTitle = shortcutTitle ?? gameName,
        LauncherKey = launcherKey
      });
    }

    private void DeleteDirectories(string gameDirectory)
    {
      try
      {
        if (!Directory.Exists(gameDirectory))
          return;
        FileSystemHelper.RemoveEmptyDirectories(gameDirectory);
        string[] strArray1 = new string[2]
        {
          "Frost",
          "EnvSet"
        };
        string[] strArray2 = new string[1]{ "BC.log" };
        foreach (string path2 in strArray1)
          FileSystemHelper.DeleteDirectoryIfExists(Path.Combine(gameDirectory, path2), true);
        foreach (string path2 in strArray2)
          FileSystemHelper.DeleteFileIfExists(Path.Combine(gameDirectory, path2));
        FileSystemHelper.RemoveDirectoryIfEmpty(gameDirectory);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Error while delete empty folders in " + gameDirectory);
      }
    }
  }
}
