// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.CookieMigrator
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Properties;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Services
{
  public sealed class CookieMigrator : ICookieMigrator
  {
    private readonly ILauncherInSystemRegistrator _launcherInSystemRegistrator;
    private readonly ILauncherConfigurationProvider _configurationProvider;
    private readonly ILogger _logger;

    public CookieMigrator(
      ILauncherInSystemRegistrator launcherInSystemRegistrator,
      ILauncherConfigurationProvider configurationProvider,
      ILoggerFactory loggerFactory)
    {
      this._launcherInSystemRegistrator = launcherInSystemRegistrator ?? throw new ArgumentNullException(nameof (launcherInSystemRegistrator));
      this._configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof (configurationProvider));
      this._logger = loggerFactory.GetCurrentClassLogger<CookieMigrator>();
    }

    public void Import()
    {
      try
      {
        if (Settings.Default.CookieImported)
          return;
        RegisterLauncherSoftwareInfo launcherSoftwareInfo = this._launcherInSystemRegistrator.GetLauncherSoftwareInfo("4game2.0");
        if (string.IsNullOrWhiteSpace(launcherSoftwareInfo.TrackingId))
          return;
        ICookieManager globalCookieManager = Cef.GetGlobalCookieManager();
        string[] source = new string[3]{ "ru", "eu", "br" };
        foreach (string url in ((IEnumerable<string>) source).Select<string, string>((Func<string, string>) (region => this._configurationProvider.StartPage.Replace("{region}", region))).Distinct<string>().ToArray<string>())
        {
          string str = url.Contains("test4game") ? ".test4game.com" : ".4game.com";
          globalCookieManager.SetCookie(url, new Cookie()
          {
            Domain = str,
            Creation = DateTime.UtcNow.AddDays(-1.0),
            Expires = new DateTime?(DateTime.UtcNow.AddYears(1)),
            HttpOnly = false,
            Name = "trk",
            Value = launcherSoftwareInfo.TrackingId,
            Path = "/",
            Secure = true
          });
        }
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, "Error while migrating the cookies", Array.Empty<object>());
      }
      finally
      {
        try
        {
          Settings.Default.CookieImported = true;
          Settings.Default.Save();
        }
        catch (Exception ex)
        {
          this._logger.Log(LogLevel.Error, ex, "Error while saving settings", Array.Empty<object>());
        }
      }
    }
  }
}
