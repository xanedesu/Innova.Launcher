// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.IPC.PipesIpcServer
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Infrastructure.IPC
{
  public class PipesIpcServer : IIpcServer<PipesIpcConnection>, IDisposable
  {
    private readonly ConcurrentDictionary<PipesIpcConnection, object> _connections = new ConcurrentDictionary<PipesIpcConnection, object>();
    private readonly ILogger _logger;
    private readonly int _maxConnectionsAmount;
    private readonly string _pipeName;
    private readonly object _stopLock = new object();
    private readonly int _startServerRetryCount = 10;
    private int _connectedClients;
    private Task _worker;
    private bool _isStopped;

    public event IpcServerConnectedEventHander<PipesIpcConnection> Connected;

    public PipesIpcServer(ILoggerFactory logerFactory, IPipesIpcServerConfigProvider configProvider)
    {
      this._logger = logerFactory.GetCurrentClassLogger<PipesIpcServer>();
      this._maxConnectionsAmount = configProvider.MaxConnectionsAmount;
      this._pipeName = configProvider.PipeName;
    }

    public void Start() => this._worker = Task.Factory.StartNew((Action) (() =>
    {
      int num = 0;
      while (num < this._startServerRetryCount)
      {
        lock (this._stopLock)
        {
          if (!this._isStopped)
          {
            if (this._connectedClients >= this._maxConnectionsAmount)
            {
              TaskHelper.Delay(TimeSpan.FromMilliseconds(1000.0), CancellationToken.None);
            }
            else
            {
              try
              {
                NamedPipeServerStream pipeServerStream = new NamedPipeServerStream(this._pipeName, PipeDirection.InOut, this._maxConnectionsAmount, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                try
                {
                  Interlocked.Increment(ref this._connectedClients);
                  IAsyncResult asyncResult = pipeServerStream.BeginWaitForConnection((AsyncCallback) null, (object) pipeServerStream);
                  if (!asyncResult.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000.0)))
                  {
                    pipeServerStream.Dispose();
                    Interlocked.Decrement(ref this._connectedClients);
                  }
                  else
                  {
                    pipeServerStream.EndWaitForConnection(asyncResult);
                    this.ProcessConnection(pipeServerStream);
                  }
                }
                catch (ThreadAbortException ex)
                {
                }
                catch (Exception ex)
                {
                  this._logger.Error(ex, string.Format("Error while creating pipe connection count={0} max_count={1}, try={2}", (object) this._connectedClients, (object) this._maxConnectionsAmount, (object) num));
                  Interlocked.Decrement(ref this._connectedClients);
                  pipeServerStream.Dispose();
                  ++num;
                }
              }
              catch (ThreadAbortException ex)
              {
              }
              catch (Exception ex)
              {
                this._logger.Error(ex, "Error while creating pipe");
                throw;
              }
            }
          }
          else
            break;
        }
      }
      if (num >= this._startServerRetryCount)
        throw new Exception(string.Format("Start server fault after {0} tries", (object) num));
    }), TaskCreationOptions.LongRunning);

    public void Stop()
    {
      lock (this._stopLock)
      {
        if (this._isStopped)
          return;
        this._isStopped = true;
        foreach (KeyValuePair<PipesIpcConnection, object> connection in this._connections)
        {
          PipesIpcConnection key = connection.Key;
          try
          {
            key.Close();
          }
          catch
          {
          }
        }
      }
    }

    public void Dispose()
    {
      if (this._isStopped)
        return;
      this.Stop();
    }

    private void ProcessConnection(NamedPipeServerStream stream)
    {
      if (!stream.IsConnected)
        return;
      PipesIpcConnection pipesIpcConnection = new PipesIpcConnection(stream);
      pipesIpcConnection.Closed += new EventHandler(this.ConnectionClosed);
      this._connections.TryAdd(pipesIpcConnection, (object) null);
      try
      {
        this.OnConnected((IIpcServerConnectedEventArgs<PipesIpcConnection>) new IpcServerConnectedEventArgs<PipesIpcConnection>(pipesIpcConnection));
      }
      catch (Exception ex)
      {
        this._logger.Warn(ex, "Connection callback failed");
      }
    }

    private void OnConnected(
      IIpcServerConnectedEventArgs<PipesIpcConnection> args)
    {
      IpcServerConnectedEventHander<PipesIpcConnection> connected = this.Connected;
      if (connected == null)
        return;
      connected((IIpcServer<PipesIpcConnection>) this, args);
    }

    private void ConnectionClosed(object sender, EventArgs e) => this.DisposeConnection(sender as PipesIpcConnection);

    private void DisposeConnection(PipesIpcConnection connection)
    {
      if (!this._connections.TryRemove(connection, out object _))
        return;
      Interlocked.Decrement(ref this._connectedClients);
    }
  }
}
