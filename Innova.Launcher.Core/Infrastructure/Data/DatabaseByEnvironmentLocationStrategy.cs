// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Data.DatabaseByEnvironmentLocationStrategy
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using System;
using System.IO;

namespace Innova.Launcher.Core.Infrastructure.Data
{
  public class DatabaseByEnvironmentLocationStrategy : 
    IDatabaseByEnvironmentLocationStrategy,
    IDatabaseLocationSelectionStrategy
  {
    private readonly IGamesEnvironmentProvider _gamesEnvironmentProvider;
    private readonly string _databaseFolderPath;

    public DatabaseByEnvironmentLocationStrategy(IGamesEnvironmentProvider gamesEnvironmentProvider)
    {
      this._gamesEnvironmentProvider = gamesEnvironmentProvider;
      string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Innova");
      FileSystemHelper.CreateDirectoryIfNotExists(str);
      string directoryPath = Path.Combine(str, "4game2.0");
      FileSystemHelper.CreateDirectoryIfNotExists(directoryPath);
      this._databaseFolderPath = directoryPath;
    }

    public string GetDatabaseLocation(string dataBaseName)
    {
      string gamesEnvironment = this._gamesEnvironmentProvider.CurrentGamesEnvironment;
      return Path.Combine(this._databaseFolderPath, dataBaseName + "-" + gamesEnvironment + ".db");
    }
  }
}
