// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.ProgressBarSmoothBehavior
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace Innova.Launcher.UI.Behaviors
{
  public static class ProgressBarSmoothBehavior
  {
    public static readonly DependencyProperty SmoothValueProperty = DependencyProperty.RegisterAttached("SmoothValue", typeof (double), typeof (ProgressBarSmoothBehavior), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ProgressBarSmoothBehavior.changing)));

    public static double GetSmoothValue(DependencyObject obj) => (double) obj.GetValue(ProgressBarSmoothBehavior.SmoothValueProperty);

    public static void SetSmoothValue(DependencyObject obj, double value) => obj.SetValue(ProgressBarSmoothBehavior.SmoothValueProperty, (object) value);

    private static void changing(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DoubleAnimation animation = new DoubleAnimation((double) e.OldValue, (double) e.NewValue, Duration.op_Implicit(new TimeSpan(0, 0, 0, 0, 200)));
      (d as ProgressBar).BeginAnimation(RangeBase.ValueProperty, (AnimationTimeline) animation, HandoffBehavior.Compose);
    }
  }
}
