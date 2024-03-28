// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Converters.DynamicStringFormatConverter
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Innova.Launcher.Shared.Localization.Converters
{
  public class DynamicStringFormatConverter : MarkupExtension, IMultiValueConverter
  {
    private BindingExpressionBase _binding;
    private string _format;

    public string Format
    {
      get => this._format;
      set
      {
        this._format = value;
        Application.Current.Dispatcher.BeginInvoke((Delegate) (() =>
        {
          try
          {
            this._binding?.UpdateTarget();
          }
          catch
          {
          }
        }), Array.Empty<object>());
      }
    }

    public DependencyProperty Property { get; set; }

    public virtual object ProvideValue(IServiceProvider serviceProvider) => (object) this;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values == null || !(values[0] is DependencyObject target) || this.Property == null)
        return (object) null;
      this._binding = BindingOperations.GetBindingExpressionBase(target, this.Property);
      string format = this.Format;
      values = ((IEnumerable<object>) values).Skip<object>(1).ToArray<object>();
      return string.IsNullOrWhiteSpace(format) ? (object) string.Concat(values) : (object) string.Format((IFormatProvider) culture, format, values);
    }

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
