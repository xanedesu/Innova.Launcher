// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.StatusSender
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services
{
  public class StatusSender : IStatusSender
  {
    private readonly IMessageBus _messageBus;
    private readonly IOutputMessageDispatcher _messageDispatcher;

    public StatusSender(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      IMessageBus messageBus)
    {
      this._messageBus = messageBus;
      this._messageDispatcher = outputMessageDispatcherProvider.GetMain();
    }

    public void Send(ServiceStatus status) => this.Send((IEnumerable<ServiceStatus>) new ServiceStatus[1]
    {
      status
    });

    public void Send(IEnumerable<ServiceStatus> statuses)
    {
      List<ServiceStatus> list = statuses.ToList<ServiceStatus>();
      foreach (ServiceStatus status in list)
        this._messageBus.SendMessage<GameStatusChangingEvent>(new GameStatusChangingEvent(status));
      this._messageDispatcher.Dispatch((LauncherMessage) new StatusesLauncherMessage(new StatusesLauncherMessageData()
      {
        ServiceStatuses = (IEnumerable<ServiceStatus>) list
      }));
    }
  }
}
