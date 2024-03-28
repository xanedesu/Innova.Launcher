// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Tracking.Models.AddEventsRequest
// Assembly: Innova.Launcher.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: DA384C86-6E9B-47C9-B483-AED3A5709C44
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Tracking.dll

using Innova.Launcher.Shared.Tracking.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Innova.Launcher.Tracking.Models
{
  public class AddEventsRequest
  {
    [JsonProperty("events", Required = Required.Always)]
    public List<UserEventBase> Events { get; set; } = new List<UserEventBase>();
  }
}
