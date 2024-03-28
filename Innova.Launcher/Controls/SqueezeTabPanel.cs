// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Controls.SqueezeTabPanel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Innova.Launcher.Controls
{
  public class SqueezeTabPanel : TabPanel
  {
    protected override Size MeasureOverride(Size availableSize)
    {
      foreach (UIElement child in this.Children)
        child.Measure(availableSize);
      double height = ((IEnumerable) this.Children).OfType<UIElement>().Select<UIElement, double>((Func<UIElement, double>) (v => v.DesiredSize.Height)).DefaultIfEmpty<double>(0.0).Max();
      double val1 = ((IEnumerable) this.Children).OfType<UIElement>().Sum<UIElement>((Func<UIElement, double>) (v => v.DesiredSize.Width));
      if (val1 > availableSize.Width)
      {
        double width = availableSize.Width / (double) this.Children.Count;
        foreach (UIElement child in this.Children)
          child.Measure(new Size(width, height));
      }
      return new Size(Math.Min(val1, availableSize.Width), height);
    }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      if (((IEnumerable) this.Children).OfType<UIElement>().Sum<UIElement>((Func<UIElement, double>) (v => v.DesiredSize.Width)) > arrangeSize.Width)
      {
        double width = arrangeSize.Width / (double) this.Children.Count;
        double x = 0.0;
        foreach (UIElement child in this.Children)
        {
          child.Arrange(new System.Windows.Rect(x, 0.0, width, child.DesiredSize.Height));
          x += width;
        }
      }
      else
      {
        double num1 = 0.0;
        foreach (UIElement child in this.Children)
        {
          UIElement uiElement = child;
          double x = num1;
          Size desiredSize = child.DesiredSize;
          double width1 = desiredSize.Width;
          desiredSize = child.DesiredSize;
          double height = desiredSize.Height;
          System.Windows.Rect finalRect = new System.Windows.Rect(x, 0.0, width1, height);
          uiElement.Arrange(finalRect);
          double num2 = num1;
          desiredSize = child.DesiredSize;
          double width2 = desiredSize.Width;
          num1 = num2 + width2;
        }
      }
      return arrangeSize;
    }
  }
}
