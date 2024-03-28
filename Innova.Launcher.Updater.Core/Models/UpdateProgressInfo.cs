// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Models.UpdateProgressInfo
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;

namespace Innova.Launcher.Updater.Core.Models
{
  public class UpdateProgressInfo
  {
    public Decimal Progress { get; set; }

    public int Speed { get; set; }

    public string FileName { get; set; }

    public long Downloaded { get; set; }

    public long Size { get; set; }
  }
}
