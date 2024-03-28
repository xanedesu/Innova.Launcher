// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Glow
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Innova.Launcher.UI.Controls
{
  public class Glow : Control
  {
    public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof (GlowBrush), typeof (Brush), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Transparent));
    public static readonly DependencyProperty NonActiveGlowBrushProperty = DependencyProperty.Register(nameof (NonActiveGlowBrush), typeof (Brush), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Brushes.Transparent));
    public static readonly DependencyProperty IsGlowProperty = DependencyProperty.Register(nameof (IsGlow), typeof (bool), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) true));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) Orientation.Vertical));
    public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof (Direction), typeof (GlowDirection), typeof (Glow), (PropertyMetadata) new UIPropertyMetadata((object) GlowDirection.Top));

    static Glow() => FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Glow), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Glow)));

    public Brush GlowBrush
    {
      get => (Brush) this.GetValue(Glow.GlowBrushProperty);
      set => this.SetValue(Glow.GlowBrushProperty, (object) value);
    }

    public Brush NonActiveGlowBrush
    {
      get => (Brush) this.GetValue(Glow.NonActiveGlowBrushProperty);
      set => this.SetValue(Glow.NonActiveGlowBrushProperty, (object) value);
    }

    public bool IsGlow
    {
      get => (bool) this.GetValue(Glow.IsGlowProperty);
      set => this.SetValue(Glow.IsGlowProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) this.GetValue(Glow.OrientationProperty);
      set => this.SetValue(Glow.OrientationProperty, (object) value);
    }

    public GlowDirection Direction
    {
      get => (GlowDirection) this.GetValue(Glow.DirectionProperty);
      set => this.SetValue(Glow.DirectionProperty, (object) value);
    }
  }
}
