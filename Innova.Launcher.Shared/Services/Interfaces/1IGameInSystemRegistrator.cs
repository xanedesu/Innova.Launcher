// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Interfaces.GameRegistrationData
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Services.Interfaces
{
  public class GameRegistrationData
  {
    public string LauncherKey { get; set; }

    public string IconPath { get; set; }

    public string Name { get; set; }

    public string ShotcutTitle { get; set; }

    public string UninstallCommand { get; set; }

    public string Key { get; set; }

    public string Version { get; set; }

    public string RunnerPath { get; set; }

    public byte[] IconData { get; set; }

    public string InstallationPath { get; set; }

    public DateTime InstallationDate { get; set; }

    public string Size { get; set; }

    public string RunnerArgs { get; set; }
  }
}
