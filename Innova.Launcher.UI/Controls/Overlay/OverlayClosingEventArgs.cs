// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Overlay.OverlayClosingEventArgs
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;

namespace Innova.Launcher.UI.Controls.Overlay
{
  public class OverlayClosingEventArgs : RoutedEventArgs
  {
    public bool IsCancelled { get; private set; }

    public object Parameter { get; }

    public OverlaySession Session { get; }

    public OverlayClosingEventArgs(OverlaySession session, object parameter)
    {
      this.Session = session ?? throw new ArgumentNullException(nameof (session));
      this.Parameter = parameter;
    }

    public OverlayClosingEventArgs(
      OverlaySession session,
      object parameter,
      RoutedEvent routedEvent)
      : base(routedEvent)
    {
      this.Session = session ?? throw new ArgumentNullException(nameof (session));
      this.Parameter = parameter;
    }

    public OverlayClosingEventArgs(
      OverlaySession session,
      object parameter,
      RoutedEvent routedEvent,
      object source)
      : base(routedEvent, source)
    {
      this.Session = session ?? throw new ArgumentNullException(nameof (session));
      this.Parameter = parameter;
    }

    public void Cancel() => this.IsCancelled = true;
  }
}
