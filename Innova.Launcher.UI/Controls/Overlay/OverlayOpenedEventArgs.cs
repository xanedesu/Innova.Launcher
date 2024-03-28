// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Overlay.OverlayOpenedEventArgs
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;

namespace Innova.Launcher.UI.Controls.Overlay
{
  public sealed class OverlayOpenedEventArgs : RoutedEventArgs
  {
    public OverlaySession Session { get; }

    public OverlayOpenedEventArgs(OverlaySession session, RoutedEvent routedEvent)
    {
      this.Session = session ?? throw new ArgumentNullException(nameof (session));
      this.RoutedEvent = routedEvent;
    }
  }
}
