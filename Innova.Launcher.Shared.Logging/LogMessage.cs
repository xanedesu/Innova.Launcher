// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Logging.LogMessage
// Assembly: Innova.Launcher.Shared.Logging, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 00910835-5246-4CB6-87E9-1F840D471FFA
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Logging.dll

using NLog;
using System;

namespace Innova.Launcher.Shared.Logging
{
  public class LogMessage
  {
    public DateTime Time { get; set; }

    public LogLevel LogLevel { get; set; }

    public string Message { get; set; } = string.Empty;
  }
}
