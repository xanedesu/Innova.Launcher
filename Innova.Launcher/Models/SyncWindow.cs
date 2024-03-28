// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Models.SyncWindow
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Threading;
using System.Windows;

namespace Innova.Launcher.Models
{
  public sealed class SyncWindow
  {
    private readonly ManualResetEvent _manualResetEvent;

    public Window Window { get; }

    public SyncWindow(Window window)
    {
      this._manualResetEvent = new ManualResetEvent(false);
      this.Window = window ?? throw new ArgumentNullException(nameof (window));
      this.Window.Closed += new EventHandler(this.WindowClosed);
    }

    public void Show() => this.Window.Show();

    public void Wait() => this._manualResetEvent.WaitOne();

    private void WindowClosed(object sender, EventArgs e) => this._manualResetEvent.Set();
  }
}
