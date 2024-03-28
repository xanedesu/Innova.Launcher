// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherMessage
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innova.Launcher.Core.Services
{
  public class LauncherMessage
  {
    public LauncherMessage(string name)
      : this(name, new object())
    {
    }

    public LauncherMessage(string id, string name)
      : this(id, name, new object())
    {
    }

    public LauncherMessage(string name, object data)
    {
      this.Name = name;
      this.Data = JToken.FromObject(data);
    }

    public LauncherMessage(string id, string name, object data)
    {
      this.Id = id;
      this.Name = name;
      this.Data = JToken.FromObject(data);
    }

    [JsonConstructor]
    protected LauncherMessage()
    {
    }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("data")]
    public JToken Data { get; set; }

    [JsonIgnore]
    public bool Async { get; set; } = true;
  }
}
