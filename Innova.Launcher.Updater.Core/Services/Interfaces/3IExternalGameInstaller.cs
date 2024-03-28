// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.Interfaces.IExternalGameInstaller
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services.Interfaces
{
  public interface IExternalGameInstaller
  {
    event EventHandler<GameInstallCompletedEventArgs> InstallCompleted;

    event EventHandler<GameInstallProgressInfoEventArgs> InstallProgressReceived;

    event EventHandler<GameInstallErrorEventArgs> InstallError;

    event EventHandler UninstallCompleted;

    Task InstallAsync(string gameKey, string path, string gameSourceUrl, string updaterName = null);

    Task UpdateAsync(string gameKey, string path, string gameSourceUrl, string updaterName = null);

    Task UninstallAsync(string gameDirectory, string gameUrl, string updaterName = null);

    Task ResumeInstallAsync(string gameKey, string path, string gameSourceUrl, string updaterName = null);

    Task ResumeUpdateAsync(string gameKey, string path, string gameSourceUrl, string updaterName = null);

    void PauseInstall(string gameKey);

    void CancelInstall(string gameKey);

    void PauseUpdate(string gameKey);

    void CancelUpdate(string gameKey);
  }
}
