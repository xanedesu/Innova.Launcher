// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LifetimeManager
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Threading;

namespace Innova.Launcher.Shared.Services
{
  public class LifetimeManager : ILifetimeManager
  {
    private readonly CancellationTokenSource _tokenSource;

    public event EventHandler TimeToDie;

    public LifetimeManager() => this._tokenSource = new CancellationTokenSource();

    public bool IsAlive => !this._tokenSource.IsCancellationRequested;

    public void Die()
    {
      this._tokenSource.Cancel();
      this.OnTimeToDie();
    }

    public CancellationToken CancellationToken => this._tokenSource.Token;

    protected virtual void OnTimeToDie()
    {
      EventHandler timeToDie = this.TimeToDie;
      if (timeToDie == null)
        return;
      timeToDie((object) this, EventArgs.Empty);
    }
  }
}
