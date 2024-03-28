// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Events.NotificationOccuredEvent
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Models.Notifications;
using System;

namespace Innova.Launcher.Events
{
  public class NotificationOccuredEvent
  {
    public Notification Notification { get; }

    public NotificationOccuredEvent(Notification notification) => this.Notification = notification ?? throw new ArgumentNullException(nameof (notification));
  }
}
