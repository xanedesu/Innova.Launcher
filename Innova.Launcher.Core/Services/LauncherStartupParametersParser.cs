// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LauncherStartupParametersParser
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Innova.Launcher.Core.Services
{
  public class LauncherStartupParametersParser : ILauncherStartupParametersParser
  {
    private readonly IStartupParametersParser _startupParametersParser;
    private readonly ILogger _logger;

    public LauncherStartupParametersParser(
      ILoggerFactory loggerFactory,
      IStartupParametersParser startupParametersParser)
    {
      this._startupParametersParser = startupParametersParser;
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherStartupParametersParser>();
    }

    public LauncherStartupParameters ParseParametersFromArgs(string[] args)
    {
      LauncherStartupParameters parametersFromArgs = new LauncherStartupParameters();
      string schemePrefix = "forgame://";
      string str1 = ((IEnumerable<string>) args).FirstOrDefault<string>((Func<string, bool>) (a => a.StartsWith(schemePrefix)));
      if (str1 != null)
      {
        parametersFromArgs.RelativePath = str1.Remove(0, schemePrefix.Length);
      }
      else
      {
        string str2 = schemePrefix;
        string schemeWithoutSlashes = str2.Substring(0, str2.Length - 2 - 0);
        if (((IEnumerable<string>) args).Any<string>((Func<string, bool>) (p => p.StartsWith(schemeWithoutSlashes))))
          args = ((IEnumerable<string>) args).First<string>((Func<string, bool>) (p => p.StartsWith(schemeWithoutSlashes))).Replace(schemeWithoutSlashes, "").Split('$');
        parametersFromArgs.GamesConfigUrl = this._startupParametersParser.GetParameterValue(args, "/c");
        parametersFromArgs.SingleGamesConfigUrl = this._startupParametersParser.GetParameterValue(args, "/s");
        parametersFromArgs.SingleGamesValidationUrl = this._startupParametersParser.GetParameterValue(args, "/v");
        parametersFromArgs.StartPage = HttpUtility.UrlDecode(this._startupParametersParser.GetParameterValue(args, "/f"));
        parametersFromArgs.Environment = this._startupParametersParser.GetParameterValue(args, "/e");
        parametersFromArgs.Region = this._startupParametersParser.GetParameterValue(args, "/r");
        parametersFromArgs.TrackingId = this._startupParametersParser.GetParameterValue(args, "/t");
        parametersFromArgs.Origin = this._startupParametersParser.GetParameterValue(args, "/o");
        parametersFromArgs.Culture = this._startupParametersParser.GetParameterValue(args, "/l");
        string parameterValue = this._startupParametersParser.GetParameterValue(args, "/g");
        if ((string.Equals(parameterValue, "none", StringComparison.Ordinal) ? 1 : (string.IsNullOrWhiteSpace(parameterValue) ? 1 : 0)) == 0)
          parametersFromArgs.GameKey = parameterValue;
        parametersFromArgs.FolderToInstallGame = this._startupParametersParser.GetParameterValue(args, "/i");
      }
      return parametersFromArgs;
    }

    public string SerializeParameters(LauncherStartupParameters parameters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrWhiteSpace(parameters.GamesConfigUrl))
        stringBuilder.Append(" /c " + parameters.GamesConfigUrl);
      if (!string.IsNullOrWhiteSpace(parameters.SingleGamesConfigUrl))
        stringBuilder.Append(" /s " + parameters.SingleGamesConfigUrl);
      if (!string.IsNullOrWhiteSpace(parameters.SingleGamesValidationUrl))
        stringBuilder.Append(" /v " + parameters.SingleGamesValidationUrl);
      if (!string.IsNullOrWhiteSpace(parameters.StartPage))
        stringBuilder.Append(" /f " + parameters.StartPage);
      if (!string.IsNullOrWhiteSpace(parameters.Environment))
        stringBuilder.Append(" /e " + parameters.Environment);
      if (!string.IsNullOrWhiteSpace(parameters.GameKey))
        stringBuilder.Append(" /g " + parameters.GameKey);
      if (!string.IsNullOrWhiteSpace(parameters.FolderToInstallGame))
        stringBuilder.Append(" /i " + parameters.FolderToInstallGame);
      if (!string.IsNullOrWhiteSpace(parameters.TrackingId))
        stringBuilder.Append(" /t " + parameters.TrackingId);
      if (!string.IsNullOrWhiteSpace(parameters.Origin))
        stringBuilder.Append(" /o " + parameters.Origin);
      if (!string.IsNullOrWhiteSpace(parameters.Culture))
        stringBuilder.Append(" /l " + parameters.Culture);
      return stringBuilder.ToString();
    }
  }
}
