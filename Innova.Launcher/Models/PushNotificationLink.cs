// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Models.PushNotificationLink
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Converters;
using Newtonsoft.Json;

namespace Innova.Launcher.Models
{
  public sealed class PushNotificationLink
  {
    [JsonProperty("url", Required = Required.Always)]
    public string Url { get; set; }

    [JsonProperty("name", Required = Required.Always)]
    [JsonConverter(typeof (HtmlDecodeJsonConverter))]
    public string Name { get; set; }
  }
}
