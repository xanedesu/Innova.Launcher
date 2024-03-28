// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.Interfaces.UpdateCompletedEventArgs
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;

namespace Innova.Launcher.Updater.Core.Services.Interfaces
{
  public class UpdateCompletedEventArgs : EventArgs
  {
    public int ServiceId { get; }

    public UpdateProgressError Error { get; }

    public bool IsCancelled { get; set; }

    public UpdateCompletedEventArgs(int serviceId, UpdateProgressError error)
    {
      this.ServiceId = serviceId;
      this.Error = error;
    }

    public UpdateCompletedEventArgs(int serviceId, bool isCancelled)
    {
      this.ServiceId = serviceId;
      this.IsCancelled = isCancelled;
    }
  }
}
