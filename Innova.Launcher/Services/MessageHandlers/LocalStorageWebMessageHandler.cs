// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.LocalStorageWebMessageHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"saveLocalStorage", "restoreLocalStorage"})]
  public class LocalStorageWebMessageHandler : IWebMessageHandler
  {
    private readonly IWindowsService _windowsService;

    public LocalStorageWebMessageHandler(IWindowsService windowsService) => this._windowsService = windowsService;

    public void Handle(WebMessage webMessage)
    {
      switch (webMessage.Type)
      {
        case "saveLocalStorage":
          this.HandleSaveLocalStorage(webMessage);
          break;
        case "restoreLocalStorage":
          this.HandleRestoreLocalStorage(webMessage);
          break;
      }
    }

    private void HandleRestoreLocalStorage(WebMessage webMessage) => this._windowsService.RestoreWindowLocalStorage(webMessage.WindowId, false);

    private void HandleSaveLocalStorage(WebMessage webMessage) => this._windowsService.SaveWindowLocalStorage(webMessage.WindowId);
  }
}
