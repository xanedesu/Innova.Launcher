// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherStructureProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Configuration.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using System.IO;

namespace Innova.Launcher.Shared.Services
{
  public class LauncherStructureProvider : ILauncherStructureProvider
  {
    private readonly ILauncherStructureConfigurationProvider _configurationProvider;
    private string _baseDirectory;

    public string BaseDirectory
    {
      get => this._baseDirectory ?? Directory.GetCurrentDirectory();
      set => this._baseDirectory = value;
    }

    public LauncherStructureProvider(
      ILauncherStructureConfigurationProvider configurationProvider)
    {
      this._configurationProvider = configurationProvider;
    }

    public string GetRunnerExeFilePath() => Path.Combine(this.BaseDirectory, this._configurationProvider.RunnerExeName);

    public string GetRunnerFromUpdateExeFileName() => this._configurationProvider.RunnerFromUpdateExeName;

    public string GetLauncherExeFilePath() => Path.Combine(this.BaseDirectory, this._configurationProvider.BinariesFolderName, this._configurationProvider.LauncherExeName);

    public string GetLauncherTempUpdateFolderPath() => Path.Combine(this.BaseDirectory, this._configurationProvider.BinariesFolderName, this._configurationProvider.TempUpdateFolderName);

    public string GetUpdateVersionFilePath() => Path.Combine(this.BaseDirectory, this._configurationProvider.BinariesFolderName, this._configurationProvider.TempUpdateFolderName, this._configurationProvider.UpdateVersionFileName);

    public string GetBinaryFolderPath() => Path.Combine(this.BaseDirectory, this._configurationProvider.BinariesFolderName);
  }
}
