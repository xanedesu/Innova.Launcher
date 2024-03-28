// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.LauncherDeployVersionProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using System;
using System.Reflection;

namespace Innova.Launcher.Services
{
  public class LauncherDeployVersionProvider : ILauncherVersionProvider
  {
    public string CurrentLauncherVersion { get; private set; }

    public LauncherDeployVersionProvider() => this.CurrentLauncherVersion = this.FormatVersion(Assembly.GetExecutingAssembly().GetName().Version);

    public void UpdateVersion(string version) => this.CurrentLauncherVersion = version;

    private string FormatVersion(Version version) => version.ToString();
  }
}
