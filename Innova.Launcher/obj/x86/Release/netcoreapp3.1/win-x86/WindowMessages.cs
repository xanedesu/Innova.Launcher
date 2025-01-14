﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.WindowMessages
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

namespace Innova.Launcher
{
  public enum WindowMessages
  {
    WM_NULL = 0,
    WM_CREATE = 1,
    WM_DESTROY = 2,
    WM_MOVE = 3,
    WM_SIZE = 5,
    WM_ACTIVATE = 6,
    WM_SETFOCUS = 7,
    WM_KILLFOCUS = 8,
    WM_ENABLE = 10, // 0x0000000A
    WM_SETREDRAW = 11, // 0x0000000B
    WM_SETTEXT = 12, // 0x0000000C
    WM_GETTEXT = 13, // 0x0000000D
    WM_GETTEXTLENGTH = 14, // 0x0000000E
    WM_PAINT = 15, // 0x0000000F
    WM_CLOSE = 16, // 0x00000010
    WM_QUIT = 18, // 0x00000012
    WM_ERASEBKGND = 20, // 0x00000014
    WM_SYSCOLORCHANGE = 21, // 0x00000015
    WM_SHOWWINDOW = 24, // 0x00000018
    WM_ACTIVATEAPP = 28, // 0x0000001C
    WM_SETCURSOR = 32, // 0x00000020
    WM_MOUSEACTIVATE = 33, // 0x00000021
    WM_GETMINMAXINFO = 36, // 0x00000024
    WM_WINDOWPOSCHANGING = 70, // 0x00000046
    WM_WINDOWPOSCHANGED = 71, // 0x00000047
    WM_CONTEXTMENU = 123, // 0x0000007B
    WM_STYLECHANGING = 124, // 0x0000007C
    WM_STYLECHANGED = 125, // 0x0000007D
    WM_DISPLAYCHANGE = 126, // 0x0000007E
    WM_GETICON = 127, // 0x0000007F
    WM_SETICON = 128, // 0x00000080
    WM_NCCREATE = 129, // 0x00000081
    WM_NCDESTROY = 130, // 0x00000082
    WM_NCCALCSIZE = 131, // 0x00000083
    WM_NCHITTEST = 132, // 0x00000084
    WM_NCPAINT = 133, // 0x00000085
    WM_NCACTIVATE = 134, // 0x00000086
    WM_GETDLGCODE = 135, // 0x00000087
    WM_SYNCPAINT = 136, // 0x00000088
    WM_NCMOUSEMOVE = 160, // 0x000000A0
    WM_NCLBUTTONDOWN = 161, // 0x000000A1
    WM_NCLBUTTONUP = 162, // 0x000000A2
    WM_NCLBUTTONDBLCLK = 163, // 0x000000A3
    WM_NCRBUTTONDOWN = 164, // 0x000000A4
    WM_NCRBUTTONUP = 165, // 0x000000A5
    WM_NCRBUTTONDBLCLK = 166, // 0x000000A6
    WM_NCMBUTTONDOWN = 167, // 0x000000A7
    WM_NCMBUTTONUP = 168, // 0x000000A8
    WM_NCMBUTTONDBLCLK = 169, // 0x000000A9
    WM_KEYDOWN = 256, // 0x00000100
    WM_KEYUP = 257, // 0x00000101
    WM_CHAR = 258, // 0x00000102
    WM_SYSCOMMAND = 274, // 0x00000112
    WM_HSCROLL = 276, // 0x00000114
    WM_VSCROLL = 277, // 0x00000115
    WM_INITMENU = 278, // 0x00000116
    WM_INITMENUPOPUP = 279, // 0x00000117
    WM_MENUSELECT = 287, // 0x0000011F
    WM_MENUCHAR = 288, // 0x00000120
    WM_ENTERIDLE = 289, // 0x00000121
    WM_MENURBUTTONUP = 290, // 0x00000122
    WM_MENUDRAG = 291, // 0x00000123
    WM_MENUGETOBJECT = 292, // 0x00000124
    WM_UNINITMENUPOPUP = 293, // 0x00000125
    WM_MENUCOMMAND = 294, // 0x00000126
    WM_CHANGEUISTATE = 295, // 0x00000127
    WM_UPDATEUISTATE = 296, // 0x00000128
    WM_QUERYUISTATE = 297, // 0x00000129
    WM_MOUSEFIRST = 512, // 0x00000200
    WM_MOUSEMOVE = 512, // 0x00000200
    WM_LBUTTONDOWN = 513, // 0x00000201
    WM_LBUTTONUP = 514, // 0x00000202
    WM_LBUTTONDBLCLK = 515, // 0x00000203
    WM_RBUTTONDOWN = 516, // 0x00000204
    WM_RBUTTONUP = 517, // 0x00000205
    WM_RBUTTONDBLCLK = 518, // 0x00000206
    WM_MBUTTONDOWN = 519, // 0x00000207
    WM_MBUTTONUP = 520, // 0x00000208
    WM_MBUTTONDBLCLK = 521, // 0x00000209
    WM_MOUSEWHEEL = 522, // 0x0000020A
    WM_MOUSELAST = 525, // 0x0000020D
    WM_PARENTNOTIFY = 528, // 0x00000210
    WM_ENTERMENULOOP = 529, // 0x00000211
    WM_EXITMENULOOP = 530, // 0x00000212
    WM_NEXTMENU = 531, // 0x00000213
    WM_SIZING = 532, // 0x00000214
    WM_CAPTURECHANGED = 533, // 0x00000215
    WM_MOVING = 534, // 0x00000216
    WM_MDIACTIVATE = 546, // 0x00000222
    WM_ENTERSIZEMOVE = 561, // 0x00000231
    WM_EXITSIZEMOVE = 562, // 0x00000232
    WM_NCMOUSEHOVER = 672, // 0x000002A0
    WM_MOUSEHOVER = 673, // 0x000002A1
    WM_NCMOUSELEAVE = 674, // 0x000002A2
    WM_MOUSELEAVE = 675, // 0x000002A3
    WM_SYSMENU = 787, // 0x00000313
    WM_PRINT = 791, // 0x00000317
    WM_PRINTCLIENT = 792, // 0x00000318
  }
}
