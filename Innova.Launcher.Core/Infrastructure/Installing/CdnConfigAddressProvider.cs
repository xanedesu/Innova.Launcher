// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Installing.CdnConfigAddressProvider
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using System;

namespace Innova.Launcher.Core.Infrastructure.Installing
{
  public class CdnConfigAddressProvider : ICdnGamesConfigAddressProvider
  {
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;

    public string Address => this._gamesEnvironmentProvider.CurrentGamesConfigUrl;

    public string SinglesAddress => this._gamesEnvironmentProvider.CurrentSingleGamesConfigUrl;

    public CdnConfigAddressProvider(IGamesEnvironmentProvider gamesEnvironmentProvider) => this._gamesEnvironmentProvider = gamesEnvironmentProvider ?? throw new ArgumentNullException(nameof (gamesEnvironmentProvider));
  }
}
