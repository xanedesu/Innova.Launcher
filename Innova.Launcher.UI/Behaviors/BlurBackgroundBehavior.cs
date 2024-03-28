// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.BlurBackgroundBehavior
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;

namespace Innova.Launcher.UI.Behaviors
{
  public class BlurBackgroundBehavior : Behavior<Shape>
  {
    public static readonly DependencyProperty BlurContainerProperty = DependencyProperty.Register(nameof (BlurContainer), typeof (UIElement), typeof (BlurBackgroundBehavior), new PropertyMetadata(new PropertyChangedCallback(BlurBackgroundBehavior.OnContainerChanged)));
    private static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof (Brush), typeof (VisualBrush), typeof (BlurBackgroundBehavior), new PropertyMetadata());

    private VisualBrush Brush
    {
      get => (VisualBrush) this.GetValue(BlurBackgroundBehavior.BrushProperty);
      set => this.SetValue(BlurBackgroundBehavior.BrushProperty, (object) value);
    }

    public UIElement BlurContainer
    {
      get => (UIElement) this.GetValue(BlurBackgroundBehavior.BlurContainerProperty);
      set => this.SetValue(BlurBackgroundBehavior.BlurContainerProperty, (object) value);
    }

    protected override void OnAttached()
    {
      this.AssociatedObject.Effect = (Effect) new BlurEffect()
      {
        Radius = 200.0,
        KernelType = KernelType.Gaussian,
        RenderingBias = RenderingBias.Quality
      };
      this.AssociatedObject.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
      {
        Source = (object) this,
        Path = new PropertyPath((object) BlurBackgroundBehavior.BrushProperty)
      });
      this.AssociatedObject.LayoutUpdated += (EventHandler) ((sender, args) => this.UpdateBounds());
      this.UpdateBounds();
    }

    protected override void OnDetaching() => BindingOperations.ClearBinding((DependencyObject) this.AssociatedObject, Border.BackgroundProperty);

    private static void OnContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((BlurBackgroundBehavior) d).OnContainerChanged((UIElement) e.OldValue, (UIElement) e.NewValue);

    private void OnContainerChanged(UIElement oldValue, UIElement newValue)
    {
      if (oldValue != null)
        oldValue.LayoutUpdated -= new EventHandler(this.OnContainerLayoutUpdated);
      if (newValue != null)
      {
        VisualBrush visualBrush = new VisualBrush((Visual) newValue);
        visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
        this.Brush = visualBrush;
        newValue.LayoutUpdated += new EventHandler(this.OnContainerLayoutUpdated);
        this.UpdateBounds();
      }
      else
        this.Brush = (VisualBrush) null;
    }

    private void OnContainerLayoutUpdated(object sender, EventArgs eventArgs) => this.UpdateBounds();

    private void UpdateBounds()
    {
      if (this.AssociatedObject == null || this.BlurContainer == null || this.Brush == null)
        return;
      this.Brush.Viewbox = new Rect(this.AssociatedObject.TranslatePoint(new System.Windows.Point(), this.BlurContainer), this.AssociatedObject.RenderSize);
    }
  }
}
