﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.LauncherRam
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Shared.Tracking.Models
{
  public sealed class LauncherRam
  {
    [JsonProperty("speed")]
    public int Speed { get; set; }

    [JsonProperty("manufacturer")]
    public string Manufacturer { get; set; }

    [JsonProperty("capacity")]
    public long Capacity { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
  }
}
