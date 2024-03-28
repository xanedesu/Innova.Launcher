// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.HardwareProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Tracking.Helpers;
using Innova.Launcher.Shared.Tracking.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace Innova.Launcher.Shared.Services
{
  public sealed class HardwareProvider : IHardwareProvider
  {
    private readonly Lazy<LauncherHardware> _hardware;
    private readonly IOperatingSystemProvider _operatingSystemProvider;
    private readonly ILogger _logger;

    public HardwareProvider(
      ILoggerFactory loggerFactory,
      IOperatingSystemProvider operatingSystemProvider)
    {
      this._operatingSystemProvider = operatingSystemProvider ?? throw new ArgumentNullException(nameof (operatingSystemProvider));
      this._hardware = new Lazy<LauncherHardware>(new Func<LauncherHardware>(this.GetHardware));
      this._logger = loggerFactory.GetCurrentClassLogger<HardwareProvider>();
    }

    public LauncherHardware Get() => this._hardware.Value;

    private LauncherHardware GetHardware()
    {
      LauncherHardware hardware = new LauncherHardware()
      {
        NetVersion = Environment.Version.ToString(),
        OS = this._operatingSystemProvider.Get(),
        CPU = this.GetCPU(),
        VideoCards = this.GetVideoCards(),
        Ram = this.GetRam()
      };
      hardware.TotalRam = hardware.Ram.Select<LauncherRam, long>((Func<LauncherRam, long>) (v => v.Capacity)).DefaultIfEmpty<long>(0L).Sum();
      hardware.TotalVideoCardRam = hardware.VideoCards.Select<LauncherVideoCard, long>((Func<LauncherVideoCard, long>) (v => v.AdapterRam)).DefaultIfEmpty<long>(0L).Sum();
      return hardware;
    }

    private List<LauncherVideoCard> GetVideoCards()
    {
      try
      {
        List<LauncherVideoCard> videoCards = new List<LauncherVideoCard>();
        using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("select * from Win32_VideoController").Get().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ManagementBaseObject current = enumerator.Current;
            long result;
            if (long.TryParse(current["AdapterRAM"].ToString(), out result))
              videoCards.Add(new LauncherVideoCard()
              {
                Name = current["Name"]?.ToString().Trim(),
                VideoProcessor = current["VideoProcessor"]?.ToString().Trim(),
                DriverVersion = current["DriverVersion"]?.ToString().Trim(),
                AdapterRam = result
              });
          }
        }
        return videoCards;
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return new List<LauncherVideoCard>();
      }
    }

    private List<LauncherCPU> GetCPU()
    {
      try
      {
        List<LauncherCPU> cpu = new List<LauncherCPU>();
        using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("select * from Win32_Processor").Get().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ManagementBaseObject current = enumerator.Current;
            int result1;
            int result2;
            if (int.TryParse(current["NumberOfCores"].ToString(), out result1) && int.TryParse(current["CurrentClockSpeed"].ToString(), out result2))
              cpu.Add(new LauncherCPU()
              {
                Name = current["Name"]?.ToString().Trim(),
                Manufacturer = current["Manufacturer"]?.ToString().Trim(),
                NumberOfCores = result1,
                ClockSpeed = result2
              });
          }
        }
        return cpu;
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return new List<LauncherCPU>();
      }
    }

    private List<LauncherRam> GetRam()
    {
      try
      {
        List<LauncherRam> ram = new List<LauncherRam>();
        using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("select * from Win32_PhysicalMemory").Get().GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ManagementBaseObject current = enumerator.Current;
            int result1;
            long result2;
            if (int.TryParse(current["Speed"].ToString(), out result1) && long.TryParse(current["Capacity"].ToString(), out result2))
            {
              ushort result3;
              ushort.TryParse(current["MemoryType"].ToString(), out result3);
              ram.Add(new LauncherRam()
              {
                Speed = result1,
                Capacity = result2,
                Manufacturer = current["Manufacturer"]?.ToString().Trim(),
                Type = RamTypeHelper.Resolve(result3)
              });
            }
          }
        }
        return ram;
      }
      catch (Exception ex)
      {
        this._logger.Warn<Exception>(ex);
        return new List<LauncherRam>();
      }
    }
  }
}
