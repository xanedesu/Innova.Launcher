// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.ILauncherStateService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using System;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface ILauncherStateService
  {
    event EventHandler<IsAutoUpdatesAvailableChangedEventArgs> IsAutoUpdatesAvailableChanged;

    event EventHandler<RegionUpdatedEventArgs> RegionUpdated;

    event EventHandler RegionUpdating;

    string CurrentEnvironment { get; }

    string CurrentStartPage { get; }

    AppIdentityInfo CurrentIdentity { get; }

    bool IsAutoUpdatesAvailable { get; }

    string LauncherKey { get; }

    string LauncherName { get; }

    string LauncherRegion { get; }

    bool IsLocalVersion { get; }

    bool IsAppIdentityReceived { get; }

    string Culture { get; }

    void SetLastGamesFolder(string gamesFolderPath);

    string GetLastGamesFolder();

    void SetEnvironment(string newEnvironment);

    void SendLauncherIdentity(WebMessage webMessage);

    void SendUpdateInfo(AppUpdateInfo info);

    void SendUpdatedVersion(string version);

    void SendUpdateProgress(AppUpdateProgressInfo info);

    void RequestIsAutoUpdatesAvailable();

    void UpdateSettings(LauncherSettings settings);

    void UpdateIsAutoUpdatesAvailable(bool dataIsAutoUpdatesAvailable);

    void UpdateRegionByGame(string gameKey);

    void SendMainWindowSize(WebMessage webMessage, WindowSize mainWindowSize);

    void AppIdentityReceived();
  }
}
