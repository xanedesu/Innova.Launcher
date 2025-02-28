﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.SendWindowEventData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services;
using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models
{
  public class SendWindowEventData
  {
    [JsonProperty("windowId", Required = Required.Always)]
    public string WindowId { get; set; }

    [JsonProperty("detail", Required = Required.Always)]
    public LauncherMessage Message { get; set; }
  }
}
