// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.ScriptExecutorProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;
using CommonServiceLocator;
using Innova.Launcher.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Innova.Launcher.Services
{
  public class ScriptExecutorProvider : IScriptExecutorProvider
  {
    private readonly ConcurrentDictionary<string, IScriptExecutor> _executors = new ConcurrentDictionary<string, IScriptExecutor>();

    public void Add(string windowId, ChromiumWebBrowser browser) => this._executors.AddOrUpdate(windowId, (Func<string, IScriptExecutor>) (_ =>
    {
      IScriptExecutor<ChromiumWebBrowser> instance = ServiceLocator.Current.GetInstance<IScriptExecutor<ChromiumWebBrowser>>();
      instance.Init(browser);
      ((Component) browser).Disposed += (EventHandler) ((_param1, _param2) => this._executors.TryRemove(windowId, out IScriptExecutor _));
      return (IScriptExecutor) instance;
    }), (Func<string, IScriptExecutor, IScriptExecutor>) ((k, v) => v));

    public IScriptExecutor Get(string windowId)
    {
      IScriptExecutor scriptExecutor;
      return !this._executors.TryGetValue(windowId, out scriptExecutor) ? (IScriptExecutor) null : scriptExecutor;
    }
  }
}
