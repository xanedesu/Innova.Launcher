// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.WebBrowserUtility
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.Services
{
  public static class WebBrowserUtility
  {
    public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title", typeof (string), typeof (WebBrowserUtility), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(WebBrowserUtility.TitlePropertyChanged)));
    public static readonly DependencyProperty BindableSourceProperty = DependencyProperty.RegisterAttached("BindableSource", typeof (string), typeof (WebBrowserUtility), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(WebBrowserUtility.BindableSourcePropertyChanged)));

    public static string GetTitle(DependencyObject obj) => (string) obj.GetValue(WebBrowserUtility.BindableSourceProperty);

    public static void SetTitle(DependencyObject obj, string value) => obj.SetValue(WebBrowserUtility.BindableSourceProperty, (object) value);

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

    public static string GetBindableSource(DependencyObject obj) => (string) obj.GetValue(WebBrowserUtility.BindableSourceProperty);

    public static void SetBindableSource(DependencyObject obj, string value) => obj.SetValue(WebBrowserUtility.BindableSourceProperty, (object) value);

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
