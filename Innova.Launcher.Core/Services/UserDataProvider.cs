// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.UserDataProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class UserDataProvider : IUserDataProvider
  {
    private readonly IOutputMessageDispatcher _outputMessageDispatcher;

    public event EventHandler<UserDataReceivedEventArgs> UserDataReceived;

    public UserData CurrentData { get; private set; }

    public UserDataProvider(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._outputMessageDispatcher = outputMessageDispatcherProvider.GetMain();
    }

    public void RequestUserData() => this._outputMessageDispatcher.Dispatch(new LauncherMessage("getUserData"));

    public string GetUserToken()
    {
      string userToken = string.Empty;
      TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
      this.UserDataReceived += new EventHandler<UserDataReceivedEventArgs>(HandleUserData);
      this.RequestUserData();
      if (tcs.Task.Wait(TimeSpan.FromMilliseconds(1000.0)))
        userToken = tcs.Task.Result;
      return userToken;

      void HandleUserData(object _, EventArgs ea)
      {
        this.UserDataReceived -= new EventHandler<UserDataReceivedEventArgs>(HandleUserData);
        tcs.SetResult(this.CurrentData.AuthToken);
      }
    }

    public void UpdateUserData(UserData data)
    {
      this.CurrentData = new UserData(data.AuthToken);
      EventHandler<UserDataReceivedEventArgs> userDataReceived = this.UserDataReceived;
      if (userDataReceived == null)
        return;
      userDataReceived((object) this, new UserDataReceivedEventArgs(this.CurrentData));
    }
  }
}
