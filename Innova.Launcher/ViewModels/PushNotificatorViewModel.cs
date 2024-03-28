// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.PushNotificatorViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Events;
using Innova.Launcher.Models;
using Innova.Launcher.Services;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.Views;
using ReactiveUI;
using System;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Innova.Launcher.ViewModels
{
  public sealed class PushNotificatorViewModel : ReactiveObject
  {
    private readonly BlockingCollection<PushNotification> _notifications;
    private readonly Task _showNotificationsTask;

    public PushNotificatorViewModel(IPushNotificationCoordinator coordinator)
    {
      this._notifications = new BlockingCollection<PushNotification>();
      this._showNotificationsTask = Task.Factory.StartNew(new Action(this.ShowNotifications), TaskCreationOptions.LongRunning);
      MessageBus.Current.ListenIncludeLatest<PushNotificationOccuredEvent>().Where<PushNotificationOccuredEvent>((Func<PushNotificationOccuredEvent, bool>) (v => v?.Notification != null)).SubscribeOnDispatcher<PushNotificationOccuredEvent>().ObserveOnDispatcher<PushNotificationOccuredEvent>().Subscribe<PushNotificationOccuredEvent>((Action<PushNotificationOccuredEvent>) (v => this._notifications.Add(v.Notification)));
    }

    private void ShowNotifications()
    {
      foreach (PushNotification consuming in this._notifications.GetConsumingEnumerable())
      {
        PushNotification notification = consuming;
        this.Gui<SyncWindow>((Func<SyncWindow>) (() =>
        {
          PushNotificationViewModel notificationViewModel = new PushNotificationViewModel(notification);
          SyncWindow syncWindow = new SyncWindow((Window) new PushNotificatorWindow()
          {
            DataContext = (object) notificationViewModel
          });
          syncWindow.Show();
          return syncWindow;
        })).Wait();
      }
    }
  }
}
