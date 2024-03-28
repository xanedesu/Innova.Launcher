// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Interfaces.ILocalizationService
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using System;

namespace Innova.Launcher.Shared.Localization.Interfaces
{
  public interface ILocalizationService
  {
    event EventHandler LanguageChanged;

    void UpdateLanguageByRegion(string region, string cultureName);
  }
}
