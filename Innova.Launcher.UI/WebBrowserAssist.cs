// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.WebBrowserAssist
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.UI
{
  public static class WebBrowserAssist
  {
    public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached("BindableSource", typeof (string), typeof (WebBrowserAssist), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(WebBrowserAssist.BindableSourcePropertyChanged)));

    public static string GetBindableSource(DependencyObject obj) => (string) obj.GetValue(WebBrowserAssist.BindableSourceProperty);

    public static void SetBindableSource(DependencyObject obj, string value) => obj.SetValue(WebBrowserAssist.BindableSourceProperty, (object) value);

    public static void BindableSourcePropertyChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(o is WebBrowser webBrowser) || !(e.NewValue is string newValue))
        return;
      webBrowser.Navigate(newValue);
    }
  }
}
