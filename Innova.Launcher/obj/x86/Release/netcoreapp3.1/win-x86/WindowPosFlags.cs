// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.WindowPosFlags
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;

namespace Innova.Launcher
{
  [Flags]
  public enum WindowPosFlags : uint
  {
    AsyncWindowPos = 16384, // 0x00004000
    Defererase = 8192, // 0x00002000
    DrawFrame = 32, // 0x00000020
    HideWindow = 128, // 0x00000080
    NoMove = 2,
    NoZOrder = 4,
  }
}
