// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.FastEncryptor
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;
using System.Text;

namespace Innova.Launcher.Core.Services
{
  public class FastEncryptor
  {
    private readonly byte[] _key;

    public FastEncryptor(string secret) => this._key = Encoding.UTF8.GetBytes(secret);

    public string Encrypt(string source)
    {
      if (source == null)
        return (string) null;
      byte[] bytes = Encoding.UTF8.GetBytes(source);
      for (int index = 0; index < bytes.Length; ++index)
        bytes[index] = (byte) ((uint) bytes[index] ^ (uint) this._key[index % this._key.Length]);
      return Convert.ToBase64String(bytes);
    }

    public string Decrypt(string value)
    {
      if (value == null)
        return (string) null;
      byte[] bytes = Convert.FromBase64String(value);
      for (int index = 0; index < bytes.Length; ++index)
        bytes[index] = (byte) ((uint) bytes[index] ^ (uint) this._key[index % this._key.Length]);
      return Encoding.UTF8.GetString(bytes);
    }
  }
}
