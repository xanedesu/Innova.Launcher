// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.HookType
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

namespace Innova.Launcher
{
  public enum HookType
  {
    WH_MIN = -1, // 0xFFFFFFFF
    WH_MSGFILTER = -1, // 0xFFFFFFFF
    WH_JOURNALRECORD = 0,
    WH_JOURNALPLAYBACK = 1,
    WH_KEYBOARD = 2,
    WH_GETMESSAGE = 3,
    WH_CALLWNDPROC = 4,
    WH_CBT = 5,
    WH_SYSMSGFILTER = 6,
    WH_MOUSE = 7,
    WH_HARDWARE = 8,
    WH_DEBUG = 9,
    WH_SHELL = 10, // 0x0000000A
    WH_FOREGROUNDIDLE = 11, // 0x0000000B
    WH_CALLWNDPROCRET = 12, // 0x0000000C
    WH_KEYBOARD_LL = 13, // 0x0000000D
    WH_MOUSE_LL = 14, // 0x0000000E
  }
}
