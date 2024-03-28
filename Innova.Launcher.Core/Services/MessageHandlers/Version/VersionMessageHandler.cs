// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Version.VersionMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.Version
{
  [WebMessageHandlerFilter(new string[] {"getVersions"})]
  public class VersionMessageHandler : IWebMessageHandler
  {
    private readonly IOutputMessageDispatcher _messageDispatcher;

    public VersionMessageHandler(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._messageDispatcher = outputMessageDispatcherProvider.GetMain();
    }

    public void Handle(WebMessage webMessage)
    {
      VersionsLauncherMessageData data = new VersionsLauncherMessageData();
      this._messageDispatcher.Dispatch((LauncherMessage) new VersionsLauncherMessage(webMessage.Id, data));
    }
  }
}
