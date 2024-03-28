// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.UserData.UserDataMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services.MessageHandlers.UserData
{
  [WebMessageHandlerFilter(new string[] {"setUserData"})]
  public class UserDataMessageHandler : IWebMessageHandler
  {
    private readonly IUserDataProvider _userDataProvider;

    public UserDataMessageHandler(IUserDataProvider userDataProvider) => this._userDataProvider = userDataProvider;

    public void Handle(WebMessage webMessage)
    {
      if (!(webMessage.Type == "setUserData"))
        return;
      this._userDataProvider.UpdateUserData(webMessage.Data.ToObject<UserDataMessage>().ToUserData());
    }
  }
}
