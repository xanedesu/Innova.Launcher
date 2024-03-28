// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Exceptions.ProcessWaitingTimeoutException
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Services.Exceptions
{
  public class ProcessWaitingTimeoutException : Exception
  {
    public ProcessWaitingTimeoutException(int processId, long milliseconds)
      : base(string.Format("Process {0} has not exited after {1} ms", (object) processId, (object) milliseconds))
    {
    }
  }
}
