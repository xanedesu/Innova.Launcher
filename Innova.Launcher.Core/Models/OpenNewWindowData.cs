// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.OpenWindowData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models
{
  public class OpenWindowData
  {
    [JsonProperty("windowId", Required = Required.Always)]
    public string WindowId { get; set; }

    [JsonProperty("url", Required = Required.Always)]
    public string Url { get; set; }

    [JsonProperty("canResize")]
    public bool CanResize { get; set; }

    [JsonProperty("width")]
    public double? Width { get; set; }

    [JsonProperty("height")]
    public double? Height { get; set; }

    [JsonProperty("minHeight")]
    public double? MinHeight { get; set; }

    [JsonProperty("minWidth")]
    public double? MinWidth { get; set; }

    [JsonProperty("isMaximized")]
    public bool? IsMaximized { get; set; }
  }
}
