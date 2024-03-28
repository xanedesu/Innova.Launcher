// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Enums.GameStatus
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services;
using Newtonsoft.Json;

namespace Innova.Launcher.Core.Enums
{
  [JsonConverter(typeof (StringEnumSnakeCaseConverter))]
  public enum GameStatus
  {
    NotInstalled,
    NotAvailable,
    InstallProgress,
    UpdateProgress,
    RepairProgress,
    InstallationPaused,
    UpdatePaused,
    RepairPaused,
    Installed,
    OutOfDate,
    NotAvailableBecauseOsVersion,
  }
}
