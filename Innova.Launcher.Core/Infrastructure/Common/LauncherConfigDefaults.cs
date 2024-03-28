// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Common.LauncherConfigDefaults
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

namespace Innova.Launcher.Core.Infrastructure.Common
{
  public static class LauncherConfigDefaults
  {
    public const string WhiteList = "^(.*-)?launcher(\\.[a-z]+)?\\.(test4game|4game)\\.com";
    public const string FrostUpdaterUrl = "http://frostsecurity.net/frost/frostupdater/";
    public const string LauncherHostingRoot = "http://cdn.inn.ru/new4game/launcher/";
    public const string StartPage = "https://launcher.{region}.4game.com/";
    public const string GamesConfigUrl = "http://cdn.inn.ru/4game/config-live.xml";
    public const string SingleGamesConfigUrl = "http://cdn.inn.ru/4game/config-singles-live.xml";
    public const string ProxyConfigUrl = "http://cdn.inn.ru/new4game/launcher/proxy.pac";
    public const string SingleGamesValidationUrl = "https://ru-ps.4gametest.com/singles/access/";
    public const string LauncherEnvironment = "prod";
    public const string Region = "ru";
    public const int GameVersionPollingInterval = 600000;
    public const int ForgameUpdaterTcpServerPort = 56563;
    public const int LauncherVersionPollingInterval = 600000;
    public const string TrackingUrl = "https://launcherbff.{region}.4game.com/api/exporter/events";
  }
}
