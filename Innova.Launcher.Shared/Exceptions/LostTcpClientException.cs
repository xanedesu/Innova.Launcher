// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Exceptions.LostTcpClientException
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Runtime.Serialization;

namespace Innova.Launcher.Shared.Exceptions
{
  public class LostTcpClientException : Exception
  {
    public LostTcpClientException()
    {
    }

    public LostTcpClientException(string message)
      : base(message)
    {
    }

    public LostTcpClientException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected LostTcpClientException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
