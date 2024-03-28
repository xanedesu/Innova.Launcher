// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.ForgameUpdater
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Updater.Core.Exceptions;
using Innova.Launcher.Updater.Core.Infrastructure;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using Newtonsoft.Json;
using NLog;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services
{
  public class ForgameUpdater : IBinaryUpdater, IDisposable
  {
    private readonly IForgameUpdaterProgressHandler _forgameUpdaterProgressHandler;
    private readonly ITcpServerConfigProvider _tcpServerConfigProvider;
    private readonly ILogger _logger;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IProcessManager _processManager;
    private readonly AsyncRetryPolicy<ProcessResult> _retryPolicy;
    private readonly ConcurrentDictionary<int, UpdateModel> _startedUpdates;
    private readonly Lazy<ForgameUpdaterExecutable> _updaterExe;
    private const string _updaterDidntStartMessage = "Updater process was unable to start";
    private const int _retryAttempts = 6;
    private const int _maxRetryAttemptTimeout = 64;

    public event EventHandler<UpdateCompletedEventArgs> UpdateCompleted;

    public event EventHandler<UpdateProgressInfoEventArgs> UpdateProgressReceived;

    public event EventHandler UninstallCompleted;

    public ForgameUpdater(
      IForgameUpdaterProgressHandler forgameUpdaterProgressHandler,
      ITcpServerConfigProvider tcpServerConfigProvider,
      IInternetConnectionChecker internetConnectionChecker,
      ILoggerFactory loggerFactory,
      ILifetimeManager lifetimeManager,
      IProcessManager processManager)
    {
      this._forgameUpdaterProgressHandler = forgameUpdaterProgressHandler;
      this._forgameUpdaterProgressHandler.ProgressCancelled += new EventHandler<UpdateCompletedEventArgs>(this.UpdateProgressCompleted);
      this._forgameUpdaterProgressHandler.ProgressCompleted += new EventHandler<UpdateCompletedEventArgs>(this.UpdateProgressCompleted);
      this._forgameUpdaterProgressHandler.ProgressUpdated += new EventHandler<UpdateProgressInfoEventArgs>(this.UpdateProgressUpdated);
      this._lifetimeManager = lifetimeManager;
      this._tcpServerConfigProvider = tcpServerConfigProvider;
      this._internetConnectionChecker = internetConnectionChecker;
      this._processManager = processManager;
      this._logger = loggerFactory.GetCurrentClassLogger<ForgameUpdater>();
      this._updaterExe = new Lazy<ForgameUpdaterExecutable>((Func<ForgameUpdaterExecutable>) (() => new ForgameUpdaterExecutable()), LazyThreadSafetyMode.ExecutionAndPublication);
      this._lifetimeManager.TimeToDie += new EventHandler(this.LifetimeEnded);
      this._retryPolicy = AsyncRetryTResultSyntax.WaitAndRetryAsync<ProcessResult>(Policy<ProcessResult>.HandleResult((Func<ProcessResult, bool>) (c => c == ProcessResult.Crashed)), 6, (Func<int, TimeSpan>) (attempt => TimeSpan.FromSeconds(Math.Min(Math.Pow(2.0, (double) attempt), 64.0))));
      this._startedUpdates = new ConcurrentDictionary<int, UpdateModel>();
    }

    public async Task UpdateAsync(
      int serviceId,
      string path,
      string sourceUrl,
      bool fullUpdate = true,
      string logKey = null,
      string updaterName = null)
    {
      try
      {
        UpdateModel updateModel = new UpdateModel(serviceId, path, sourceUrl, logKey);
        ForgameUpdaterExecutable updater = this.GetUpdater(updaterName);
        this._startedUpdates.AddOrUpdate(serviceId, updateModel, (Func<int, UpdateModel, UpdateModel>) ((key, oldValue) => updateModel));
        this._forgameUpdaterProgressHandler.StartListenProgress();
        string updaterLogPath = this.GetUpdaterLogPath(logKey ?? serviceId.ToString());
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
          WorkingDirectory = updater.DirectoryName,
          FileName = Path.Combine(updater.DirectoryName, updater.FileName),
          Arguments = string.Format("--game-id={0} --server-port={1} --progress=detailed {2} --dest=\"{3}\" --url=\"{4}\" --log-file=\"{5}\" --name={6}", (object) serviceId, (object) this._tcpServerConfigProvider.TcpPort, fullUpdate ? (object) "--full" : (object) "", (object) path, (object) sourceUrl, (object) updaterLogPath, (object) serviceId),
          UseShellExecute = true,
          CreateNoWindow = true,
          WindowStyle = ProcessWindowStyle.Hidden
        };
        switch (await this._retryPolicy.ExecuteAsync((Func<CancellationToken, Task<ProcessResult>>) (token => this._processManager.RunAsync(startInfo, token)), updateModel.Cancellation.Token, false))
        {
          case ProcessResult.Crashed:
          case ProcessResult.FailedToStart:
            this._logger.Error("Updater process was unable to start");
            this.UpdateProgressCompleted(serviceId, new UpdateProgressError(400, "Updater process was unable to start"));
            break;
        }
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Update error");
      }
    }

    public void PauseUpdate(int serviceId) => this._forgameUpdaterProgressHandler.PauseProgressIfExists(serviceId);

    public void CancelUpdate(int serviceId)
    {
      this._forgameUpdaterProgressHandler.FinishProgressIfExists(serviceId);
      this.UpdateProgressCompleted((object) this, new UpdateCompletedEventArgs(serviceId, true));
    }

    public async Task ResumeOrStartNewUpdateAsync(
      int serviceId,
      string path,
      string sourceUrl,
      string logKey = null,
      string updaterName = null)
    {
      if (this._forgameUpdaterProgressHandler.ResumeProgressIfExists(serviceId))
        return;
      await this.UpdateAsync(serviceId, path, sourceUrl, logKey: logKey, updaterName: updaterName).ConfigureAwait(false);
    }

    public async Task UninstallAsync(string path, string url, string updaterName = null)
    {
      ForgameUpdater sender = this;
      string updaterLogPath = sender.GetUpdaterLogPath("uninstall");
      ForgameUpdaterExecutable updater = sender.GetUpdater(updaterName);
      ProcessStartInfo startInfo = new ProcessStartInfo()
      {
        WorkingDirectory = updater.DirectoryName,
        FileName = Path.Combine(updater.DirectoryName, updater.FileName),
        Arguments = "--uninstall --progress=dummy --dest=\"" + path + "\" --url=\"" + url + "\" --log-file=\"" + updaterLogPath + "\"",
        UseShellExecute = true,
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden
      };
      if (await sender._retryPolicy.ExecuteAsync((Func<CancellationToken, Task<ProcessResult>>) (token => this._processManager.RunAsync(startInfo, token)), CancellationToken.None, false) != ProcessResult.Done)
        return;
      EventHandler uninstallCompleted = sender.UninstallCompleted;
      if (uninstallCompleted == null)
        return;
      uninstallCompleted((object) sender, EventArgs.Empty);
    }

    public async Task UpdateWithoutProgressAsync(string path, string url, Action finishCallback)
    {
      string updaterLogPath = this.GetUpdaterLogPath("frost");
      ForgameUpdaterExecutable updaterExecutable = this._updaterExe.Value;
      ProcessStartInfo startInfo = new ProcessStartInfo()
      {
        WorkingDirectory = updaterExecutable.DirectoryName,
        FileName = Path.Combine(updaterExecutable.DirectoryName, updaterExecutable.FileName),
        Arguments = "--progress=none --full --dest=\"" + path + "\" --url=" + url + " --log-file=\"" + updaterLogPath + "\"",
        UseShellExecute = true,
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden
      };
      switch (await this._retryPolicy.ExecuteAsync((Func<CancellationToken, Task<ProcessResult>>) (token => this._processManager.RunAsync(startInfo, token)), CancellationToken.None, false))
      {
        case ProcessResult.Done:
          Action action = finishCallback;
          if (action == null)
            break;
          action();
          break;
        case ProcessResult.FailedToStart:
          this._logger.Error("Updater process was unable to start");
          throw new ForgameUpdaterStartException("Updater process was unable to start");
      }
    }

    private ForgameUpdaterExecutable GetUpdater(string updaterName) => string.IsNullOrWhiteSpace(updaterName) ? this._updaterExe.Value : new ForgameUpdaterExecutable(updaterName);

    private void UpdateProgressCompleted(int serviceId, UpdateProgressError error)
    {
      UpdateModel updateModel;
      if (this._startedUpdates.TryRemove(serviceId, out updateModel))
        updateModel.Dispose();
      this._logger.Trace(string.Format("Progress of serviceId={0} key={1} completed. {2}", (object) serviceId, (object) (updateModel?.LogKey ?? "no key"), error != null ? (object) ("error " + JsonConvert.SerializeObject((object) error)) : (object) ""));
      EventHandler<UpdateCompletedEventArgs> updateCompleted = this.UpdateCompleted;
      if (updateCompleted == null)
        return;
      updateCompleted((object) this, new UpdateCompletedEventArgs(serviceId, error));
    }

    private void UpdateProgressCompleted(object sender, UpdateCompletedEventArgs args)
    {
      UpdateModel model;
      if (!this._startedUpdates.TryRemove(args.ServiceId, out model))
        return;
      model.Dispose();
      bool flag = args.Error != null && !this._internetConnectionChecker.Check();
      this._logger.Trace(string.Format("Progress of serviceId={0} key={1} completed isCanceled={2}. {3}", (object) args.ServiceId, (object) model.LogKey, (object) args.IsCancelled, args.Error != null ? (object) ("error " + JsonConvert.SerializeObject((object) args.Error)) : (object) ""));
      if (flag)
      {
        this._logger.Trace(string.Format("Progress of serviceId={0} key={1} completed because connection lost. Wait for restore..", (object) args.ServiceId, (object) model.LogKey));
        this._internetConnectionChecker.DoWhenConnectionExistsAsync((Action) (() => this.ResumeOrStartNewUpdateAsync(model.ServiceId, model.Path, model.Url, model.LogKey, (string) null)));
      }
      else
      {
        EventHandler<UpdateCompletedEventArgs> updateCompleted = this.UpdateCompleted;
        if (updateCompleted == null)
          return;
        updateCompleted((object) this, args);
      }
    }

    private void UpdateProgressUpdated(object sender, UpdateProgressInfoEventArgs args)
    {
      UpdateModel updateModel;
      this._startedUpdates.TryGetValue(args.ServiceId, out updateModel);
      this._logger.Trace(string.Format("Progress of serviceId={0} key={1} changed: {2}", (object) args.ServiceId, (object) updateModel?.LogKey, (object) JsonConvert.SerializeObject((object) args.ProgressInfo)));
      EventHandler<UpdateProgressInfoEventArgs> progressReceived = this.UpdateProgressReceived;
      if (progressReceived == null)
        return;
      progressReceived((object) this, args);
    }

    private string GetUpdaterLogPath(string serviceId) => "\\4game2.0\\launcher\\updater\\" + serviceId;

    private void LifetimeEnded(object sender, EventArgs e) => this.Dispose();

    public void Dispose()
    {
      try
      {
        foreach (UpdateModel updateModel in (IEnumerable<UpdateModel>) this._startedUpdates.Values)
          updateModel.Dispose();
        this._processManager.Dispose();
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Dispose error");
      }
    }
  }
}
