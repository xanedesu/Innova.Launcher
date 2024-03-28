// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Helpers.WindowHelper
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Innova.Launcher.UI.Helpers
{
  public static class WindowHelper
  {
    public static void AnimationResize(Window window, double width, double height)
    {
      double fromWidth = window.ActualWidth;
      double fromHeight = window.ActualHeight;
      window.BeginInit();
      window.Dispatcher.BeginInvoke((Delegate) (() =>
      {
        DoubleAnimation animation1 = new DoubleAnimation()
        {
          Duration = new Duration(TimeSpan.FromMilliseconds(400.0)),
          From = new double?(fromWidth),
          To = new double?(width),
          FillBehavior = FillBehavior.HoldEnd
        };
        DoubleAnimation animation2 = new DoubleAnimation()
        {
          Duration = new Duration(TimeSpan.FromMilliseconds(400.0)),
          From = new double?(fromHeight),
          To = new double?(height),
          FillBehavior = FillBehavior.HoldEnd
        };
        window.BeginAnimation(FrameworkElement.WidthProperty, (AnimationTimeline) animation1, HandoffBehavior.Compose);
        window.BeginAnimation(FrameworkElement.HeightProperty, (AnimationTimeline) animation2, HandoffBehavior.Compose);
      }), Array.Empty<object>());
      window.EndInit();
    }
  }
}
