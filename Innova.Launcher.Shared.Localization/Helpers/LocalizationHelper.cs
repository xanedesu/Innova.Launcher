// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Helpers.LocalizationHelper
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using System.Globalization;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;

namespace Innova.Launcher.Shared.Localization.Helpers
{
  public static class LocalizationHelper
  {
    private static CultureInfo _currentCulture = CultureInfo.InvariantCulture;

    public static CultureInfo CurrentCulture => LocalizationHelper._currentCulture;

    public static void SetCurrentCulture(CultureInfo info)
    {
      LocalizationHelper._currentCulture = info;
      LocalizeDictionary.Instance.Culture = info;
    }

    public static string Localize(string stringToLocalize) => stringToLocalize == null ? (string) null : LocExtension.GetLocalizedValue<string>(stringToLocalize, LocalizationHelper._currentCulture);

    public static string ToMoreSpecific(string culture)
    {
      string moreSpecific;
      switch (culture)
      {
        case "ru":
          moreSpecific = "ru-RU";
          break;
        case "en":
          moreSpecific = "en-US";
          break;
        case "pt":
          moreSpecific = "pt-BR";
          break;
        case "es":
          moreSpecific = "es-AR";
          break;
        case "pl":
          moreSpecific = "pl-PL";
          break;
        default:
          moreSpecific = culture;
          break;
      }
      return moreSpecific;
    }
  }
}
