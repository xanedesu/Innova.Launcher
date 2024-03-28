// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.ValidationAssist
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;

namespace Innova.Launcher.UI
{
  public static class ValidationAssist
  {
    public static readonly DependencyProperty UsePopupProperty = DependencyProperty.RegisterAttached("UsePopup", typeof (bool), typeof (ValidationAssist), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));
    public static readonly DependencyProperty SuppressProperty = DependencyProperty.RegisterAttached("Suppress", typeof (bool), typeof (ValidationAssist), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetUsePopup(DependencyObject element) => (bool) element.GetValue(ValidationAssist.UsePopupProperty);

    public static void SetUsePopup(DependencyObject element, bool value) => element.SetValue(ValidationAssist.UsePopupProperty, (object) value);

    public static void SetSuppress(DependencyObject element, bool value) => element.SetValue(ValidationAssist.SuppressProperty, (object) value);

    public static bool GetSuppress(DependencyObject element) => (bool) element.GetValue(ValidationAssist.SuppressProperty);
  }
}
