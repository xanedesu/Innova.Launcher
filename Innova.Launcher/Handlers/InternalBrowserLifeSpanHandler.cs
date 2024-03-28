// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Handlers.InternalBrowserLifeSpanHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using Innova.Launcher.Services;
using System;

namespace Innova.Launcher.Handlers
{
  public sealed class InternalBrowserLifeSpanHandler : ILifeSpanHandler
  {
    private readonly IBrowserDispatcher _browserDispatcher;

    public InternalBrowserLifeSpanHandler(IBrowserDispatcher browserDispatcher) => this._browserDispatcher = browserDispatcher ?? throw new ArgumentNullException(nameof (browserDispatcher));

    public bool OnBeforePopup(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      string targetUrl,
      string targetFrameName,
      WindowOpenDisposition targetDisposition,
      bool userGesture,
      IPopupFeatures popupFeatures,
      IWindowInfo windowInfo,
      IBrowserSettings browserSettings,
      ref bool noJavascriptAccess,
      out IWebBrowser newBrowser)
    {
      newBrowser = (IWebBrowser) null;
      this._browserDispatcher.Open(targetUrl);
      return true;
    }

    public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;

    public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }
  }
}
