// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.LaunchData
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;

namespace Innova.Launcher.Core.Models
{
  public class LaunchData
  {
    public string Path { get; set; }

    public string FrostPath { get; set; }

    public string LaunchParams { get; set; }

    public string ExePath { get; set; }

    public string FrostLauncherExe { get; set; }

    public LaunchType LaunchType { get; set; }
  }
}
