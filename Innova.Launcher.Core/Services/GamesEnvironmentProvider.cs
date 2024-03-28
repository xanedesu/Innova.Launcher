// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GamesEnvironmentProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Exceptions.GamesEnvironment;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.IO;

namespace Innova.Launcher.Core.Services
{
  public class GamesEnvironmentProvider : IGamesEnvironmentProvider
  {
    private readonly string GamesEnvironmentsFileUrl = Path.Combine("http://cdn.inn.ru/new4game/launcher/", "games-environments.json");
    private readonly int _retryCount = 3;
    private readonly ILogger _logger;

    public event EventHandler GamesEnvironmentChanged;

    public string CurrentGamesConfigUrl { get; private set; }

    public string CurrentSingleGamesConfigUrl { get; private set; }

    public string CurrentGamesEnvironment { get; private set; }

    public GamesEnvironmentProvider(
      ILoggerFactory loggerFactory,
      ILauncherConfigurationProvider launcherConfigurationProvider)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GamesEnvironmentProvider>();
      this.CurrentGamesConfigUrl = launcherConfigurationProvider.GamesConfigUrl;
      this.CurrentSingleGamesConfigUrl = launcherConfigurationProvider.SingleGamesConfigUrl;
      this.RefreshCurrentGamesEnvironment();
    }

    public void UpdateGamesEnvironment(string environment)
    {
      if (string.IsNullOrWhiteSpace(environment) || !(this.CurrentGamesEnvironment != environment))
        return;
      UriBuilder uriBuilder = new UriBuilder(this.CurrentGamesConfigUrl);
      string directoryName = Path.GetDirectoryName(uriBuilder.Path);
      string extension = Path.GetExtension(uriBuilder.Path);
      string path2 = Path.GetFileNameWithoutExtension(uriBuilder.Path).Replace("config-" + this.CurrentGamesEnvironment, "config-" + environment) + extension;
      string str = Path.Combine(directoryName, path2);
      uriBuilder.Path = str;
      this.UpdateGamesConfigUrl(uriBuilder.ToString());
    }

    public void UpdateGamesConfigUrl(string newConfigUrl)
    {
      if (string.Equals(this.CurrentGamesConfigUrl, newConfigUrl))
        return;
      this.CurrentGamesConfigUrl = newConfigUrl;
      this.RefreshCurrentGamesEnvironment();
      this.OnGamesEnvironmentChanged();
    }

    public void UpdateSingleGamesConfigUrl(string newConfigUrl)
    {
      if (string.Equals(this.CurrentSingleGamesConfigUrl, newConfigUrl))
        return;
      this.CurrentSingleGamesConfigUrl = newConfigUrl;
      this.OnGamesEnvironmentChanged();
    }

    public GamesEnvironments GetAvailableGamesEnvironments()
    {
      try
      {
        return WebHelper.TryToLoadJsonData<GamesEnvironments>(this.GamesEnvironmentsFileUrl, this._retryCount);
      }
      catch (Exception ex)
      {
        LauncherGetgamesEnvironmentsInfoException environmentsInfoException = new LauncherGetgamesEnvironmentsInfoException(ex);
        this._logger.Error((Exception) environmentsInfoException, string.Format("Error while getting a games environments info from {0}. Retry count {1}.", (object) this.GamesEnvironmentsFileUrl, (object) this._retryCount));
        throw environmentsInfoException;
      }
    }

    private void RefreshCurrentGamesEnvironment() => this.CurrentGamesEnvironment = Path.GetFileNameWithoutExtension(this.CurrentGamesConfigUrl)?.Replace("config-", "") ?? throw new ArgumentOutOfRangeException("CurrentGamesConfigUrl", "config file must be in config-{env}.* format");

    private void OnGamesEnvironmentChanged()
    {
      EventHandler environmentChanged = this.GamesEnvironmentChanged;
      if (environmentChanged == null)
        return;
      environmentChanged((object) this, EventArgs.Empty);
    }
  }
}
