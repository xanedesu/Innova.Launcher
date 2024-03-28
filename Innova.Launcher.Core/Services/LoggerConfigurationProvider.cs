// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.LoggerConfigurationProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using System.IO;

namespace Innova.Launcher.Core.Services
{
  public class LoggerConfigurationProvider : ILoggerConfigurationProvider
  {
    public string LogPath { get; }

    public LoggerConfigurationProvider() => this.LogPath = Path.Combine(Path.GetTempPath(), "4game2.0", "launcher", "launcher.log");
  }
}
