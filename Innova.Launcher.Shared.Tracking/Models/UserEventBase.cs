// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.UserEventBase
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;
using System;

namespace Innova.Launcher.Shared.Tracking.Models
{
  public abstract class UserEventBase
  {
    [JsonProperty("serviceId")]
    public string ServiceId { get; set; }

    [JsonProperty("type", Required = Required.Always)]
    public string Type { get; set; }

    [JsonProperty("when", Required = Required.Always)]
    public DateTime When { get; set; } = DateTime.UtcNow;

    [JsonConstructor]
    protected UserEventBase()
    {
    }

    public virtual UserEventBase TrimData(int length) => this;

    protected UserEventBase(string type) => this.Type = type ?? throw new ArgumentNullException(nameof (type));
  }
}
