// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Accounts.AccountRepositoryFactory
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Threading;

namespace Innova.Launcher.Core.Services.Accounts
{
  public class AccountRepositoryFactory : IAccountRepositoryFactory
  {
    private readonly IDatabaseLocationSelectionStrategy _databaseLocationSelectionStrategy;
    private readonly ILauncherIdProvider _launcherIdProvider;
    private readonly Lazy<IAccountRepository> _instanceLazy;

    public AccountRepositoryFactory(
      ILauncherIdProvider launcherIdProvider,
      IDatabaseByEnvironmentLocationStrategy databaseLocationSelectionStrategy)
    {
      this._launcherIdProvider = launcherIdProvider ?? throw new ArgumentNullException(nameof (launcherIdProvider));
      this._databaseLocationSelectionStrategy = (IDatabaseLocationSelectionStrategy) (databaseLocationSelectionStrategy ?? throw new ArgumentNullException(nameof (databaseLocationSelectionStrategy)));
      this._instanceLazy = new Lazy<IAccountRepository>((Func<IAccountRepository>) (() => (IAccountRepository) new EncryptedAccountRepository(this._launcherIdProvider, (IAccountRepository) new AccountRepository(this._databaseLocationSelectionStrategy.GetDatabaseLocation("accounts")))), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public IAccountRepository Get() => this._instanceLazy.Value;
  }
}
