// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Helpers.IpcConnectionHelper
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Innova.Launcher.Updater.Core.Helpers
{
  public static class IpcConnectionHelper
  {
    public static void SubscribeToJsonIpcServer(
      IIpcServer<IIpcConnection> ipcServer,
      Action<IIpcConnection, IObservable<JObject>> connectionHandler)
    {
      ipcServer.Connected += (IpcServerConnectedEventHander<IIpcConnection>) ((_, args) =>
      {
        IIpcConnection connection = args.Connection;
        IObservable<JObject> observable = Observable.FromEventPattern<IpcMessageEventArgs>((Action<EventHandler<IpcMessageEventArgs>>) (h => connection.MessageReceived += h), (Action<EventHandler<IpcMessageEventArgs>>) (h => connection.MessageReceived -= h)).Select<EventPattern<IpcMessageEventArgs>, JObject>((Func<EventPattern<IpcMessageEventArgs>, JObject>) (v => JObject.Parse(v.EventArgs.Message)));
        Action<IIpcConnection, IObservable<JObject>> action = connectionHandler;
        if (action != null)
          action(connection, observable);
        connection.StartReceive();
      });
    }
  }
}
