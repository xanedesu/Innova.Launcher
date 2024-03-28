// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Converters.WindowStateToVisibilityConverter
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Innova.Launcher.UI.Converters
{
  public class WindowStateToVisibilityConverter : IValueConverter
  {
    public Visibility MaximizeVisibility { get; set; }

    public Visibility RestoreVisibility { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is WindowState windowState ? (object) (Visibility) (windowState == WindowState.Maximized ? (int) this.RestoreVisibility : (int) this.MaximizeVisibility) : (object) this.MaximizeVisibility;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
