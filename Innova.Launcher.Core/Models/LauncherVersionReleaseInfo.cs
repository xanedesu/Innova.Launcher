// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.LauncherVersionReleaseInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models
{
  public class LauncherVersionReleaseInfo
  {
    [JsonProperty("version", Required = Required.Always)]
    public string Version { get; private set; }

    [JsonProperty("size", Required = Required.Always)]
    public long? SizeKilobytes { get; set; }

    [JsonProperty("minRequiredVersion", Required = Required.Always)]
    public string MinRequiredVersion { get; private set; }

    [JsonConstructor]
    private LauncherVersionReleaseInfo()
    {
    }

    public LauncherVersionReleaseInfo(string version) => this.Version = version;
  }
}
