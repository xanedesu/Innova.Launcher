// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.Application.LauncherGameLaunchErrorEvent
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Shared.Tracking.Models.Application
{
  public class LauncherGameLaunchErrorEvent : UserEventBase
  {
    [JsonProperty("gameVersion")]
    public string GameVersion { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("extraInfo")]
    public string ExtraInfo { get; set; }

    public LauncherGameLaunchErrorEvent()
      : base("users.game.launch.error")
    {
    }
  }
}
