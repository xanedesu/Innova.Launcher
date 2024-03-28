// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Crypto.RSAParametersSerializable
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using LiteDB;
using System;

namespace Innova.Launcher.Core.Infrastructure.Crypto
{
  public struct RSAParametersSerializable
  {
    [BsonId]
    public Guid Id { get; set; }

    public byte[] Exponent { get; set; }

    public byte[] Modulus { get; set; }

    public byte[] P { get; set; }

    public byte[] Q { get; set; }

    public byte[] DP { get; set; }

    public byte[] DQ { get; set; }

    public byte[] InverseQ { get; set; }

    public byte[] D { get; set; }
  }
}
