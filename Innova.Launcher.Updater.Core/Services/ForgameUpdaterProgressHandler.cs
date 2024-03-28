// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.ForgameUpdaterProgressHandler
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Shared.Exceptions;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Updater.Core.Helpers;
using Innova.Launcher.Updater.Core.Models;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Innova.Launcher.Updater.Core.Services
{
  public class ForgameUpdaterProgressHandler : IForgameUpdaterProgressHandler
  {
    private readonly IIpcServer<IIpcConnection> _ipcServer;
    private readonly ConcurrentDictionary<int, ForgameUpdaterProgressHandler.ServiceConnection> _connections = new ConcurrentDictionary<int, ForgameUpdaterProgressHandler.ServiceConnection>();
    private bool _isServerStarted;
    private readonly object _serverStartLock = new object();
    private readonly ILogger _logger;

    public event EventHandler<UpdateCompletedEventArgs> ProgressCompleted;

    public event EventHandler<UpdateCompletedEventArgs> ProgressCancelled;

    public event EventHandler<UpdateProgressInfoEventArgs> ProgressUpdated;

    public ForgameUpdaterProgressHandler(
      IIpcServer<IIpcConnection> ipcServer,
      ILoggerFactory loggerFactory)
    {
      this._ipcServer = ipcServer;
      this._logger = loggerFactory.GetCurrentClassLogger<ForgameUpdaterProgressHandler>();
      this.SubscribeToIpcServer();
    }

    public void StartListenProgress()
    {
      if (this._isServerStarted)
        return;
      lock (this._serverStartLock)
      {
        if (this._isServerStarted)
          return;
        this._ipcServer.Start();
        this._isServerStarted = true;
      }
    }

    public bool PauseProgressIfExists(int serviceId)
    {
      ForgameUpdaterProgressHandler.ServiceConnection serviceConnection;
      if (!this._connections.TryGetValue(serviceId, out serviceConnection))
        return false;
      string message = JsonConvert.SerializeObject((object) new
      {
        name = "pause"
      });
      try
      {
        serviceConnection.Connection.Send(message);
      }
      catch (LostTcpClientException ex)
      {
        this._logger.Error("Connection with updater lost.");
        this._connections.TryRemove(serviceId, out ForgameUpdaterProgressHandler.ServiceConnection _);
        return false;
      }
      return true;
    }

    public bool ResumeProgressIfExists(int serviceId)
    {
      ForgameUpdaterProgressHandler.ServiceConnection serviceConnection;
      if (!this._connections.TryGetValue(serviceId, out serviceConnection))
        return false;
      string message = JsonConvert.SerializeObject((object) new
      {
        name = "resume"
      });
      try
      {
        serviceConnection.Connection.Send(message);
      }
      catch (LostTcpClientException ex)
      {
        this._logger.Error("Connection with updater lost.");
        this._connections.TryRemove(serviceId, out ForgameUpdaterProgressHandler.ServiceConnection _);
        return false;
      }
      return true;
    }

    public bool FinishProgressIfExists(int serviceId)
    {
      ForgameUpdaterProgressHandler.ServiceConnection serviceConnection;
      if (!this._connections.TryGetValue(serviceId, out serviceConnection))
        return false;
      serviceConnection.IsCancelled = true;
      string message = JsonConvert.SerializeObject((object) new
      {
        name = "stop"
      });
      try
      {
        serviceConnection.Connection.Send(message);
      }
      catch (LostTcpClientException ex)
      {
        this._logger.Error("Connection with updater lost.");
        this._connections.TryRemove(serviceId, out ForgameUpdaterProgressHandler.ServiceConnection _);
        this.OnProgressCancelled(serviceId);
        return false;
      }
      return true;
    }

    private void SubscribeToIpcServer() => IpcConnectionHelper.SubscribeToJsonIpcServer(this._ipcServer, (Action<IIpcConnection, IObservable<JObject>>) ((connection, messageSource) =>
    {
      ForgameUpdaterProgressHandler.ServiceConnection currentConnection = (ForgameUpdaterProgressHandler.ServiceConnection) null;
      int? registrationId = new int?();
      messageSource.Select<JObject, JObject>((Func<JObject, JObject>) (v =>
      {
        JToken jtoken = v["registration_id"];
        int? registrationIdField = jtoken != null ? new int?(Extensions.Value<int>((IEnumerable<JToken>) jtoken)) : new int?();
        if (registrationIdField.HasValue)
        {
          registrationId = new int?(registrationIdField.Value);
          this._logger.Trace(string.Format("IPC client send registration_id {0}", (object) registrationIdField));
          currentConnection = this._connections.AddOrUpdate(registrationIdField.Value, (Func<int, ForgameUpdaterProgressHandler.ServiceConnection>) (__ => new ForgameUpdaterProgressHandler.ServiceConnection()
          {
            Connection = connection,
            ServiceId = registrationIdField.Value
          }), (Func<int, ForgameUpdaterProgressHandler.ServiceConnection, ForgameUpdaterProgressHandler.ServiceConnection>) ((__, existConnection) => new ForgameUpdaterProgressHandler.ServiceConnection()
          {
            Connection = connection,
            ServiceId = registrationIdField.Value
          }));
        }
        return v;
      })).TakeWhile<JObject>((Func<JObject, bool>) (v =>
      {
        JToken jtoken = v["name"];
        return (jtoken != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken) : (string) null) != "exit";
      })).Finally<JObject>((Action) (() =>
      {
        if (!registrationId.HasValue || !this._connections.TryRemove(registrationId.Value, out currentConnection))
          return;
        if (currentConnection.IsCancelled)
          this.OnProgressCancelled(currentConnection.ServiceId);
        else
          this.OnProgressCompleted(currentConnection.ServiceId, currentConnection.Error);
      })).Subscribe<JObject>((Action<JObject>) (v => this.HandleIpcMessage(currentConnection, v)));
    }));

    private void HandleIpcMessage(
      ForgameUpdaterProgressHandler.ServiceConnection serviceConnection,
      JObject jObjectMessage)
    {
      if (serviceConnection == null)
      {
        this._logger.Trace(string.Format("IPC client send message for unknown service , message: {0}", (object) jObjectMessage));
      }
      else
      {
        this._logger.Trace(string.Format("IPC client send message for service {0}, message: {1}", (object) serviceConnection.ServiceId, (object) jObjectMessage));
        JToken jtoken1 = jObjectMessage["name"];
        string str1 = jtoken1 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken1) : (string) null;
        if (str1 == "progress")
        {
          ForgameUpdaterProgressHandler.ProgressMessage progressMessage = jObjectMessage.ToObject<ForgameUpdaterProgressHandler.ProgressMessage>();
          UpdateProgressInfo progressInfo = new UpdateProgressInfo()
          {
            Progress = progressMessage.Percent,
            Speed = progressMessage.Speed,
            FileName = progressMessage.Item,
            Downloaded = progressMessage.TotalDownloaded,
            Size = progressMessage.TotalSize
          };
          this.OnProgressUpdated(serviceConnection.ServiceId, progressInfo);
        }
        if (!(str1 == "error"))
          return;
        JToken jtoken2 = jObjectMessage["code"];
        int code = jtoken2 != null ? Extensions.Value<int>((IEnumerable<JToken>) jtoken2) : 0;
        JToken jtoken3 = jObjectMessage["desc"];
        string error = jtoken3 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken3) : (string) null;
        JToken jtoken4 = jObjectMessage["type"];
        string str2 = jtoken4 != null ? Extensions.Value<string>((IEnumerable<JToken>) jtoken4) : (string) null;
        this._logger.Error(string.Format("Handled ipc progress error.. {0} {1} {2}", (object) code, (object) str2, (object) error));
        serviceConnection.Error = new UpdateProgressError(code, error);
      }
    }

    private void OnProgressCompleted(int serviceId, UpdateProgressError currentConnectionError)
    {
      EventHandler<UpdateCompletedEventArgs> progressCompleted = this.ProgressCompleted;
      if (progressCompleted == null)
        return;
      progressCompleted((object) this, new UpdateCompletedEventArgs(serviceId, currentConnectionError));
    }

    private void OnProgressCancelled(int serviceId)
    {
      EventHandler<UpdateCompletedEventArgs> progressCancelled = this.ProgressCancelled;
      if (progressCancelled == null)
        return;
      progressCancelled((object) this, new UpdateCompletedEventArgs(serviceId, true));
    }

    private void OnProgressUpdated(int serviceId, UpdateProgressInfo progressInfo)
    {
      EventHandler<UpdateProgressInfoEventArgs> progressUpdated = this.ProgressUpdated;
      if (progressUpdated == null)
        return;
      progressUpdated((object) this, new UpdateProgressInfoEventArgs(serviceId, progressInfo));
    }

    private class ServiceConnection
    {
      public IIpcConnection Connection { get; set; }

      public int ServiceId { get; set; }

      public UpdateProgressError Error { get; set; }

      public bool IsCancelled { get; set; }
    }

    private class ProgressMessage
    {
      [JsonProperty("name")]
      public string Name { get; set; }

      [JsonProperty("percent")]
      public Decimal Percent { get; set; }

      [JsonProperty("eta")]
      public int EstimatedTimeOfArrival { get; set; }

      [JsonProperty("stage")]
      public int CurrentStage { get; set; }

      [JsonProperty("speed")]
      public int Speed { get; set; }

      [JsonProperty("type")]
      public string Type { get; set; }

      [JsonProperty("item")]
      public string Item { get; set; }

      [JsonProperty("stages")]
      public ForgameUpdaterProgressHandler.ProgressMessage.Stage[] Stages { get; set; }

      [JsonProperty("proc_vol")]
      public long TotalDownloaded { get; set; }

      [JsonProperty("totl_vol")]
      public long TotalSize { get; set; }

      public class Stage
      {
        [JsonProperty("stageSize")]
        public long Size { get; set; }

        [JsonProperty("updateDownloaded")]
        public long UpdateDownloaded { get; set; }

        [JsonProperty("updateSize")]
        public long UpdateSize { get; set; }
      }
    }
  }
}
