// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.GameInWindowsRegistrator
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Innova.Launcher.Shared.Services
{
  public class GameInWindowsRegistrator : IGameInSystemRegistrator
  {
    public static readonly string GameIconName = "4game_icon.ico";
    private readonly ILogger _logger;

    public GameInWindowsRegistrator(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<GameInWindowsRegistrator>();

    public void Register(GameRegistrationData registrationData)
    {
      string iconPath = this.GetIconPath(registrationData.InstallationPath);
      registrationData.IconPath = iconPath;
      bool gameWasInstalled = this.IsRegistered(registrationData.LauncherKey, registrationData.Key);
      this.RegisterInRegistry(registrationData);
      this.CreateDesktopShortcut(registrationData, gameWasInstalled);
    }

    public void Unregister(GameUnregistrationData unregistrationData)
    {
      try
      {
        RegistryHelper.DeleteUninstallInfo(this.GetUninstallGameRegistryKey(unregistrationData.LauncherKey, unregistrationData.Key));
        RegistryHelper.DeleteGameSoftwareData(new DeleteGameSoftwareInfo()
        {
          Publisher = "Innova Co. SARL",
          LauncherKey = unregistrationData.LauncherKey,
          GameName = unregistrationData.Name
        });
        FileSystemHelper.DeleteFileIfExists(this.GetIconPath(unregistrationData.InstallationPath));
        ShortcutHelper.DeleteDesktopShortcut(unregistrationData.ShotcutTitle ?? unregistrationData.Name);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Problem with unregistering the game " + unregistrationData.Key + " in " + unregistrationData.InstallationPath);
        throw;
      }
    }

    public bool IsRegistered(string launcherKey, string gameRegistrationKey) => RegistryHelper.UninstallInfoExists(this.GetUninstallGameRegistryKey(launcherKey, gameRegistrationKey));

    public List<InstalledGameInfo> GetInstalledGames(string launcherKey)
    {
      string gameKeysPrefix = launcherKey + "_";
      return RegistryHelper.GetUninstallInfoKeys(gameKeysPrefix).Select(k =>
      {
        RegistryUninstallInfo uninstallInfo = RegistryHelper.GetUninstallInfo(k);
        RegisterGameSoftwareInfo gameSoftwareData = uninstallInfo != null ? RegistryHelper.GetGameSoftwareData("Innova Co. SARL", launcherKey, uninstallInfo.Name) : (RegisterGameSoftwareInfo) null;
        return new
        {
          Key = k,
          Info = uninstallInfo,
          SoftwareInfo = gameSoftwareData
        };
      }).Where(e => e.Info != null && e.SoftwareInfo != null).Select(info => new InstalledGameInfo()
      {
        GameKey = info.Key.Replace(gameKeysPrefix, ""),
        GameName = info.Info.Name,
        Path = info.Info.InstallationPath,
        Version = info.Info.Version,
        InstallationDate = new DateTime?(info.Info.InstallationDate),
        IconPath = info.Info.IconPath
      }).ToList<InstalledGameInfo>();
    }

    private void CreateDesktopShortcut(GameRegistrationData registrationData, bool gameWasInstalled)
    {
      try
      {
        this._logger.Trace("Copy icon to game folder (" + registrationData.Key + "," + registrationData.InstallationPath + ")");
        File.WriteAllBytes(registrationData.IconPath, registrationData.IconData);
        if (gameWasInstalled)
          return;
        this._logger.Trace("Create shortcut for game (" + registrationData.Key + "," + registrationData.Name + ")");
        ShortcutHelper.CreateOrReplaceDesktopShortcut(registrationData.ShotcutTitle ?? registrationData.Name, registrationData.IconPath, registrationData.RunnerPath, registrationData.RunnerArgs);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Problem with creation shortcut for game (" + registrationData.Key + "," + registrationData.Name + ")");
        throw;
      }
    }

    private void RegisterInRegistry(GameRegistrationData registrationData)
    {
      this._logger.Trace("Registrate in registry game (" + registrationData.Key + "," + registrationData.Name + ")");
      string uninstallGameRegistryKey = this.GetUninstallGameRegistryKey(registrationData.LauncherKey, registrationData.Key);
      long? nullable = new long?();
      long result;
      if (long.TryParse(registrationData.Size, out result))
        nullable = new long?(result);
      if (!RegistryHelper.TryRegisterUninstallInfo(uninstallGameRegistryKey, new RegistryUninstallInfo()
      {
        Version = registrationData.Version,
        SizeBytes = nullable,
        InstallationPath = registrationData.InstallationPath,
        IconPath = registrationData.IconPath,
        Name = registrationData.Name,
        InstallationDate = registrationData.InstallationDate,
        UninstallCommand = registrationData.UninstallCommand,
        Publisher = "Innova Co. SARL"
      }))
      {
        this._logger.Error("Can't register game " + uninstallGameRegistryKey + " in unninstall registry");
        throw new Exception("Can't register game " + uninstallGameRegistryKey + " in unninstall registry");
      }
      if (!RegistryHelper.TryRegisterGameSoftwareData(new RegisterGameSoftwareInfo()
      {
        Publisher = "Innova Co. SARL",
        LauncherKey = registrationData.LauncherKey,
        Version = registrationData.Version,
        InstallationPath = registrationData.InstallationPath,
        InstallationDate = new DateTime?(registrationData.InstallationDate),
        GameName = registrationData.Name
      }))
      {
        this._logger.Error("Can't register game " + uninstallGameRegistryKey + " in software registry");
        RegistryHelper.DeleteUninstallInfo(uninstallGameRegistryKey);
        throw new Exception("Can't register game " + uninstallGameRegistryKey + " in software registry");
      }
    }

    private string GetIconPath(string gamePath) => Path.Combine(gamePath, GameInWindowsRegistrator.GameIconName);

    private string GetUninstallGameRegistryKey(string launcherKey, string gameKey) => launcherKey + "_" + gameKey;
  }
}
