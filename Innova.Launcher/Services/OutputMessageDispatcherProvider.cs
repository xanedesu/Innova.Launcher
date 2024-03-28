// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.OutputMessageDispatcherProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Collections.Concurrent;

namespace Innova.Launcher.Services
{
  public class OutputMessageDispatcherProvider : IOutputMessageDispatcherProvider
  {
    private readonly ConcurrentDictionary<string, IOutputMessageDispatcher> _dispatchers = new ConcurrentDictionary<string, IOutputMessageDispatcher>();

    public IOutputMessageDispatcher GetMain() => this.Get("main");

    public bool Remove(string windowId) => this._dispatchers.TryRemove(windowId, out IOutputMessageDispatcher _);

    public IOutputMessageDispatcher Get(string windowId) => this._dispatchers.GetOrAdd(windowId, (Func<string, IOutputMessageDispatcher>) (id =>
    {
      IOutputMessageDispatcher instance = ServiceLocator.Current.GetInstance<IOutputMessageDispatcher>();
      instance.WindowId = id;
      return instance;
    }));
  }
}
