// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.CaptionAssist
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;

namespace Innova.Launcher.UI
{
  public static class CaptionAssist
  {
    public static readonly DependencyProperty CaptionProperty = DependencyProperty.RegisterAttached("Caption", typeof (string), typeof (CaptionAssist), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.Inherits));

    public static bool GetCaption(DependencyObject element) => (bool) element.GetValue(CaptionAssist.CaptionProperty);

    public static void SetCaption(DependencyObject element, string value) => element.SetValue(CaptionAssist.CaptionProperty, (object) value);
  }
}
