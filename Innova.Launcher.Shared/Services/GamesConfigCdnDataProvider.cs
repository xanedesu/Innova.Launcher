// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.GamesConfigCdnDataProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Internet;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Innova.Launcher.Shared.Services
{
  public class GamesConfigCdnDataProvider : IGamesConfigCdnDataProvider
  {
    private readonly ICdnGamesConfigAddressProvider _configAddressProvider;
    private readonly ILogger _logger;
    private readonly RetryPolicy _retryPolicy;
    private readonly ILifetimeManager _lifetimeManager;

    public GamesConfigCdnDataProvider(
      ICdnGamesConfigAddressProvider configAddressProvider,
      ILifetimeManager lifetimeManager,
      ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GamesConfigCdnDataProvider>();
      this._configAddressProvider = configAddressProvider ?? throw new ArgumentNullException(nameof (configAddressProvider));
      this._lifetimeManager = lifetimeManager ?? throw new ArgumentNullException(nameof (lifetimeManager));
      this._retryPolicy = RetrySyntax.WaitAndRetry(Policy.Handle<WebException>(), 5, (Func<int, TimeSpan>) (i => TimeSpan.FromMilliseconds(300.0)));
    }

    public string[] GetConfigs()
    {
      try
      {
        return this._retryPolicy.Execute<string[]>((Func<CancellationToken, string[]>) (cancellationToken => ((IEnumerable<string>) new string[2]
        {
          this._configAddressProvider.Address,
          this._configAddressProvider.SinglesAddress
        }).AsParallel<string>().Select<string, string>((Func<string, string>) (uri =>
        {
          using (TimeoutWebClient timeoutWebClient = new TimeoutWebClient(15000)
          {
            Encoding = Encoding.UTF8
          })
            return timeoutWebClient.DownloadString(new Uri(uri));
        })).ToArray<string>()), this._lifetimeManager.CancellationToken);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "GamesConfigCdnCdnDataProvider exception");
        throw new GamesConfigDataLoadingException(ex);
      }
    }
  }
}
