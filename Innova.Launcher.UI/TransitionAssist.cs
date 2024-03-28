// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.TransitionAssist
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;

namespace Innova.Launcher.UI
{
  public static class TransitionAssist
  {
    public static readonly DependencyProperty DisableTransitionsProperty = DependencyProperty.RegisterAttached("DisableTransitions", typeof (bool), typeof (TransitionAssist), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));

    public static void SetDisableTransitions(DependencyObject element, bool value) => element.SetValue(TransitionAssist.DisableTransitionsProperty, (object) value);

    public static bool GetDisableTransitions(DependencyObject element) => (bool) element.GetValue(TransitionAssist.DisableTransitionsProperty);
  }
}
