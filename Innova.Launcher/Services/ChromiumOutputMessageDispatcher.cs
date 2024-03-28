// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.ChromiumOutputMessageDispatcher
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Newtonsoft.Json;
using NLog;
using Polly;
using Polly.Retry;
using System;

namespace Innova.Launcher.Services
{
  public class ChromiumOutputMessageDispatcher : IOutputMessageDispatcher
  {
    private readonly IScriptExecutorProvider _scriptExecutorProvider;
    private readonly RetryPolicy _retryPolicy;
    private readonly ILogger _logger;

    public string WindowId { get; set; }

    public ChromiumOutputMessageDispatcher(
      IScriptExecutorProvider scriptExecutorProvider,
      ILoggerFactory loggerFactory)
    {
      this._scriptExecutorProvider = scriptExecutorProvider;
      this._logger = loggerFactory.GetCurrentClassLogger<ChromiumOutputMessageDispatcher>();
      this._retryPolicy = RetrySyntax.WaitAndRetry(Policy.Handle<InvalidOperationException>(), 5, (Func<int, TimeSpan>) (_ => TimeSpan.FromMilliseconds(300.0)));
    }

    public void SetWindowId(string id) => this._scriptExecutorProvider.Get(this.WindowId)?.ExecuteJavaScript("window.id = '" + id + "'");

    public void Dispatch(LauncherMessage message)
    {
      string script = "window.dispatchEvent(new CustomEvent('launcherMessage', " + JsonConvert.SerializeObject((object) new
      {
        bubbles = true,
        cancelable = true,
        detail = message
      }) + "));";
      try
      {
        this._retryPolicy.Execute((Action) (() => this._scriptExecutorProvider.Get(this.WindowId)?.ExecuteJavaScript(script, message.Async)));
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Error while sending message to chromium");
      }
    }
  }
}
