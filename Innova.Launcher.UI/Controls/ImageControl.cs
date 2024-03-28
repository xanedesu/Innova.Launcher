// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.ImageControl
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Innova.Launcher.UI.Controls
{
  public class ImageControl : Image
  {
    public static readonly RoutedEvent SourceChangedEvent = EventManager.RegisterRoutedEvent("SourceChanged", RoutingStrategy.Direct, typeof (RoutedEventHandler), typeof (ImageControl));

    static ImageControl() => Image.SourceProperty.OverrideMetadata(typeof (ImageControl), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(ImageControl.SourcePropertyChanged)));

    public event RoutedEventHandler SourceChanged
    {
      add => this.AddHandler(ImageControl.SourceChangedEvent, (Delegate) value);
      remove => this.RemoveHandler(ImageControl.SourceChangedEvent, (Delegate) value);
    }

    private static void SourcePropertyChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      Image image = obj as Image;
      if (image?.Source is BitmapImage source)
      {
        if (source.IsDownloading)
          source.DownloadCompleted += (EventHandler) ((_param1, _param2) => image.RaiseEvent(new RoutedEventArgs(ImageControl.SourceChangedEvent)));
        else
          image.RaiseEvent(new RoutedEventArgs(ImageControl.SourceChangedEvent));
      }
      else
        image?.RaiseEvent(new RoutedEventArgs(ImageControl.SourceChangedEvent));
    }
  }
}
