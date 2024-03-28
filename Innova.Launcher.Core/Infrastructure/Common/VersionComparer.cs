// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Common.VersionComparer
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Infrastructure.Common
{
  public sealed class VersionComparer : IComparer<string>, IEqualityComparer<string>
  {
    public static VersionComparer Default = new VersionComparer();

    public bool Equals(string x, string y) => this.Compare(x, y) == 0;

    public int Compare(string versionX, string versionY)
    {
      if (string.Equals(versionX, versionY))
        return 0;
      if (string.IsNullOrWhiteSpace(versionX))
        return -1;
      if (string.IsNullOrWhiteSpace(versionY))
        return 1;
      versionX = this.ClearVersionName(versionX);
      versionY = this.ClearVersionName(versionY);
      string[] strArray1 = versionX.Split('.');
      string[] strArray2 = versionY.Split('.');
      int num1 = Math.Min(strArray1.Length, strArray2.Length);
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        int result1;
        int result2;
        if (!int.TryParse(strArray1[index], out result1) || !int.TryParse(strArray2[index], out result2))
          return -string.Compare(strArray1[index], strArray2[index], StringComparison.InvariantCulture);
        num2 = result1.CompareTo(result2);
        if (num2 != 0)
          return num2;
      }
      return num2;
    }

    private string ClearVersionName(string source) => new string(source.Where<char>((Func<char, bool>) (v => char.IsDigit(v) || v == '.')).ToArray<char>());

    public int GetHashCode(string obj)
    {
      string str = this.ClearVersionName(obj);
      return str == null ? 0 : str.GetHashCode();
    }
  }
}
