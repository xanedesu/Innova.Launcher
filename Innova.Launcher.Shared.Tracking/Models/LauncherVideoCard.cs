// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.LauncherVideoCard
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Shared.Tracking.Models
{
  public sealed class LauncherVideoCard
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("adapterRam")]
    public long AdapterRam { get; set; }

    [JsonProperty("driverVersion")]
    public string DriverVersion { get; set; }

    [JsonProperty("videoProcessor")]
    public string VideoProcessor { get; set; }
  }
}
