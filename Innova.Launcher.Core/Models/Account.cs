// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.Account
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using LiteDB;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Models
{
  public class Account
  {
    [JsonProperty("refreshToken")]
    public string RefreshToken { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("gender")]
    public string Gender { get; set; }

    [JsonProperty("lastOpenedGames")]
    public Dictionary<string, string> LastOpenedGames { get; set; }

    [JsonProperty("session")]
    public AccountSession Session { get; set; }

    [JsonProperty("encrypted")]
    public bool Encrypted { get; set; }

    [JsonProperty("gamesMeta")]
    [BsonIgnore]
    public GamesMeta GamesMeta { get; set; }

    [JsonIgnore]
    public string SerializedGamesMeta
    {
      get => this.GamesMeta == null ? string.Empty : JsonConvert.SerializeObject((object) this.GamesMeta);
      set => this.GamesMeta = JsonConvert.DeserializeObject<GamesMeta>(value ?? string.Empty);
    }
  }
}
