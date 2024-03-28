// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Services.LocalizationService
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using Innova.Launcher.Shared.Localization.Helpers;
using Innova.Launcher.Shared.Localization.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Innova.Launcher.Shared.Localization.Services
{
  public class LocalizationService : ILocalizationService
  {
    private readonly Dictionary<string, string> _regionCultureMapping = new Dictionary<string, string>()
    {
      {
        "ru",
        "ru-RU"
      },
      {
        "eu",
        "en-US"
      },
      {
        "br",
        "pt-BR"
      },
      {
        "la",
        "pt-BR"
      }
    };

    public event EventHandler LanguageChanged;

    public void UpdateLanguageByRegion(string region, string cultureName)
    {
      if (string.IsNullOrWhiteSpace(cultureName))
        this._regionCultureMapping.TryGetValue(region, out cultureName);
      LocalizationHelper.SetCurrentCulture(CultureInfo.CreateSpecificCulture(cultureName ?? region));
      this.OnLanguageChanged();
    }

    protected virtual void OnLanguageChanged()
    {
      EventHandler languageChanged = this.LanguageChanged;
      if (languageChanged == null)
        return;
      languageChanged((object) this, EventArgs.Empty);
    }
  }
}
