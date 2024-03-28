// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.AppIdentity.GetAppIdentityHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.AppIdentity
{
  [WebMessageHandlerFilter(new string[] {"getAppIdentity"})]
  public class GetAppIdentityHandler : IWebMessageHandler
  {
    private readonly ILauncherStateService _launcherStateService;
    private readonly ILauncherManager _launcherManager;

    public GetAppIdentityHandler(
      ILauncherStateService launcherStateService,
      ILauncherManager launcherManager)
    {
      this._launcherStateService = launcherStateService;
      this._launcherManager = launcherManager;
    }

    public void Handle(WebMessage webMessage)
    {
      this._launcherManager.CheckAvailableUpdatesAsync().Wait();
      this._launcherStateService.SendLauncherIdentity(webMessage);
    }
  }
}
