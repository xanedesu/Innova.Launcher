// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.EmptyLauncherUpdateService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class EmptyLauncherUpdateService : ILauncherUpdateService
  {
    public event EventHandler<AppUpdateProgressEventAgrs> UpdateProgressChanged;

    public event EventHandler UpdateCompleted;

    public event EventHandler<AppUpdateErrorEventArgs> UpdateError;

    public event EventHandler UpdateCanceled;

    public void Start(string version)
    {
      Thread.Sleep(2);
      this.OnUpdateProgressChanged(new AppUpdateProgressInfo()
      {
        Downloaded = 1000000L,
        Size = 2000000L
      });
      Thread.Sleep(2);
      this.OnUpdateCompleted();
    }

    public void Stop() => this.OnUpdateCanceled();

    public Task<string> GetMinimalAcceptableVersionForVersion(string version) => Task.Factory.StartNew<string>((Func<string>) (() => (string) null));

    public Task<long> GetTotalSizeForVersionAsync(string version) => Task.Factory.StartNew<long>((Func<long>) (() => 0L));

    public void RestartApplication()
    {
    }

    private void OnUpdateProgressChanged(AppUpdateProgressInfo e)
    {
      EventHandler<AppUpdateProgressEventAgrs> updateProgressChanged = this.UpdateProgressChanged;
      if (updateProgressChanged == null)
        return;
      updateProgressChanged((object) this, new AppUpdateProgressEventAgrs(e));
    }

    private void OnUpdateCompleted()
    {
      EventHandler updateCompleted = this.UpdateCompleted;
      if (updateCompleted == null)
        return;
      updateCompleted((object) this, EventArgs.Empty);
    }

    private void OnUpdateCanceled()
    {
      EventHandler updateCanceled = this.UpdateCanceled;
      if (updateCanceled == null)
        return;
      updateCanceled((object) this, EventArgs.Empty);
    }
  }
}
