// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Converters.CustomBooleanToVisibilityConverter
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Innova.Launcher.UI.Converters
{
  [ValueConversion(typeof (bool), typeof (Visibility))]
  public class CustomBooleanToVisibilityConverter : IValueConverter
  {
    public Visibility TrueValue { get; set; }

    public Visibility FalseValue { get; set; } = Visibility.Collapsed;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is bool flag1)
        return (object) (Visibility) (flag1 ? (int) this.TrueValue : (int) this.FalseValue);
      return parameter is bool flag2 ? (object) (Visibility) (flag2 ? (int) this.TrueValue : (int) this.FalseValue) : (object) null;
    }

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      if (object.Equals(value, (object) this.TrueValue))
        return (object) true;
      if (object.Equals(value, (object) this.FalseValue))
        return (object) false;
      if (object.Equals(parameter, (object) this.TrueValue))
        return (object) true;
      return object.Equals(parameter, (object) this.FalseValue) ? (object) false : (object) null;
    }
  }
}
