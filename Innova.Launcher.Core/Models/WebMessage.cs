// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.WebMessage
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innova.Launcher.Core.Models
{
  public class WebMessage
  {
    [JsonProperty("windowId")]
    public string WindowId { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name", Required = Required.Always)]
    public string Type { get; set; }

    [JsonProperty("data")]
    public JObject Data { get; set; }
  }
}
