// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.ExistingLauncherChecker
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Innova.Launcher.Shared.Services
{
  public class ExistingLauncherChecker : IExistingLauncherChecker
  {
    private readonly ILogger _logger;

    public ExistingLauncherChecker(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<ExistingLauncherChecker>();

    public void WaitAlreadyRunningLauncherForExit(string launcherExepath, TimeSpan waitInterval)
    {
      Process processOrDefault = this.GetExistingLauncherProcessOrDefault(launcherExepath);
      if (processOrDefault == null)
        return;
      try
      {
        this._logger.Trace(string.Format("Wait for existing launcher process {0}", (object) processOrDefault.Id));
        processOrDefault.WaitForExit((int) waitInterval.TotalMilliseconds);
        if (!processOrDefault.HasExited)
          throw new ProcessWaitingTimeoutException(processOrDefault.Id, (long) (int) waitInterval.TotalMilliseconds);
      }
      catch (ProcessWaitingTimeoutException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        this._logger.Fatal(ex, "Existing launcher process waiting problem");
        throw;
      }
    }

    public Process GetExistingLauncherProcessOrDefault(string launcherExepath)
    {
      string processName = Path.GetFileNameWithoutExtension(launcherExepath);
      return ((IEnumerable<Process>) Process.GetProcesses()).Where<Process>((Func<Process, bool>) (e => e.ProcessName == processName)).FirstOrDefault<Process>((Func<Process, bool>) (p => p.MainModule == null || p.MainModule?.FileName == launcherExepath));
    }
  }
}
