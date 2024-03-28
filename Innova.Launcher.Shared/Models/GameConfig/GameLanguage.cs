﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.GameLanguage
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Models.GameConfig.Operations;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig
{
  public class GameLanguage
  {
    [XmlAttribute("language")]
    public string Culture { get; set; }

    [XmlAttribute("displayName")]
    public string DisplayName { get; set; }

    [XmlAttribute("description")]
    public string Description { get; set; }

    [XmlAttribute("launchParam")]
    public string LaunchParam { get; set; }

    [XmlArray("beforeLaunch")]
    [XmlArrayItem("replace", typeof (ReplaceOperation))]
    [XmlArrayItem("copyFolder", typeof (CopyFolderOperation))]
    public Operation[] BeforeLaunch { get; set; }
  }
}
