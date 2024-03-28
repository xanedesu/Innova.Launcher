// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.WebMessageType
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

namespace Innova.Launcher.Core.Services.Interfaces
{
  public static class WebMessageType
  {
    public const string Install = "install";
    public const string Update = "update";
    public const string Repair = "repair";
    public const string Redirect = "redirect";
    public const string Cancel = "cancel";
    public const string Pause = "pause";
    public const string Resume = "resume";
    public const string Play = "play";
    public const string PlayEncrypted = "playEncrypted";
    public const string PlaySuccess = "playSuccess";
    public const string PlayFailed = "playFailed";
    public const string SetGameCulture = "setGameCulture";
    public const string GetLanguage = "getLanguage";
    public const string GetStatuses = "getStatuses";
    public const string GetVersions = "getVersions";
    public const string GetMainWindowSize = "getMainWindowSize";
    public const string GetOpenedWindows = "getOpenedWindows";
    public const string GetInstallDir = "getInstallDir";
    public const string BrowseDir = "browseDir";
    public const string GetAppIdentity = "getAppIdentity";
    public const string AppIdentityReceived = "appIdentityReceived";
    public const string Configure = "configure";
    public const string LauncherUpdated = "appUpdated";
    public const string StartLauncherUpdate = "startUpdateApp";
    public const string CancelLauncherUpdate = "cancelUpdateApp";
    public const string AppIdentity = "appIdentity";
    public const string GamesStatus = "status";
    public const string WindowClosed = "closed";
    public const string WindowStateChanged = "windowStateChanged";
    public const string WindowActivated = "windowActivated";
    public const string WindowDeactivated = "windowDeactivated";
    public const string NotificationRead = "notificationRead";
    public const string SetInstallDir = "setInstallDir";
    public const string GetIsAutoUpdateEnabled = "getIsAutoUpdateEnabled";
    public const string SetIsAutoUpdateEnabled = "setIsAutoUpdateEnabled";
    public const string LauncherSettingsUpdated = "launcherSettingsUpdated";
    public const string GetUserData = "getUserData";
    public const string SetUserData = "setUserData";
    public const string SessionEnded = "sessionEnded";
    public const string TrackingEvent = "launcherRuntimeEvent";
    public const string SaveLocalStorage = "saveLocalStorage";
    public const string RestoreLocalStorage = "restoreLocalStorage";
    public const string SetGameEvent = "setGameEvent";
    public const string InternetDisconnected = "internetDisconnected";
    public const string InternetConnected = "internetConnected";

    public static class Accounts
    {
      public const string Get = "getAccounts";
      public const string Save = "saveAccount";
      public const string Delete = "deleteAccount";
      public const string LoggedOut = "loggedOut";
      public const string LogOut = "logout";
    }
  }
}
