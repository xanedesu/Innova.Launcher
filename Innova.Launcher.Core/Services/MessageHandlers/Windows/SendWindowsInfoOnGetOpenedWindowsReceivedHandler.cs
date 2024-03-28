// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Windows.SendWindowsInfoOnGetOpenedWindowsReceivedHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.MessageHandlers.Windows
{
  [WebMessageHandlerFilter(new string[] {"getOpenedWindows"})]
  public class SendWindowsInfoOnGetOpenedWindowsReceivedHandler : IWebMessageHandler
  {
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private readonly IWindowsService _windowsService;

    public SendWindowsInfoOnGetOpenedWindowsReceivedHandler(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      IWindowsService windowsService)
    {
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider ?? throw new ArgumentNullException(nameof (outputMessageDispatcherProvider));
      this._windowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));
    }

    public void Handle(WebMessage webMessage)
    {
      List<WindowInfo> windowsInfo = this._windowsService.GetWindowsInfo();
      this._outputMessageDispatcherProvider.GetMain().Dispatch(new LauncherMessage(webMessage.Id, "getOpenedWindows", (object) windowsInfo));
    }
  }
}
