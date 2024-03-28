// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.GameActualVersionProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Internet;
using Innova.Launcher.Shared.Services.Exceptions;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Services
{
  public class GameActualVersionProvider : IGameActualVersionProvider
  {
    private readonly ILifetimeManager _lifetimeManager;
    private readonly int _retryCount = 5;

    public GameActualVersionProvider(ILifetimeManager lifetimeManager) => this._lifetimeManager = lifetimeManager ?? throw new ArgumentNullException(nameof (lifetimeManager));

    public async Task<string> GetActualVersionFromUrl(string url)
    {
      TimeoutWebClient timeoutWebClient = new TimeoutWebClient(15000);
      timeoutWebClient.Encoding = Encoding.UTF8;
      string actualVersionFromUrl;
      using (TimeoutWebClient client = timeoutWebClient)
      {
        int tries = 0;
        WebException innerException;
        while (true)
        {
          int num;
          do
          {
            if (tries < this._retryCount)
            {
              ++tries;
              try
              {
                actualVersionFromUrl = this.ProcessVersion(await client.DownloadStringTaskAsync(new Uri(url)));
                goto label_15;
              }
              catch (WebException ex)
              {
                num = 1;
              }
            }
            else
              goto label_11;
          }
          while (num != 1);
          innerException = ex;
          if (tries != this._retryCount)
            await Task.Delay(300, this._lifetimeManager.CancellationToken);
          else
            break;
        }
        throw new GamesConfigDataLoadingException((Exception) innerException);
label_11:
        throw new GamesConfigDataLoadingException();
      }
label_15:
      return actualVersionFromUrl;
    }

    private string ProcessVersion(string downloadedVersion) => downloadedVersion == null ? (string) null : downloadedVersion.ReplaceNewLineWith(".");
  }
}
