// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Common.Interfaces.ILauncherConfigurationProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;

namespace Innova.Launcher.Core.Infrastructure.Common.Interfaces
{
  public interface ILauncherConfigurationProvider
  {
    LauncherStartupParameters StartupParameters { get; }

    string FrostUpdaterUrl { get; }

    string RelativePath { get; }

    string GamesConfigUrl { get; }

    string SingleGamesConfigUrl { get; }

    string SingleGamesValidationUrl { get; }

    int GameVersionPollingInterval { get; }

    int LauncherVersionPollingInterval { get; }

    int ForgameUpdaterTcpServerPort { get; }

    string LauncherEnvironment { get; }

    string WhiteList { get; }

    string StartPage { get; }

    string StartupGameKey { get; }

    string StartupGameInstallPath { get; }

    string LauncherRegion { get; }

    string TrackingId { get; }

    string Culture { get; }
  }
}
