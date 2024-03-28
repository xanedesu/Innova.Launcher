// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.Operations.ReplaceOperation
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System.IO;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig.Operations
{
  public class ReplaceOperation : Operation
  {
    [XmlAttribute("from")]
    public string From { get; set; }

    [XmlAttribute("to")]
    public string To { get; set; }

    public override void Execute() => File.Copy(this.From, this.To, true);
  }
}
