// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Overlay.OverlaySession
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows.Threading;

namespace Innova.Launcher.UI.Controls.Overlay
{
  public class OverlaySession
  {
    private readonly OverlayHost _owner;

    internal OverlaySession(OverlayHost owner) => this._owner = owner ?? throw new ArgumentNullException(nameof (owner));

    public bool IsEnded { get; internal set; }

    public object Content => this._owner.OverlayContent;

    public void UpdateContent(object content)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      this._owner.AssertTargetableContent();
      this._owner.OverlayContent = content;
      this._owner.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this._owner.FocusPopup()));
    }

    public void Close()
    {
      if (this.IsEnded)
        throw new InvalidOperationException("Overlay session has ended.");
      this._owner.Close((object) null);
    }

    public void Close(object parameter)
    {
      if (this.IsEnded)
        throw new InvalidOperationException("Overlay session has ended.");
      this._owner.Close(parameter);
    }
  }
}
