// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherConfigurationProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services;

namespace Innova.Launcher.Core.Services
{
  public class LauncherConfigurationProvider : ILauncherConfigurationProvider
  {
    public LauncherStartupParameters StartupParameters { get; }

    public string FrostUpdaterUrl { get; }

    public string GamesConfigUrl { get; }

    public string SingleGamesConfigUrl { get; }

    public int GameVersionPollingInterval { get; }

    public int ForgameUpdaterTcpServerPort { get; }

    public string LauncherEnvironment { get; }

    public string WhiteList { get; }

    public string RelativePath { get; }

    public string StartPage { get; }

    public string StartupGameKey { get; }

    public string StartupGameInstallPath { get; }

    public string LauncherRegion { get; }

    public int LauncherVersionPollingInterval { get; }

    public string TrackingId { get; }

    public string Origin { get; }

    public string Culture { get; }

    public string SingleGamesValidationUrl { get; }

    public LauncherConfigurationProvider(
      ILauncherStartupParametersParser launcherStartupParametersParser,
      ICommandLineArgsProvider commandLineArgsProvider,
      ILoggerFactory loggerFactory)
    {
      this.FrostUpdaterUrl = "http://frostsecurity.net/frost/frostupdater/";
      this.WhiteList = "^(.*-)?launcher(\\.[a-z]+)?\\.(test4game|4game)\\.com";
      this.GameVersionPollingInterval = 600000;
      this.ForgameUpdaterTcpServerPort = 56563;
      this.LauncherVersionPollingInterval = 600000;
      LauncherStartupParameters parametersFromArgs = launcherStartupParametersParser.ParseParametersFromArgs(commandLineArgsProvider.GetCommandLineArgs());
      this.StartupGameKey = parametersFromArgs.GameKey;
      this.StartupGameInstallPath = parametersFromArgs.FolderToInstallGame;
      this.StartupParameters = parametersFromArgs;
      this.RelativePath = parametersFromArgs.RelativePath;
      this.LauncherEnvironment = !string.IsNullOrWhiteSpace(parametersFromArgs.Environment) ? parametersFromArgs.Environment : "prod";
      this.GamesConfigUrl = !string.IsNullOrWhiteSpace(parametersFromArgs.GamesConfigUrl) ? parametersFromArgs.GamesConfigUrl : "http://cdn.inn.ru/4game/config-live.xml";
      this.SingleGamesConfigUrl = !string.IsNullOrWhiteSpace(parametersFromArgs.SingleGamesConfigUrl) ? parametersFromArgs.SingleGamesConfigUrl : "http://cdn.inn.ru/4game/config-singles-live.xml";
      this.SingleGamesValidationUrl = !string.IsNullOrWhiteSpace(parametersFromArgs.SingleGamesValidationUrl) ? parametersFromArgs.SingleGamesValidationUrl : "https://ru-ps.4gametest.com/singles/access/";
      this.StartPage = !string.IsNullOrWhiteSpace(parametersFromArgs.StartPage) ? parametersFromArgs.StartPage : "https://launcher.{region}.4game.com/";
      this.LauncherRegion = !string.IsNullOrWhiteSpace(parametersFromArgs.Region) ? parametersFromArgs.Region : (string) null;
      this.TrackingId = !string.IsNullOrWhiteSpace(parametersFromArgs.TrackingId) ? parametersFromArgs.TrackingId : (string) null;
      this.Origin = !string.IsNullOrWhiteSpace(parametersFromArgs.Origin) ? parametersFromArgs.Origin : (string) null;
      this.Culture = parametersFromArgs.Culture;
    }
  }
}
