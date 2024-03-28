// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.AppUpdateErrorEventArgs
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public class AppUpdateErrorEventArgs : EventArgs
  {
    public Exception Error { get; }

    public AppUpdateErrorEventArgs(Exception error) => this.Error = error ?? throw new ArgumentNullException(nameof (error));
  }
}
