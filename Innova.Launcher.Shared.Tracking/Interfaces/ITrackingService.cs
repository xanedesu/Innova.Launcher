// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Tracking.Interfaces.ITrackingService
// Assembly: Innova.Launcher.Shared.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 3617FF8A-994D-461B-B82B-0A45D5981063
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Tracking.dll

namespace Innova.Launcher.Shared.Tracking.Interfaces
{
  public interface ITrackingService
  {
    string TrackingId { get; set; }

    string LauncherId { get; }

    string HardwareId { get; }

    void Start();

    void Flush();
  }
}
