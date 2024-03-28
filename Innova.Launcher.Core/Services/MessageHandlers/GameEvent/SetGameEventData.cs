// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.GameEvent.SetGameEventData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Services.MessageHandlers.GameEvent
{
  public class SetGameEventData
  {
    [JsonProperty("serviceId")]
    public string GameKey { get; set; }

    [JsonProperty("eventKey")]
    public string GameEventKey { get; set; }
  }
}
