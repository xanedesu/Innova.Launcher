// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.InstallDir.GetInstallDirLauncherMessageData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models.Errors;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.MessageHandlers.InstallDir
{
  public class GetInstallDirLauncherMessageData
  {
    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("gameSize")]
    public long? GameSize { get; set; }

    [JsonProperty("freeSpace")]
    public long? FreeSpace { get; set; }

    [JsonProperty("driveLetter")]
    public string DriveLetter { get; set; }

    [JsonProperty("errors")]
    public List<BaseError> Errors { get; set; }
  }
}
