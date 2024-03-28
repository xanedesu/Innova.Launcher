// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.ButtonTextBlock
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Innova.Launcher.UI.Controls
{
  public class ButtonTextBlock : ContentControl
  {
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(nameof (ButtonContent), typeof (object), typeof (ButtonTextBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(nameof (ButtonCommand), typeof (ICommand), typeof (ButtonTextBlock), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));

    public object ButtonContent
    {
      get => this.GetValue(ButtonTextBlock.ButtonContentProperty);
      set => this.SetValue(ButtonTextBlock.ButtonContentProperty, value);
    }

    public ICommand ButtonCommand
    {
      get => (ICommand) this.GetValue(ButtonTextBlock.ButtonCommandProperty);
      set => this.SetValue(ButtonTextBlock.ButtonCommandProperty, (object) value);
    }
  }
}
