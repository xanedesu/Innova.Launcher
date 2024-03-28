// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.TrackingConfigurationProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Tracking.Interfaces;
using System;

namespace Innova.Launcher.Services
{
  public class TrackingConfigurationProvider : ITrackingConfiguration
  {
    private readonly ILauncherStateService _launcherStateService;
    private readonly string _serviceIdDefault = "forgame-{region}";

    public string Url { get; set; }

    public string ServiceId { get; set; }

    public TrackingConfigurationProvider(ILauncherStateService launcherStateService)
    {
      this._launcherStateService = launcherStateService;
      launcherStateService.RegionUpdated += new EventHandler<RegionUpdatedEventArgs>(this.LauncherRegionUpdated);
      this.HandleLauncherRegion();
    }

    private void LauncherRegionUpdated(object sender, EventArgs e) => this.HandleLauncherRegion();

    private void HandleLauncherRegion()
    {
      this.Url = "https://launcherbff.{region}.4game.com/api/exporter/events".Replace("{region}", this._launcherStateService.LauncherRegion);
      this.ServiceId = this._serviceIdDefault.Replace("{region}", this._launcherStateService.LauncherRegion);
    }
  }
}
