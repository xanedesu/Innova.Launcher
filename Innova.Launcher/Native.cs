// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Rect
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

namespace Innova.Launcher
{
  public struct Rect
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public Rect(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    public static Rect FromXYWH(int x, int y, int width, int height) => new Rect(x, y, x + width, y + height);
  }
}
