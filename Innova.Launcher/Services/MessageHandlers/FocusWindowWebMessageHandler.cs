// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.FocusWindowWebMessageHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"focusWindow"})]
  public class FocusWindowWebMessageHandler : IWebMessageHandler
  {
    public void Handle(WebMessage webMessage) => ServiceLocator.Current.GetInstance<IWindowsService>().FocusWindow(webMessage.Data.ToObject<FocusWindowData>().WindowId);
  }
}
