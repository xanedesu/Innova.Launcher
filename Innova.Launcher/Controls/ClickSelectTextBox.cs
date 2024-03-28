// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Controls.ClickSelectTextBox
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Innova.Launcher.Controls
{
  public class ClickSelectTextBox : TextBox
  {
    public ClickSelectTextBox()
    {
      this.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, (Delegate) new MouseButtonEventHandler(ClickSelectTextBox.SelectivelyIgnoreMouseButton), true);
      this.AddHandler(UIElement.GotKeyboardFocusEvent, (Delegate) new RoutedEventHandler(ClickSelectTextBox.SelectAllText), true);
      this.AddHandler(Control.MouseDoubleClickEvent, (Delegate) new RoutedEventHandler(ClickSelectTextBox.SelectAllText), true);
    }

    private static void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
    {
      DependencyObject reference = (DependencyObject) (e.OriginalSource as UIElement);
      while (true)
      {
        switch (reference)
        {
          case null:
          case TextBox _:
            goto label_3;
          default:
            reference = VisualTreeHelper.GetParent(reference);
            continue;
        }
      }
label_3:
      if (reference == null)
        return;
      TextBox textBox = (TextBox) reference;
      if (textBox.IsKeyboardFocusWithin)
        return;
      textBox.Focus();
      e.Handled = true;
    }

    private static void SelectAllText(object sender, RoutedEventArgs e)
    {
      if (!(e.OriginalSource is TextBox originalSource))
        return;
      originalSource.SelectAll();
    }

    public override void EndInit()
    {
      this.ContextMenuOpening += new ContextMenuEventHandler(this.OnContextMenuOpening);
      base.EndInit();
    }

    private void OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
      object source = e.Source;
    }
  }
}
