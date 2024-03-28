// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Models.TrackingEvent
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

using System;

namespace Innova.Launcher.Shared.Tracking.Models
{
  public class TrackingEvent
  {
    public UserEventBase UserEvent { get; }

    public TrackingEvent(UserEventBase userEvent)
    {
      this.UserEvent = userEvent ?? throw new ArgumentNullException(nameof (userEvent));
      this.UserEvent.When = DateTime.UtcNow;
    }
  }
}
