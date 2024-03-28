// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.Icons
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  public class Icons
  {
    [XmlAttribute("icons_url")]
    public string IconsUrl { get; set; }

    [XmlAttribute("covers_url")]
    public string CoversUrl { get; set; }

    [XmlAttribute("logos_url")]
    public string LogosUrl { get; set; }
  }
}
