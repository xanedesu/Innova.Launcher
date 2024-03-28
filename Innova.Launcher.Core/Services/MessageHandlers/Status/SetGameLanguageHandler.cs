// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Status.SetGameLanguageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.Status
{
  [WebMessageHandlerFilter(new string[] {"setGameCulture"})]
  public class SetGameLanguageHandler : IWebMessageHandler
  {
    private readonly IGameRepositoryFactory _factory;
    private readonly IOutputMessageDispatcher _outputDispatcher;

    public SetGameLanguageHandler(
      IGameRepositoryFactory factory,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._factory = factory;
      this._outputDispatcher = outputMessageDispatcherProvider.GetMain();
    }

    public void Handle(WebMessage webMessage)
    {
      SetGameLanguageData data = webMessage.Data.ToObject<SetGameLanguageData>();
      if (data != null)
      {
        Game orDefault = this._factory.Get().GetOrDefault(data.ServiceId);
        orDefault.Culture = data.Culture;
        this._factory.Get().Save(orDefault);
      }
      this._outputDispatcher.Dispatch((LauncherMessage) new SetGameLanguageResponse(webMessage.Id, data));
    }
  }
}
