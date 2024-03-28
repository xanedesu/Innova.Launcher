// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Services.Pixel
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Media;

namespace Innova.Launcher.UI.Services
{
  public struct Pixel
  {
    public Point Point { get; }

    public Color Color { get; }

    public Pixel(Color color, Point point)
    {
      this.Color = color;
      this.Point = point;
    }
  }
}
