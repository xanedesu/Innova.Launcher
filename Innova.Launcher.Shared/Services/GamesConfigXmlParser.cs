// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.GamesConfigXmlParser
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Services
{
  public class GamesConfigXmlParser : IGamesConfigParser
  {
    private readonly ILogger _logger;

    public GamesConfigXmlParser(ILoggerFactory loggerFactory) => this._logger = loggerFactory.GetCurrentClassLogger<GamesConfigXmlParser>();

    public GamesConfig Parse(string data)
    {
      try
      {
        using (TextReader textReader = (TextReader) new StringReader(data))
          return (GamesConfig) new XmlSerializer(typeof (GamesConfig)).Deserialize(textReader);
      }
      catch (Exception ex)
      {
        this._logger.Error(ex, "Unable to parse games config xml: " + data);
        throw;
      }
    }
  }
}
