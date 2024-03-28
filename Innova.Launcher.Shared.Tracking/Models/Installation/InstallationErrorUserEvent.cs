// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.Installation.InstallationErrorUserEvent
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Shared.Tracking.Models.Installation
{
  public class InstallationErrorUserEvent : UserEventBase
  {
    [JsonProperty("error", Required = Required.Always)]
    public string Error { get; set; }

    [JsonProperty("extraInfo")]
    public string ExtraInfo { get; set; }

    public InstallationErrorUserEvent()
      : base("users.installer.installation.error")
    {
    }

    public override UserEventBase TrimData(int length)
    {
      base.TrimData(length);
      string error = this.Error;
      if ((error != null ? (error.Length > length ? 1 : 0) : 0) != 0)
        this.Error = this.Error?.Substring(0, length);
      string extraInfo = this.ExtraInfo;
      if ((extraInfo != null ? (extraInfo.Length > length ? 1 : 0) : 0) != 0)
        this.ExtraInfo = this.ExtraInfo?.Substring(0, length);
      return (UserEventBase) this;
    }
  }
}
