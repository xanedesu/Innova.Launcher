// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.GamesConfig
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  [XmlRoot("config")]
  public class GamesConfig
  {
    [XmlElement("icons", IsNullable = false)]
    public Icons Icons { get; set; }

    [XmlIgnore]
    public Icons SinglesIcons { get; set; }

    [XmlArray("games", IsNullable = false)]
    [XmlArrayItem("game")]
    public List<Innova.Launcher.Shared.Models.GameConfig.GameConfig> Games { get; set; }

    public string GetGameIconUrl(string gameKey) => Path.Combine((this.GetGameConfig(gameKey).IsSingle ? this.SinglesIcons : this.Icons).IconsUrl, gameKey + ".ico");

    public string GetGameLogoUrl(string gameKey) => Path.Combine((this.GetGameConfig(gameKey).IsSingle ? this.SinglesIcons : this.Icons).LogosUrl, gameKey + ".png");

    public string GetGameCoverUrl(string gameKey) => Path.Combine((this.GetGameConfig(gameKey).IsSingle ? this.SinglesIcons : this.Icons).CoversUrl, gameKey + ".png");

    public Innova.Launcher.Shared.Models.GameConfig.GameConfig GetGameConfig(string gameKey) => this.Games.FirstOrDefault<Innova.Launcher.Shared.Models.GameConfig.GameConfig>((Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, bool>) (a => a.Key == gameKey));
  }
}
