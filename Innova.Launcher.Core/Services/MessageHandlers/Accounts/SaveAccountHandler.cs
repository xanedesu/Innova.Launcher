// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Accounts.SaveAccountHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;

namespace Innova.Launcher.Core.Services.MessageHandlers.Accounts
{
  [WebMessageHandlerFilter(new string[] {"saveAccount"})]
  public class SaveAccountHandler : IWebMessageHandler
  {
    private readonly IAccountRepositoryFactory _accountRepositoryFactory;
    private readonly IOutputMessageDispatcherProvider _messageDispatcherProvider;

    public SaveAccountHandler(
      IAccountRepositoryFactory accountRepositoryFactory,
      IOutputMessageDispatcherProvider messageDispatcherProvider)
    {
      this._messageDispatcherProvider = messageDispatcherProvider ?? throw new ArgumentNullException(nameof (messageDispatcherProvider));
      this._accountRepositoryFactory = accountRepositoryFactory ?? throw new ArgumentNullException(nameof (accountRepositoryFactory));
    }

    public void Handle(WebMessage webMessage)
    {
      Account account = webMessage.Data.ToObject<Account>();
      this._accountRepositoryFactory.Get().Save(account);
      this._messageDispatcherProvider.Get(webMessage.WindowId ?? "main").Dispatch(new LauncherMessage(webMessage.Id, "saveAccount"));
    }
  }
}
