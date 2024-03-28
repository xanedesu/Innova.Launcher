// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.WinApiUtils
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Innova.Launcher.Shared.Utils
{
  public static class WinApiUtils
  {
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool SetWindowText(IntPtr hWnd, string windowName);

    [DllImport("user32.dll")]
    public static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern int GetWindowModuleFileName(IntPtr hWnd, StringBuilder title, int size);

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder title, int size);

    public static class WindowStateAction
    {
      public const int Hide = 0;
      public const int ShowMin = 6;
      public const int ShowMax = 3;
      public const int Restore = 9;
      public const int Show = 5;
      public const int ShowNormal = 1;
    }

    public class WindowsEnumerator : 
      IEnumerable<WinApiUtils.WindowInfo>,
      IEnumerable,
      IEnumerator<WinApiUtils.WindowInfo>,
      IEnumerator,
      IDisposable
    {
      private readonly int? _processId;
      private int _position = -1;
      private readonly List<WinApiUtils.WindowInfo> _windowsArray = new List<WinApiUtils.WindowInfo>();

      [DllImport("user32.dll")]
      private static extern int EnumWindows(
        WinApiUtils.WindowsEnumerator.EnumWindowsProc ewp,
        int lParam);

      public WindowsEnumerator() => WinApiUtils.WindowsEnumerator.EnumWindows(new WinApiUtils.WindowsEnumerator.EnumWindowsProc(this.EvalWindow), 0);

      public WindowsEnumerator(int processId)
      {
        this._processId = new int?(processId);
        WinApiUtils.WindowsEnumerator.EnumWindows(new WinApiUtils.WindowsEnumerator.EnumWindowsProc(this.EvalWindow), 0);
      }

      public IEnumerator<WinApiUtils.WindowInfo> GetEnumerator() => (IEnumerator<WinApiUtils.WindowInfo>) this;

      public bool MoveNext()
      {
        ++this._position;
        return this._position < this._windowsArray.Count;
      }

      public void Reset() => this._position = -1;

      object IEnumerator.Current => (object) this.Current;

      public WinApiUtils.WindowInfo Current => this._windowsArray[this._position];

      private bool EvalWindow(int hWnd, int lParam)
      {
        IntPtr num = (IntPtr) hWnd;
        StringBuilder title1 = new StringBuilder(256);
        WinApiUtils.GetWindowText(num, title1, 256);
        StringBuilder title2 = new StringBuilder(256);
        WinApiUtils.GetWindowModuleFileName(num, title2, 256);
        bool isVisible = WinApiUtils.IsWindowVisible(num);
        WinApiUtils.WindowInfo windowInfo = new WinApiUtils.WindowInfo(title1.ToString(), title2.ToString(), isVisible, num);
        if (this._processId.HasValue)
        {
          uint lpdwProcessId;
          int windowThreadProcessId = (int) WinApiUtils.GetWindowThreadProcessId(num, out lpdwProcessId);
          if ((long) lpdwProcessId == (long) this._processId.Value)
            this._windowsArray.Add(windowInfo);
        }
        else
          this._windowsArray.Add(windowInfo);
        return true;
      }

      public void Dispose()
      {
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

      public delegate bool EnumWindowsProc(int hWnd, int lParam);
    }

    public class WindowInfo
    {
      public WindowInfo(string title, string module, bool isVisible, IntPtr handle)
      {
        this.Title = title;
        this.Module = module;
        this.IsVisible = isVisible;
        this.Handle = handle;
      }

      public string Title { get; }

      public string Module { get; }

      public bool IsVisible { get; }

      public IntPtr Handle { get; }
    }
  }
}
