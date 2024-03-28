// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.AppIdentityInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Shared.Tracking.Models;
using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models
{
  public sealed class AppIdentityInfo
  {
    [JsonProperty("launcherId")]
    public string LauncherId { get; set; }

    [JsonProperty("hardwareId")]
    public string HardwareId { get; set; }

    [JsonProperty("hardware")]
    public LauncherHardware Hardware { get; set; }

    [JsonProperty("computerName")]
    public string ComputerName { get; set; }

    [JsonProperty("launcherVersion")]
    public string LauncherVersion { get; set; }

    [JsonProperty("status")]
    public LauncherStatus Status { get; set; }

    [JsonProperty("update")]
    public AppUpdateInfo Update { get; set; }

    [JsonProperty("gamesEnv")]
    public string GamesEnv { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("startPage")]
    public string StartPage { get; set; }

    [JsonProperty("launcherEnv")]
    public string LauncherEnv { get; set; }

    [JsonProperty("culture")]
    public string Culture { get; set; }
  }
}
