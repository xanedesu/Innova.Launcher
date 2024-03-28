// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherIpcService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Helpers;
using Innova.Launcher.Shared.Infrastructure.IPC;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Core.Services
{
  public class LauncherIpcService : ILauncherIpcService, IDisposable
  {
    private const string WakeUpMessage = "wakeup";
    private const string WakeUpWithDataMessagePrefix = "wakeup_";
    private const string PingMessage = "ping";
    private const string PongMessage = "pong";
    private readonly IIpcServer<PipesIpcConnection> _ipcServer;
    private readonly IWindowsService _windowsService;
    private readonly ILauncherStartupParametersParser _launcherStartupParametersParser;
    private readonly IGamesConfigProvider _gamesConfigProvider;
    private readonly ILauncherStateService _launcherStateService;
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;
    private readonly IGameManager _gameManager;
    private readonly ILogger _logger;

    public LauncherIpcService(
      ILoggerFactory loggerFactory,
      IIpcServer<PipesIpcConnection> ipcServer,
      ILifetimeManager lifetimeManager,
      IWindowsService windowsService,
      ILauncherStartupParametersParser launcherStartupParametersParser,
      IGamesConfigProvider gamesConfigProvider,
      ILauncherStateService launcherStateService,
      IGamesEnvironmentProvider gamesEnvironmentProvider,
      IGameManager gameManager)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherIpcService>();
      this._ipcServer = ipcServer;
      this._windowsService = windowsService;
      this._launcherStartupParametersParser = launcherStartupParametersParser;
      this._gamesConfigProvider = gamesConfigProvider;
      this._launcherStateService = launcherStateService;
      this._gamesEnvironmentProvider = gamesEnvironmentProvider;
      this._gameManager = gameManager;
      this._ipcServer.Connected += new IpcServerConnectedEventHander<PipesIpcConnection>(this.NewClientConnected);
      lifetimeManager.TimeToDie += new EventHandler(this.TimeToDie);
    }

    public void StartListen() => this._ipcServer.Start();

    public void Dispose() => this._ipcServer?.Dispose();

    private void NewClientConnected(
      IIpcServer<PipesIpcConnection> server,
      IIpcServerConnectedEventArgs<PipesIpcConnection> args)
    {
      PipesIpcConnection connection = args.Connection;
      connection.MessageReceived += new EventHandler<IpcMessageEventArgs>(this.MessageReceived);
      connection.StartReceive();
    }

    private void MessageReceived(object sender, IpcMessageEventArgs e) => this.ProcessMessage(e.Message, sender as PipesIpcConnection);

    private void ProcessMessage(string message, PipesIpcConnection connection)
    {
      if (string.Equals(message, "wakeup") || message != null && message.StartsWith("wakeup_"))
      {
        this.ProcessWakeMessage(message);
      }
      else
      {
        if (!string.Equals(message, "ping"))
          return;
        this.ProcessPingMessage(connection);
      }
    }

    private void ProcessWakeMessage(string message)
    {
      this.TryParseWakeUpParameters(message);
      this._windowsService.RiseAllWindows();
    }

    private void TryParseWakeUpParameters(string message)
    {
      if (message == null || !message.StartsWith("wakeup_"))
        return;
      string str = message.Replace("wakeup_", "");
      try
      {
        LauncherStartupParameters parametersFromArgs = this._launcherStartupParametersParser.ParseParametersFromArgs(((IEnumerable<string>) str.Split(' ')).Select<string, string>((Func<string, string>) (s => s.Trim(' ', '"'))).ToArray<string>());
        if (!string.IsNullOrWhiteSpace(parametersFromArgs.GamesConfigUrl))
          this._gamesEnvironmentProvider.UpdateGamesConfigUrl(parametersFromArgs.GamesConfigUrl);
        if (!string.IsNullOrWhiteSpace(parametersFromArgs.SingleGamesConfigUrl))
          this._gamesEnvironmentProvider.UpdateSingleGamesConfigUrl(parametersFromArgs.SingleGamesConfigUrl);
        if (!string.IsNullOrWhiteSpace(parametersFromArgs.SingleGamesValidationUrl))
          this._gamesEnvironmentProvider.UpdateSingleGamesConfigUrl(parametersFromArgs.SingleGamesValidationUrl);
        if (!string.IsNullOrWhiteSpace(parametersFromArgs.GameKey))
        {
          if (this._gamesConfigProvider.GetGame(parametersFromArgs.GameKey) == null)
          {
            this._logger.Error("Bad gameKey parameter value " + parametersFromArgs.GameKey);
          }
          else
          {
            this._launcherStateService.UpdateRegionByGame(parametersFromArgs.GameKey);
            this._windowsService.OpenUrlInMainWindow(this._gameManager.GetGameWindowUrl(parametersFromArgs.GameKey, false));
          }
        }
        if (!string.IsNullOrWhiteSpace(parametersFromArgs.RelativePath))
          this._windowsService.OpenUrlInMainWindow(UrlHelper.ConcatPathWithParams(this._launcherStateService.CurrentStartPage, parametersFromArgs.RelativePath));
        if (string.IsNullOrWhiteSpace(parametersFromArgs.StartPage))
          return;
        this._windowsService.OpenUrlInMainWindow(parametersFromArgs.StartPage);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Problem with parsing wakeup parameters");
      }
    }

    private void ProcessPingMessage(PipesIpcConnection connection) => connection.Send("pong");

    private void TimeToDie(object sender, EventArgs e) => this._ipcServer?.Dispose();
  }
}
