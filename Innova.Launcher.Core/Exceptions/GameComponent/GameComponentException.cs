// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Exceptions.GameComponent.GameComponentException
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Shared.Models.GameConfig;
using System;

namespace Innova.Launcher.Core.Exceptions.GameComponent
{
  public class GameComponentException : Exception
  {
    public GameComponentConfig ComponentConfig { get; }

    protected GameComponentException(string message, GameComponentConfig componentConfig)
      : base(message)
    {
      this.ComponentConfig = componentConfig ?? throw new ArgumentNullException(nameof (componentConfig));
    }
  }
}
