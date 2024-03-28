// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Accounts.EncryptedAccountRepository
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.Accounts
{
  public sealed class EncryptedAccountRepository : IAccountRepository
  {
    private readonly IAccountRepository _repository;
    private readonly ILauncherIdProvider _launcherIdProvider;
    private volatile FastEncryptor _encryptor;
    private volatile bool _useEncryption = true;

    public EncryptedAccountRepository(
      ILauncherIdProvider launcherIdProvider,
      IAccountRepository repository)
    {
      this._launcherIdProvider = launcherIdProvider ?? throw new ArgumentNullException(nameof (launcherIdProvider));
      this._repository = repository ?? throw new ArgumentNullException(nameof (repository));
    }

    public void Delete(long id) => this._repository.Delete(id);

    public List<Account> GetAll()
    {
      List<Account> all = this._repository.GetAll();
      foreach (Account account in all)
        this.Decrypt(account);
      return all;
    }

    public void Save(Account account)
    {
      this.Encrypt(account);
      this._repository.Save(account);
    }

    private void Decrypt(Account account)
    {
      if (!account.Encrypted || !this.ShouldUseEncryption())
        return;
      account.RefreshToken = this._encryptor.Decrypt(account.RefreshToken);
    }

    private void Encrypt(Account account)
    {
      if (!this.ShouldUseEncryption())
        return;
      account.Encrypted = true;
      account.RefreshToken = this._encryptor.Encrypt(account.RefreshToken);
    }

    private bool ShouldUseEncryption()
    {
      if (!this._useEncryption)
        return false;
      if (this._encryptor == null)
      {
        try
        {
          this._encryptor = new FastEncryptor(this._launcherIdProvider.Get());
        }
        catch (Exception ex)
        {
          this._useEncryption = false;
        }
      }
      return this._encryptor != null;
    }
  }
}
