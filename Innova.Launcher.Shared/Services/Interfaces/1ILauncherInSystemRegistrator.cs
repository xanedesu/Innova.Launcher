﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Interfaces.LauncherRegistrationDataUpdate
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Services.Interfaces
{
  public class LauncherRegistrationDataUpdate
  {
    public string Key { get; set; }

    public string Name { get; set; }

    public string Version { get; set; }

    public DateTime UpdateDate { get; set; }

    public string UninstallCommand { get; set; }

    public long SizeBytes { get; set; }
  }
}
