// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.TaskHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Threading;

namespace Innova.Launcher.Shared.Utils
{
  public static class TaskHelper
  {
    public static void Delay(TimeSpan delay, CancellationToken cancellationToken)
    {
      try
      {
        WaitHandle.WaitAny(new WaitHandle[1]
        {
          cancellationToken.WaitHandle
        }, delay);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
