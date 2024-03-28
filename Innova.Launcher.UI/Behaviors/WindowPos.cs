// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.WindowPos
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Standard;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Innova.Launcher.UI.Behaviors
{
  public sealed class WindowPos : DependencyObject
  {
    public static readonly DependencyProperty IsLockedProperty = DependencyProperty.RegisterAttached("IsLocked", typeof (bool), typeof (WindowPos), new PropertyMetadata((object) false, new PropertyChangedCallback(WindowPos.IsLocked_Changed)));
    private static readonly DependencyProperty IsHookedProperty = DependencyProperty.RegisterAttached("IsHooked", typeof (WindowPos.WindowLockHook), typeof (WindowPos), new PropertyMetadata((PropertyChangedCallback) null));

    public static bool GetIsLocked(DependencyObject obj) => (bool) obj.GetValue(WindowPos.IsLockedProperty);

    public static void SetIsLocked(DependencyObject obj, bool value) => obj.SetValue(WindowPos.IsLockedProperty, (object) value);

    private static void IsLocked_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      Window window = (Window) d;
      if (d.GetValue(WindowPos.IsHookedProperty) != null)
        return;
      WindowPos.WindowLockHook windowLockHook = new WindowPos.WindowLockHook(window);
      d.SetValue(WindowPos.IsHookedProperty, (object) windowLockHook);
    }

    private class WindowLockHook
    {
      private readonly Window _window;

      public WindowLockHook(Window window)
      {
        this._window = window;
        if (!(PresentationSource.FromVisual((Visual) window) is HwndSource hwndSource))
          window.SourceInitialized += new EventHandler(this.Window_SourceInitialized);
        else
          hwndSource.AddHook(new HwndSourceHook(this.WndProc));
      }

      private void Window_SourceInitialized(object sender, EventArgs e) => ((HwndSource) PresentationSource.FromVisual((Visual) this._window))?.AddHook(new HwndSourceHook(this.WndProc));

      public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
      {
        if (msg == 70 && WindowPos.GetIsLocked((DependencyObject) this._window))
        {
          WINDOWPOS structure = Marshal.PtrToStructure<WINDOWPOS>(lParam);
          structure.flags |= SWP.NOMOVE | SWP.NOSIZE;
          Marshal.StructureToPtr<WINDOWPOS>(structure, lParam, false);
        }
        return IntPtr.Zero;
      }
    }
  }
}
