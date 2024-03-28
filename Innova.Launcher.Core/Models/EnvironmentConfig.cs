// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.EnvironmentConfig
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Models
{
  public class EnvironmentConfig
  {
    [JsonProperty("environments")]
    public List<EnvironmentConfig.Environment> Environments { get; set; }

    [JsonProperty("versionsFilePrefix")]
    public string VersionsFilePrefix { get; set; }

    [JsonProperty("versionsFileExtention")]
    public string VersionsFileExtention { get; set; }

    [JsonProperty("versionReleaseInfoFileName")]
    public string VersionReleaseInfoFileName { get; set; }

    public class Environment
    {
      [JsonProperty("name")]
      public string Name { get; set; }
    }
  }
}
