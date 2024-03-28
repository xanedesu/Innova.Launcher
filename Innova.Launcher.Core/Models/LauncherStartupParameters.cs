// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.LauncherStartupParameters
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

namespace Innova.Launcher.Core.Models
{
  public class LauncherStartupParameters
  {
    public string GameKey { get; set; }

    public string FolderToInstallGame { get; set; }

    public string Environment { get; set; }

    public string GamesConfigUrl { get; set; }

    public string SingleGamesConfigUrl { get; set; }

    public string SingleGamesValidationUrl { get; set; }

    public string StartPage { get; set; }

    public string RelativePath { get; set; }

    public string Region { get; set; }

    public string TrackingId { get; set; }

    public string Origin { get; set; }

    public string Culture { get; set; }
  }
}
