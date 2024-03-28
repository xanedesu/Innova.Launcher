// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Accounts.AccountRepository
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Innova.Launcher.Core.Services.Accounts
{
  public sealed class AccountRepository : IAccountRepository
  {
    private static readonly string _collectionName = "accounts";
    private readonly string _databaseFilePath;
    private readonly object _lockObject;

    public AccountRepository(string databaseFilePath)
    {
      this._databaseFilePath = databaseFilePath ?? throw new ArgumentNullException(nameof (databaseFilePath));
      this._lockObject = new object();
      this.EnsureIndex();
    }

    public void Delete(long id)
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          liteDatabase.GetCollection<Account>(AccountRepository._collectionName).Delete((BsonValue) id);
      }
    }

    public List<Account> GetAll()
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
        {
          List<Account> list = liteDatabase.GetCollection<Account>(AccountRepository._collectionName).FindAll().ToList<Account>();
          list.ForEach((Action<Account>) (v =>
          {
            Account account = v;
            if (account.Session != null)
              return;
            AccountSession accountSession;
            account.Session = accountSession = new AccountSession();
          }));
          return list;
        }
      }
    }

    public void Save(Account account)
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
        {
          LiteCollection<Account> collection = liteDatabase.GetCollection<Account>(AccountRepository._collectionName);
          Account byId = collection.FindById((BsonValue) account.Id);
          Account account1 = account;
          string email = account.Email;
          string str1 = email != null ? email.AsNullIfEmpty() : (string) null;
          account1.Email = str1;
          Account account2 = account;
          string str2 = account.Value;
          string str3 = str2 != null ? str2.AsNullIfEmpty() : (string) null;
          account2.Value = str3;
          Account account3 = account;
          string str4 = account.Value;
          string str5 = str4 != null ? str4.AsNullIfEmpty() : (string) null;
          account3.Gender = str5;
          Account account4 = account;
          string refreshToken = account.RefreshToken;
          string str6 = refreshToken != null ? refreshToken.AsNullIfEmpty() : (string) null;
          account4.RefreshToken = str6;
          Account account5 = account;
          string serializedGamesMeta = account.SerializedGamesMeta;
          string str7 = serializedGamesMeta != null ? serializedGamesMeta.AsNullIfEmpty() : (string) null;
          account5.SerializedGamesMeta = str7;
          Account account6 = account;
          string str8;
          if (account6.Email == null)
            account6.Email = str8 = byId?.Email;
          Account account7 = account;
          if (account7.Value == null)
            account7.Value = str8 = byId?.Value;
          Account account8 = account;
          if (account8.LastOpenedGames == null)
          {
            Dictionary<string, string> lastOpenedGames;
            account8.LastOpenedGames = lastOpenedGames = byId?.LastOpenedGames;
          }
          Account account9 = account;
          if (account9.Gender == null)
            account9.Gender = str8 = byId?.Gender;
          Account account10 = account;
          if (account10.RefreshToken == null)
            account10.RefreshToken = str8 = byId?.RefreshToken;
          Account account11 = account;
          if (account11.SerializedGamesMeta == null)
            account11.SerializedGamesMeta = str8 = byId?.SerializedGamesMeta;
          if (account.Email == null && account.Value == null)
            return;
          collection.Upsert(account);
        }
      }
    }

    private void EnsureIndex()
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          liteDatabase.GetCollection<Account>(AccountRepository._collectionName).EnsureIndex<long>((Expression<Func<Account, long>>) (x => x.Id), true);
      }
    }
  }
}
