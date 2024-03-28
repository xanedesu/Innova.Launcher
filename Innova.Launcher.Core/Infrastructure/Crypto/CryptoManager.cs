// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Crypto.CryptoManager
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Crypto.Interfaces;
using Innova.Launcher.Core.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Innova.Launcher.Core.Infrastructure.Crypto
{
  public class CryptoManager : ICryptoManager
  {
    private readonly IRsaStorage _rsaStorage;

    public CryptoManager(IRsaStorage rsaStorage) => this._rsaStorage = rsaStorage;

    public CryptoPublicKey GetPublicKey()
    {
      RSAParameters rsaParameters = this._rsaStorage.Get();
      return new CryptoPublicKey(rsaParameters.Modulus, rsaParameters.Exponent);
    }

    public string Encrypt(string input)
    {
      RSAParameters parameters = this._rsaStorage.Get();
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(1024))
      {
        try
        {
          cryptoServiceProvider.ImportParameters(parameters);
          byte[] bytes = Encoding.UTF8.GetBytes(input);
          return Convert.ToBase64String(cryptoServiceProvider.Encrypt(bytes, true));
        }
        finally
        {
          cryptoServiceProvider.PersistKeyInCsp = false;
        }
      }
    }

    public string Decrypt(string input)
    {
      RSAParameters parameters = this._rsaStorage.Get();
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(1024))
      {
        try
        {
          cryptoServiceProvider.ImportParameters(parameters);
          byte[] rgb = Convert.FromBase64String(input);
          return Encoding.UTF8.GetString(cryptoServiceProvider.Decrypt(rgb, true));
        }
        finally
        {
          cryptoServiceProvider.PersistKeyInCsp = false;
        }
      }
    }
  }
}
