// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.Internet.InternetConnectionChecker
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Internet.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using Polly;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Infrastructure.Internet
{
  public class InternetConnectionChecker : IInternetConnectionChecker
  {
    private readonly ILogger _logger;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly IInternetConnectionCheckerConfigProvider _serverConfigProvider;
    private readonly Policy _retryPolicy;
    private WebException _lastWebException;

    public InternetConnectionChecker(
      ILoggerFactory loggerFactory,
      ILifetimeManager lifetimeManager,
      IInternetConnectionCheckerConfigProvider serverConfigProvider)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<InternetConnectionChecker>();
      this._lifetimeManager = lifetimeManager;
      this._serverConfigProvider = serverConfigProvider;
      this._retryPolicy = (Policy) RetrySyntax.WaitAndRetry(Policy.Handle<WebException>(), 3, (Func<int, TimeSpan>) (v => TimeSpan.FromMilliseconds((double) (100 * v))));
    }

    public Task<bool> CheckAsync() => Task.Factory.StartNew<bool>(new Func<bool>(this.Check));

    public bool Check()
    {
      try
      {
        return this._retryPolicy.Execute<bool>((Func<bool>) (() =>
        {
          using (TimeoutWebClient timeoutWebClient = new TimeoutWebClient(30000))
          {
            string a = timeoutWebClient.DownloadString(this._serverConfigProvider.CheckAddress);
            int num = string.Equals(a, this._serverConfigProvider.SuccessResponseText, StringComparison.OrdinalIgnoreCase) ? 1 : 0;
            if (num == 0)
              this._logger.Error("Connection check failed: " + this._serverConfigProvider.CheckAddress + " / " + a);
            return num != 0;
          }
        }));
      }
      catch (WebException ex)
      {
        if (this._lastWebException != null)
        {
          if (!(this._lastWebException.Message != ex.Message))
            goto label_5;
        }
        this._lastWebException = ex;
        this._logger.Error((Exception) ex, "Problem to connect to " + this._serverConfigProvider.CheckAddress);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Unknown problem to check server " + this._serverConfigProvider.CheckAddress);
      }
label_5:
      return false;
    }

    public Task WaitConnectionAsync() => Task.Factory.StartNew((Action) (() =>
    {
      while (this._lifetimeManager.IsAlive && !this.Check())
        TaskHelper.Delay(TimeSpan.FromMilliseconds((double) this._serverConfigProvider.CheckInterval), this._lifetimeManager.CancellationToken);
    }), this._lifetimeManager.CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    public Task DoWhenConnectionExistsAsync(Action action) => Task.Factory.StartNew((Action) (() =>
    {
      if (this.Check())
        action();
      else
        this.WaitConnectionAsync().ContinueWith((Action<Task>) (t => action())).Wait(this._lifetimeManager.CancellationToken);
    }), this._lifetimeManager.CancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
  }
}
