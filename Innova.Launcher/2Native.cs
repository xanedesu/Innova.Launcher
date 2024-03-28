// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.NativeConstants
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;

namespace Innova.Launcher
{
  public static class NativeConstants
  {
    public const int SM_CXSIZEFRAME = 32;
    public const int SM_CYSIZEFRAME = 33;
    public const int SM_CXPADDEDBORDER = 92;
    public const int GWL_ID = -12;
    public const int GWL_STYLE = -16;
    public const int GWL_EXSTYLE = -20;
    public const int WM_NCLBUTTONDOWN = 161;
    public const int WM_NCRBUTTONUP = 165;
    public const uint TPM_LEFTBUTTON = 0;
    public const uint TPM_RIGHTBUTTON = 2;
    public const uint TPM_RETURNCMD = 256;
    public static readonly IntPtr TRUE = new IntPtr(1);
    public static readonly IntPtr FALSE = new IntPtr(0);
    public const uint ABM_GETSTATE = 4;
    public const int ABS_AUTOHIDE = 1;
  }
}
