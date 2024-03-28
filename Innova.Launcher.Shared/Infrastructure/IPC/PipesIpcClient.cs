// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.IPC.PipesIpcClient
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.IO;
using System.IO.Pipes;

namespace Innova.Launcher.Shared.Infrastructure.IPC
{
  public class PipesIpcClient : IDisposable
  {
    private readonly NamedPipeClientStream _client;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    public PipesIpcClient(string pipeName)
    {
      this._client = new NamedPipeClientStream(pipeName);
      this._client.Connect(1000);
      this._reader = new StreamReader((Stream) this._client);
      this._writer = new StreamWriter((Stream) this._client);
    }

    public void WriteLine(string message)
    {
      this._writer.WriteLine(message);
      this._writer.Flush();
    }

    public string ReadLine() => this._reader.ReadLine();

    public void Dispose()
    {
      try
      {
        this._client?.Dispose();
        this._reader?.Dispose();
        this._writer?.Dispose();
      }
      catch
      {
      }
    }
  }
}
