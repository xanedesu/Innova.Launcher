﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.RegisterLauncherSoftwareInfo
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Utils
{
  public class RegisterLauncherSoftwareInfo
  {
    public string Publisher { get; set; }

    public string LauncherKey { get; set; }

    public string InstallationPath { get; set; }

    public string Version { get; set; }

    public DateTime? InstallationDate { get; set; }

    public string LauncherId { get; set; }

    public string TrackingId { get; set; }

    public bool? OldGamesTaken { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string LastGamesInstallDirectory { get; set; }

    public string LauncherRegion { get; set; }
  }
}
