// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.RunningLauncherRepairer
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Configuration.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Innova.Launcher.Shared.Services
{
  public sealed class RunningLauncherRepairer : IRunningLauncherRepairer
  {
    private readonly ILauncherStructureConfigurationProvider _launcherStructureConfigurationProvider;
    private readonly ILauncherStructureProvider _launcherStructureProvider;
    private readonly ILogger _logger;

    public RunningLauncherRepairer(
      ILoggerFactory loggerFactory,
      ILauncherStructureConfigurationProvider launcherStructureConfigurationProvider,
      ILauncherStructureProvider launcherStructureProvider)
    {
      this._launcherStructureConfigurationProvider = launcherStructureConfigurationProvider ?? throw new ArgumentNullException(nameof (launcherStructureConfigurationProvider));
      this._launcherStructureProvider = launcherStructureProvider ?? throw new ArgumentNullException(nameof (launcherStructureProvider));
      this._logger = loggerFactory.GetCurrentClassLogger<RunningLauncherRepairer>();
    }

    public bool TryWakeUp(string[] args)
    {
      try
      {
        string processName = Path.GetFileNameWithoutExtension(this._launcherStructureConfigurationProvider.LauncherExeName);
        Process currentProcess = Process.GetCurrentProcess();
        Process process = ((IEnumerable<Process>) Process.GetProcesses()).Where<Process>((Func<Process, bool>) (e => e.ProcessName == processName)).FirstOrDefault<Process>((Func<Process, bool>) (v => v.Id != currentProcess.Id));
        if (process == null)
          return true;
        WinApiUtils.WindowsEnumerator source = new WinApiUtils.WindowsEnumerator(process.Id);
        bool flag;
        if (!(flag = source.Where<WinApiUtils.WindowInfo>((Func<WinApiUtils.WindowInfo, bool>) (e => e.Title == "4game")).ToList<WinApiUtils.WindowInfo>().Count == 0))
        {
          int num = this.TryWakeUpLauncher(args) ? 1 : 0;
          if (num == 0)
            this._logger.Warn("Launcher wann't wake up.");
          flag = num == 0;
        }
        if (!flag)
          return true;
        this._logger.Warn("Kill existing launcher");
        process.Kill();
        return false;
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, "Error while trying to repair existing launcher", Array.Empty<object>());
        return true;
      }
    }

    private bool TryWakeUpLauncher(string[] args)
    {
      try
      {
        LauncherIpcClient launcherIpcClient = new LauncherIpcClient();
        if (launcherIpcClient.PingExists())
        {
          launcherIpcClient.WakeUp(string.Join(" ", args));
          return true;
        }
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, nameof (TryWakeUpLauncher), Array.Empty<object>());
      }
      return false;
    }
  }
}
