// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Common.EnvironmentProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Exceptions.Launcher;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;

namespace Innova.Launcher.Core.Infrastructure.Common
{
  public class EnvironmentProvider : IEnvironmentProvider
  {
    private readonly IEnvironmentConfigurationProvider _configurationProvider;
    private readonly ILogger _logger;
    private readonly int _retryCount = 3;

    public EnvironmentProvider(
      IEnvironmentConfigurationProvider configurationProvider,
      ILoggerFactory loggerFactory)
    {
      this._configurationProvider = configurationProvider;
      this._logger = loggerFactory.GetCurrentClassLogger<EnvironmentProvider>();
    }

    public LauncherAvailableVersionsInfo GetAvailableVersions(string environment)
    {
      string versionsFilePath = this._configurationProvider.GetFullVersionsFilePath(environment);
      try
      {
        return WebHelper.TryToLoadJsonData<LauncherAvailableVersionsInfo>(versionsFilePath, this._retryCount);
      }
      catch (Exception ex)
      {
        LauncherGetVersionInfoException versionInfoException = new LauncherGetVersionInfoException(ex);
        this._logger.Error((Exception) versionInfoException, string.Format("Error while getting an updates info from {0}. Retry count {1}.", (object) versionsFilePath, (object) this._retryCount));
        throw versionInfoException;
      }
    }

    public string GetVersionHostingPath(string version) => this._configurationProvider.GetVersionHostingPath(version);

    public LauncherVersionReleaseInfo GetVersionReleaseInfo(string environment, string version)
    {
      string releaseInfoFilePath = this._configurationProvider.GetVersionReleaseInfoFilePath(version);
      try
      {
        return WebHelper.TryToLoadJsonData<LauncherVersionReleaseInfo>(releaseInfoFilePath, this._retryCount);
      }
      catch (LauncherGetVersionInfoException ex)
      {
        LauncherGetVersionInfoException versionInfoException = new LauncherGetVersionInfoException((Exception) ex);
        this._logger.Error((Exception) versionInfoException, string.Format("Error while getting an version release info from {0}. Retry count {1}.", (object) releaseInfoFilePath, (object) this._retryCount));
        throw versionInfoException;
      }
    }
  }
}
