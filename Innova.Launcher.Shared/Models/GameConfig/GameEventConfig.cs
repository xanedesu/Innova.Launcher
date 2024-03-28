// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.GameEventConfig
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Shared.Extensions;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  public class GameEventConfig
  {
    private string _launchTypeAsInt;

    [XmlAttribute("key")]
    public string EventKey { get; set; }

    [XmlAttribute("name")]
    public string EventName { get; set; }

    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("base_url")]
    public string BaseUrl { get; set; }

    [XmlAttribute("url")]
    public string Url { get; set; }

    [XmlAttribute("ex_name")]
    public string FrostGame { get; set; }

    [XmlAttribute("ln_chk_name")]
    public string FrostPath { get; set; }

    [XmlAttribute("ln_params")]
    public string LaunchParams { get; set; }

    [XmlAttribute("ln_name")]
    public string FrostLauncher { get; set; }

    [XmlAttribute("size")]
    public string Size { get; set; }

    [XmlAttribute("type")]
    public string Environment { get; set; }

    [XmlIgnore]
    public LaunchType LaunchType { get; private set; }

    [XmlAttribute("ln_type")]
    public string LaunchTypeAsInt
    {
      get => this._launchTypeAsInt;
      set
      {
        this._launchTypeAsInt = value;
        this.LaunchType = this._launchTypeAsInt.ToLaunchType();
      }
    }
  }
}
