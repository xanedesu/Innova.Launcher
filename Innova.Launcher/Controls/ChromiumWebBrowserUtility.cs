// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Controls.ChromiumWebBrowserUtility
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.Controls
{
  public static class ChromiumWebBrowserUtility
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof (string), typeof (ChromiumWebBrowserUtility), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ChromiumWebBrowserUtility.TitlePropertyChanged)));
    public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached("BindableSource", typeof (string), typeof (ChromiumWebBrowserUtility), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ChromiumWebBrowserUtility.BindableSourcePropertyChanged)));

    public static string GetTitle(DependencyObject obj) => (string) obj.GetValue(ChromiumWebBrowserUtility.BindableSourceProperty);

    public static void SetTitle(DependencyObject obj, string value) => obj.SetValue(ChromiumWebBrowserUtility.BindableSourceProperty, (object) value);

    public static void TitlePropertyChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(o is WebBrowser webBrowser))
        return;
      string newValue = e.NewValue as string;
      Uri uri = !string.IsNullOrEmpty(newValue) ? new Uri(newValue) : (Uri) null;
      webBrowser.Source = uri;
    }

    public static string GetBindableSource(DependencyObject obj) => (string) obj.GetValue(ChromiumWebBrowserUtility.BindableSourceProperty);

    public static void SetBindableSource(DependencyObject obj, string value) => obj.SetValue(ChromiumWebBrowserUtility.BindableSourceProperty, (object) value);

    public static void BindableSourcePropertyChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(o is WebBrowser webBrowser))
        return;
      string newValue = e.NewValue as string;
      Uri uri = !string.IsNullOrEmpty(newValue) ? new Uri(newValue) : (Uri) null;
      webBrowser.Source = uri;
    }
  }
}
