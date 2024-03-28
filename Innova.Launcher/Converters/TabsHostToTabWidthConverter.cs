// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Converters.TabsHostToTabWidthConverter
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Dragablz;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Innova.Launcher.Converters
{
  public class TabsHostToTabWidthConverter : IMultiValueConverter
  {
    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      TabablzControl tabablzControl = (TabablzControl) values[0];
      double num1 = (double) values[1];
      double closeButtonWidth = this.GetTabCloseButtonWidth(tabablzControl);
      double headerSuffixWidth = this.GetHeaderSuffixWidth(tabablzControl);
      int num2 = tabablzControl.VisualBreadthFirstTraversal().OfType<DragablzItem>().Count<DragablzItem>((Func<DragablzItem, bool>) (c => c.ActualWidth <= double.Epsilon));
      double num3 = tabablzControl.ActualWidth - headerSuffixWidth;
      return num3 < (num1 + closeButtonWidth) * (double) tabablzControl.Items.Count ? (object) ((num3 - closeButtonWidth * (double) (tabablzControl.Items.Count + num2)) / (double) tabablzControl.Items.Count + (double) num2) : (object) num1;
    }

    private double GetHeaderSuffixWidth(TabablzControl host)
    {
      FrameworkElement headerSuffixContent = (FrameworkElement) host.HeaderSuffixContent;
      return headerSuffixContent.ActualWidth + headerSuffixContent.Margin.Left + headerSuffixContent.Margin.Right;
    }

    private double GetTabCloseButtonWidth(TabablzControl host) => host.VisualBreadthFirstTraversal().OfType<Button>().First<Button>((Func<Button, bool>) (c => c.Name == "CloseTabButton")).Width;

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
