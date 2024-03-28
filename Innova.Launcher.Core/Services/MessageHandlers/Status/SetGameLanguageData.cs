// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Status.SetGameLanguageData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Services.MessageHandlers.Status
{
  public class SetGameLanguageData
  {
    [JsonProperty("serviceId")]
    public string ServiceId { get; set; }

    [JsonProperty("culture")]
    public string Culture { get; set; }
  }
}
