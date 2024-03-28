// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Properties.Settings
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Innova.Launcher.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default => Settings.defaultInstance;

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("0")]
    public double ZoomLevel
    {
      get => (double) this[nameof (ZoomLevel)];
      set => this[nameof (ZoomLevel)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    public DateTime LastDisconnectTime
    {
      get => (DateTime) this[nameof (LastDisconnectTime)];
      set => this[nameof (LastDisconnectTime)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool CookieMigrated
    {
      get => (bool) this[nameof (CookieMigrated)];
      set => this[nameof (CookieMigrated)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool HasLastDisconnectTime
    {
      get => (bool) this[nameof (HasLastDisconnectTime)];
      set => this[nameof (HasLastDisconnectTime)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("True")]
    public bool UpgradeRequired
    {
      get => (bool) this[nameof (UpgradeRequired)];
      set => this[nameof (UpgradeRequired)] = (object) value;
    }

    [UserScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool CookieImported
    {
      get => (bool) this[nameof (CookieImported)];
      set => this[nameof (CookieImported)] = (object) value;
    }
  }
}
