// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.UpdateModel
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using System;
using System.Threading;

namespace Innova.Launcher.Updater.Core.Services
{
  public class UpdateModel : IDisposable
  {
    public int ServiceId { get; }

    public string Path { get; }

    public string Url { get; }

    public string LogKey { get; }

    public CancellationTokenSource Cancellation { get; }

    public UpdateModel(int serviceId, string path, string url, string logKey)
    {
      this.Path = path;
      this.Url = url;
      this.LogKey = logKey;
      this.ServiceId = serviceId;
      this.Cancellation = new CancellationTokenSource();
    }

    public void Dispose()
    {
      this.Cancellation.Cancel();
      this.Cancellation.Dispose();
    }
  }
}
