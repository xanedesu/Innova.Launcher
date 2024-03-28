// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Crypto.RsaStorage
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Crypto.Interfaces;
using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Core.Infrastructure.Mapping.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Innova.Launcher.Core.Infrastructure.Crypto
{
  public class RsaStorage : IRsaStorage
  {
    private readonly RSAParameters _rsaParameters;
    private readonly object _lockObject = new object();

    public RsaStorage(
      IMapper mapper,
      IDatabaseInProgramDataLocationStrategy dbPathStrategy)
    {
      this._rsaParameters = this.ReadParametersWithRecovery(mapper, dbPathStrategy.GetDatabaseLocation("rsaConfig"));
    }

    private RsaStorage()
    {
    }

    public RSAParameters Get() => this._rsaParameters;

    private RSAParameters ReadParametersWithRecovery(IMapper mapper, string databaseFilePath)
    {
      try
      {
        return this.ReadParameters(mapper, databaseFilePath);
      }
      catch (Exception ex) when (ex is NullReferenceException || ex is ArgumentException)
      {
        if (File.Exists(databaseFilePath))
          File.Delete(databaseFilePath);
      }
      return this.ReadParameters(mapper, databaseFilePath);
    }

    private RSAParameters ReadParameters(IMapper mapper, string databaseFilePath)
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(databaseFilePath))
        {
          LiteCollection<RSAParametersSerializable> collection = liteDatabase.GetCollection<RSAParametersSerializable>("rsa2");
          List<RSAParametersSerializable> list = collection.FindAll().ToList<RSAParametersSerializable>();
          if (list.Count > 0)
            return mapper.Map<RSAParametersSerializable, RSAParameters>(list[0]);
          RSAParameters rsaParameters = this.GenerateRsaParameters();
          RSAParametersSerializable document = mapper.Map<RSAParameters, RSAParametersSerializable>(rsaParameters);
          collection.Insert(document);
          return rsaParameters;
        }
      }
    }

    private RSAParameters GenerateRsaParameters()
    {
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider(1024))
        return cryptoServiceProvider.ExportParameters(true);
    }
  }
}
