// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Helpers.WindowHelper
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Innova.Launcher.Helpers
{
  public static class WindowHelper
  {
    private const int SWRestore = 9;

    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr handle);

    [DllImport("User32.dll")]
    private static extern bool ShowWindow(IntPtr handle, int nCmdShow);

    [DllImport("User32.dll")]
    private static extern bool IsIconic(IntPtr handle);

    public static void BringProcessToFront(Process process)
    {
      IntPtr mainWindowHandle = process.MainWindowHandle;
      if (WindowHelper.IsIconic(mainWindowHandle))
        WindowHelper.ShowWindow(mainWindowHandle, 9);
      WindowHelper.SetForegroundWindow(mainWindowHandle);
    }
  }
}
