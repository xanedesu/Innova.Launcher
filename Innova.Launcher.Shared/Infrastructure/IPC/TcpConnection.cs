// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.IPC.TcpConnection
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Exceptions;
using Innova.Launcher.Shared.Extensions;
using Innova.Launcher.Shared.Infrastructure.IPC.Interfaces;
using System;
using System.Net.Sockets;
using System.Text;

namespace Innova.Launcher.Shared.Infrastructure.IPC
{
  public class TcpConnection : IIpcConnection
  {
    private const int LengthBytesCount = 2;
    private readonly TcpClient _tcpClient;
    private byte[] _buffer;
    private int _bufferLength;

    public event EventHandler<IpcMessageEventArgs> MessageReceived;

    public TcpConnection(TcpClient tcpClient) => this._tcpClient = tcpClient;

    public void StartReceive()
    {
      try
      {
        this._buffer = new byte[2];
        this._tcpClient.Client.BeginReceive(this._buffer, 0, 2, SocketFlags.None, new AsyncCallback(this.ReceiveCallback), (object) null);
      }
      catch
      {
        this.Close();
      }
    }

    public void Send(string message)
    {
      if (string.IsNullOrEmpty(message))
        return;
      Socket client = this._tcpClient.Client;
      if (client == null)
        throw new LostTcpClientException();
      byte[] bytes = Encoding.UTF8.GetBytes(message);
      byte[] buffer = bytes.AppendLeft(BitConverter.GetBytes((short) bytes.Length));
      try
      {
        client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.SendCallback), (object) this);
      }
      catch (SocketException ex)
      {
        throw new LostTcpClientException("Lost connection while sending", (Exception) ex);
      }
    }

    public void Close() => this._tcpClient.Close();

    private void SendCallback(IAsyncResult ar) => this._tcpClient.Client.EndSend(ar, out SocketError _);

    private void ReceiveCallback(IAsyncResult ar)
    {
      try
      {
        if (this._tcpClient.Client.EndReceive(ar) > 0)
        {
          this._bufferLength = (int) BitConverter.ToInt16(this._buffer, 0);
          this._buffer = new byte[this._bufferLength];
          this._tcpClient.Client.BeginReceive(this._buffer, 0, this._bufferLength, SocketFlags.None, new AsyncCallback(this.ReceiveBodyCallback), (object) null);
        }
        else
        {
          if (this._tcpClient.Client != null)
            return;
          this.Close();
        }
      }
      catch
      {
        this.Close();
      }
    }

    private void ReceiveBodyCallback(IAsyncResult ar)
    {
      this._tcpClient.Client.EndReceive(ar);
      this.OnMessageReceived(Encoding.UTF8.GetString(this._buffer, 0, this._bufferLength));
      this.StartReceive();
    }

    private void OnMessageReceived(string obj)
    {
      EventHandler<IpcMessageEventArgs> messageReceived = this.MessageReceived;
      if (messageReceived == null)
        return;
      messageReceived((object) this, new IpcMessageEventArgs(obj));
    }
  }
}
