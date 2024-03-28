// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameRepository
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Innova.Launcher.Core.Services
{
  public class GameRepository : IGameRepository
  {
    private readonly string _databaseFilePath;
    private readonly object _lockObject;

    public GameRepository(ILoggerFactory loggerFactory, string databaseFilePath)
    {
      this._databaseFilePath = databaseFilePath;
      this._lockObject = new object();
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          liteDatabase.GetCollection<Game>("games").EnsureIndex<string>((Expression<Func<Game, string>>) (x => x.Key), true);
      }
    }

    public Game GetOrDefault(string gameKey)
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          return liteDatabase.GetCollection<Game>("games").FindById(new BsonValue(gameKey));
      }
    }

    public IEnumerable<Game> GetAll()
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          return (IEnumerable<Game>) liteDatabase.GetCollection<Game>("games").FindAll().ToList<Game>();
      }
    }

    public void Save(Game game)
    {
      lock (this._lockObject)
      {
        using (LiteDatabase liteDatabase = new LiteDatabase(this._databaseFilePath))
          liteDatabase.GetCollection<Game>("games").Upsert(game);
      }
    }
  }
}
