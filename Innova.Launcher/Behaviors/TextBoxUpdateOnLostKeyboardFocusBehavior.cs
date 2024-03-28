// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Behaviors.TextBoxUpdateOnLostKeyboardFocusBehavior
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Innova.Launcher.Behaviors
{
  public class TextBoxUpdateOnLostKeyboardFocusBehavior : Behavior<TextBox>
  {
    protected override void OnAttached()
    {
      if (this.AssociatedObject == null)
        return;
      base.OnAttached();
      this.AssociatedObject.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(this.OnKeyboardLostFocus);
    }

    protected override void OnDetaching()
    {
      if (this.AssociatedObject == null)
        return;
      this.AssociatedObject.LostKeyboardFocus -= new KeyboardFocusChangedEventHandler(this.OnKeyboardLostFocus);
      base.OnDetaching();
    }

    private void OnKeyboardLostFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      if (!(sender is TextBox element) || e.NewFocus != null)
        return;
      FrameworkElement parent = (FrameworkElement) element.Parent;
      while (parent != null && !parent.Focusable)
        parent = (FrameworkElement) parent.Parent;
      FocusManager.SetFocusedElement(FocusManager.GetFocusScope((DependencyObject) element), (IInputElement) parent);
    }
  }
}
