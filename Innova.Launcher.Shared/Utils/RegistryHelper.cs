// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.RegistryHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Exceptions;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Innova.Launcher.Shared.Utils
{
  public static class RegistryHelper
  {
    private static readonly RegistryKey _defaultRootKey = Registry.LocalMachine;
    private static readonly char[] _quotes = new char[2]
    {
      '\'',
      '"'
    };
    private static readonly string _uninstallBaseFolder = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

    public static bool TryRegisterUninstallInfo(string appKey, RegistryUninstallInfo info)
    {
      try
      {
        string uninstallRegistryFolder = RegistryHelper.GetUninstallRegistryFolder(appKey);
        using (RegistryKey registryKey1 = RegistryHelper._defaultRootKey.OpenSubKey(uninstallRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(uninstallRegistryFolder))
        {
          if (registryKey1 == null)
            return false;
          registryKey1.SetValue(RegistryConstants.Uninstall.DisplayName, (object) (info.Name ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.ApplicationVersion, (object) (info.Version ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.Publisher, (object) info.Publisher);
          registryKey1.SetValue(RegistryConstants.Uninstall.ShortcutName, (object) (info.Name ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.InstallLocation, (object) (info.InstallationPath ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.DisplayIcon, (object) (info.IconPath ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.UninstallString, (object) (info.UninstallCommand ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.DisplayVersion, (object) (info.Version ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Uninstall.InstallationDate, (object) info.InstallationDate.ToString(RegistryConstants.Uninstall.RegistryDateFormat));
          long? sizeBytes = info.SizeBytes;
          if (sizeBytes.HasValue)
          {
            RegistryKey registryKey2 = registryKey1;
            string estimatedSize = RegistryConstants.Uninstall.EstimatedSize;
            sizeBytes = info.SizeBytes;
            long num = 1024;
            // ISSUE: variable of a boxed type
            __Boxed<int> local = (ValueType) (int) (sizeBytes.HasValue ? new long?(sizeBytes.GetValueOrDefault() / num) : new long?()).Value;
            registryKey2.SetValue(estimatedSize, (object) local, RegistryValueKind.DWord);
          }
          registryKey1.SetValue(RegistryConstants.Uninstall.NoModify, (object) 1, RegistryValueKind.DWord);
          registryKey1.SetValue(RegistryConstants.Uninstall.NoRepair, (object) 1, RegistryValueKind.DWord);
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static bool TryUpdateUninstallInfo(string appKey, RegistryUninstallInfo info)
    {
      try
      {
        string uninstallRegistryFolder = RegistryHelper.GetUninstallRegistryFolder(appKey);
        using (RegistryKey registryKey1 = RegistryHelper._defaultRootKey.OpenSubKey(uninstallRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(uninstallRegistryFolder))
        {
          if (registryKey1 == null)
            return false;
          if (!string.IsNullOrWhiteSpace(info.Name))
          {
            registryKey1.SetValue(RegistryConstants.Uninstall.DisplayName, (object) (info.Name ?? string.Empty));
            registryKey1.SetValue(RegistryConstants.Uninstall.ShortcutName, (object) (info.Name ?? string.Empty));
          }
          if (!string.IsNullOrWhiteSpace(info.Version))
          {
            registryKey1.SetValue(RegistryConstants.Uninstall.ApplicationVersion, (object) (info.Version ?? string.Empty));
            registryKey1.SetValue(RegistryConstants.Uninstall.DisplayVersion, (object) (info.Version ?? string.Empty));
          }
          if (!string.IsNullOrWhiteSpace(info.Publisher))
            registryKey1.SetValue(RegistryConstants.Uninstall.Publisher, (object) info.Publisher);
          if (!string.IsNullOrWhiteSpace(info.IconPath))
            registryKey1.SetValue(RegistryConstants.Uninstall.DisplayIcon, (object) info.IconPath);
          if (!string.IsNullOrWhiteSpace(info.UninstallCommand))
            registryKey1.SetValue(RegistryConstants.Uninstall.UninstallString, (object) info.UninstallCommand);
          registryKey1.SetValue(RegistryConstants.Uninstall.LastUpdateDate, (object) info.InstallationDate.ToString(RegistryConstants.Uninstall.RegistryDateFormat));
          if (info.SizeBytes.HasValue)
          {
            RegistryKey registryKey2 = registryKey1;
            string estimatedSize = RegistryConstants.Uninstall.EstimatedSize;
            long? sizeBytes = info.SizeBytes;
            long num = 1024;
            // ISSUE: variable of a boxed type
            __Boxed<int> local = (ValueType) (int) (sizeBytes.HasValue ? new long?(sizeBytes.GetValueOrDefault() / num) : new long?()).Value;
            registryKey2.SetValue(estimatedSize, (object) local, RegistryValueKind.DWord);
          }
          registryKey1.SetValue(RegistryConstants.Uninstall.NoModify, (object) 1, RegistryValueKind.DWord);
          registryKey1.SetValue(RegistryConstants.Uninstall.NoRepair, (object) 1, RegistryValueKind.DWord);
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static RegistryUninstallInfo GetUninstallInfo(string key)
    {
      string uninstallRegistryFolder = RegistryHelper.GetUninstallRegistryFolder(key);
      using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(uninstallRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(uninstallRegistryFolder))
      {
        if (registryKey == null)
          throw new InvalidOperationException("Can't get uninstall info node " + uninstallRegistryFolder);
        DateTime result1;
        DateTime.TryParseExact(registryKey.GetValue(RegistryConstants.Uninstall.InstallationDate)?.ToString(), RegistryConstants.Uninstall.RegistryDateFormat, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result1);
        int result2;
        int.TryParse(registryKey.GetValue(RegistryConstants.Uninstall.EstimatedSize)?.ToString() ?? "0", out result2);
        return new RegistryUninstallInfo()
        {
          Version = registryKey.GetValue(RegistryConstants.Uninstall.ApplicationVersion)?.ToString(),
          Name = registryKey.GetValue(RegistryConstants.Uninstall.DisplayName)?.ToString(),
          IconPath = registryKey.GetValue(RegistryConstants.Uninstall.DisplayIcon)?.ToString(),
          InstallationPath = registryKey.GetValue(RegistryConstants.Uninstall.InstallLocation)?.ToString(),
          InstallationDate = result1,
          UninstallCommand = registryKey.GetValue(RegistryConstants.Uninstall.UninstallString)?.ToString(),
          Publisher = registryKey.GetValue(RegistryConstants.Uninstall.Publisher)?.ToString(),
          SizeBytes = new long?((long) (result2 * 1024))
        };
      }
    }

    public static void DeleteUninstallInfo(string appKey)
    {
      try
      {
        string uninstallRegistryFolder = RegistryHelper.GetUninstallRegistryFolder(appKey);
        RegistryHelper._defaultRootKey.DeleteSubKeyTree(uninstallRegistryFolder, false);
      }
      catch (BadRegistryPathPart ex)
      {
      }
    }

    public static bool UninstallInfoExists(string appKey)
    {
      try
      {
        string uninstallRegistryFolder = RegistryHelper.GetUninstallRegistryFolder(appKey);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(uninstallRegistryFolder))
          return registryKey != null;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static List<string> GetUninstallInfoKeys(string prefix)
    {
      using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(RegistryHelper._uninstallBaseFolder))
        return registryKey == null ? new List<string>() : ((IEnumerable<string>) registryKey.GetSubKeyNames()).Where<string>((Func<string, bool>) (e => e.StartsWith(prefix))).ToList<string>();
    }

    public static bool TryRegisterUrlScheme(
      string schemeName,
      string schemeExePath,
      ILogger logger)
    {
      try
      {
        string schemeRegistryFolder = RegistryHelper.GetUrlSchemeRegistryFolder(schemeName);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(schemeRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(schemeRegistryFolder))
        {
          if (registryKey == null)
            return false;
          registryKey.SetValue(string.Empty, (object) ("URL:" + schemeName));
          registryKey.SetValue(RegistryConstants.UrlScheme.UrlProtocolKey, (object) string.Empty);
          using (RegistryKey subKey = registryKey.CreateSubKey(RegistryConstants.UrlScheme.DefaultIconKey))
          {
            if (subKey == null)
              return false;
            subKey.SetValue(string.Empty, (object) (Path.GetFileName(schemeExePath) + ",1"));
          }
          using (RegistryKey subKey = registryKey.CreateSubKey("shell\\open\\command"))
          {
            if (subKey == null)
              return false;
            subKey.SetValue(string.Empty, (object) ("\"" + schemeExePath + "\" \"%1\""));
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        logger.Error(ex, "TryRegisterUrlScheme failed");
        return false;
      }
    }

    public static void DeleteUrlScheme(string schemeName)
    {
      try
      {
        string schemeRegistryFolder = RegistryHelper.GetUrlSchemeRegistryFolder(schemeName);
        RegistryHelper._defaultRootKey.DeleteSubKeyTree(schemeRegistryFolder, false);
      }
      catch (BadRegistryPathPart ex)
      {
      }
    }

    public static UrlSchemeInfo GetUrlSchemeInfo(string schemeName)
    {
      UrlSchemeInfo urlSchemeInfo = RegistryHelper.GetUrlSchemeInfo(Registry.LocalMachine, schemeName);
      return RegistryHelper.GetUrlSchemeInfo(Registry.CurrentUser, schemeName) ?? urlSchemeInfo;
    }

    public static RegisterLauncherSoftwareInfo GetRegisterLauncherSoftwareData(
      string publisher,
      string launcherKey)
    {
      string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey);
      using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
      {
        if (registryKey == null)
          throw new InvalidOperationException("Can't get software info node " + launcherRegistryFolder);
        string str1 = registryKey.GetValue(RegistryConstants.Software.InstallationDate)?.ToString();
        object obj = registryKey.GetValue(RegistryConstants.Software.OldGamesTaken);
        string str2 = registryKey.GetValue(RegistryConstants.Software.LastGamesInstallDirectory)?.ToString();
        string str3 = registryKey.GetValue(RegistryConstants.Software.LauncherRegion)?.ToString();
        return new RegisterLauncherSoftwareInfo()
        {
          Version = registryKey.GetValue(RegistryConstants.Software.Version)?.ToString(),
          LauncherKey = launcherKey,
          InstallationPath = registryKey.GetValue(RegistryConstants.Software.Path)?.ToString(),
          Publisher = publisher,
          LauncherId = registryKey.GetValue(RegistryConstants.Software.LauncherId)?.ToString(),
          TrackingId = registryKey.GetValue(RegistryConstants.Software.TrackingId)?.ToString(),
          OldGamesTaken = new bool?(obj != null && (int) obj == 1),
          LastGamesInstallDirectory = str2,
          LauncherRegion = str3,
          InstallationDate = RegistryHelper.ParseRegistryDate(str1, RegistryConstants.Format.DateFormat)
        };
      }
    }

    public static bool TryRegisterLauncherSoftwareData(RegisterLauncherSoftwareInfo softwareInfo)
    {
      try
      {
        string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(softwareInfo.Publisher, softwareInfo.LauncherKey);
        using (RegistryKey registryKey1 = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
        {
          if (registryKey1 == null)
            return false;
          if (!string.IsNullOrWhiteSpace(softwareInfo.Version))
            registryKey1.SetValue(RegistryConstants.Software.Version, (object) (softwareInfo.Version ?? string.Empty));
          DateTime? nullable;
          if (softwareInfo.InstallationDate.HasValue)
          {
            RegistryKey registryKey2 = registryKey1;
            string installationDate = RegistryConstants.Software.InstallationDate;
            nullable = softwareInfo.InstallationDate;
            ref DateTime? local = ref nullable;
            string str = (local.HasValue ? local.GetValueOrDefault().ToString(RegistryConstants.Format.DateFormat) : (string) null) ?? string.Empty;
            registryKey2.SetValue(installationDate, (object) str);
          }
          if (softwareInfo.OldGamesTaken.HasValue)
          {
            RegistryKey registryKey3 = registryKey1;
            string oldGamesTaken1 = RegistryConstants.Software.OldGamesTaken;
            bool? oldGamesTaken2 = softwareInfo.OldGamesTaken;
            bool flag = true;
            // ISSUE: variable of a boxed type
            __Boxed<int> local = (ValueType) (oldGamesTaken2.GetValueOrDefault() == flag & oldGamesTaken2.HasValue ? 1 : 0);
            registryKey3.SetValue(oldGamesTaken1, (object) local);
          }
          if (!string.IsNullOrWhiteSpace(softwareInfo.InstallationPath))
            registryKey1.SetValue(RegistryConstants.Software.Path, (object) (softwareInfo.InstallationPath ?? string.Empty));
          nullable = softwareInfo.LastUpdateDate;
          if (nullable.HasValue)
          {
            RegistryKey registryKey4 = registryKey1;
            string lastUpdateDate = RegistryConstants.Software.LastUpdateDate;
            nullable = softwareInfo.LastUpdateDate;
            ref DateTime? local = ref nullable;
            string str = (local.HasValue ? local.GetValueOrDefault().ToString(RegistryConstants.Format.DateFormat) : (string) null) ?? string.Empty;
            registryKey4.SetValue(lastUpdateDate, (object) str);
          }
          if (registryKey1.GetValue(RegistryConstants.Software.LauncherId) == null && !string.IsNullOrWhiteSpace(softwareInfo.LauncherId))
          {
            string launcherId = softwareInfo.LauncherId;
            registryKey1.SetValue(RegistryConstants.Software.LauncherId, (object) launcherId);
          }
          if (!string.IsNullOrWhiteSpace(softwareInfo.LastGamesInstallDirectory))
            registryKey1.SetValue(RegistryConstants.Software.LastGamesInstallDirectory, (object) (softwareInfo.LastGamesInstallDirectory ?? string.Empty));
          if (!string.IsNullOrWhiteSpace(softwareInfo.LauncherRegion))
            registryKey1.SetValue(RegistryConstants.Software.LauncherRegion, (object) (softwareInfo.LauncherRegion ?? string.Empty));
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static bool TrySetLauncherIdIfNotExist(
      string publisher,
      string launcherKey,
      string launcherId)
    {
      try
      {
        string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
        {
          if (registryKey == null)
            return false;
          if (string.IsNullOrWhiteSpace(registryKey.GetValue(RegistryConstants.Software.LauncherId)?.ToString()))
            registryKey.SetValue(RegistryConstants.Software.LauncherId, (object) launcherId);
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static bool TrySetTrackingIdIfNotExist(
      string publisher,
      string launcherKey,
      string trackingId)
    {
      try
      {
        string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
        {
          if (registryKey == null)
            return false;
          if (string.IsNullOrWhiteSpace(registryKey.GetValue(RegistryConstants.Software.TrackingId)?.ToString()))
            registryKey.SetValue(RegistryConstants.Software.TrackingId, (object) trackingId);
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static bool TryRegisterGameSoftwareData(RegisterGameSoftwareInfo softwareInfo)
    {
      try
      {
        string gameRegistryFolder = RegistryHelper.GetSoftwareGameRegistryFolder(softwareInfo.Publisher, softwareInfo.LauncherKey, softwareInfo.GameName);
        using (RegistryKey registryKey1 = RegistryHelper._defaultRootKey.OpenSubKey(gameRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(gameRegistryFolder))
        {
          if (registryKey1 == null)
            return false;
          registryKey1.SetValue(RegistryConstants.Software.Path, (object) (softwareInfo.InstallationPath ?? string.Empty));
          registryKey1.SetValue(RegistryConstants.Software.Version, (object) (softwareInfo.Version ?? string.Empty));
          RegistryKey registryKey2 = registryKey1;
          string installationDate1 = RegistryConstants.Software.InstallationDate;
          DateTime? installationDate2 = softwareInfo.InstallationDate;
          ref DateTime? local = ref installationDate2;
          string str = (local.HasValue ? local.GetValueOrDefault().ToString(RegistryConstants.Format.DateFormat) : (string) null) ?? string.Empty;
          registryKey2.SetValue(installationDate1, (object) str);
        }
        return true;
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    public static RegisterGameSoftwareInfo GetGameSoftwareData(
      string publisher,
      string launcherKey,
      string gameName)
    {
      try
      {
        string gameRegistryFolder = RegistryHelper.GetSoftwareGameRegistryFolder(publisher, launcherKey, gameName);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(gameRegistryFolder, true))
        {
          if (registryKey == null)
            return (RegisterGameSoftwareInfo) null;
          string str = registryKey.GetValue(RegistryConstants.Software.InstallationDate)?.ToString();
          return new RegisterGameSoftwareInfo()
          {
            Publisher = publisher,
            LauncherKey = launcherKey,
            InstallationPath = registryKey.GetValue(RegistryConstants.Software.Path)?.ToString(),
            Version = registryKey.GetValue(RegistryConstants.Software.Version)?.ToString(),
            InstallationDate = new DateTime?(RegistryHelper.ParseRegistryDate(str, RegistryConstants.Format.DateFormat) ?? DateTime.Now)
          };
        }
      }
      catch (BadRegistryPathPart ex)
      {
        return (RegisterGameSoftwareInfo) null;
      }
    }

    public static void DeleteGameSoftwareData(DeleteGameSoftwareInfo softwareInfo)
    {
      try
      {
        string gameRegistryFolder = RegistryHelper.GetSoftwareGameRegistryFolder(softwareInfo.Publisher, softwareInfo.LauncherKey, softwareInfo.GameName);
        RegistryHelper._defaultRootKey.DeleteSubKeyTree(gameRegistryFolder, false);
      }
      catch (BadRegistryPathPart ex)
      {
      }
    }

    public static DateTime? GetLastErrorCheckDate(string publisher, string launcherKey)
    {
      string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey);
      using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
      {
        if (registryKey == null)
          throw new InvalidOperationException("Can't get software info node " + launcherRegistryFolder);
        return RegistryHelper.ParseRegistryDate(registryKey.GetValue(RegistryConstants.Software.LastErrorCheckDate)?.ToString(), RegistryConstants.Format.DateTimeFormat);
      }
    }

    public static bool TryUpdateLastErrorCheckDate(
      string publisher,
      string launcherKey,
      DateTime dateTime)
    {
      try
      {
        string launcherRegistryFolder = RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey);
        using (RegistryKey registryKey = RegistryHelper._defaultRootKey.OpenSubKey(launcherRegistryFolder, true) ?? RegistryHelper._defaultRootKey.CreateSubKey(launcherRegistryFolder))
        {
          if (registryKey == null)
            return false;
          registryKey.SetValue(RegistryConstants.Software.LastErrorCheckDate, (object) dateTime.ToString(RegistryConstants.Format.DateTimeFormat));
          return true;
        }
      }
      catch (BadRegistryPathPart ex)
      {
        return false;
      }
    }

    private static UrlSchemeInfo GetUrlSchemeInfo(RegistryKey rootKey, string schemeName)
    {
      string schemeRegistryFolder = RegistryHelper.GetUrlSchemeRegistryFolder(schemeName);
      using (RegistryKey registryKey = rootKey.OpenSubKey(schemeRegistryFolder, true))
      {
        if (registryKey == null)
          return (UrlSchemeInfo) null;
        UrlSchemeInfo urlSchemeInfo = new UrlSchemeInfo()
        {
          SchemeName = schemeName,
          ApplicationName = schemeName
        };
        using (RegistryKey subKey = registryKey.CreateSubKey(RegistryConstants.UrlScheme.DefaultIconKey))
        {
          if (subKey == null)
            return (UrlSchemeInfo) null;
          string str = subKey.GetValue(string.Empty).ToString().Split(',')[0].RemoveLeadingSymbols(RegistryHelper._quotes);
          urlSchemeInfo.ExePath = str;
          if (File.Exists(str))
          {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str);
            urlSchemeInfo.ApplicationName = versionInfo.FileDescription;
          }
        }
        return urlSchemeInfo;
      }
    }

    private static string GetValidPathPart(string partToClear)
    {
      string part = partToClear != null ? partToClear : throw new BadRegistryPathPart(partToClear);
      foreach (char invalidFileNameChar in Path.GetInvalidFileNameChars())
        partToClear = partToClear.Replace(invalidFileNameChar.ToString(), string.Empty);
      return !string.IsNullOrWhiteSpace(partToClear) ? partToClear : throw new BadRegistryPathPart(part);
    }

    private static string GetUninstallRegistryFolder(string key)
    {
      string validPathPart = RegistryHelper.GetValidPathPart(key);
      return RegistryHelper._uninstallBaseFolder + "\\" + validPathPart;
    }

    private static string GetUrlSchemeRegistryFolder(string schemeName) => "Software\\Classes\\" + RegistryHelper.GetValidPathPart(schemeName);

    private static string GetSoftwareLauncherRegistryFolder(string publisher, string launcherKey)
    {
      string validPathPart = RegistryHelper.GetValidPathPart(launcherKey);
      return "SOFTWARE\\" + publisher + "\\" + validPathPart;
    }

    private static string GetSoftwareGameRegistryFolder(
      string publisher,
      string launcherKey,
      string gameName)
    {
      return Path.Combine(RegistryHelper.GetSoftwareGamesRegistryFolder(publisher, launcherKey), RegistryHelper.GetValidPathPart(gameName) ?? "");
    }

    private static string GetSoftwareGamesRegistryFolder(string publisher, string launcherKey) => Path.Combine(RegistryHelper.GetSoftwareLauncherRegistryFolder(publisher, launcherKey), "Games");

    private static DateTime? ParseRegistryDate(string value, string format)
    {
      if (string.IsNullOrWhiteSpace(value))
        return new DateTime?();
      DateTime result;
      return !DateTime.TryParseExact(value, format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result) ? new DateTime?() : new DateTime?(result);
    }
  }
}
