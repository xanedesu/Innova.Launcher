// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Extensions.WebBrowserExtensions
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using System;

namespace Innova.Launcher.Extensions
{
  public static class WebBrowserExtensions
  {
    public static void SafeExecuteScriptAsyncWhenPageLoaded(
      this IWebBrowser webBrowser,
      string script,
      bool oneTime = true)
    {
      bool flag = !webBrowser.IsBrowserInitialized || !oneTime;
      if (webBrowser.IsBrowserInitialized)
      {
        IBrowser browser = webBrowser.GetBrowser();
        if (browser.HasDocument && !browser.IsLoading)
          webBrowser.ExecuteScriptAsync(script);
        else
          flag = true;
      }
      if (!flag)
        return;
      webBrowser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(Handler);

      void Handler(object sender, LoadingStateChangedEventArgs args)
      {
        if (args.IsLoading || !webBrowser.CanExecuteJavascriptInMainFrame)
          return;
        if (oneTime)
          webBrowser.LoadingStateChanged -= new EventHandler<LoadingStateChangedEventArgs>(Handler);
        webBrowser.ExecuteScriptAsync(script);
      }
    }
  }
}
