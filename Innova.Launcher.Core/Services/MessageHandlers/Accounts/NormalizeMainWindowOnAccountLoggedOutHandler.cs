// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Accounts.NormalizeMainWindowOnAccountLoggedOutHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;

namespace Innova.Launcher.Core.Services.MessageHandlers.Accounts
{
  [WebMessageHandlerFilter(new string[] {"loggedOut"})]
  public class NormalizeMainWindowOnAccountLoggedOutHandler : IWebMessageHandler
  {
    private readonly IWindowsService _windowsService;

    public NormalizeMainWindowOnAccountLoggedOutHandler(IWindowsService windowsService) => this._windowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));

    public void Handle(WebMessage webMessage) => this._windowsService.Normalize("main");
  }
}
