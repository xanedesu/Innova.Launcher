// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.AppIdentity.LauncherSettingsUpdatedMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.AppIdentity
{
  [WebMessageHandlerFilter(new string[] {"launcherSettingsUpdated"})]
  public class LauncherSettingsUpdatedMessageHandler : IWebMessageHandler
  {
    private readonly ILauncherStateService _launcherStateService;

    public LauncherSettingsUpdatedMessageHandler(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      ILauncherStateService launcherStateService)
    {
      this._launcherStateService = launcherStateService;
    }

    public void Handle(WebMessage webMessage)
    {
      if (!(webMessage.Type == "launcherSettingsUpdated"))
        return;
      this._launcherStateService.UpdateSettings(webMessage.Data.ToObject<LauncherSettingsUpdatedMessage>().ToLauncherSettings());
    }
  }
}
