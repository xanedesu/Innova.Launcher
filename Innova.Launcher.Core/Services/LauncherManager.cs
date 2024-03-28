// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherManager
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Extensions;
using Innova.Launcher.Core.Infrastructure.Common;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Session;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class LauncherManager : ILauncherManager
  {
    private readonly Random _versionsRandomizer;
    private readonly ILauncherUpdateService _updateService;
    private readonly ILauncherStateService _launcherStateService;
    private readonly ILauncherVersionProvider _launcherVersionProvider;
    private readonly ILauncherConfigurationProvider _configurationProvider;
    private readonly IEnvironmentProvider _environmentProvider;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IInternetConnectionChecker _connectionChecker;
    private readonly SessionEndedMessageHandler _sessionEndedMessageHandler;
    private readonly ILauncherTrackingService _launcherTrackingService;
    private readonly ILogger _logger;
    private readonly Semaphore _checkerLocker = new Semaphore(1, 1);
    private readonly object _versionProcessingLocker = new object();
    private bool _versionProcessing;
    private bool _updateInitiatedByUser;
    private Task _worker;

    public LauncherManager(
      ILauncherUpdateServiceFactory updateServiceFactory,
      ILauncherStateService launcherStateService,
      ILauncherVersionProvider launcherVersionProvider,
      ILauncherConfigurationProvider configurationProvider,
      IEnvironmentProvider environmentProvider,
      ILifetimeManager lifetimeManager,
      ILoggerFactory loggerFactory,
      IInternetConnectionChecker connectionChecker,
      SessionEndedMessageHandler sessionEndedMessageHandler,
      ILauncherTrackingService launcherTrackingService)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherManager>();
      this._updateService = updateServiceFactory.GetLauncherUpdateService();
      this._launcherStateService = launcherStateService;
      this._launcherVersionProvider = launcherVersionProvider;
      this._configurationProvider = configurationProvider;
      this._environmentProvider = environmentProvider;
      this._lifetimeManager = lifetimeManager;
      this._connectionChecker = connectionChecker;
      this._sessionEndedMessageHandler = sessionEndedMessageHandler;
      this._launcherTrackingService = launcherTrackingService;
      this._versionsRandomizer = new Random((int) (DateTime.Now.Ticks % 1117L));
      this._updateService.UpdateCompleted += new EventHandler(this.NotifyAboutUpdateCompleted);
      this._updateService.UpdateProgressChanged += new EventHandler<AppUpdateProgressEventAgrs>(this.NotifyAboutUpdateProgress);
      this._updateService.UpdateError += new EventHandler<AppUpdateErrorEventArgs>(this.HandleUpdateError);
      this._updateService.UpdateCanceled += new EventHandler(this.HandleUpdateCanceled);
      this._launcherStateService.IsAutoUpdatesAvailableChanged += new EventHandler<IsAutoUpdatesAvailableChangedEventArgs>(this.IsAutoUpdateAvailableChanged);
      SystemEvents.SessionEnded += new SessionEndedEventHandler(this.SessionEnded);
    }

    private void SessionEnded(object sender, SessionEndedEventArgs e) => this.ProcessSessionEnd();

    private void ProcessSessionEnd() => this._sessionEndedMessageHandler.SendSessionEndedMessage();

    public void StartPollLauncherUpdates() => this._worker = Task.Factory.StartNew((Action) (() =>
    {
      try
      {
        while (this._lifetimeManager.IsAlive)
        {
          TaskHelper.Delay(TimeSpan.FromMilliseconds((double) this._configurationProvider.LauncherVersionPollingInterval), this._lifetimeManager.CancellationToken);
          this.CheckAvailableUpdatesAsync().Wait(this._lifetimeManager.CancellationToken);
        }
      }
      catch (OperationCanceledException ex)
      {
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, nameof (StartPollLauncherUpdates));
      }
    }), this._lifetimeManager.CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    public void ChangeEnvironment(string environment)
    {
      this._launcherStateService.SetEnvironment(environment);
      this.CheckAvailableUpdatesAsync();
    }

    public Task CheckAvailableUpdatesAsync()
    {
      this._checkerLocker.WaitOne();
      try
      {
        return this._connectionChecker.DoWhenConnectionExistsAsync((Action) (() =>
        {
          try
          {
            this.ProcessNewAvailableVersions(this._environmentProvider.GetAvailableVersions(this._launcherStateService.CurrentEnvironment));
          }
          finally
          {
            this._checkerLocker.Release();
          }
        }));
      }
      catch
      {
        this._checkerLocker.Release();
        throw;
      }
    }

    public void ProcessNewAvailableVersions(
      LauncherAvailableVersionsInfo availableVersionsInfo)
    {
      if (this._launcherStateService.IsLocalVersion)
        return;
      LauncherVersionInfo update = this.SelectVersionToUpdate(availableVersionsInfo);
      if (update == null)
        return;
      this._logger.Trace("The new version " + update.Version + " is ready to update. Current version " + this._launcherVersionProvider.CurrentLauncherVersion + "!");
      LauncherVersionInfo version1 = new LauncherVersionInfo(this._launcherVersionProvider.CurrentLauncherVersion);
      int num1 = version1.IsGreaterThen(update) ? 1 : 0;
      LauncherVersionReleaseInfo versionReleaseInfo = this._environmentProvider.GetVersionReleaseInfo(this._launcherStateService.CurrentEnvironment, update.Version);
      long valueOrDefault = versionReleaseInfo.SizeKilobytes.GetValueOrDefault();
      string minRequiredVersion = versionReleaseInfo.MinRequiredVersion;
      int num2 = minRequiredVersion == null ? (false ? 1 : 0) : (version1.IsLessThen(minRequiredVersion) ? 1 : 0);
      if ((num1 | num2) != 0)
      {
        this.NotifyAboutCriticalUpdateAvailable(update, valueOrDefault);
      }
      else
      {
        if (!version1.IsLessThen(update))
          return;
        this.NotifyAboutUpdateAvailable(update, valueOrDefault);
      }
    }

    public LauncherVersionInfo SelectVersionToUpdate(
      LauncherAvailableVersionsInfo availableVersionsInfo)
    {
      string currentEnvironment = this._launcherStateService.CurrentEnvironment;
      if (!string.Equals(availableVersionsInfo.Environment, currentEnvironment))
        return (LauncherVersionInfo) null;
      double? nullable1 = availableVersionsInfo.Versions.Where<LauncherVersionInfo>((Func<LauncherVersionInfo, bool>) (e => e.Percent.HasValue)).Select<LauncherVersionInfo, double?>((Func<LauncherVersionInfo, double?>) (e => e.Percent)).Sum();
      double maxValue = nullable1.Value;
      if (maxValue < 100.0)
        maxValue = 100.0;
      List<LauncherVersionInfo> list1 = availableVersionsInfo.Versions.Where<LauncherVersionInfo>((Func<LauncherVersionInfo, bool>) (e => !e.Percent.HasValue)).ToList<LauncherVersionInfo>();
      double? nullable2;
      if (list1.Count <= 0)
      {
        nullable2 = new double?(0.0);
      }
      else
      {
        double num = maxValue;
        double? nullable3 = nullable1;
        double? nullable4 = nullable3.HasValue ? new double?(num - nullable3.GetValueOrDefault()) : new double?();
        double count = (double) list1.Count;
        if (!nullable4.HasValue)
        {
          nullable3 = new double?();
          nullable2 = nullable3;
        }
        else
          nullable2 = new double?(nullable4.GetValueOrDefault() / count);
      }
      double? nullable5 = nullable2;
      foreach (LauncherVersionInfo launcherVersionInfo in list1)
        launcherVersionInfo.Percent = nullable5;
      string currentVersion = this._launcherVersionProvider.CurrentLauncherVersion;
      if (availableVersionsInfo.Versions.Any<LauncherVersionInfo>((Func<LauncherVersionInfo, bool>) (e =>
      {
        double? percent = e.Percent;
        double num = 0.0;
        return percent.GetValueOrDefault() > num & percent.HasValue && VersionComparer.Default.Equals(e.Version, currentVersion);
      })))
        return (LauncherVersionInfo) null;
      int num1 = this._versionsRandomizer.Next((int) maxValue);
      List<LauncherVersionInfo> list2 = availableVersionsInfo.Versions.OrderBy<LauncherVersionInfo, double?>((Func<LauncherVersionInfo, double?>) (e => e.Percent)).ToList<LauncherVersionInfo>();
      LauncherVersionInfo update = (LauncherVersionInfo) null;
      double num2 = 0.0;
      foreach (LauncherVersionInfo launcherVersionInfo in list2)
      {
        double? percent = launcherVersionInfo.Percent;
        if (percent.HasValue)
        {
          double num3 = num2;
          percent = launcherVersionInfo.Percent;
          double num4 = percent.Value;
          double num5 = num3 + num4;
          if ((double) num1 >= num2 && (double) num1 < num5)
          {
            update = launcherVersionInfo;
            break;
          }
          num2 = num5;
        }
      }
      return update;
    }

    public void StartLauncherUpdate(string version, bool byUser = true)
    {
      if (!this.CanStartProcessing())
        return;
      this._updateInitiatedByUser = byUser;
      this._launcherStateService.SendUpdateInfo(new AppUpdateInfo()
      {
        IsCritical = this._launcherStateService.CurrentIdentity.Update.IsCritical,
        Status = LauncherUpdateStatus.InstallProgress,
        Version = version ?? this._launcherStateService.CurrentIdentity.Update.Version,
        Info = this._launcherStateService.CurrentIdentity.Update.Info
      });
      this._updateService.Start(version);
      this._launcherTrackingService.SendLauncherUpdateStarted(this._launcherStateService.CurrentIdentity.Update.IsCritical);
    }

    public void CancelLauncherUpdate()
    {
      this._updateService.Stop();
      this._launcherStateService.SendUpdatedVersion(this._launcherVersionProvider.CurrentLauncherVersion);
    }

    private void IsAutoUpdateAvailableChanged(
      object sender,
      IsAutoUpdatesAvailableChangedEventArgs e)
    {
      if (e.OldValue || !e.NewValue)
        return;
      this.CheckAvailableUpdatesAsync();
    }

    private bool CanStartProcessing()
    {
      lock (this._versionProcessingLocker)
      {
        if (this._versionProcessing)
          return false;
        this._versionProcessing = true;
      }
      return true;
    }

    private void ReleaseProcessingBlock()
    {
      lock (this._versionProcessingLocker)
        this._versionProcessing = false;
    }

    private void NotifyAboutCriticalUpdateAvailable(LauncherVersionInfo versionInfo, long totalSize) => this._launcherStateService.SendUpdateInfo(new AppUpdateInfo()
    {
      IsCritical = true,
      Status = LauncherUpdateStatus.ReadyToInstall,
      Version = versionInfo.Version,
      Info = new AppUpdateProgressInfo()
      {
        Downloaded = 0L,
        Size = totalSize
      }
    });

    private void NotifyAboutUpdateAvailable(LauncherVersionInfo versionInfo, long totalSize)
    {
      this._launcherStateService.SendUpdateInfo(new AppUpdateInfo()
      {
        IsCritical = false,
        Status = LauncherUpdateStatus.ReadyToInstall,
        Version = versionInfo.Version,
        Info = new AppUpdateProgressInfo()
        {
          Downloaded = 0L,
          Size = totalSize
        }
      });
      if (this._launcherStateService.IsAutoUpdatesAvailable)
        this.StartLauncherUpdate(versionInfo.Version, false);
      else
        this._launcherStateService.RequestIsAutoUpdatesAvailable();
    }

    private void NotifyAboutUpdateProgress(
      object sender,
      AppUpdateProgressEventAgrs appUpdateProgressEventAgrs)
    {
      this._launcherStateService.SendUpdateProgress(appUpdateProgressEventAgrs.ProgressInfo);
    }

    private void NotifyAboutUpdateCompleted(object sender, EventArgs eventArgs)
    {
      this.ReleaseProcessingBlock();
      this._launcherTrackingService.SendLauncherUpdated(this._launcherStateService.CurrentIdentity.Update.IsCritical);
      if (this._launcherStateService.CurrentIdentity.Update.IsCritical || this._updateInitiatedByUser)
        this._updateService.RestartApplication();
      this._launcherStateService.SendUpdatedVersion(this._launcherStateService.CurrentIdentity.Update.Version);
    }

    private void HandleUpdateCanceled(object sender, EventArgs eventArgs)
    {
      this.ReleaseProcessingBlock();
      this._launcherStateService.SendUpdateInfo(new AppUpdateInfo()
      {
        IsCritical = this._launcherStateService.CurrentIdentity.Update.IsCritical,
        Status = LauncherUpdateStatus.ReadyToInstall,
        Version = this._launcherStateService.CurrentIdentity.Update.Version
      });
    }

    private void HandleUpdateError(object sender, AppUpdateErrorEventArgs appUpdateErrorEventArgs)
    {
      this.ReleaseProcessingBlock();
      this._logger.Error(appUpdateErrorEventArgs.Error, "Error while launcher updating");
      this.CancelLauncherUpdate();
    }

    public void Dispose()
    {
    }
  }
}
