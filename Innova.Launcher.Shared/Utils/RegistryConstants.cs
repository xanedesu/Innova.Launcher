// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.RegistryConstants
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

namespace Innova.Launcher.Shared.Utils
{
  public static class RegistryConstants
  {
    public static class Uninstall
    {
      public static readonly string DisplayName = nameof (DisplayName);
      public static readonly string ApplicationVersion = nameof (ApplicationVersion);
      public static readonly string Publisher = nameof (Publisher);
      public static readonly string ShortcutName = nameof (ShortcutName);
      public static readonly string InstallLocation = nameof (InstallLocation);
      public static readonly string DisplayIcon = nameof (DisplayIcon);
      public static readonly string UninstallString = nameof (UninstallString);
      public static readonly string DisplayVersion = nameof (DisplayVersion);
      public static readonly string InstallationDate = "InstallDate";
      public static readonly string EstimatedSize = nameof (EstimatedSize);
      public static readonly string NoModify = nameof (NoModify);
      public static readonly string NoRepair = nameof (NoRepair);
      public static readonly string LastUpdateDate = nameof (LastUpdateDate);
      public static readonly string RegistryDateFormat = "yyyyMMdd";
    }

    public static class UrlScheme
    {
      public static readonly string UrlProtocolKey = "URL Protocol";
      public static readonly string DefaultIconKey = "DefaultIcon";
    }

    public static class Software
    {
      public static readonly string LauncherId = nameof (LauncherId);
      public static readonly string TrackingId = nameof (TrackingId);
      public static readonly string Version = nameof (Version);
      public static readonly string InstallationDate = nameof (InstallationDate);
      public static readonly string Path = nameof (Path);
      public static readonly string LastUpdateDate = nameof (LastUpdateDate);
      public static readonly string OldGamesTaken = nameof (OldGamesTaken);
      public static readonly string LastGamesInstallDirectory = nameof (LastGamesInstallDirectory);
      public static readonly string LastErrorCheckDate = nameof (LastErrorCheckDate);
      public static readonly string LauncherRegion = nameof (LauncherRegion);
    }

    public static class Format
    {
      public static readonly string DateFormat = "dd.MM.yyyy";
      public static readonly string DateTimeFormat = "dd.MM.yyyy HH:mm";
    }
  }
}
