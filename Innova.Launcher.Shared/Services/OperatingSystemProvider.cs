// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.OperatingSystemProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Tracking.Models;
using NLog;
using System;
using System.Management;

namespace Innova.Launcher.Shared.Services
{
  public sealed class OperatingSystemProvider : IOperatingSystemProvider
  {
    private readonly Lazy<LauncherOperatingSystem> _os;
    private readonly ILogger _logger;

    public OperatingSystemProvider(ILoggerFactory loggerFactory)
    {
      this._os = new Lazy<LauncherOperatingSystem>(new Func<LauncherOperatingSystem>(this.GetOS));
      this._logger = loggerFactory.GetCurrentClassLogger<OperatingSystemProvider>();
    }

    public LauncherOperatingSystem Get() => this._os.Value;

    private LauncherOperatingSystem GetOS() => new LauncherOperatingSystem()
    {
      Version = this.GetVersion(),
      Name = this.GetName(),
      Architecture = this.GetArchitecture()
    };

    private int? GetArchitecture()
    {
      try
      {
        return new int?(Environment.Is64BitOperatingSystem ? 64 : 32);
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return new int?();
      }
    }

    private string GetVersion()
    {
      try
      {
        return OperatingSystemProvider.GetManagementObject("Win32_OperatingSystem")["Version"]?.ToString().Trim();
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return (string) null;
      }
    }

    private string GetName()
    {
      try
      {
        OperatingSystem osVersion = Environment.OSVersion;
        Version version = osVersion.Version;
        string str = string.Empty;
        if (osVersion.Platform == PlatformID.Win32Windows)
        {
          switch (version.Minor)
          {
            case 0:
              str = "95";
              break;
            case 10:
              str = !(version.Revision.ToString() == "2222A") ? "98" : "98SE";
              break;
            case 90:
              str = "Me";
              break;
          }
        }
        else if (osVersion.Platform == PlatformID.Win32NT)
        {
          switch (version.Major)
          {
            case 3:
              str = "NT 3.51";
              break;
            case 4:
              str = "NT 4.0";
              break;
            case 5:
              str = version.Minor != 0 ? "XP" : "2000";
              break;
            case 6:
              str = version.Minor != 0 ? (version.Minor != 1 ? (version.Minor != 2 ? "8.1" : "8") : "7") : "Vista";
              break;
            case 10:
              str = version.Build >= 22000 ? "11" : "10";
              break;
          }
        }
        if (str != "")
        {
          str = "Windows " + str;
          if (!string.IsNullOrEmpty(osVersion.ServicePack))
            str = str + " " + osVersion.ServicePack;
        }
        return str.Trim();
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return (string) null;
      }
    }

    private static ManagementObject GetManagementObject(string className)
    {
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementClass(className).GetInstances().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          ManagementObject current = (ManagementObject) enumerator.Current;
          if (current != null)
            return current;
        }
      }
      return (ManagementObject) null;
    }
  }
}
