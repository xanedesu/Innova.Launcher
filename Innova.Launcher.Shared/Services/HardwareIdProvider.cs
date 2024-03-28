// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.HardwareIdProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Management;

namespace Innova.Launcher.Shared.Services
{
  public sealed class HardwareIdProvider : IHardwareIdProvider
  {
    private readonly ILauncherIdProvider _launcherIdProvider;
    private readonly Lazy<string> _hardwareId;
    private readonly ILogger _logger;

    public HardwareIdProvider(ILoggerFactory loggerFactory, ILauncherIdProvider launcherIdProvider)
    {
      this._launcherIdProvider = launcherIdProvider;
      this._logger = loggerFactory.GetCurrentClassLogger<HardwareIdProvider>();
      this._hardwareId = new Lazy<string>(new Func<string>(this.GetHardwareId));
    }

    public string Get() => this._hardwareId.Value;

    private string GetHardwareId()
    {
      string motherBoardId = this.GetMotherBoardId();
      string str1 = motherBoardId != null ? motherBoardId.Replace(" ", "").SubstringOrSelf(0, 53) : (string) null;
      string hardDiskId = this.GetHardDiskId();
      string str2 = hardDiskId != null ? hardDiskId.Replace(" ", "").SubstringOrSelf(0, 53) : (string) null;
      string cpuId = this.GetCpuId();
      string str3 = cpuId != null ? cpuId.Replace(" ", "").SubstringOrSelf(0, 53) : (string) null;
      if (string.IsNullOrWhiteSpace(str1) && string.IsNullOrWhiteSpace(str1) && string.IsNullOrWhiteSpace(str1))
        return this._launcherIdProvider.Get();
      return (str1 + "-" + str2 + "-" + str3).Normalize();
    }

    private string GetMotherBoardId()
    {
      try
      {
        return ((IEnumerable) new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard").Get()).OfType<ManagementObject>().Select<ManagementObject, string>((Func<ManagementObject, string>) (v => v["SerialNumber"]?.ToString())).FirstOrDefault<string>((Func<string, bool>) (v => v != null));
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return string.Empty;
      }
    }

    private string GetHardDiskId()
    {
      try
      {
        ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"" + Path.GetPathRoot(Environment.SystemDirectory).Substring(0, 2).ToLower() + "\"");
        managementObject.Get();
        return managementObject["VolumeSerialNumber"].ToString();
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return string.Empty;
      }
    }

    private string GetCpuId()
    {
      try
      {
        return ((IEnumerable) new ManagementObjectSearcher("Select * From Win32_processor").Get()).OfType<ManagementObject>().Select<ManagementObject, string>((Func<ManagementObject, string>) (v => v["ProcessorId"]?.ToString())).FirstOrDefault<string>((Func<string, bool>) (v => v != null));
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return string.Empty;
      }
    }
  }
}
