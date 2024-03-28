// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameUpdateHandlers.BdoGameUpdateHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Shared.Logging.Interfaces;
using NLog;
using System.IO;
using System.Net;

namespace Innova.Launcher.Core.Services.GameUpdateHandlers
{
  public class BdoGameUpdateHandler : IGameUpdateHandler
  {
    private readonly ILogger _logger;

    public string GameKey => "bdo-ru";

    public BdoGameUpdateHandler(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<BdoGameUpdateHandler>();

    public bool ShouldUpdateGame(Game game)
    {
      if (game.Status != GameStatus.Installed)
      {
        this._logger.Trace("[" + this.GameKey + "] Status is not Installed");
        return false;
      }
      if (string.IsNullOrEmpty(game.Path) || !Directory.Exists(game.Path))
      {
        this._logger.Trace("[" + this.GameKey + "] path '" + game.Path + "' is null or not exists");
        return false;
      }
      string str = System.IO.File.ReadAllText(Path.Combine(game.Path, "ads_version"));
      string fileName = Path.Combine(game.Path, "ads", "languagedata_ru.loc");
      WebRequest webRequest = WebRequest.Create(game.Url + "ads/languagedata_ru/" + str + "/languagedata_ru.loc");
      webRequest.Method = "HEAD";
      long result = 0;
      using (WebResponse response = webRequest.GetResponse())
        long.TryParse(response.Headers.Get("Content-Length"), out result);
      long length = new FileInfo(fileName).Length;
      if (result != length)
      {
        this._logger.Trace(string.Format("There is localization file update on game key={0} (current version = {1}) local file size {2} remote file size {3}", (object) this.GameKey, (object) game.Version, (object) length, (object) result));
        return true;
      }
      this._logger.Trace(string.Format("There is no localization file update on game key={0} (current version = {1}) local file size {2} remote file size {3}", (object) this.GameKey, (object) game.Version, (object) length, (object) result));
      return false;
    }
  }
}
