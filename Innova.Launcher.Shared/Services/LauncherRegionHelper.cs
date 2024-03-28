// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherRegionHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

namespace Innova.Launcher.Shared.Services
{
  public class LauncherRegionHelper
  {
    private static readonly char _gameKeyRegionSplitter = '-';

    public static string GetRegionByGameKey(string gameKey)
    {
      if (string.IsNullOrWhiteSpace(gameKey))
        return (string) null;
      string[] strArray = gameKey.Split(LauncherRegionHelper._gameKeyRegionSplitter);
      if (strArray.Length < 2)
        return (string) null;
      string regionByGameKey = strArray[strArray.Length - 1];
      if (regionByGameKey == "br")
        regionByGameKey = "la";
      return regionByGameKey;
    }
  }
}
