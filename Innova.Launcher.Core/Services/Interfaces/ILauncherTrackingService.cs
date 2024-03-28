// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.ILauncherTrackingService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models.Errors;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface ILauncherTrackingService
  {
    void SendTrackingInfo(string type, bool sendAsync = true);

    void SendUntrackedErrors();

    void SendTrackingError(LauncherError error);

    void UpdateLastErrorCheckDate();

    void SendLauncherUpdated(bool updateIsCritical);

    void SendLauncherUpdateStarted(bool updateIsCritical);

    void SendLauncherLaunched();

    void SendLauncherClosed();

    void SendMainWindowMinimized();

    void SendMainWindowRestored();

    void SendLauncherHidedToTray();

    void SendLauncherRestoredFromTray();

    void SendHardware();
  }
}
