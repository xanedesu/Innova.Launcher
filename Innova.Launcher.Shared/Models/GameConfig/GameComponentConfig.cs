// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.GameComponentConfig
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  public class GameComponentConfig
  {
    [XmlAttribute("name")]
    public string Name { get; set; }

    [XmlAttribute("enableForOS")]
    public string EnableForOS { get; set; }

    [XmlAttribute("path")]
    public string Path { get; set; }

    [XmlAttribute("key")]
    public string Key { get; set; }

    [XmlAttribute("value")]
    public string Value { get; set; }

    [XmlAttribute("exe")]
    public string Exe { get; set; }

    [XmlAttribute("source")]
    public string Source { get; set; }

    [XmlAttribute("compare_type")]
    public string CompareType { get; set; }

    [XmlAttribute("install")]
    public string Install { get; set; }

    [XmlAttribute("args")]
    public string Args { get; set; }
  }
}
