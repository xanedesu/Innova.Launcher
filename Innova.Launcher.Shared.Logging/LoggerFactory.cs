// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Logging.LoggerFactory
// Assembly: Innova.Launcher.Shared.Logging, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 00910835-5246-4CB6-87E9-1F840D471FFA
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Logging.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Innova.Launcher.Shared.Logging
{
  public class LoggerFactory : ILoggerFactory
  {
    private readonly ILoggerConfigurationProvider _configurationProvider;
    private readonly Regex messageHeader = new Regex("(\\d\\d\\.\\d\\d\\.\\d\\d\\d\\d \\d\\d:\\d\\d:\\d\\d.*) \\[(\\w+)\\] (.*)$");

    public LoggerFactory(ILoggerConfigurationProvider configurationProvider)
    {
      this._configurationProvider = configurationProvider;
      LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
      string str = "${date:format=dd.MM.yyyy HH\\:mm\\:ss zz} [${level}] ${logger} ${callsite:methodName=true:includeNamespace=false} ${message} ${exception:format=tostring}";
      FileTarget fileTarget1 = new FileTarget();
      fileTarget1.FileName = (Layout) configurationProvider.LogPath;
      fileTarget1.Layout = (Layout) str;
      fileTarget1.ArchiveNumbering = ArchiveNumberingMode.Rolling;
      fileTarget1.ArchiveFileName = (Layout) Path.GetFileName(configurationProvider.LogPath);
      fileTarget1.ArchiveAboveSize = 10485760L;
      fileTarget1.MaxArchiveFiles = 1;
      fileTarget1.Encoding = Encoding.UTF8;
      FileTarget fileTarget2 = fileTarget1;
      loggingConfiguration.AddTarget("file", (Target) fileTarget2);
      loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, (Target) fileTarget2));
      LogManager.Configuration = loggingConfiguration;
    }

    public ILogger GetCurrentClassLogger<TCaller>() => (ILogger) LogManager.GetLogger(typeof (TCaller).FullName);

    public IEnumerable<LogMessage> ParseLogFile()
    {
      using (StreamReader logReader = new StreamReader(this._configurationProvider.LogPath))
      {
        LogMessage logMessage1 = (LogMessage) null;
        while (!logReader.EndOfStream)
        {
          string input = logReader.ReadLine();
          if (input != null)
          {
            Match match = this.messageHeader.Match(input);
            if (!match.Success)
            {
              if (logMessage1 != null)
                logMessage1.Message += input;
            }
            else
            {
              if (logMessage1 != null)
                yield return logMessage1;
              LogMessage logMessage2 = new LogMessage();
              DateTime result;
              if (DateTime.TryParseExact(match.Groups[1].Value, "dd.MM.yyyy HH:mm:ss z", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                logMessage2.Time = result.ToUniversalTime();
              else if (DateTime.TryParseExact(match.Groups[1].Value, "dd.MM.yyyy HH:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
              {
                logMessage2.Time = result.ToUniversalTime();
              }
              else
              {
                logMessage2.Time = DateTime.UtcNow;
                logMessage2.Message = match.Groups[1].Value + logMessage2.Message;
              }
              logMessage2.LogLevel = LogLevel.FromString(match.Groups[2].Value);
              logMessage2.Message = match.Groups[3].Value;
              logMessage1 = logMessage2;
              match = (Match) null;
            }
          }
          else
            break;
        }
        if (logMessage1 != null)
          yield return logMessage1;
      }
    }
  }
}
