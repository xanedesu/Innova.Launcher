// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.PushNotificationCoordinator
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Events;
using Innova.Launcher.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Innova.Launcher.Services
{
  public sealed class PushNotificationCoordinator : IPushNotificationCoordinator
  {
    public PushNotificationCoordinator(
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      IOutputMessageDispatcher dispatcher = outputMessageDispatcherProvider.GetMain();
      MessageBus.Current.Listen<PushNotificationReadEvent>().Select<PushNotificationReadEvent, int>((Func<PushNotificationReadEvent, int>) (v => v.Id)).DistinctUntilChanged<int>().Subscribe<int>((Action<int>) (id => dispatcher.Dispatch((LauncherMessage) new NotificationReadLauncherMessage(id))));
    }
  }
}
