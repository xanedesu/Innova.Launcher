// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.GameConfig
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  [DebuggerDisplay("{Key}")]
  public class GameConfig
  {
    private string _stageSizes;

    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("key")]
    public string OldKey { get; set; }

    [XmlAttribute("key_new")]
    public string Key { get; set; }

    [XmlIgnore]
    public string EnvKey => this.Key + "_" + this.Environment;

    [XmlIgnore]
    public string FrostKey => this.OldKey + "_" + this.Environment;

    [XmlAttribute("install_key")]
    public string InstallKey { get; set; }

    [XmlIgnore]
    public string InstallEnvKey => this.InstallKey + "_" + this.Environment;

    [XmlAttribute("type")]
    public string Environment { get; set; }

    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("display_name")]
    public string DisplayName { get; set; }

    [XmlAttribute("shortcut_title")]
    public string ShortcutTitle { get; set; }

    [XmlIgnore]
    public bool IsSingle { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("version_check_url")]
    public string VersionCheckUrl { get; set; }

    [XmlAttribute("updater_exe_name")]
    public string UpdaterExeName { get; set; }

    [XmlAttribute("url")]
    public string Url { get; set; }

    [XmlIgnore]
    public bool CheckTorrent { get; private set; }

    [XmlAttribute("check_torrent")]
    public string CheckTorrentStr
    {
      get => (this.CheckTorrent ? 1 : 0).ToString("D");
      set => this.CheckTorrent = value == "1";
    }

    [XmlIgnore]
    public bool OnlyX64 { get; private set; }

    [XmlAttribute("only_x64")]
    public string OnlyX64Str
    {
      get => (this.OnlyX64 ? 1 : 0).ToString("D");
      set => this.OnlyX64 = value == "1";
    }

    [XmlIgnore]
    public bool HasLightClient { get; private set; }

    [XmlAttribute("has_light_client")]
    public string HasLightClientStr
    {
      get => (this.HasLightClient ? 1 : 0).ToString("D");
      set => this.HasLightClient = value == "1";
    }

    [XmlAttribute("game_url_2")]
    public string GameUrl { get; set; }

    [XmlAttribute("base_url")]
    public string BaseUrl { get; set; }

    [XmlAttribute("ex_name")]
    public string FrostGame { get; set; }

    [XmlAttribute("ln_chk_name")]
    public string FrostPath { get; set; }

    [XmlAttribute("ln_params")]
    public string LaunchParams { get; set; }

    [XmlAttribute("ln_name")]
    public string FrostLauncher { get; set; }

    [XmlAttribute("opt_name")]
    public string OptionsExe { get; set; }

    [XmlAttribute("size")]
    public string Size { get; set; }

    [XmlAttribute("description")]
    public string Description { get; set; }

    [XmlAttribute("workdir")]
    public string Workdir { get; set; }

    [XmlIgnore]
    public long[] StageSizesArray { get; private set; } = new long[0];

    [XmlAttribute("stage_sizes")]
    public string StageSizes
    {
      get => this._stageSizes;
      set
      {
        this._stageSizes = value;
        if (string.IsNullOrWhiteSpace(value))
        {
          this.StageSizesArray = new long[0];
        }
        else
        {
          long result;
          this.StageSizesArray = ((IEnumerable<string>) value.Split(',')).Select<string, long>((Func<string, long>) (e => long.TryParse(e, out result) ? result : 0L)).ToArray<long>();
        }
      }
    }

    [XmlIgnore]
    public LaunchType LaunchType { get; private set; }

    [XmlAttribute("ln_type")]
    public string LaunchTypeAsInt
    {
      get => ((int) this.LaunchType).ToString();
      set => this.LaunchType = value.ToLaunchType();
    }

    [XmlElement("check")]
    public GameComponents Components { get; set; } = new GameComponents();

    [XmlElement("uninstall")]
    public GameUninstallerConfig Uninstall { get; set; }

    [XmlElement("env")]
    public GameEventConfigs Events { get; set; } = new GameEventConfigs();

    [XmlArray("languages")]
    [XmlArrayItem("language")]
    public GameLanguage[] Languages { get; set; }

    public Innova.Launcher.Shared.Models.GameConfig.GameConfig WithEvent(string eventKey)
    {
      GameEventConfig gameEventConfig = this.Events.FirstOrDefault<GameEventConfig>((Func<GameEventConfig, bool>) (e => e.EventKey == eventKey));
      if (gameEventConfig == null)
        return this;
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this.Clone();
      if (gameEventConfig.Version != null)
        gameConfig.Version = gameEventConfig.Version;
      if (gameEventConfig.BaseUrl != null)
        gameConfig.BaseUrl = gameEventConfig.BaseUrl;
      if (gameEventConfig.FrostGame != null)
        gameConfig.FrostGame = gameEventConfig.FrostGame;
      if (gameEventConfig.FrostPath != null)
        gameConfig.FrostPath = gameEventConfig.FrostPath;
      if (gameEventConfig.LaunchParams != null)
        gameConfig.LaunchParams = gameEventConfig.LaunchParams;
      if (gameEventConfig.FrostLauncher != null)
        gameConfig.FrostLauncher = gameEventConfig.FrostLauncher;
      if (gameEventConfig.Size != null)
        gameConfig.Size = gameEventConfig.Size;
      if (gameEventConfig.LaunchTypeAsInt != null)
        gameConfig.LaunchTypeAsInt = gameEventConfig.LaunchTypeAsInt;
      if (gameEventConfig.Url != null)
        gameConfig.Url = gameEventConfig.Url;
      if (gameEventConfig.Environment != null)
        gameConfig.Environment = gameEventConfig.Environment;
      gameConfig.OldKey = gameEventConfig.EventKey;
      return gameConfig;
    }

    public Innova.Launcher.Shared.Models.GameConfig.GameConfig WithVersion(string version)
    {
      Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig = this.Clone();
      gameConfig.Version = version;
      return gameConfig;
    }

    private Innova.Launcher.Shared.Models.GameConfig.GameConfig Clone() => this.MemberwiseClone() as Innova.Launcher.Shared.Models.GameConfig.GameConfig;
  }
}
