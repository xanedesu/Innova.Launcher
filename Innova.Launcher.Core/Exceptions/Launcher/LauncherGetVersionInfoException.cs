// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Exceptions.Launcher.LauncherGetVersionInfoException
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;
using System.Runtime.Serialization;

namespace Innova.Launcher.Core.Exceptions.Launcher
{
  public class LauncherGetVersionInfoException : Exception
  {
    public LauncherGetVersionInfoException()
      : base("Problem to download versions")
    {
    }

    public LauncherGetVersionInfoException(Exception innerException)
      : base("Problem to download versions", innerException)
    {
    }

    protected LauncherGetVersionInfoException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
