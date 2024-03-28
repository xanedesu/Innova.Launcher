// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Converters.LoadImageConverter
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Innova.Launcher.UI.Converters
{
  public class LoadImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return (object) null;
      BitmapImage bitmapImage = new BitmapImage();
      bitmapImage.BeginInit();
      bitmapImage.UriSource = new Uri(value.ToString());
      bitmapImage.EndInit();
      return (object) bitmapImage;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return Binding.DoNothing;
    }
  }
}
