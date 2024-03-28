// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.LauncherUpdate.LauncherUpdateMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.LauncherUpdate
{
  [WebMessageHandlerFilter(new string[] {"appUpdated", "startUpdateApp", "cancelUpdateApp"})]
  public class LauncherUpdateMessageHandler : IWebMessageHandler
  {
    private readonly ILauncherManager _launcherManager;

    public LauncherUpdateMessageHandler(ILauncherManager launcherManager) => this._launcherManager = launcherManager;

    public void Handle(WebMessage webMessage)
    {
      switch (webMessage.Type)
      {
        case "startUpdateApp":
          this._launcherManager.StartLauncherUpdate(webMessage.Data.ToObject<LauncherVersionInfo>().Version);
          break;
        case "cancelUpdateApp":
          this._launcherManager.CancelLauncherUpdate();
          break;
        case "appUpdated":
          this._launcherManager.ProcessNewAvailableVersions(webMessage.Data.ToObject<LauncherAvailableVersionsInfo>());
          break;
      }
    }
  }
}
