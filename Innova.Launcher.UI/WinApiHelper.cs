// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.WinApiHelper
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Standard;
using System;
using System.Windows;

namespace Innova.Launcher.UI
{
  public static class WinApiHelper
  {
    public static Point GetRelativeMousePosition(IntPtr hWnd)
    {
      if (hWnd == IntPtr.Zero)
        return new Point();
      POINT physicalCursorPos = WinApiHelper.GetPhysicalCursorPos();
      NativeMethods.ScreenToClient(hWnd, ref physicalCursorPos);
      return new Point((double) physicalCursorPos.X, (double) physicalCursorPos.Y);
    }

    public static bool TryGetRelativeMousePosition(IntPtr hWnd, out Point point)
    {
      POINT pt = new POINT();
      int num = !(hWnd != IntPtr.Zero) ? 0 : (NativeMethods.TryGetPhysicalCursorPos(out pt) ? 1 : 0);
      if (num != 0)
      {
        NativeMethods.ScreenToClient(hWnd, ref pt);
        point = new Point((double) pt.X, (double) pt.Y);
        return num != 0;
      }
      point = new Point();
      return num != 0;
    }

    internal static POINT GetPhysicalCursorPos()
    {
      try
      {
        return NativeMethods.GetPhysicalCursorPos();
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Uups, this should not happen! Sorry for this exception! Is this maybe happend on a virtual machine or via remote desktop? Please let us know, thx.", ex);
      }
    }
  }
}
