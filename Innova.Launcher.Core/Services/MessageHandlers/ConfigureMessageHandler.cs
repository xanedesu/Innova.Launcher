// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.ConfigureMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using Newtonsoft.Json;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Innova.Launcher.Core.Services.MessageHandlers
{
  [WebMessageHandlerFilter(new string[] {"configure"})]
  public class ConfigureMessageHandler : IWebMessageHandler
  {
    private readonly IRegionalGamesService _regionalGamesService;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly ILogger _logger;

    public ConfigureMessageHandler(
      ILifetimeManager lifetimeManager,
      ILoggerFactory loggerFactory,
      IRegionalGamesService regionalGamesService)
    {
      this._lifetimeManager = lifetimeManager;
      this._regionalGamesService = regionalGamesService;
      this._logger = loggerFactory.GetCurrentClassLogger<ConfigureMessageHandler>();
    }

    public void Handle(WebMessage webMessage)
    {
      FileInfo configExecutable = this.GetConfigExecutable(webMessage.Data.ToObject<ConfigureMessageHandler.ConfigureRequestData>().GameKey);
      if (configExecutable?.Directory == null)
        return;
      Process process = Process.Start(new ProcessStartInfo()
      {
        WorkingDirectory = configExecutable.DirectoryName,
        FileName = Path.Combine(configExecutable.DirectoryName, configExecutable.Name),
        UseShellExecute = true
      });
      if (process == null)
        return;
      process.WaitForInputIdle();
      IntPtr mainWindowHandle = process.MainWindowHandle;
      Thread.Sleep(200);
      WinApiUtils.SetForegroundWindow(mainWindowHandle);
      this._lifetimeManager.TimeToDie += new EventHandler(StopProcessWhenMainWindowClosing);
      process.EnableRaisingEvents = true;
      process.Exited += (EventHandler) ((_, __) => this._lifetimeManager.TimeToDie -= new EventHandler(StopProcessWhenMainWindowClosing));

      void StopProcessWhenMainWindowClosing(object sender, EventArgs e) => process.Kill();
    }

    private FileInfo GetConfigExecutable(string gameKey)
    {
      Game gameOrDefault = this._regionalGamesService.GetGameOrDefault(gameKey);
      if (!string.IsNullOrEmpty(gameOrDefault.OptionsExe))
      {
        string str = Path.Combine(gameOrDefault.Path, gameOrDefault.OptionsExe);
        if (File.Exists(str))
          return new FileInfo(str);
      }
      this._logger.Error("Configure executable not found for game " + gameKey);
      return (FileInfo) null;
    }

    private struct ConfigureRequestData
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }
    }
  }
}
