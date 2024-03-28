// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.Interfaces.GameInstallProgressInfoEventArgs
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Updater.Core.Models;
using System;

namespace Innova.Launcher.Updater.Core.Services.Interfaces
{
  public class GameInstallProgressInfoEventArgs : EventArgs
  {
    public string GameKey { get; }

    public UpdateProgressInfo ProgressInfo { get; }

    public GameInstallProgressInfoEventArgs(string gameKey, UpdateProgressInfo progressInfo)
    {
      this.GameKey = gameKey;
      this.ProgressInfo = progressInfo;
    }
  }
}
