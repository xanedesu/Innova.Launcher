// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherInWindowsRegistrator
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;

namespace Innova.Launcher.Shared.Services
{
  public class LauncherInWindowsRegistrator : ILauncherInSystemRegistrator
  {
    private readonly ILogger _logger;

    public LauncherInWindowsRegistrator(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<LauncherInWindowsRegistrator>();

    public void Register(LauncherRegistrationData registrationData, bool createShortcut)
    {
      this.RegisterInRegistry(registrationData);
      this.AddLauncherUrlScheme(registrationData);
      if (!createShortcut)
        return;
      this.CreateShortcuts(registrationData);
    }

    public void Unregister(LauncherUnregistrationData unregistrationData)
    {
      try
      {
        RegistryHelper.DeleteUninstallInfo(unregistrationData.Key);
        ShortcutHelper.DeleteDesktopShortcut(unregistrationData.Name);
        ShortcutHelper.DeleteStartMenuShortcut("Innova Co. SARL", unregistrationData.Name);
        RegistryHelper.DeleteUrlScheme(unregistrationData.UrlSchemeName);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Problem with unregistering launcher " + unregistrationData.Key);
        throw;
      }
    }

    public bool IsRegistered(string launcherKey) => RegistryHelper.UninstallInfoExists(launcherKey);

    public void SetLauncherIdIfNotExist(string launcherKey, string launcherId) => RegistryHelper.TrySetLauncherIdIfNotExist("Innova Co. SARL", launcherKey, launcherId);

    public void Update(LauncherRegistrationDataUpdate updateData)
    {
      this._logger.Trace("Update registry launcher info (" + updateData.Key + "," + updateData.Name + ")");
      if (!RegistryHelper.TryUpdateUninstallInfo(updateData.Key, new RegistryUninstallInfo()
      {
        Version = updateData.Version,
        SizeBytes = new long?(updateData.SizeBytes),
        Name = updateData.Name,
        InstallationDate = updateData.UpdateDate,
        UninstallCommand = updateData.UninstallCommand,
        Publisher = "Innova Co. SARL"
      }))
      {
        this._logger.Error("Can't update launcher uninstall info " + updateData.Key + " in uninstall registry");
        throw new InvalidOperationException("Can't update launcher uninstall info  " + updateData.Key + " in uninstall registry");
      }
      this.UpdateSoftwareInfo(new RegisterLauncherSoftwareInfo()
      {
        Publisher = "Innova Co. SARL",
        LauncherKey = updateData.Key,
        Version = updateData.Version,
        LastUpdateDate = new DateTime?(updateData.UpdateDate)
      });
    }

    public string GetInstallPath(string launcherKey) => RegistryHelper.GetUninstallInfo(launcherKey).InstallationPath;

    public RegisterLauncherSoftwareInfo GetLauncherSoftwareInfo(string launcherKey) => RegistryHelper.GetRegisterLauncherSoftwareData("Innova Co. SARL", launcherKey);

    public void UpdateSoftwareInfo(RegisterLauncherSoftwareInfo updateData)
    {
      if (!RegistryHelper.TryRegisterLauncherSoftwareData(updateData))
      {
        this._logger.Error("Can't update launcher info " + updateData.LauncherKey + " in software registry");
        throw new InvalidOperationException("Can't update launcher info " + updateData.LauncherKey + " in software registry");
      }
    }

    private void CreateShortcuts(LauncherRegistrationData registrationData)
    {
      this._logger.Trace("Create shortcuts for launcher (" + registrationData.Key + "," + registrationData.Name + ")");
      try
      {
        ShortcutHelper.CreateOrReplaceDesktopShortcut(registrationData.Name, registrationData.IconPath, registrationData.IconTarget);
        ShortcutHelper.CreateOrReplaceStartMenuShortcut("Innova Co. SARL", registrationData.Name, registrationData.IconPath, registrationData.IconTarget);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Problem with creation shortcuts for launcher (" + registrationData.Key + "," + registrationData.Name + ")");
        throw;
      }
    }

    private void RegisterInRegistry(LauncherRegistrationData registrationData)
    {
      this._logger.Trace("Registrate in registry launcher (" + registrationData.Key + "," + registrationData.Name + ")");
      bool flag = !this.IsRegistered(registrationData.Key);
      if (!RegistryHelper.TryRegisterUninstallInfo(registrationData.Key, new RegistryUninstallInfo()
      {
        Version = registrationData.Version,
        SizeBytes = new long?(registrationData.SizeBytes),
        InstallationPath = registrationData.InstallationPath,
        IconPath = registrationData.IconPath,
        Name = registrationData.Name,
        InstallationDate = registrationData.InstallationDate,
        UninstallCommand = registrationData.UninstallCommand,
        Publisher = "Innova Co. SARL"
      }))
      {
        this._logger.Error("Can't register launcher " + registrationData.Key + " in unninstall registry");
        throw new InvalidOperationException("Can't register launcher " + registrationData.Key + " in unninstall registry");
      }
      if (!RegistryHelper.TryRegisterLauncherSoftwareData(new RegisterLauncherSoftwareInfo()
      {
        Publisher = "Innova Co. SARL",
        LauncherKey = registrationData.Key,
        Version = registrationData.Version,
        InstallationPath = registrationData.InstallationPath,
        InstallationDate = new DateTime?(registrationData.InstallationDate),
        OldGamesTaken = flag ? new bool?(false) : new bool?(),
        LastGamesInstallDirectory = registrationData.LastGamesInstallPath,
        LauncherRegion = registrationData.LauncherRegion
      }))
      {
        this._logger.Error("Can't register launcher " + registrationData.Key + " in software registry");
        RegistryHelper.DeleteUninstallInfo(registrationData.Key);
        throw new InvalidOperationException("Can't register launcher " + registrationData.Key + " in software registry");
      }
    }

    private void AddLauncherUrlScheme(LauncherRegistrationData registrationData)
    {
      if (!RegistryHelper.TryRegisterUrlScheme(registrationData.UrlSchemeName, registrationData.RunnerPath, this._logger))
      {
        this._logger.Error("Can't register launcher urlScheme");
        throw new InvalidOperationException("Can't register launcher urlScheme");
      }
    }
  }
}
