// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Exceptions.GameStart.GameStartException
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;

namespace Innova.Launcher.Core.Exceptions.GameStart
{
  public class GameStartException : Exception
  {
    public string GameKey { get; }

    public string Code { get; }

    public GameStartException(string gameKey, string code)
      : base("Game problem. Code: " + code + " Game: " + gameKey)
    {
      this.Code = code;
      this.GameKey = gameKey;
    }

    public GameStartException(string gameKey, string code, string message)
      : base(message + " Code: " + code + " Game: " + gameKey)
    {
      this.Code = code;
      this.GameKey = gameKey;
    }

    public GameStartException(
      string gameKey,
      string code,
      string message,
      Exception innerException)
      : base(message + " Code: " + code + " Game:" + gameKey, innerException)
    {
      this.Code = code;
      this.GameKey = gameKey;
    }
  }
}
