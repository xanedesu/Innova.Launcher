// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.LauncherBinaryUpdater
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using NLog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services
{
  public class LauncherBinaryUpdater
  {
    private readonly int _launcherDefaultId = 9999;
    private readonly IBinaryUpdater _updater;
    private readonly ILogger _logger;
    private TaskCompletionSource<object> _updateTaskCompletionSource;
    private string _updateDirectory;
    private string _installingVersionUrl;
    private string _hostingVersionDistributesFolder = "distr";

    public event EventHandler<UpdateCompletedEventArgs> UpdateCompleted;

    public event EventHandler<UpdateProgressInfoEventArgs> UpdateProgressReceived;

    public event EventHandler UninstallCompleted;

    public LauncherBinaryUpdater(IBinaryUpdater updater, ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherBinaryUpdater>();
      this._updater = updater;
      updater.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(this.OnInstallCompleted);
      updater.UpdateProgressReceived += new EventHandler<UpdateProgressInfoEventArgs>(this.OnInstallProgressReceived);
      updater.UninstallCompleted += new EventHandler(this.OnUninstallCompleted);
    }

    private void OnUninstallCompleted(object sender, EventArgs e)
    {
      this._logger.Trace("Uninstall completed!");
      EventHandler uninstallCompleted = this.UninstallCompleted;
      if (uninstallCompleted == null)
        return;
      uninstallCompleted(sender, e);
    }

    private void OnInstallProgressReceived(object sender, UpdateProgressInfoEventArgs e)
    {
      if (e.ServiceId != this._launcherDefaultId)
        return;
      this._logger.Trace(string.Format("Progress.. {0}", (object) e.ProgressInfo.Progress));
      EventHandler<UpdateProgressInfoEventArgs> progressReceived = this.UpdateProgressReceived;
      if (progressReceived == null)
        return;
      progressReceived(sender, e);
    }

    private void OnInstallCompleted(object sender, UpdateCompletedEventArgs e)
    {
      if (e.ServiceId != this._launcherDefaultId)
        return;
      this._updateTaskCompletionSource?.SetResult((object) null);
      this._logger.Trace("Update completed!");
      EventHandler<UpdateCompletedEventArgs> updateCompleted = this.UpdateCompleted;
      if (updateCompleted == null)
        return;
      updateCompleted(sender, e);
    }

    public Task UpdateAsync(string updateDirectory, string versionSourceUrl)
    {
      this._updateDirectory = updateDirectory;
      this._installingVersionUrl = versionSourceUrl;
      this._updateTaskCompletionSource = new TaskCompletionSource<object>();
      Task.Factory.StartNew<Task>((Func<Task>) (async () =>
      {
        try
        {
          this._logger.Trace("Start update in directory " + updateDirectory);
          if (!string.Equals(Path.GetDirectoryName(versionSourceUrl), this._hostingVersionDistributesFolder, StringComparison.OrdinalIgnoreCase))
            versionSourceUrl = Path.Combine(versionSourceUrl, this._hostingVersionDistributesFolder);
          await this._updater.UpdateAsync(this._launcherDefaultId, updateDirectory, versionSourceUrl, logKey: "launcher");
        }
        catch (Exception ex)
        {
          this._logger.Error(ex, "Problem to update in directory " + updateDirectory + " by url " + versionSourceUrl);
          throw;
        }
      }), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
      return (Task) this._updateTaskCompletionSource.Task;
    }

    public void PauseUpdate()
    {
      this._logger.Trace("Pause update..");
      this._updater.PauseUpdate(this._launcherDefaultId);
    }

    public void CancelUpdate()
    {
      this._logger.Trace("Cancel update..");
      this._updater.CancelUpdate(this._launcherDefaultId);
    }

    public async Task ResumeUpdateAsync()
    {
      this._logger.Trace("Resume update..");
      await this._updater.ResumeOrStartNewUpdateAsync(this._launcherDefaultId, this._updateDirectory, this._installingVersionUrl, "launcher");
    }

    public async Task UninstallAsync(string gameDirectory) => await this._updater.UninstallAsync(gameDirectory, string.Empty);

    public void ExtractRunner(string sourceFilePath, string destinationFilePath)
    {
      if (!File.Exists(sourceFilePath))
      {
        this._logger.Error("Runner source file does not exist in path " + sourceFilePath);
        throw new Exception("Runner source file $" + sourceFilePath + " was not found.");
      }
      File.Copy(sourceFilePath, destinationFilePath, true);
    }
  }
}
