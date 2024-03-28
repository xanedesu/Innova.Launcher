// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.EnvironmentConfigurationProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Innova.Launcher.Services
{
  public class EnvironmentConfigurationProvider : IEnvironmentConfigurationProvider
  {
    private static readonly string ConfigResourceName = "environments.json";
    private readonly EnvironmentConfig _config;

    public EnvironmentConfigurationProvider()
    {
      using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Innova.Launcher." + EnvironmentConfigurationProvider.ConfigResourceName))
      {
        if (manifestResourceStream == null)
          throw new InvalidOperationException("There is no embeded resource " + EnvironmentConfigurationProvider.ConfigResourceName + " in assembly");
        using (StreamReader streamReader = new StreamReader(manifestResourceStream))
        {
          using (JsonTextReader reader = new JsonTextReader((TextReader) streamReader))
            this._config = new JsonSerializer().Deserialize<EnvironmentConfig>((JsonReader) reader);
        }
      }
    }

    public string GetFullVersionsFilePath(string environment) => Path.Combine("http://cdn.inn.ru/new4game/launcher/", this._config.VersionsFilePrefix + "_" + environment + this._config.VersionsFileExtention);

    public string GetVersionReleaseInfoFilePath(string version) => Path.Combine("http://cdn.inn.ru/new4game/launcher/", version ?? "", this._config.VersionReleaseInfoFileName ?? "");

    public string GetVersionHostingPath(string version) => Path.Combine("http://cdn.inn.ru/new4game/launcher/", version ?? "");
  }
}
