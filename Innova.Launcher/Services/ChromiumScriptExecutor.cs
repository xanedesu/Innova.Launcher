// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.ChromiumScriptExecutor
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;
using Innova.Launcher.Services.Interfaces;
using System;

namespace Innova.Launcher.Services
{
  public class ChromiumScriptExecutor : IScriptExecutor<ChromiumWebBrowser>, IScriptExecutor
  {
    private volatile ChromiumWebBrowser _chromium;

    public void Init(ChromiumWebBrowser browser)
    {
      if (this._chromium != null)
        return;
      this._chromium = browser;
    }

    public void ExecuteJavaScript(string script, bool executeAsync = true)
    {
      this.ThrowIfNotInited();
      if (this._chromium.CanExecuteJavascriptInMainFrame)
        this._chromium.ExecuteScriptAsync(script);
      else
        this._chromium.SafeExecuteScriptAsyncWhenPageLoaded(script);
    }

    private void ThrowIfNotInited()
    {
      if (this._chromium == null)
        throw new InvalidOperationException("Browser not inited");
    }
  }
}
