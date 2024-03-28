// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Install.InstallMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Newtonsoft.Json;
using System.Threading;

namespace Innova.Launcher.Core.Services.MessageHandlers.Install
{
  [WebMessageHandlerFilter(new string[] {"install", "update", "repair", "pause", "resume", "cancel"})]
  public class InstallMessageHandler : IWebMessageHandler
  {
    private readonly IGameInstaller _gameInstaller;
    private readonly SemaphoreSlim _semaphoreSlim;

    public InstallMessageHandler(IGameInstaller gameInstaller)
    {
      this._gameInstaller = gameInstaller;
      this._semaphoreSlim = new SemaphoreSlim(1);
    }

    public async void Handle(WebMessage webMessage)
    {
      await this._semaphoreSlim.WaitAsync();
      try
      {
        switch (webMessage.Type)
        {
          case "install":
            InstallMessageHandler.InstallInputMessage installInputMessage = webMessage.Data.ToObject<InstallMessageHandler.InstallInputMessage>();
            await this._gameInstaller.InstallAsync(installInputMessage.GameKey, installInputMessage.Path, installInputMessage.ExtendedStatus, installInputMessage.Culture);
            break;
          case "repair":
            await this._gameInstaller.RepairAsync(webMessage.Data.ToObject<InstallMessageHandler.UpdateInputMessage>().GameKey);
            break;
          case "update":
            await this._gameInstaller.UpdateAsync(webMessage.Data.ToObject<InstallMessageHandler.UpdateInputMessage>().GameKey);
            break;
          case "pause":
            this._gameInstaller.Pause(webMessage.Data.ToObject<InstallMessageHandler.ProcessInputMessage>().GameKey);
            break;
          case "resume":
            await this._gameInstaller.ResumeAsync(webMessage.Data.ToObject<InstallMessageHandler.ProcessInputMessage>().GameKey);
            break;
          case "cancel":
            this._gameInstaller.Cancel(webMessage.Data.ToObject<InstallMessageHandler.ProcessInputMessage>().GameKey);
            break;
        }
      }
      finally
      {
        this._semaphoreSlim.Release();
      }
    }

    private sealed class ProcessInputMessage
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }
    }

    private sealed class UpdateInputMessage
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }

      [JsonProperty("isFullUpdate")]
      public bool IsFullUpdate { get; set; }
    }

    private sealed class InstallInputMessage
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }

      [JsonProperty("path")]
      public string Path { get; set; }

      [JsonProperty("extended_status")]
      public string ExtendedStatus { get; set; }

      [JsonProperty("isFullUpdate")]
      public bool IsFullUpdate { get; set; }

      [JsonProperty("culture")]
      public string Culture { get; set; }
    }
  }
}
