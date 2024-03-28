// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.ButtonAssist
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Media;

namespace Innova.Launcher.UI
{
  public static class ButtonAssist
  {
    public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.RegisterAttached("MouseOverBrush", typeof (Brush), typeof (ButtonAssist), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));

    public static Brush GetMouseOverBrush(DependencyObject element) => (Brush) element.GetValue(ButtonAssist.MouseOverBrushProperty);

    public static void SetMouseOverBrush(DependencyObject element, Brush value) => element.SetValue(ButtonAssist.MouseOverBrushProperty, (object) value);
  }
}
