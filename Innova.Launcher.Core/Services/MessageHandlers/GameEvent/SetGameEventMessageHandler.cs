// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.GameEvent.SetGameEventMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;

namespace Innova.Launcher.Core.Services.MessageHandlers.GameEvent
{
  [WebMessageHandlerFilter(new string[] {"setGameEvent"})]
  public class SetGameEventMessageHandler : IWebMessageHandler
  {
    private readonly IOutputMessageDispatcherProvider _messageDispatcherProvider;
    private readonly IGameManager _gameManager;

    public SetGameEventMessageHandler(
      IOutputMessageDispatcherProvider messageDispatcherProvider,
      IGameManager gameManager)
    {
      this._messageDispatcherProvider = messageDispatcherProvider ?? throw new ArgumentNullException(nameof (messageDispatcherProvider));
      this._gameManager = gameManager ?? throw new ArgumentNullException(nameof (gameManager));
    }

    public void Handle(WebMessage webMessage)
    {
      if (!(webMessage.Type == "setGameEvent"))
        return;
      SetGameEventData setGameEventData = webMessage.Data.ToObject<SetGameEventData>();
      this._gameManager.SelectEventForGame(setGameEventData.GameKey, setGameEventData.GameEventKey);
    }
  }
}
