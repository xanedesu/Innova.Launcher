// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.AppIdentity.SendLauncherLaunchedOnFirstAppIdentityReceivedHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using System.Threading;

namespace Innova.Launcher.Core.Services.MessageHandlers.AppIdentity
{
  [WebMessageHandlerFilter(new string[] {"appIdentityReceived"})]
  public class SendLauncherLaunchedOnFirstAppIdentityReceivedHandler : IWebMessageHandler
  {
    private readonly ILauncherTrackingService _launcherTrackingService;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private static long _messagesReceived;
    private const long _messageToSendLauncherLaunchedOn = 1;

    public SendLauncherLaunchedOnFirstAppIdentityReceivedHandler(
      ILauncherTrackingService launcherTrackingService,
      ILauncherStateService launcherStateService,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._launcherTrackingService = launcherTrackingService;
      this._launcherStateService = launcherStateService;
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider;
    }

    public void Handle(WebMessage webMessage)
    {
      if (Interlocked.Increment(ref SendLauncherLaunchedOnFirstAppIdentityReceivedHandler._messagesReceived) != 1L)
        return;
      this._launcherTrackingService.SendLauncherLaunched();
      this._launcherStateService.AppIdentityReceived();
      this._outputMessageDispatcherProvider.GetMain().Dispatch(new LauncherMessage("windowActivated"));
    }
  }
}
