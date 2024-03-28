// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.ProcessManager
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services
{
  public class ProcessManager : IProcessManager, IDisposable
  {
    private readonly HashSet<int> _updaterSuccessExitCodes = new HashSet<int>()
    {
      -1,
      0
    };
    private readonly ConcurrentDictionary<Task, bool> _tasksHistory = new ConcurrentDictionary<Task, bool>();
    private readonly ILogger _logger;

    public ProcessManager(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<ProcessManager>();

    public async Task<ProcessResult> RunAsync(
      ProcessStartInfo startInfo,
      CancellationToken cancellationToken)
    {
      TaskCompletionSource<ProcessResult> result = new TaskCompletionSource<ProcessResult>();
      bool cancelled = false;
      try
      {
        Process process = Process.Start(startInfo);
        if (process == null)
          return ProcessResult.FailedToStart;
        cancellationToken.Register((Action) (() =>
        {
          cancelled = true;
          Process processByIdOrDefault = this.GetProcessByIdOrDefault(process.Id);
          if (processByIdOrDefault == null || processByIdOrDefault.HasExited)
            return;
          processByIdOrDefault.Kill();
        }));
        process.EnableRaisingEvents = true;
        process.Exited += (EventHandler) ((sender, args) =>
        {
          if (process.HasExited && !cancelled && this._updaterSuccessExitCodes.Contains(process.ExitCode))
            result.SetResult(ProcessResult.Done);
          else
            result.SetResult(ProcessResult.Crashed);
          this._tasksHistory.TryRemove((Task) result.Task, out bool _);
        });
        this._tasksHistory.TryAdd((Task) result.Task, false);
        return await result.Task;
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Exception during updater process run. Arguments: " + startInfo.Arguments);
        return ProcessResult.FailedToStart;
      }
    }

    private Process GetProcessByIdOrDefault(int processId)
    {
      try
      {
        return Process.GetProcessById(processId);
      }
      catch (ArgumentException ex)
      {
        return (Process) null;
      }
    }

    private void WaitAllTasks(int timeoutMsec) => Task.WaitAll(this._tasksHistory.Keys.ToArray<Task>(), timeoutMsec);

    public void Dispose() => this.WaitAllTasks(5000);
  }
}
