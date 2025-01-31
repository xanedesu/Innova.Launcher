﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.WindowStyle
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;

namespace Innova.Launcher
{
  [Flags]
  public enum WindowStyle
  {
    WS_OVERLAPPED = 0,
    WS_POPUP = -2147483648, // 0x80000000
    WS_CHILD = 1073741824, // 0x40000000
    WS_MINIMIZE = 536870912, // 0x20000000
    WS_VISIBLE = 268435456, // 0x10000000
    WS_DISABLED = 134217728, // 0x08000000
    WS_CLIPSIBLINGS = 67108864, // 0x04000000
    WS_CLIPCHILDREN = 33554432, // 0x02000000
    WS_MAXIMIZE = 16777216, // 0x01000000
    WS_CAPTION = 12582912, // 0x00C00000
    WS_BORDER = 8388608, // 0x00800000
    WS_DLGFRAME = 4194304, // 0x00400000
    WS_VSCROLL = 2097152, // 0x00200000
    WS_HSCROLL = 1048576, // 0x00100000
    WS_SYSMENU = 524288, // 0x00080000
    WS_THICKFRAME = 262144, // 0x00040000
    WS_GROUP = 131072, // 0x00020000
    WS_TABSTOP = 65536, // 0x00010000
    WS_MINIMIZEBOX = WS_GROUP, // 0x00020000
    WS_MAXIMIZEBOX = WS_TABSTOP, // 0x00010000
    WS_TILED = 0,
    WS_ICONIC = WS_MINIMIZE, // 0x20000000
    WS_SIZEBOX = WS_THICKFRAME, // 0x00040000
    WS_TILEDWINDOW = WS_SIZEBOX | WS_MAXIMIZEBOX | WS_MINIMIZEBOX | WS_SYSMENU | WS_DLGFRAME | WS_BORDER, // 0x00CF0000
    WS_OVERLAPPEDWINDOW = WS_TILEDWINDOW, // 0x00CF0000
    WS_POPUPWINDOW = WS_SYSMENU | WS_BORDER | WS_POPUP, // 0x80880000
    WS_CHILDWINDOW = WS_CHILD, // 0x40000000
  }
}
