// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.UniformVisiblePanel
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.UI.Controls
{
  public sealed class UniformVisiblePanel : Panel
  {
    public int RowSpacing { get; set; }

    protected override Size ArrangeOverride(Size arrangeSize)
    {
      int num = ((IEnumerable) this.InternalChildren).Cast<UIElement>().Count<UIElement>((Func<UIElement, bool>) (v => (Visibility) v.GetValue(UIElement.VisibilityProperty) == Visibility.Visible));
      if (num == 0)
        return arrangeSize;
      double width = Math.Max(0.0, arrangeSize.Width - (double) ((num - 1) * this.RowSpacing)) / (double) num;
      double x = 0.0;
      foreach (UIElement internalChild in this.InternalChildren)
      {
        internalChild.Arrange(new Rect(x, 0.0, width, arrangeSize.Height));
        x += width + (double) this.RowSpacing;
      }
      return arrangeSize;
    }

    protected override Size MeasureOverride(Size constraint)
    {
      int num1 = ((IEnumerable) this.InternalChildren).Cast<UIElement>().Count<UIElement>((Func<UIElement, bool>) (v => (Visibility) v.GetValue(UIElement.VisibilityProperty) == Visibility.Visible));
      if (num1 == 0)
        return Size.Empty;
      double width = Math.Max(0.0, constraint.Width - (double) ((num1 - 1) * this.RowSpacing)) / (double) num1;
      double num2 = 0.0;
      Size availableSize = new Size(width, constraint.Height);
      Size size = new Size(0.0, 0.0);
      foreach (UIElement internalChild in this.InternalChildren)
      {
        internalChild.Measure(availableSize);
        Size desiredSize = internalChild.DesiredSize;
        if (desiredSize.Height > num2)
        {
          desiredSize = internalChild.DesiredSize;
          num2 = desiredSize.Height;
        }
      }
      size.Height = double.IsPositiveInfinity(constraint.Height) ? num2 : constraint.Height;
      return new Size(210.0, size.Height);
    }
  }
}
