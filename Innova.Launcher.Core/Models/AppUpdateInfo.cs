// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.AppUpdateInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models
{
  public sealed class AppUpdateInfo
  {
    [JsonProperty("isCritical")]
    public bool IsCritical { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("status")]
    public LauncherUpdateStatus Status { get; set; }

    [JsonProperty("info")]
    public AppUpdateProgressInfo Info { get; set; }
  }
}
