// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Models.PushNotification
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Converters;
using Newtonsoft.Json;

namespace Innova.Launcher.Models
{
  public class PushNotification
  {
    [JsonProperty("id", Required = Required.Always)]
    public int Id { get; set; }

    [JsonProperty("message", Required = Required.Always)]
    [JsonConverter(typeof (HtmlDecodeJsonConverter))]
    public string Message { get; set; }

    [JsonProperty("imageUrl")]
    public string ImageUrl { get; set; }

    [JsonProperty("svg")]
    public string SVG { get; set; }

    [JsonProperty("primaryLink")]
    public PushNotificationLink PrimaryLink { get; set; }

    [JsonProperty("secondaryLink")]
    public PushNotificationLink SecondaryLink { get; set; }
  }
}
