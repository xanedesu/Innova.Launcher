// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Services.ColorExtensions
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows.Media;

namespace Innova.Launcher.UI.Services
{
  public static class ColorExtensions
  {
    public static byte GetLuminance(this Color value) => (byte) (0.2126 * (double) value.R + 447.0 / 625.0 * (double) value.G + 0.0722 * (double) value.B);
  }
}
