// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherStateService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Localization.Helpers;
using Innova.Launcher.Shared.Localization.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;

namespace Innova.Launcher.Core.Services
{
  public class LauncherStateService : ILauncherStateService
  {
    private readonly IOutputMessageDispatcherProvider _messageDispatcherProvider;
    private readonly IHardwareIdProvider _hardwareIdProvider;
    private readonly ILauncherIdProvider _launcherIdProvider;
    private readonly IHardwareProvider _hardwareProvider;
    private readonly ILauncherVersionProvider _launcherVersionProvider;
    private readonly IComputerNameProvider _computerNameProvider;
    private readonly ILauncherConfigurationProvider _configurationProvider;
    private readonly ILauncherInSystemRegistrator _launcherInSystemRegistrator;
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;
    private readonly ILocalizationService _localizationService;
    private readonly ILogger _logger;

    public event EventHandler<IsAutoUpdatesAvailableChangedEventArgs> IsAutoUpdatesAvailableChanged;

    public event EventHandler<RegionUpdatedEventArgs> RegionUpdated;

    public event EventHandler RegionUpdating;

    public string CurrentStartPage { get; set; }

    public AppIdentityInfo CurrentIdentity { get; private set; }

    public bool IsAutoUpdatesAvailable { get; private set; }

    public string CurrentEnvironment { get; private set; }

    public string LauncherKey { get; }

    public string LauncherName { get; }

    public string LauncherRegion { get; private set; }

    public bool IsLocalVersion { get; }

    public bool IsAppIdentityReceived { get; private set; }

    public string Culture { get; private set; }

    public LauncherStateService(
      IOutputMessageDispatcherProvider messageDispatcherProvider,
      IHardwareIdProvider hardwareIdProvider,
      ILauncherIdProvider launcherIdProvider,
      ILauncherVersionProvider launcherVersionProvider,
      IComputerNameProvider computerNameProvider,
      ILauncherConfigurationProvider configurationProvider,
      ILauncherInSystemRegistrator launcherInSystemRegistrator,
      IGamesEnvironmentProvider gamesEnvironmentProvider,
      ILoggerFactory loggerFactory,
      IHardwareProvider hardwareProvider,
      ILocalizationService localizationService)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherStateService>();
      this._messageDispatcherProvider = messageDispatcherProvider;
      this._hardwareIdProvider = hardwareIdProvider;
      this._launcherIdProvider = launcherIdProvider;
      this._launcherVersionProvider = launcherVersionProvider;
      this._computerNameProvider = computerNameProvider;
      this._configurationProvider = configurationProvider;
      this._launcherInSystemRegistrator = launcherInSystemRegistrator;
      this._gamesEnvironmentProvider = gamesEnvironmentProvider;
      this._hardwareProvider = hardwareProvider;
      this._localizationService = localizationService;
      this._gamesEnvironmentProvider.GamesEnvironmentChanged += new EventHandler(this.GamesEnvironmentChanged);
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherStateService>();
      this.CurrentEnvironment = configurationProvider.LauncherEnvironment;
      this.LauncherKey = "4game2.0";
      this.LauncherName = "4game";
      this.IsAutoUpdatesAvailable = false;
      this.IsLocalVersion = this._launcherVersionProvider.CurrentLauncherVersion.Equals("1.0.0.0");
      this.Culture = configurationProvider.Culture;
      if (string.IsNullOrWhiteSpace(configurationProvider.StartupGameKey))
        this.UpdateRegion(configurationProvider.LauncherRegion);
      else
        this.UpdateRegionByGame(configurationProvider.StartupGameKey);
      this.CurrentIdentity = this.GetBaseInfo();
    }

    public void SetLastGamesFolder(string gamesFolderPath)
    {
      try
      {
        RegisterLauncherSoftwareInfo launcherSoftwareInfo = this._launcherInSystemRegistrator.GetLauncherSoftwareInfo(this.LauncherKey);
        launcherSoftwareInfo.LastGamesInstallDirectory = gamesFolderPath;
        this._launcherInSystemRegistrator.UpdateSoftwareInfo(launcherSoftwareInfo);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Error on registy update");
      }
    }

    public string GetLastGamesFolder()
    {
      try
      {
        return this._launcherInSystemRegistrator.GetLauncherSoftwareInfo(this.LauncherKey).LastGamesInstallDirectory;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Error on registy getting");
        return (string) null;
      }
    }

    public void SetEnvironment(string newEnvironment) => this.CurrentEnvironment = newEnvironment;

    public void SendLauncherIdentity(WebMessage webMessage)
    {
      this.CurrentIdentity.Culture = LocalizationHelper.CurrentCulture.Name;
      this._messageDispatcherProvider.Get(webMessage.WindowId ?? "main").Dispatch(new LauncherMessage(webMessage.Id, "appIdentity", (object) this.CurrentIdentity));
    }

    public void SendUpdateInfo(AppUpdateInfo info)
    {
      this.CurrentIdentity.Status = LauncherStatus.OutOfDate;
      this.CurrentIdentity.Update = info;
      this._messageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("appIdentity", (object) this.CurrentIdentity));
    }

    public void SendUpdateProgress(AppUpdateProgressInfo info)
    {
      this.CurrentIdentity.Update.Info = info;
      this._messageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("appIdentity", (object) this.CurrentIdentity));
    }

    public void SendUpdatedVersion(string version)
    {
      this._launcherVersionProvider.UpdateVersion(version);
      this.CurrentIdentity.Status = LauncherStatus.Stable;
      this.CurrentIdentity.Update = (AppUpdateInfo) null;
      this.CurrentIdentity.LauncherVersion = version;
      this._messageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("appIdentity", (object) this.CurrentIdentity));
    }

    public void RequestIsAutoUpdatesAvailable() => this._messageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("getIsAutoUpdateEnabled"));

    public void UpdateSettings(LauncherSettings settings)
    {
      if (!string.IsNullOrWhiteSpace(settings.Region))
        this.UpdateRegion(settings.Region, settings.NewUrlPath);
      if (!string.IsNullOrWhiteSpace(settings.GamesEnvironment))
        this._gamesEnvironmentProvider.UpdateGamesEnvironment(settings.GamesEnvironment);
      if (string.IsNullOrWhiteSpace(settings.Culture))
        return;
      this.Culture = LocalizationHelper.ToMoreSpecific(settings.Culture);
      this._localizationService.UpdateLanguageByRegion(this.LauncherRegion, this.Culture);
    }

    public void UpdateRegionByGame(string gameKey)
    {
      string[] strArray = gameKey?.Split('-');
      if (strArray != null && strArray.Length < 2)
        return;
      this.UpdateRegion(strArray[1] == "br" ? "la" : strArray[1]);
    }

    public void UpdateIsAutoUpdatesAvailable(bool value)
    {
      if (value == this.IsAutoUpdatesAvailable)
        return;
      bool updatesAvailable = this.IsAutoUpdatesAvailable;
      this.IsAutoUpdatesAvailable = value;
      EventHandler<IsAutoUpdatesAvailableChangedEventArgs> availableChanged = this.IsAutoUpdatesAvailableChanged;
      if (availableChanged == null)
        return;
      availableChanged((object) this, new IsAutoUpdatesAvailableChangedEventArgs(updatesAvailable, value));
    }

    public void SendMainWindowSize(WebMessage webMessage, WindowSize mainWindowSize) => this._messageDispatcherProvider.Get("main").Dispatch(new LauncherMessage(webMessage.Id, "getMainWindowSize", (object) mainWindowSize));

    private AppIdentityInfo GetBaseInfo() => new AppIdentityInfo()
    {
      LauncherId = this.IsLocalVersion ? Guid.Empty.ToString() : this._launcherIdProvider.Get(),
      HardwareId = this._hardwareIdProvider.Get(),
      Hardware = this._hardwareProvider.Get(),
      ComputerName = this._computerNameProvider.Get(),
      LauncherVersion = this._launcherVersionProvider.CurrentLauncherVersion,
      Status = LauncherStatus.Stable,
      GamesEnv = this._gamesEnvironmentProvider.CurrentGamesEnvironment,
      Region = this.LauncherRegion,
      StartPage = this.CurrentStartPage,
      LauncherEnv = this.CurrentEnvironment
    };

    private void GamesEnvironmentChanged(object sender, EventArgs e) => this.CurrentIdentity = this.GetBaseInfo();

    private void UpdateRegion(string region, string newPath = null)
    {
      this.OnRegionUpdating();
      RegisterLauncherSoftwareInfo launcherSoftwareData = RegistryHelper.GetRegisterLauncherSoftwareData("Innova Co. SARL", this.LauncherKey);
      string launcherRegion = launcherSoftwareData.LauncherRegion;
      if (region == null)
        region = launcherRegion ?? "ru";
      if (region == "br")
        region = "la";
      this.CurrentStartPage = this.FormatStartPage(region);
      if (string.Equals(region, this.LauncherRegion))
        return;
      launcherSoftwareData.LauncherRegion = region;
      RegistryHelper.TryRegisterLauncherSoftwareData(launcherSoftwareData);
      this.LauncherRegion = region;
      this.CurrentIdentity = this.GetBaseInfo();
      string startPage = this.AddPathToStartPage(newPath);
      this.OnRegionUpdated(region, startPage, this.Culture);
    }

    private string FormatStartPage(string region)
    {
      if (region == "br")
        region = "la";
      return this._configurationProvider.StartPage.Replace("{region}", region).AddOrUpdateParameterToUrl("area", region).AddOrUpdateParameterToUrl("trk", this._configurationProvider.TrackingId);
    }

    private string AddPathToStartPage(string newPath)
    {
      if (string.IsNullOrWhiteSpace(newPath))
        return this.CurrentStartPage;
      string[] strArray = newPath.Split('?');
      return new UriBuilder(this.CurrentStartPage)
      {
        Path = strArray[0]
      }.Uri.ToString();
    }

    private void OnRegionUpdated(string region, string urlAfterUpdate, string culture)
    {
      EventHandler<RegionUpdatedEventArgs> regionUpdated = this.RegionUpdated;
      if (regionUpdated == null)
        return;
      regionUpdated((object) this, new RegionUpdatedEventArgs(region, urlAfterUpdate, culture));
    }

    private void OnRegionUpdating()
    {
      EventHandler regionUpdating = this.RegionUpdating;
      if (regionUpdating == null)
        return;
      regionUpdating((object) this, EventArgs.Empty);
    }

    public void AppIdentityReceived() => this.IsAppIdentityReceived = true;
  }
}
