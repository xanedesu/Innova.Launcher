// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.Interfaces.IBinaryUpdater
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;
using System.Threading.Tasks;

namespace Innova.Launcher.Updater.Core.Services.Interfaces
{
  public interface IBinaryUpdater
  {
    event EventHandler<UpdateCompletedEventArgs> UpdateCompleted;

    event EventHandler<UpdateProgressInfoEventArgs> UpdateProgressReceived;

    event EventHandler UninstallCompleted;

    Task UpdateAsync(
      int serviceId,
      string path,
      string sourceUrl,
      bool fullUpdate = true,
      string logKey = null,
      string updaterName = null);

    Task ResumeOrStartNewUpdateAsync(
      int serviceId,
      string path,
      string sourceUrl,
      string logKey = null,
      string updaterName = null);

    Task UninstallAsync(string path, string url, string updaterName = null);

    Task UpdateWithoutProgressAsync(string path, string url, Action finishCallback);

    void PauseUpdate(int serviceId);

    void CancelUpdate(int serviceId);
  }
}
