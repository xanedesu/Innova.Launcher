// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Converters.BooleanToWidthConverter
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Globalization;
using System.Windows.Data;

namespace Innova.Launcher.UI.Converters
{
  [ValueConversion(typeof (bool), typeof (double))]
  public class BooleanToWidthConverter : IValueConverter
  {
    public double TrueValue { get; set; }

    public double FalseValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool flag ? (object) (flag ? this.TrueValue : this.FalseValue) : (object) this.FalseValue;

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
