// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.OldForgameGameFinder
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models;
using Innova.Launcher.Shared.Services.Interfaces;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Innova.Launcher.Shared.Services
{
  public class OldForgameGameFinder : IOldForgameGameFinder
  {
    private const string ForgameGamesRegKey = "SOFTWARE\\4game\\4gameservice\\Games";
    private const string ForgameGamesUninstallRegKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
    private const string PathValueName = "path";
    private const string VersionValueName = "last_version";
    private const string UninstallDisplayNameValueName = "DisplayName";
    private const string UninstallInstallationDateValueName = "InstallDate";
    private const string UninstallDisplayIconValueName = "DisplayIcon";
    private const string ForgameGamesUninstallPrefix = "4game_";
    private const string GameInstallationDateFormat = "MM/dd/yyyy";
    private readonly RegistryKey ForgameRootKey = Registry.LocalMachine;
    private readonly ILogger _logger;

    public OldForgameGameFinder(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<OldForgameGameFinder>();

    public List<InstalledGameInfo> FindExistingGames()
    {
      this._logger.Trace("Try find old forgame games in registry");
      using (RegistryKey uninstallFolder = this.ForgameRootKey.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
      {
        if (uninstallFolder == null)
          return new List<InstalledGameInfo>();
        List<\u003C\u003Ef__AnonymousType2<string, string, string, string>> list = ((IEnumerable<string>) uninstallFolder.GetSubKeyNames()).Where<string>((Func<string, bool>) (k => k.StartsWith("4game_"))).Select(e => new
        {
          Key = e.Replace("4game_", ""),
          RegistryKey = uninstallFolder.OpenSubKey(e)
        }).Select(e => new
        {
          Key = e.Key,
          Name = e.RegistryKey.GetValue("DisplayName")?.ToString(),
          InstallationDate = e.RegistryKey.GetValue("InstallDate")?.ToString(),
          DisplayIcon = e.RegistryKey.GetValue("DisplayIcon")?.ToString()
        }).ToList();
        this._logger.Trace(string.Format("Found {0} old forgame games in uninstall registry", (object) list.Count));
        List<InstalledGameInfo> source = new List<InstalledGameInfo>();
        using (RegistryKey registryKey1 = this.ForgameRootKey.OpenSubKey("SOFTWARE\\4game\\4gameservice\\Games", false))
        {
          if (registryKey1 == null)
            return source;
          foreach (var data in list)
          {
            var gameInUninstall = data;
            using (RegistryKey registryKey2 = registryKey1.OpenSubKey(gameInUninstall.Name))
            {
              if (registryKey2 == null)
              {
                this._logger.Warn("Game with name " + gameInUninstall.Name + " was in uninstall but wasn't in software");
              }
              else
              {
                InstalledGameInfo installedGameInfo = new InstalledGameInfo()
                {
                  GameKey = gameInUninstall.Key,
                  GameName = gameInUninstall.Name,
                  InstallationDate = this.ParseInstallationDate(gameInUninstall.InstallationDate),
                  IconPath = gameInUninstall.DisplayIcon,
                  Path = registryKey2.GetValue("path")?.ToString(),
                  Version = registryKey2.GetValue("last_version")?.ToString()
                };
                if (source.Any<InstalledGameInfo>((Func<InstalledGameInfo, bool>) (e => e.GameKey == gameInUninstall.Key)))
                  this._logger.Warn("Game with name " + gameInUninstall.Name + " with key " + gameInUninstall.Key + " has been allredy found.");
                else
                  source.Add(installedGameInfo);
              }
            }
          }
        }
        this._logger.Trace(string.Format("Found {0} old forgame games", (object) source.Count));
        return source;
      }
    }

    private DateTime? ParseInstallationDate(string installationDateStr)
    {
      DateTime result;
      return !DateTime.TryParseExact(installationDateStr, "MM/dd/yyyy", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? new DateTime?() : new DateTime?(result);
    }
  }
}
