// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.LauncherHardware
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Innova.Launcher.Shared.Tracking.Models
{
  public sealed class LauncherHardware
  {
    [JsonProperty("OS")]
    public LauncherOperatingSystem OS { get; set; }

    [JsonProperty("netVersion")]
    public string NetVersion { get; set; }

    [JsonProperty("ram")]
    public List<LauncherRam> Ram { get; set; }

    [JsonProperty("totalRam")]
    public long TotalRam { get; set; }

    [JsonProperty("cpu")]
    public List<LauncherCPU> CPU { get; set; }

    [JsonProperty("videoCards")]
    public List<LauncherVideoCard> VideoCards { get; set; }

    [JsonProperty("totalVideoCardRam")]
    public long TotalVideoCardRam { get; set; }
  }
}
