// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Extensions.BytesExtensions
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Extensions
{
  public static class BytesExtensions
  {
    public static byte[] AppendLeft(this byte[] source, byte[] appendix)
    {
      int length1 = source.Length;
      int length2 = appendix.Length;
      byte[] destinationArray = new byte[length1 + length2];
      Array.Copy((Array) appendix, (Array) destinationArray, length2);
      Array.Copy((Array) source, 0, (Array) destinationArray, length2, length1);
      return destinationArray;
    }

    public static byte[] TakeLeft(this byte[] source, int count)
    {
      if (source.Length < count)
        throw new ArgumentException();
      byte[] destinationArray = new byte[count];
      Array.Copy((Array) source, (Array) destinationArray, count);
      return destinationArray;
    }

    public static byte[] TrimLeft(this byte[] source, int count)
    {
      int length = source.Length - count;
      byte[] destinationArray = length >= 0 ? new byte[length] : throw new ArgumentException();
      Array.Copy((Array) source, 0, (Array) destinationArray, 0, length);
      return destinationArray;
    }
  }
}
