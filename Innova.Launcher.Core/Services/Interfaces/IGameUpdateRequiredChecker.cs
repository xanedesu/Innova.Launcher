// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.IGameUpdateRequiredChecker
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface IGameUpdateRequiredChecker
  {
    ICollection<ServiceStatus> CheckGamesUpdateRequiredStatus(ICollection<Game> games);
  }
}
