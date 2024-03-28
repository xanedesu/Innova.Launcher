// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.SendWindowEventWebMessageHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"sendWindowEvent"})]
  public class SendWindowEventWebMessageHandler : IWebMessageHandler
  {
    public void Handle(WebMessage webMessage)
    {
      IOutputMessageDispatcherProvider instance = ServiceLocator.Current.GetInstance<IOutputMessageDispatcherProvider>();
      SendWindowEventData sendWindowEventData = webMessage.Data.ToObject<SendWindowEventData>();
      string windowId = sendWindowEventData.WindowId;
      instance.Get(windowId).Dispatch(sendWindowEventData.Message);
    }
  }
}
