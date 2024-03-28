// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Data.DatabaseInProgramDataLocationStrategy
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Shared.Utils;
using System;
using System.IO;

namespace Innova.Launcher.Core.Infrastructure.Data
{
  public class DatabaseInProgramDataLocationStrategy : 
    IDatabaseInProgramDataLocationStrategy,
    IDatabaseLocationSelectionStrategy
  {
    private readonly string _launcherProgrammDataFolder;

    public DatabaseInProgramDataLocationStrategy()
    {
      string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Innova");
      FileSystemHelper.CreateDirectoryIfNotExists(str);
      this._launcherProgrammDataFolder = Path.Combine(str, "4game2.0");
      FileSystemHelper.CreateDirectoryIfNotExists(this._launcherProgrammDataFolder);
    }

    public string GetDatabaseLocation(string dataBaseName) => Path.Combine(this._launcherProgrammDataFolder, dataBaseName + ".db");
  }
}
