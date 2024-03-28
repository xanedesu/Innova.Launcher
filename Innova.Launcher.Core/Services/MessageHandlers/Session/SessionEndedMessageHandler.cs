// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Session.SessionEndedMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.Session
{
  public class SessionEndedMessageHandler
  {
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;

    public SessionEndedMessageHandler(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider;
    }

    public void SendSessionEndedMessage() => this._outputMessageDispatcherProvider.GetMain().Dispatch(new LauncherMessage("sessionEnded"));
  }
}
