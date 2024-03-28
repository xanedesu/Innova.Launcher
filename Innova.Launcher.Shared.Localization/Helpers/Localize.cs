// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Helpers.Localize
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

namespace Innova.Launcher.Shared.Localization.Helpers
{
  public static class Localize
  {
    public static string It(string stringToLocalize) => LocalizationHelper.Localize(stringToLocalize) ?? stringToLocalize;
  }
}
