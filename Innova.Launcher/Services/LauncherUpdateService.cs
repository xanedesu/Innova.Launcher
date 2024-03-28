// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.LauncherUpdateService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.Updater.Core.Services;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using Innova.Launcher.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace Innova.Launcher.Services
{
  public class LauncherUpdateService : ILauncherUpdateService
  {
    private readonly IMainShellPresenter _shell;
    private readonly ILauncherConfigurationProvider _configurationProvider;
    private readonly ILauncherStructureProvider _launcherStructureProvider;
    private readonly IEnvironmentProvider _environmentProvider;
    private readonly LauncherBinaryUpdater _launcherUpdater;
    private readonly ILauncherInSystemRegistrator _launcherInSystemRegistrator;
    private readonly ILauncherStateService _launcherStateService;
    private readonly ILauncherStartupParametersParser _launcherStartupParametersParser;
    private LauncherRegistrationDataUpdate _updateInfo;

    public event EventHandler<AppUpdateProgressEventAgrs> UpdateProgressChanged;

    public event EventHandler UpdateCompleted;

    public event EventHandler<AppUpdateErrorEventArgs> UpdateError;

    public event EventHandler UpdateCanceled;

    public LauncherUpdateService(
      MainShell shell,
      ILauncherConfigurationProvider configurationProvider,
      ILauncherStructureProvider launcherStructureProvider,
      IEnvironmentProvider environmentProvider,
      LauncherBinaryUpdater launcherUpdater,
      ILauncherInSystemRegistrator launcherInSystemRegistrator,
      ILauncherStateService launcherStateService,
      ILauncherStartupParametersParser launcherStartupParametersParser)
    {
      this._shell = (IMainShellPresenter) shell;
      this._configurationProvider = configurationProvider;
      this._launcherStructureProvider = launcherStructureProvider;
      this._environmentProvider = environmentProvider;
      this._launcherInSystemRegistrator = launcherInSystemRegistrator;
      this._launcherStateService = launcherStateService;
      this._launcherStartupParametersParser = launcherStartupParametersParser;
      this._launcherUpdater = launcherUpdater;
      this._launcherUpdater.UpdateProgressReceived += new EventHandler<UpdateProgressInfoEventArgs>(this.LauncherUpdateProgressReceived);
      this._launcherUpdater.UpdateCompleted += new EventHandler<UpdateCompletedEventArgs>(this.LauncherUpdateCompleted);
    }

    public void Start(string version)
    {
      this._launcherUpdater.UpdateAsync(this._launcherStructureProvider.GetLauncherTempUpdateFolderPath(), this._environmentProvider.GetVersionHostingPath(version));
      LauncherVersionReleaseInfo versionReleaseInfo = this._environmentProvider.GetVersionReleaseInfo(this._launcherStateService.CurrentEnvironment, version);
      LauncherRegistrationDataUpdate registrationDataUpdate = new LauncherRegistrationDataUpdate();
      registrationDataUpdate.Key = this._launcherStateService.LauncherKey;
      registrationDataUpdate.Name = this._launcherStateService.LauncherName;
      registrationDataUpdate.Version = version;
      registrationDataUpdate.UpdateDate = DateTime.Now;
      long? sizeKilobytes = versionReleaseInfo.SizeKilobytes;
      long num = 1024;
      registrationDataUpdate.SizeBytes = (sizeKilobytes.HasValue ? new long?(sizeKilobytes.GetValueOrDefault() * num) : new long?()).GetValueOrDefault();
      this._updateInfo = registrationDataUpdate;
    }

    public void Stop() => this._launcherUpdater.CancelUpdate();

    public void RestartApplication()
    {
      string runnerExe = this._launcherStructureProvider.GetRunnerExeFilePath();
      LauncherStartupParameters startupParameters = this._configurationProvider.StartupParameters;
      string restartArgsString = this._launcherStartupParametersParser.SerializeParameters(new LauncherStartupParameters()
      {
        Environment = startupParameters.Environment,
        StartPage = HttpUtility.UrlDecode(startupParameters.StartPage),
        GamesConfigUrl = startupParameters.GamesConfigUrl,
        SingleGamesConfigUrl = startupParameters.SingleGamesConfigUrl,
        SingleGamesValidationUrl = startupParameters.SingleGamesValidationUrl
      });
      Task.Delay(TimeSpan.FromMilliseconds(100.0)).ContinueWith((Action<Task>) (t => this.GuiNoWait((Action) (() =>
      {
        this._shell?.HideAndClose();
        Application.Current.Exit += (ExitEventHandler) ((s, e) => Process.Start(new ProcessStartInfo()
        {
          FileName = runnerExe,
          WorkingDirectory = Path.GetDirectoryName(runnerExe),
          Arguments = restartArgsString,
          UseShellExecute = true
        }));
      }))));
    }

    private void LauncherUpdateProgressReceived(object sender, UpdateProgressInfoEventArgs e)
    {
      if (e.ProgressInfo.Size < 1L)
        return;
      this.OnUpdateProgressChanged(new AppUpdateProgressInfo()
      {
        Size = this.ConvertBytesToKilobytes(e.ProgressInfo.Size),
        Downloaded = this.ConvertBytesToKilobytes(e.ProgressInfo.Downloaded)
      });
    }

    private void LauncherUpdateCompleted(object sender, UpdateCompletedEventArgs e)
    {
      if (e.Error != null)
        this.OnUpdateError(new Exception(e.Error.Error));
      else if (e.IsCancelled)
      {
        this.OnUpdateCanceled();
      }
      else
      {
        try
        {
          this._launcherInSystemRegistrator.Update(this._updateInfo);
        }
        catch (Exception ex)
        {
          this.OnUpdateError(new Exception("Launcher registration error", ex));
          return;
        }
        try
        {
          this._launcherUpdater.ExtractRunner(Path.Combine(this._launcherStructureProvider.GetLauncherTempUpdateFolderPath(), this._launcherStructureProvider.GetRunnerFromUpdateExeFileName()), this._launcherStructureProvider.GetRunnerExeFilePath());
        }
        catch (Exception ex)
        {
          this.OnUpdateError(new Exception("Launcher update error", ex));
          return;
        }
        File.WriteAllText(this._launcherStructureProvider.GetUpdateVersionFilePath(), this._updateInfo.Version);
        this.OnUpdateCompleted();
      }
    }

    private void OnUpdateProgressChanged(AppUpdateProgressInfo obj)
    {
      EventHandler<AppUpdateProgressEventAgrs> updateProgressChanged = this.UpdateProgressChanged;
      if (updateProgressChanged == null)
        return;
      updateProgressChanged((object) this, new AppUpdateProgressEventAgrs(obj));
    }

    private void OnUpdateCompleted()
    {
      EventHandler updateCompleted = this.UpdateCompleted;
      if (updateCompleted == null)
        return;
      updateCompleted((object) this, EventArgs.Empty);
    }

    private void OnUpdateError(Exception exception)
    {
      EventHandler<AppUpdateErrorEventArgs> updateError = this.UpdateError;
      if (updateError == null)
        return;
      updateError((object) this, new AppUpdateErrorEventArgs(exception));
    }

    private void OnUpdateCanceled()
    {
      EventHandler updateCanceled = this.UpdateCanceled;
      if (updateCanceled == null)
        return;
      updateCanceled((object) this, EventArgs.Empty);
    }

    private long ConvertBytesToKilobytes(long bytes) => bytes < 1024L ? 1L : bytes / 1024L;
  }
}
