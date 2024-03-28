// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.BlurContentControl
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Innova.Launcher.UI.Controls
{
  public class BlurContentControl : ContentControl
  {
    public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof (ContentMargin), typeof (Thickness), typeof (BlurContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Thickness(), FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (ImageSource), typeof (BlurContentControl), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));

    public Thickness ContentMargin
    {
      get => (Thickness) this.GetValue(BlurContentControl.ContentMarginProperty);
      set => this.SetValue(BlurContentControl.ContentMarginProperty, (object) value);
    }

    public ImageSource ImageSource
    {
      get => (ImageSource) this.GetValue(BlurContentControl.ImageSourceProperty);
      set => this.SetValue(BlurContentControl.ImageSourceProperty, (object) value);
    }
  }
}
