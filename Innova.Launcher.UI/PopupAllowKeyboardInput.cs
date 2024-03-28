// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.PopupAllowKeyboardInput
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Innova.Launcher.UI
{
  public class PopupAllowKeyboardInput
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof (bool), typeof (PopupAllowKeyboardInput), new PropertyMetadata((object) false, new PropertyChangedCallback(PopupAllowKeyboardInput.IsEnabledChanged)));

    public static bool GetIsEnabled(DependencyObject d) => (bool) d.GetValue(PopupAllowKeyboardInput.IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject d, bool value) => d.SetValue(PopupAllowKeyboardInput.IsEnabledProperty, (object) value);

    private static void IsEnabledChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs ea)
    {
      PopupAllowKeyboardInput.EnableKeyboardInput((Popup) sender, (bool) ea.NewValue);
    }

    private static void EnableKeyboardInput(Popup popup, bool enable)
    {
      if (!enable)
        return;
      IInputElement element = (IInputElement) null;
      popup.Loaded += (RoutedEventHandler) ((sender, args) =>
      {
        popup.Child.Focusable = true;
        popup.Child.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((o, ea) =>
        {
          if (!popup.Child.IsVisible)
            return;
          element = Keyboard.FocusedElement;
          Keyboard.Focus((IInputElement) popup.Child);
        });
      });
      popup.Closed += (EventHandler) ((sender, args) => Keyboard.Focus(element));
    }
  }
}
