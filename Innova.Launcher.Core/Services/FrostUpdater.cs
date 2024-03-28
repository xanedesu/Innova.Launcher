// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.FrostUpdater
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Infrastructure.Internet;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Updater.Core.Exceptions;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class FrostUpdater : IFrostUpdater
  {
    private readonly IBinaryUpdater _binaryUpdater;
    private readonly ILogger _logger;
    private readonly string _frostUpdaterUrl;

    public event EventHandler FrostUpdateCompleted;

    public FrostUpdater(
      ILauncherConfigurationProvider configurationProvider,
      ILoggerFactory loggerFactory,
      IBinaryUpdater binaryUpdater)
    {
      this._binaryUpdater = binaryUpdater;
      this._logger = loggerFactory.GetCurrentClassLogger<FrostUpdater>();
      this._frostUpdaterUrl = configurationProvider.FrostUpdaterUrl;
    }

    public async Task UpdateAsync(string frostExePath, string gameFrostKey)
    {
      FrostUpdater frostUpdater = this;
      int num1;
      if (num1 != 0 && string.IsNullOrWhiteSpace(frostExePath))
        throw new ForgameUpdaterStartException("Can't start update frost on empty path " + frostExePath);
      try
      {
        string str1;
        using (HeaderedWebClient headeredWebClient = new HeaderedWebClient())
        {
          string str2 = headeredWebClient.DownloadString(frostUpdater._frostUpdaterUrl + "version.txt");
          int startIndex1 = str2.IndexOf(gameFrostKey);
          int num2 = str2.IndexOf("\n", startIndex1);
          if (num2 == -1)
            num2 = str2.Length;
          int startIndex2 = startIndex1 + gameFrostKey.Length;
          str1 = str2.Substring(startIndex2, num2 - startIndex2).Trim();
        }
        string directoryName = Path.GetDirectoryName(frostExePath);
        string url = frostUpdater._frostUpdaterUrl + str1 + "/";
        frostUpdater._logger.Trace("Update frost (" + directoryName + "," + url + ")");
        try
        {
          await frostUpdater._binaryUpdater.UpdateWithoutProgressAsync(directoryName, url, new Action(frostUpdater.OnFrostUpdateComplete));
        }
        catch (Exception ex)
        {
          frostUpdater._logger.Error(ex, "Error while start frost update");
          throw;
        }
      }
      catch (Exception ex)
      {
        throw new ForgameUpdaterStartException("Can't start update frost for game " + gameFrostKey, ex);
      }
    }

    private void OnFrostUpdateComplete()
    {
      EventHandler frostUpdateCompleted = this.FrostUpdateCompleted;
      if (frostUpdateCompleted == null)
        return;
      frostUpdateCompleted((object) this, EventArgs.Empty);
    }
  }
}
