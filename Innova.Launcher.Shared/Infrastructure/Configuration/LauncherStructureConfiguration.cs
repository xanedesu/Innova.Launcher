// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.Configuration.LauncherStructureConfiguration
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Configuration.Interfaces;
using Innova.Launcher.Shared.Utils;
using System;

namespace Innova.Launcher.Shared.Infrastructure.Configuration
{
  public class LauncherStructureConfiguration : 
    EmbeddedConfigProvider<LauncherStructureConfiguration>,
    ILauncherStructureConfigurationProvider
  {
    public string RunnerExeName { get; }

    public string BinariesFolderName { get; }

    public string LauncherExeName { get; }

    public string TempUpdateFolderName { get; }

    public string UpdateVersionFileName { get; set; }

    public string RunnerFromUpdateExeName { get; }

    public LauncherStructureConfiguration()
      : base("Infrastructure.Configuration.structure.conf")
    {
      this.RunnerExeName = this.Get("runnerExeName") ?? throw new ArgumentNullException(nameof (RunnerExeName));
      this.BinariesFolderName = this.Get("binariesFolderName") ?? throw new ArgumentNullException(nameof (BinariesFolderName));
      this.LauncherExeName = this.Get("launcherExeName") ?? throw new ArgumentNullException(nameof (LauncherExeName));
      this.TempUpdateFolderName = this.Get("tempUpdateFolderName") ?? throw new ArgumentNullException(nameof (TempUpdateFolderName));
      this.UpdateVersionFileName = this.Get("updateVersionFileName") ?? throw new ArgumentNullException(nameof (UpdateVersionFileName));
      this.RunnerFromUpdateExeName = this.Get("runnerFromUpdateExeName") ?? throw new ArgumentNullException(nameof (RunnerFromUpdateExeName));
    }
  }
}
