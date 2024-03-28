// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherIpcClient
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.IPC;
using System;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Services
{
  public class LauncherIpcClient : IDisposable
  {
    private const string PingMessage = "ping";
    private const string PongMessage = "pong";
    private const string WakeUpMessage = "wakeup";
    private readonly PipesIpcClient _ipcClient;

    public LauncherIpcClient()
      : this("4game-ipc-pipe")
    {
    }

    public LauncherIpcClient(string pipeName) => this._ipcClient = new PipesIpcClient(pipeName);

    public bool PingExists()
    {
      Task<string> task = Task.Factory.StartNew<string>((Func<string>) (() =>
      {
        this._ipcClient.WriteLine("ping");
        return this._ipcClient.ReadLine();
      }));
      Task.WaitAny(new Task[1]{ (Task) task }, TimeSpan.FromMilliseconds(2000.0));
      return task.IsCompleted ? string.Equals(task.Result, "pong") : throw new TimeoutException("Read of ping timeout..");
    }

    public void WakeUp(string wakeUpData = null)
    {
      string message = string.IsNullOrWhiteSpace(wakeUpData) ? "wakeup" : "wakeup_" + wakeUpData;
      Task task = Task.Factory.StartNew((Action) (() => this._ipcClient.WriteLine(message)));
      Task.WaitAny(new Task[1]{ task }, TimeSpan.FromMilliseconds(2000.0));
      if (!task.IsCompleted)
        throw new TimeoutException("Wake up timeout..");
    }

    public void Dispose() => this._ipcClient?.Dispose();
  }
}
