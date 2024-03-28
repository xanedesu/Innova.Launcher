// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.ButtonTextBox
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Innova.Launcher.UI.Controls
{
  public class ButtonTextBox : TextBox
  {
    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(nameof (ButtonContent), typeof (object), typeof (ButtonTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(nameof (ButtonCommand), typeof (ICommand), typeof (ButtonTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.AffectsRender));

    public object ButtonContent
    {
      get => this.GetValue(ButtonTextBox.ButtonContentProperty);
      set => this.SetValue(ButtonTextBox.ButtonContentProperty, value);
    }

    public ICommand ButtonCommand
    {
      get => (ICommand) this.GetValue(ButtonTextBox.ButtonCommandProperty);
      set => this.SetValue(ButtonTextBox.ButtonCommandProperty, (object) value);
    }
  }
}
