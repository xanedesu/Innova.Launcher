// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.GameInstallerByBinaryUpdater
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services
{
  public class GameInstallerByBinaryUpdater : IExternalGameInstaller
  {
    private readonly IBinaryUpdater _binaryUpdater;
    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, int> _gamesInUpdate = new ConcurrentDictionary<string, int>();
    private int _gameCounter;

    public event EventHandler<GameInstallCompletedEventArgs> InstallCompleted;

    public event EventHandler<GameInstallErrorEventArgs> InstallError;

    public event EventHandler<GameInstallProgressInfoEventArgs> InstallProgressReceived;

    public event EventHandler UninstallCompleted;

    public GameInstallerByBinaryUpdater(IBinaryUpdater binaryUpdater, ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameInstallerByBinaryUpdater>();
      this._binaryUpdater = binaryUpdater ?? throw new ArgumentNullException(nameof (binaryUpdater));
      this._binaryUpdater.UpdateProgressReceived += new EventHandler<UpdateProgressInfoEventArgs>(this.BinaryUpdaterUpdateProgressReceived);
      this._binaryUpdater.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(this.BinaryUpdaterUpdateCompleted);
      this._binaryUpdater.UninstallCompleted += new EventHandler(this.BinaryUpdaterUninstallCompleted);
    }

    public Task InstallAsync(
      string gameKey,
      string path,
      string gameSourceUrl,
      string updaterName = null)
    {
      FileSystemHelper.CreateDirectoryIfNotExists(path);
      int gameId = this.RememberGameIfNotExists(gameKey);
      this._logger.Trace(string.Format("Install game=(id={0}, key={1}, url={2})", (object) gameId, (object) gameKey, (object) gameSourceUrl));
      Task.Run((Func<Task>) (() => this._binaryUpdater.UpdateAsync(gameId, path, gameSourceUrl, logKey: gameKey, updaterName: updaterName)));
      return Task.CompletedTask;
    }

    public Task UpdateAsync(string gameKey, string path, string gameSourceUrl, string updaterName = null)
    {
      int gameId = this.RememberGameIfNotExists(gameKey);
      this._logger.Trace(string.Format("Update game=(id={0}, key={1}, url={2})", (object) gameId, (object) gameKey, (object) gameSourceUrl));
      Task.Run((Func<Task>) (() => this._binaryUpdater.UpdateAsync(gameId, path, gameSourceUrl, logKey: gameKey, updaterName: updaterName)));
      return Task.CompletedTask;
    }

    public void PauseInstall(string gameKey)
    {
      int serviceId;
      if (!this._gamesInUpdate.TryGetValue(gameKey, out serviceId))
        return;
      this._logger.Trace(string.Format("Pause install for game=(key={0}, id={1})", (object) gameKey, (object) serviceId));
      this._binaryUpdater.PauseUpdate(serviceId);
    }

    public void CancelInstall(string gameKey)
    {
      int serviceId;
      if (!this._gamesInUpdate.TryGetValue(gameKey, out serviceId))
        return;
      this._logger.Trace(string.Format("Cancel install for game=(key={0}, id={1})", (object) gameKey, (object) serviceId));
      this._binaryUpdater.CancelUpdate(serviceId);
    }

    public async Task ResumeInstallAsync(
      string gameKey,
      string path,
      string gameSourceUrl,
      string updaterName = null)
    {
      FileSystemHelper.CreateDirectoryIfNotExists(path);
      int serviceId = this.RememberGameIfNotExists(gameKey);
      this._logger.Trace(string.Format("Resume install for game=(key={0}, id={1}, url={2})", (object) gameKey, (object) serviceId, (object) gameSourceUrl));
      await this._binaryUpdater.ResumeOrStartNewUpdateAsync(serviceId, path, gameSourceUrl, gameKey, updaterName).ConfigureAwait(false);
    }

    public void PauseUpdate(string gameKey)
    {
      int serviceId;
      if (!this._gamesInUpdate.TryGetValue(gameKey, out serviceId))
        return;
      this._logger.Trace(string.Format("Pause update for game=(key={0} id={1})", (object) gameKey, (object) serviceId));
      this._binaryUpdater.PauseUpdate(serviceId);
    }

    public void CancelUpdate(string gameKey)
    {
      int serviceId;
      if (!this._gamesInUpdate.TryGetValue(gameKey, out serviceId))
        return;
      this._logger.Trace(string.Format("Cancel update for game=(key={0} id={1})", (object) gameKey, (object) serviceId));
      this._binaryUpdater.CancelUpdate(serviceId);
    }

    public async Task ResumeUpdateAsync(
      string gameKey,
      string path,
      string gameSourceUrl,
      string updaterName = null)
    {
      FileSystemHelper.CreateDirectoryIfNotExists(path);
      int serviceId = this.RememberGameIfNotExists(gameKey);
      this._logger.Trace(string.Format("Resume update for game=(key={0} id={1}, url={2})", (object) gameKey, (object) serviceId, (object) gameSourceUrl));
      await this._binaryUpdater.ResumeOrStartNewUpdateAsync(serviceId, path, gameSourceUrl, gameKey, updaterName).ConfigureAwait(false);
    }

    public Task UninstallAsync(string gameDirectory, string gameUrl, string updaterName = null)
    {
      this._logger.Trace("Uninstall game in directory (" + gameDirectory + ")");
      return this._binaryUpdater.UninstallAsync(gameDirectory, gameUrl, updaterName);
    }

    private void BinaryUpdaterUninstallCompleted(object sender, EventArgs e) => this.OnUninstallCompleted();

    private void BinaryUpdaterUpdateCompleted(object sender, UpdateCompletedEventArgs args)
    {
      if (args.IsCancelled)
        return;
      string gameKeyOrDefault = this.GetGameKeyOrDefault(args.ServiceId);
      if (string.IsNullOrWhiteSpace(gameKeyOrDefault))
        return;
      if (args.Error != null)
        this.OnInstallError(new GameInstallErrorEventArgs(gameKeyOrDefault, args.Error.Code, args.Error.Error));
      else
        this.OnInstallCompleted(new GameInstallCompletedEventArgs(gameKeyOrDefault));
    }

    private void BinaryUpdaterUpdateProgressReceived(
      object sender,
      UpdateProgressInfoEventArgs args)
    {
      string gameKeyOrDefault = this.GetGameKeyOrDefault(args.ServiceId);
      if (string.IsNullOrWhiteSpace(gameKeyOrDefault))
        return;
      this.OnInstallProgressReceived(new GameInstallProgressInfoEventArgs(gameKeyOrDefault, args.ProgressInfo));
    }

    private void OnInstallCompleted(GameInstallCompletedEventArgs e)
    {
      EventHandler<GameInstallCompletedEventArgs> installCompleted = this.InstallCompleted;
      if (installCompleted == null)
        return;
      installCompleted((object) this, e);
    }

    private void OnInstallProgressReceived(GameInstallProgressInfoEventArgs e)
    {
      EventHandler<GameInstallProgressInfoEventArgs> progressReceived = this.InstallProgressReceived;
      if (progressReceived == null)
        return;
      progressReceived((object) this, e);
    }

    private void OnUninstallCompleted()
    {
      EventHandler uninstallCompleted = this.UninstallCompleted;
      if (uninstallCompleted == null)
        return;
      uninstallCompleted((object) this, EventArgs.Empty);
    }

    private void OnInstallError(GameInstallErrorEventArgs e)
    {
      EventHandler<GameInstallErrorEventArgs> installError = this.InstallError;
      if (installError == null)
        return;
      installError((object) this, e);
    }

    private int RememberGameIfNotExists(string gameKey) => this._gamesInUpdate.GetOrAdd(gameKey, Interlocked.Increment(ref this._gameCounter));

    private string GetGameKeyOrDefault(int gameId) => ((IEnumerable<KeyValuePair<string, int>>) this._gamesInUpdate.ToArray()).FirstOrDefault<KeyValuePair<string, int>>((Func<KeyValuePair<string, int>, bool>) (e => e.Value == gameId)).Key;
  }
}
