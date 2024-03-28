// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameStateMachineTransitionException
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using System;

namespace Innova.Launcher.Core.Services
{
  public class GameStateMachineTransitionException : Exception
  {
    public GameStateMachineTransitionException(GameStatus fromState, Trigger trigger)
      : base(string.Format("The transition {0:G} from {1:G} can't be done.", (object) trigger, (object) fromState))
    {
    }
  }
}
