// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.MouseHook
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;

namespace Innova.Launcher
{
  public struct MouseHook
  {
    public Win32Point pt;
    public IntPtr hwnd;
    public uint wHitTestCode;
    public IntPtr dwExtraInfo;
  }
}
