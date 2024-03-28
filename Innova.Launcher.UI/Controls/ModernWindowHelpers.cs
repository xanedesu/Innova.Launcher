// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.ModernWindowHelpers
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Windows;
using System.Windows.Shell;

namespace Innova.Launcher.UI.Controls
{
  public static class ModernWindowHelpers
  {
    public static void SetWindowChromeResizeGripDirection(
      this ModernWindow window,
      string name,
      ResizeGripDirection direction)
    {
      IInputElement inputElement = window != null ? window.GetPart(name) as IInputElement : throw new ArgumentNullException(nameof (window));
      if (WindowChrome.GetResizeGripDirection(inputElement) == direction)
        return;
      WindowChrome.SetResizeGripDirection(inputElement, direction);
    }
  }
}
