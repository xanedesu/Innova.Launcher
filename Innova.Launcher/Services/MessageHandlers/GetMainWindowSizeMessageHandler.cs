// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.GetMainWindowSizeMessageHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"getMainWindowSize"})]
  public class GetMainWindowSizeMessageHandler : IWebMessageHandler
  {
    private readonly ILauncherStateService _launcherStateService;
    private readonly IWindowsService _windowsService;

    public GetMainWindowSizeMessageHandler(
      IWindowsService windowsService,
      ILauncherStateService launcherStateService)
    {
      this._windowsService = windowsService;
      this._launcherStateService = launcherStateService;
    }

    public void Handle(WebMessage webMessage)
    {
      WindowSize mainWindowSize = this._windowsService.GetMainWindowSize();
      this._launcherStateService.SendMainWindowSize(webMessage, mainWindowSize);
    }
  }
}
