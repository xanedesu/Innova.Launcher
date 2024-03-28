// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Status.ProgressInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Updater.Core.Models;
using Newtonsoft.Json;
using System;

namespace Innova.Launcher.Core.Services.MessageHandlers.Status
{
  public class ProgressInfo
  {
    [JsonProperty("progress")]
    public Decimal Progress { get; set; }

    [JsonProperty("checking")]
    public bool Checking { get; set; }

    [JsonProperty("speed")]
    public int Speed { get; set; }

    [JsonProperty("size")]
    public long Size { get; set; }

    [JsonProperty("dl_size")]
    public long DlSize { get; set; }

    [JsonProperty("downloaded")]
    public long Downloaded { get; set; }

    [JsonProperty("estimated")]
    public int Estimated { get; set; }

    [JsonProperty("fileName")]
    public string FileName { get; set; }

    [JsonProperty("fileProgress")]
    public FileDownloadProgress FileProgress { get; set; }

    [JsonConstructor]
    public ProgressInfo()
    {
    }

    public ProgressInfo(UpdateProgressInfo updateProgressInfo)
    {
      this.Size = updateProgressInfo.Size;
      this.Downloaded = updateProgressInfo.Downloaded;
      this.FileName = updateProgressInfo.FileName;
      this.Progress = updateProgressInfo.Progress;
      this.Speed = updateProgressInfo.Speed;
    }
  }
}
