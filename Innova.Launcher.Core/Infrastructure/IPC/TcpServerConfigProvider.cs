﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.IPC.TcpServerConfig
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;

namespace Innova.Launcher.Core.Infrastructure.IPC
{
  public class TcpServerConfig : ITcpServerConfigProvider
  {
    public int TcpPort { get; }

    public TcpServerConfig(int port) => this.TcpPort = port;

    public TcpServerConfig(
      ILauncherConfigurationProvider appConfigurationProvider)
    {
      this.TcpPort = appConfigurationProvider.ForgameUpdaterTcpServerPort;
    }
  }
}