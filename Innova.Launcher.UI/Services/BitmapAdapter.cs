// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Services.BitmapAdapter
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Innova.Launcher.UI.Services
{
  public class BitmapAdapter : IEnumerable<Pixel>, IEnumerable
  {
    private readonly Color[,] _pixels;

    public int Stride { get; }

    public int Height { get; }

    public int Width { get; }

    public Color this[int x, int y] => this._pixels[x, y];

    public BitmapAdapter(BitmapImage source)
    {
      this.Height = source.PixelHeight;
      this.Width = source.PixelWidth;
      this.Stride = this.Width * source.Format.BitsPerPixel / 8;
      byte[] numArray = new byte[this.Height * this.Stride];
      source.CopyPixels((Array) numArray, this.Stride, 0);
      this._pixels = new Color[this.Width, this.Height];
      for (int index1 = 0; index1 < this.Height; ++index1)
      {
        for (int index2 = 0; index2 < this.Width; ++index2)
        {
          int index3 = index2 + index1 * this.Stride;
          this._pixels[index2, index1] = new Color()
          {
            B = numArray[index3],
            G = numArray[index3 + 1],
            R = numArray[index3 + 2],
            A = numArray[index3 + 3]
          };
        }
      }
    }

    public IEnumerable<Pixel> Rect(int startY, int endY)
    {
      for (int y = startY; y < endY; ++y)
      {
        for (int x = 0; x < this.Width; ++x)
          yield return new Pixel(this[x, y], new Point()
          {
            X = (double) x,
            Y = (double) y
          });
      }
    }

    public IEnumerator<Pixel> GetEnumerator()
    {
      for (int y = 0; y < this.Height; ++y)
      {
        for (int x = 0; x < this.Width; ++x)
          yield return new Pixel(this._pixels[x, y], new Point()
          {
            X = (double) x,
            Y = (double) y
          });
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
