// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Exceptions.GamesEnvironment.LauncherGetgamesEnvironmentsInfoException
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;
using System.Runtime.Serialization;

namespace Innova.Launcher.Core.Exceptions.GamesEnvironment
{
  public class LauncherGetgamesEnvironmentsInfoException : Exception
  {
    public LauncherGetgamesEnvironmentsInfoException()
      : base("Problem to download games environments info")
    {
    }

    public LauncherGetgamesEnvironmentsInfoException(Exception innerException)
      : base("Problem to download games environments info", innerException)
    {
    }

    protected LauncherGetgamesEnvironmentsInfoException(
      SerializationInfo info,
      StreamingContext context)
      : base(info, context)
    {
    }
  }
}
