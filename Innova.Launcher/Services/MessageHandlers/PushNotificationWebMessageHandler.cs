// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MessageHandlers.PushNotificationWebMessageHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Events;
using Innova.Launcher.Models;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Extensions;
using Innova.Launcher.Shared.Helpers;
using ReactiveUI;
using System;

namespace Innova.Launcher.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"pushNotification"})]
  public class PushNotificationWebMessageHandler : IWebMessageHandler
  {
    private readonly INotificationUrlService _notificationUrlService;
    private readonly ILauncherStateService _launcherStateService;

    public PushNotificationWebMessageHandler(
      INotificationUrlService notificationUrlService,
      ILauncherStateService launcherStateService)
    {
      this._notificationUrlService = notificationUrlService ?? throw new ArgumentNullException(nameof (notificationUrlService));
      this._launcherStateService = launcherStateService ?? throw new ArgumentNullException(nameof (launcherStateService));
    }

    public void Handle(WebMessage webMessage)
    {
      PushNotification notification = webMessage.Data.ToObject<PushNotification>();
      if (string.IsNullOrEmpty(notification.ImageUrl) && string.IsNullOrEmpty(notification.SVG))
        throw new InvalidOperationException("Image url or svg is required for push notification");
      notification.ImageUrl = PushNotificationWebMessageHandler.AddSchemaIfMissed(notification.ImageUrl);
      if (notification.PrimaryLink != null)
        notification.PrimaryLink.Url = UrlHelper.IsAbsoluteUrl(notification.PrimaryLink.Url) ? this._notificationUrlService.MakeNotificationUrl(notification.PrimaryLink.Url) : notification.PrimaryLink.Url;
      if (notification.SecondaryLink != null)
        notification.SecondaryLink.Url = UrlHelper.IsAbsoluteUrl(notification.SecondaryLink.Url) ? this._notificationUrlService.MakeNotificationUrl(notification.SecondaryLink.Url) : notification.SecondaryLink.Url;
      MessageBus.Current.SendMessage<PushNotificationOccuredEvent>(new PushNotificationOccuredEvent(notification));
    }

    private static string AddSchemaIfMissed(string url)
    {
      if (UrlHelper.IsAbsoluteUrl(url))
        return url;
      return string.IsNullOrWhiteSpace(url) ? (string) null : Uri.UriSchemeHttp + Uri.SchemeDelimiter + url;
    }

    private string ConvertRelativeToAbsolute(string url)
    {
      Uri result;
      return UrlHelper.IsAbsoluteUrl(url) || !Uri.TryCreate(this._launcherStateService.CurrentStartPage, UriKind.Absolute, out result) ? url : result.Scheme + Uri.SchemeDelimiter + result.Host + url.EnsureLeadingSlash();
    }
  }
}
