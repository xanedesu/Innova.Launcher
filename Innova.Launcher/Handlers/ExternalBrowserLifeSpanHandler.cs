// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Handlers.ExternalBrowserLifeSpanHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using Innova.Launcher.Services.Interfaces;
using System;

namespace Innova.Launcher.Handlers
{
  public sealed class ExternalBrowserLifeSpanHandler : ILifeSpanHandler
  {
    private readonly IDefaultBrowserService _defaultBrowserService;

    public ExternalBrowserLifeSpanHandler(IDefaultBrowserService defaultBrowserService) => this._defaultBrowserService = defaultBrowserService ?? throw new ArgumentNullException(nameof (defaultBrowserService));

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
      this._defaultBrowserService.Start(targetUrl);
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
