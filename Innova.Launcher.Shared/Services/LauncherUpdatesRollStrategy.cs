// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherUpdatesRollStrategy
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Utils;
using NLog;
using System;
using System.IO;
using System.Threading;

namespace Innova.Launcher.Shared.Services
{
  public class LauncherUpdatesRollStrategy : ILauncherUpdatesRollStrategy
  {
    private static readonly char[] _slashes = new char[2]
    {
      '\\',
      '/'
    };
    private readonly ILauncherStructureProvider _launcherStructureProvider;
    private readonly IExistingLauncherChecker _existingLauncherChecker;
    private readonly ILogger _logger;

    public LauncherUpdatesRollStrategy(
      ILoggerFactory loggerFactory,
      ILauncherStructureProvider launcherStructureProvider,
      IExistingLauncherChecker existingLauncherChecker)
    {
      this._launcherStructureProvider = launcherStructureProvider;
      this._existingLauncherChecker = existingLauncherChecker;
      this._logger = loggerFactory.GetCurrentClassLogger<LauncherUpdatesRollStrategy>();
    }

    public void RollUpdatesIfReady()
    {
      string updateFolderPath = this._launcherStructureProvider.GetLauncherTempUpdateFolderPath();
      if (!Directory.Exists(updateFolderPath))
        return;
      if (File.Exists(this._launcherStructureProvider.GetUpdateVersionFilePath()))
      {
        this._logger.Trace("There is updates folder");
        this._existingLauncherChecker.WaitAlreadyRunningLauncherForExit(this._launcherStructureProvider.GetLauncherExeFilePath(), TimeSpan.FromSeconds(10.0));
        this.DeleteCurrentVersion();
        this.ExchangeUpdateToInstalled();
        File.Delete(this._launcherStructureProvider.GetUpdateVersionFilePath());
      }
      try
      {
        Directory.Delete(updateFolderPath);
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, "Error while deleting " + updateFolderPath, Array.Empty<object>());
      }
    }

    private void DeleteCurrentVersion()
    {
      this._logger.Trace("Delete current version files");
      string str1 = this._launcherStructureProvider.GetLauncherTempUpdateFolderPath().RemoveTrailingSymbols(LauncherUpdatesRollStrategy._slashes);
      string path1 = this._launcherStructureProvider.GetBinaryFolderPath().RemoveTrailingSymbols(LauncherUpdatesRollStrategy._slashes);
      string[] files = Directory.GetFiles(path1, "*", SearchOption.TopDirectoryOnly);
      string[] directories = Directory.GetDirectories(path1, "*", SearchOption.TopDirectoryOnly);
      int num1 = 15;
      foreach (string path2 in files)
      {
        this._logger.Trace("Delete " + path2);
        int num2 = 0;
        while (num2 < num1)
        {
          try
          {
            File.Delete(path2);
            break;
          }
          catch (Exception ex)
          {
            this._logger.Error(ex, string.Format("Problem to delete file {0}. Retry {1}", (object) path2, (object) (num2 + 1)));
            if (num2 == num1)
              throw;
            else
              Thread.Sleep(200);
          }
          finally
          {
            ++num2;
          }
        }
      }
      foreach (string str2 in directories)
      {
        if (!str2.RemoveTrailingSymbols(LauncherUpdatesRollStrategy._slashes).Equals(str1))
        {
          this._logger.Trace("Delete directory " + str2);
          int num3 = 0;
          while (num3 < num1)
          {
            try
            {
              Directory.Delete(str2, true);
              break;
            }
            catch (Exception ex)
            {
              this._logger.Error(ex, string.Format("Problem to delete directory {0}. Retry {1}..", (object) str2, (object) (num3 + 1)));
              if (num3 == num1)
                throw;
              else
                Thread.Sleep(200);
            }
            finally
            {
              ++num3;
            }
          }
        }
      }
    }

    private void ExchangeUpdateToInstalled()
    {
      this._logger.Trace("Copy update files to binary folder");
      string str = this._launcherStructureProvider.GetLauncherTempUpdateFolderPath().RemoveTrailingSymbols(LauncherUpdatesRollStrategy._slashes);
      string newValue = this._launcherStructureProvider.GetBinaryFolderPath().RemoveTrailingSymbols(LauncherUpdatesRollStrategy._slashes);
      foreach (string directory in Directory.GetDirectories(str, "*", SearchOption.AllDirectories))
        FileSystemHelper.CreateDirectoryIfNotExists(directory.Replace(str, newValue));
      foreach (string file in Directory.GetFiles(str, "*", SearchOption.AllDirectories))
        File.Copy(file, file.Replace(str, newValue), true);
    }
  }
}
