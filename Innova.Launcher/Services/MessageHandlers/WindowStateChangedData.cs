// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.WindowStateChangedData
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Newtonsoft.Json;
using System.Windows;

namespace Innova.Launcher.Services.MessageHandlers
{
  public class WindowStateChangedData
  {
    [JsonProperty("windowId")]
    public string WindowId { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    public WindowStateChangedData(string windowId, WindowState state)
    {
      this.WindowId = windowId;
      this.State = state.ToString().ToLowerInvariant();
    }

    [JsonConstructor]
    private WindowStateChangedData()
    {
    }
  }
}
