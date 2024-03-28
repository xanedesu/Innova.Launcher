// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.NativeMethods
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Innova.Launcher
{
  public static class NativeMethods
  {
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("user32.dll")]
    public static extern IntPtr SetCapture(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetCapture();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hwnd, int msg, int wparam, int lparam);

    [DllImport("user32.dll")]
    public static extern int PostMessage(IntPtr hwnd, int msg, int wparam, int lparam);

    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32.dll")]
    public static extern int TrackPopupMenuEx(
      IntPtr hmenu,
      uint fuFlags,
      int x,
      int y,
      IntPtr hwnd,
      IntPtr lptpm);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hwnd, int msg, int wparam, Points pos);

    [DllImport("user32.dll")]
    public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern int PostMessage(IntPtr hwnd, int msg, int wparam, Points pos);

    [DllImport("user32.dll")]
    public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      WindowPosFlags flags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateRectRgn(
      int nLeftRect,
      int nTopRect,
      int nRightRect,
      int nBottomRect);

    [DllImport("user32.dll")]
    public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

    [DllImport("gdi32.dll")]
    public static extern int GetRgnBox(IntPtr hrgn, out Rect lprc);

    [DllImport("user32.dll")]
    public static extern int GetWindowLong(IntPtr hWnd, int Offset);

    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int smIndex);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindowEx(
      IntPtr parentHandle,
      IntPtr childAfter,
      string className,
      string windowTitle);

    [DllImport("shell32.dll")]
    public static extern int SHAppBarMessage(uint dwMessage, [In] ref AppBarData pData);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(ref Win32Point pt);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hookPtr);

    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(
      IntPtr hookPtr,
      int nCode,
      IntPtr wordParam,
      IntPtr longParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(
      HookType hookType,
      NativeMethods.CBTProc hookProc,
      IntPtr instancePtr,
      uint threadID);

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentThreadId();

    public static Point GetMousePosition()
    {
      Win32Point pt = new Win32Point();
      NativeMethods.GetCursorPos(ref pt);
      return new Point((double) pt.X, (double) pt.Y);
    }

    public delegate IntPtr CBTProc(int code, IntPtr wParam, IntPtr lParam);
  }
}
