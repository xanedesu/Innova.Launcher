// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Handlers.ForgameLoadHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using CefSharp.Handler;
using Innova.Launcher.Services;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Newtonsoft.Json;
using NLog;
using System;

namespace Innova.Launcher.Handlers
{
  public class ForgameLoadHandler : RequestHandler
  {
    private readonly IDefaultBrowserService _defaultBrowserService;
    private readonly INotificationUrlService _notificationUrlService;
    private readonly IApplicationUrlLoadService _applicationUrlLoadService;
    private readonly IWhiteListService _whiteListService;
    private readonly string _blankPageUrl = "about:blank";
    private readonly ILogger _logger;

    public ForgameLoadHandler(
      IBrowserDispatcher browserDispatcher,
      ILoggerFactory loggerFactory,
      IDefaultBrowserService defaultBrowserService,
      IApplicationUrlLoadService applicationUrlLoadService,
      INotificationUrlService notificationUrlService,
      IWhiteListService whiteListService)
    {
      this._defaultBrowserService = defaultBrowserService ?? throw new ArgumentNullException(nameof (defaultBrowserService));
      this._applicationUrlLoadService = applicationUrlLoadService ?? throw new ArgumentNullException(nameof (applicationUrlLoadService));
      this._notificationUrlService = notificationUrlService ?? throw new ArgumentNullException(nameof (notificationUrlService));
      this._whiteListService = whiteListService ?? throw new ArgumentNullException(nameof (whiteListService));
      this._logger = loggerFactory.GetCurrentClassLogger<ForgameLoadHandler>();
    }

    protected override bool OnCertificateError(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      CefErrorCode errorCode,
      string requestUrl,
      ISslInfo sslInfo,
      IRequestCallback callback)
    {
      var data = new
      {
        requestUrl = requestUrl,
        errorCode = errorCode
      };
      this._logger.Error("Handle SSL error: " + JsonConvert.SerializeObject((object) data));
      return true;
    }

    protected override bool OnBeforeBrowse(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      IRequest request,
      bool userGesture,
      bool isRedirect)
    {
      string url = request.Url;
      if (this._defaultBrowserService.CanHandle(url))
      {
        this._defaultBrowserService.Start(url);
        return true;
      }
      bool flag = this._notificationUrlService.IsNotificationUrl(url);
      if (this._applicationUrlLoadService.Handle(url) != OpenApplicationResult.Ignore)
        return true;
      if ((this._whiteListService.InWhiteList(url) || ((request.TransitionType != TransitionType.LinkClicked ? 0 : (frame.IsMain ? 1 : 0)) | (flag ? 1 : 0)) == 0 ? 0 : (url != this._blankPageUrl ? 1 : 0)) == 0)
        return false;
      this._defaultBrowserService.Start(url);
      return true;
    }
  }
}
