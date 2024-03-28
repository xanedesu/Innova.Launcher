// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.InternetConnectionStateNotifier
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Properties;
using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Innova.Launcher.Services
{
  public class InternetConnectionStateNotifier : IInternetConnectionStateNotifier
  {
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IInternetConnectionChecker _internetConnectionChecker;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private const int _checkingDelay = 10000;
    private bool _state;
    private Task _worker;

    public InternetConnectionStateNotifier(
      ILifetimeManager lifetimeManager,
      IInternetConnectionChecker internetConnectionChecker,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._lifetimeManager = lifetimeManager;
      this._internetConnectionChecker = internetConnectionChecker;
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider;
    }

    public void Start() => this._worker = (Task) Task.Factory.StartNew<Task>((Func<Task>) (async () =>
    {
      while (this._lifetimeManager.IsAlive)
      {
        bool newState = await this._internetConnectionChecker.CheckAsync();
        this.UpdateState(newState);
        if (!newState)
          await this._internetConnectionChecker.WaitConnectionAsync();
        else
          await Task.Delay(10000, this._lifetimeManager.CancellationToken);
      }
    }), TaskCreationOptions.LongRunning);

    private void UpdateState(bool newState)
    {
      if (this._state == newState)
        return;
      if (newState)
      {
        if (Settings.Default.HasLastDisconnectTime)
        {
          this.SendDisconnectedEvent(Settings.Default.LastDisconnectTime);
          this.SaveDisconnectionTime(new DateTime?());
          this.SendConnectedEvent(DateTime.UtcNow);
        }
      }
      else
        this.SaveDisconnectionTime(new DateTime?(DateTime.UtcNow));
      this._state = newState;
    }

    private void SaveDisconnectionTime(DateTime? disconnectionTime)
    {
      Settings.Default.HasLastDisconnectTime = disconnectionTime.HasValue;
      Settings.Default.LastDisconnectTime = disconnectionTime ?? DateTime.MinValue;
      Settings.Default.Save();
    }

    private void SendConnectedEvent(DateTime dt) => this._outputMessageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("internetConnected", (object) new
    {
      Timestamp = dt
    }));

    private void SendDisconnectedEvent(DateTime dt) => this._outputMessageDispatcherProvider.Get("main").Dispatch(new LauncherMessage("internetDisconnected", (object) new
    {
      Timestamp = dt
    }));
  }
}
