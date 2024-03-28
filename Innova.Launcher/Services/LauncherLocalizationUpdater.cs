// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.LauncherLocalizationUpdater
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Localization.Interfaces;
using System;

namespace Innova.Launcher.Services
{
  public sealed class LauncherLocalizationUpdater : ILauncherLocalizationUpdater
  {
    private readonly ILocalizationService _localizationService;

    public LauncherLocalizationUpdater(
      ILauncherStateService launcherStateService,
      ILocalizationService localizationService)
    {
      if (launcherStateService == null)
        throw new ArgumentNullException(nameof (launcherStateService));
      this._localizationService = localizationService ?? throw new ArgumentNullException(nameof (localizationService));
      launcherStateService.RegionUpdated += new EventHandler<RegionUpdatedEventArgs>(this.LauncherStateServiceRegionUpdated);
    }

    private void LauncherStateServiceRegionUpdated(object sender, RegionUpdatedEventArgs e) => this._localizationService.UpdateLanguageByRegion(e.Region, e.Culture);
  }
}
