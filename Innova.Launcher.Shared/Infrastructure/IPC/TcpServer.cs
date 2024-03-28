// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.IPC.TcpServer
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Infrastructure.IPC
{
  public class TcpServer : IIpcServer<TcpConnection>, IDisposable
  {
    private readonly ILogger _logger;
    private readonly ILifetimeManager _lifetimeManager;
    private volatile TcpListener _tcpListener;
    private readonly List<TcpConnection> _connections = new List<TcpConnection>();
    private readonly object _sync = new object();
    private Task _worker;

    public event IpcServerConnectedEventHander<TcpConnection> Connected;

    public int PortNumber { get; }

    public TcpServer(
      ILoggerFactory loggerFactory,
      ITcpServerConfigProvider configProvider,
      ILifetimeManager lifetimeManager)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<TcpServer>();
      this._lifetimeManager = lifetimeManager;
      this.PortNumber = configProvider.TcpPort;
    }

    public void Start()
    {
      if (this._tcpListener != null)
        return;
      lock (this._sync)
      {
        if (this._tcpListener != null)
          return;
        this._tcpListener = new TcpListener(IPAddress.Loopback, this.PortNumber);
        this._tcpListener.Start();
        this._worker = Task.Factory.StartNew(new Action(this.StartServerInNewThread), TaskCreationOptions.LongRunning);
      }
    }

    public void Stop()
    {
      try
      {
        this._tcpListener?.Stop();
        foreach (TcpConnection connection in this._connections)
          connection.Close();
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Fail to dispose connections on lifetime end");
        throw;
      }
    }

    private void StartServerInNewThread()
    {
      try
      {
        while (this._lifetimeManager.IsAlive)
        {
          IAsyncResult asyncResult = this._tcpListener.BeginAcceptTcpClient(new AsyncCallback(this.AcceptCallback), (object) null);
label_2:
          if (this._lifetimeManager.IsAlive && !asyncResult.AsyncWaitHandle.WaitOne(500))
            goto label_2;
        }
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Fail while waiting new client");
      }
      finally
      {
        this.Stop();
      }
    }

    private void AcceptCallback(IAsyncResult ar)
    {
      try
      {
        if (!this._lifetimeManager.IsAlive)
          return;
        TcpConnection tcpConnection = new TcpConnection(this._tcpListener.EndAcceptTcpClient(ar));
        this._connections.Add(tcpConnection);
        this.OnConnected(tcpConnection);
      }
      catch
      {
      }
    }

    private void OnConnected(TcpConnection obj)
    {
      IpcServerConnectedEventHander<TcpConnection> connected = this.Connected;
      if (connected == null)
        return;
      connected((IIpcServer<TcpConnection>) this, (IIpcServerConnectedEventArgs<TcpConnection>) new IpcServerConnectedEventArgs<TcpConnection>(obj));
    }

    public void Dispose()
    {
    }
  }
}
