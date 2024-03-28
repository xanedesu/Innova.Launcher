// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.BorderlessWindowBehavior
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Behaviors;
using Innova.Launcher.UI.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Innova.Launcher.UI.Behaviors
{
  public class BorderlessWindowBehavior : WindowChromeBehavior
  {
    public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof (GlowBrush), typeof (Brush), typeof (WindowChromeBehavior), new PropertyMetadata((PropertyChangedCallback) null));

    public Brush GlowBrush
    {
      get => (Brush) this.GetValue(BorderlessWindowBehavior.GlowBrushProperty);
      set => this.SetValue(BorderlessWindowBehavior.GlowBrushProperty, (object) value);
    }

    protected override void OnAttached()
    {
      BindingOperations.SetBinding((DependencyObject) this, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath((object) ModernWindow.IgnoreTaskbarOnMaximizeProperty),
        Source = (object) this.AssociatedObject
      });
      BindingOperations.SetBinding((DependencyObject) this, WindowChromeBehavior.ResizeBorderThicknessProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath((object) ModernWindow.ResizeBorderThicknessProperty),
        Source = (object) this.AssociatedObject
      });
      BindingOperations.SetBinding((DependencyObject) this, BorderlessWindowBehavior.GlowBrushProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath((object) ModernWindow.GlowBrushProperty),
        Source = (object) this.AssociatedObject
      });
      base.OnAttached();
    }

    protected override void OnDetaching()
    {
      BindingOperations.ClearBinding((DependencyObject) this, WindowChromeBehavior.IgnoreTaskbarOnMaximizeProperty);
      BindingOperations.ClearBinding((DependencyObject) this, WindowChromeBehavior.ResizeBorderThicknessProperty);
      BindingOperations.ClearBinding((DependencyObject) this, BorderlessWindowBehavior.GlowBrushProperty);
      base.OnDetaching();
    }

    protected override void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
    }
  }
}
