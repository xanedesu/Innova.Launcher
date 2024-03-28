// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Exceptions.ForgameUpdaterStartException
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;
using System.Runtime.Serialization;

namespace Innova.Launcher.Updater.Core.Exceptions
{
  public class ForgameUpdaterStartException : Exception
  {
    public ForgameUpdaterStartException()
    {
    }

    public ForgameUpdaterStartException(string message)
      : base(message)
    {
    }

    public ForgameUpdaterStartException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected ForgameUpdaterStartException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
