// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.IPC.PipesIpcConnection
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Innova.Launcher.Shared.Infrastructure.IPC
{
  public class PipesIpcConnection : IIpcConnection, IDisposable
  {
    private readonly NamedPipeServerStream _stream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;
    private Task _worker;

    public event EventHandler Closed;

    public event EventHandler<IpcMessageEventArgs> MessageReceived;

    public PipesIpcConnection(NamedPipeServerStream stream)
    {
      this._stream = stream;
      this._reader = new StreamReader((Stream) this._stream);
      this._writer = new StreamWriter((Stream) this._stream);
    }

    public void StartReceive() => this._worker = Task.Factory.StartNew((Action) (() =>
    {
      while (this._stream.IsConnected)
      {
        try
        {
          StringBuilder stringBuilder = new StringBuilder();
          do
          {
            string str = this._reader.ReadLine();
            stringBuilder.Append(str);
          }
          while (!this._stream.IsMessageComplete);
          string message = stringBuilder.ToString();
          if (!string.IsNullOrWhiteSpace(message))
          {
            try
            {
              this.OnMessageReceived(new IpcMessageEventArgs(message));
            }
            catch
            {
            }
          }
        }
        catch (Exception ex)
        {
          this.Close();
          return;
        }
      }
      this.Close();
    }), TaskCreationOptions.LongRunning);

    public void Send(string message)
    {
      if (!this._stream.IsConnected)
      {
        this.Close();
      }
      else
      {
        try
        {
          this._writer.WriteLine(message);
          this._writer.Flush();
        }
        catch (Exception ex)
        {
          this.Close();
        }
      }
    }

    public void Close()
    {
      try
      {
        this._writer.Dispose();
        this._reader.Dispose();
        this._stream.Dispose();
        this.OnClosed();
      }
      catch
      {
      }
    }

    private void OnMessageReceived(IpcMessageEventArgs e)
    {
      EventHandler<IpcMessageEventArgs> messageReceived = this.MessageReceived;
      if (messageReceived == null)
        return;
      messageReceived((object) this, e);
    }

    private void OnClosed()
    {
      EventHandler closed = this.Closed;
      if (closed == null)
        return;
      closed((object) this, EventArgs.Empty);
    }

    public void Dispose() => this.Close();
  }
}
