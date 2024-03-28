// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Infrastructure.ForgameUpdaterExecutable
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using ICSharpCode.SharpZipLib.Zip;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.Updater.Core.Exceptions;
using Innova.Launcher.Updater.Core.Helpers;
using Innova.Launcher.Updater.Core.Services;
using System;
using System.IO;
using System.Reflection;

namespace Innova.Launcher.Updater.Core.Infrastructure
{
  public class ForgameUpdaterExecutable
  {
    private const int DefaultZipCodePage = 437;
    private static readonly string _forgameBinFolderName = "forgameUpdater";
    private static readonly string _forgameExeFileName = "4updater.exe";
    private static readonly string _forgameZipFileName = "ForgameUpdater";
    private readonly string _updaterName;

    public string DirectoryName { get; }

    public string FileName { get; }

    public ForgameUpdaterExecutable()
      : this((string) null)
    {
    }

    public ForgameUpdaterExecutable(string updaterName)
    {
      this._updaterName = updaterName;
      ZipConstants.DefaultCodePage = 437;
      FileInfo updaterExecutable = this.GetUpdaterExecutable();
      this.DirectoryName = updaterExecutable?.DirectoryName ?? throw new ForgameUpdaterExtractionException("Updater exe " + updaterExecutable?.FullName + " or it's folder not found");
      this.FileName = updaterExecutable.Name;
    }

    private FileInfo GetUpdaterExecutable()
    {
      string updaterExePath = this.GetUpdaterExePath();
      if (!File.Exists(updaterExePath))
      {
        try
        {
          this.ExtractForgameUpdaterBinaries();
        }
        catch (Exception ex)
        {
          throw new ForgameUpdaterExtractionException("Can't extract updater binaries..", ex);
        }
      }
      return new FileInfo(updaterExePath);
    }

    private void ExtractForgameUpdaterBinaries()
    {
      string zipResourceName = this.GetZipResourceName();
      Assembly assembly = Assembly.GetAssembly(typeof (ForgameUpdater));
      Stream manifestResourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".ForgameUpdater." + zipResourceName + ".zip");
      string extractPath = this.GetExtractPath();
      FileSystemHelper.CreateDirectoryIfNotExists(extractPath);
      string outFolder = extractPath;
      ZipHelper.UnzipFromStream(manifestResourceStream, outFolder);
    }

    private string GetUpdaterExePath() => string.IsNullOrWhiteSpace(this._updaterName) ? Path.Combine(ForgameUpdaterExecutable._forgameBinFolderName, ForgameUpdaterExecutable._forgameExeFileName) : Path.Combine(ForgameUpdaterExecutable._forgameBinFolderName, this._updaterName, this._updaterName + ".exe");

    private string GetExtractPath() => string.IsNullOrWhiteSpace(this._updaterName) ? ForgameUpdaterExecutable._forgameBinFolderName : Path.Combine(ForgameUpdaterExecutable._forgameBinFolderName, this._updaterName);

    private string GetZipResourceName() => string.IsNullOrWhiteSpace(this._updaterName) ? ForgameUpdaterExecutable._forgameZipFileName : this._updaterName;
  }
}
