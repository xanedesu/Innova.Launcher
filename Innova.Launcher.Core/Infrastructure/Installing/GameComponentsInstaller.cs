// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Installing.GameComponentsInstaller
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Exceptions.GameComponent;
using Innova.Launcher.Core.Infrastructure.Common;
using Innova.Launcher.Core.Infrastructure.Installing.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Models.GameConfig;
using Microsoft.Win32;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Infrastructure.Installing
{
  public class GameComponentsInstaller : IGameComponentsInstaller
  {
    private readonly ILogger _logger;
    private readonly VersionComparer _versionComparer;
    private readonly List<string> _environmentPathes;
    private readonly string _osVersion;

    public GameComponentsInstaller(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<GameComponentsInstaller>();
      this._versionComparer = new VersionComparer();
      string environmentVariable = System.Environment.GetEnvironmentVariable("path");
      this._environmentPathes = (environmentVariable != null ? ((IEnumerable<string>) environmentVariable.Split(';')).ToList<string>() : (List<string>) null) ?? new List<string>();
      this._osVersion = string.Format("{0}.{1}", (object) System.Environment.OSVersion.Version.Major, (object) System.Environment.OSVersion.Version.Minor);
    }

    public void Install(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig, Game game)
    {
      this._logger.Trace(string.Format("Start to check and install game components (game={0}, count={1})", (object) game.EnvKey, (object) gameConfig.Components.Count));
      foreach (GameComponentConfig component in (List<GameComponentConfig>) gameConfig.Components)
      {
        if (!this.CheckComponentExists(component, game))
        {
          Task task = this.InstallComponent(component, game);
          try
          {
            task.Wait(CancellationToken.None);
          }
          catch (AggregateException ex)
          {
            if (ex.InnerExceptions.Count == 1)
              ExceptionDispatchInfo.Capture(ex.InnerExceptions[0]).Throw();
            throw;
          }
        }
      }
    }

    public bool CheckComponentExists(GameComponentConfig componentConfig, Game game)
    {
      string version = (string) null;
      if (!string.IsNullOrWhiteSpace(componentConfig.EnableForOS) && !this.IsOsOfVersion(componentConfig.EnableForOS))
        return true;
      if (componentConfig.Source == "file")
      {
        if (!Path.IsPathRooted(componentConfig.Path))
        {
          string str = Path.Combine(game.Path, componentConfig.Path);
          if (File.Exists(str))
          {
            if (componentConfig.CompareType == "presence")
              return true;
            if (!this.TryGetFileVersion(str, out version))
              return false;
          }
        }
        if (string.IsNullOrWhiteSpace(version) && !this.TryGetFileVersion(componentConfig.Path, out version))
          return false;
      }
      else
      {
        string str = Registry.GetValue(componentConfig.Path, componentConfig.Key, (object) null)?.ToString();
        if (str == null)
          return false;
        if (componentConfig.Source == "regfile")
        {
          if (!this.TryGetFileVersion(componentConfig.Path, out version))
            return false;
        }
        else
          version = str;
      }
      this._logger.Trace("Dependency " + componentConfig.Name + ". Current version of dependency (" + version + ") in source (" + componentConfig.Source + "), path (" + componentConfig.Path + "), key (" + componentConfig.Key + ")");
      if (version == null)
      {
        this._logger.Trace("Current version was not found.");
        return false;
      }
      if (!(componentConfig.CompareType == "min-version"))
        return Regex.IsMatch(version, componentConfig.Value);
      version = version.ToLower();
      return this._versionComparer.Compare(version, componentConfig.Value) >= 0;
    }

    private Task InstallComponent(GameComponentConfig componentConfig, Game game)
    {
      string installerFile = Path.Combine(game.Path, componentConfig.Exe);
      if (!File.Exists(installerFile))
        throw new GameComponentInstallerStartException("There is no component's exe " + installerFile, componentConfig);
      return Task.Factory.StartNew<Process>((Func<Process>) (() =>
      {
        this._logger.Trace("Start component installer " + componentConfig.Name + ", " + installerFile);
        Process process = Process.Start(new ProcessStartInfo()
        {
          WorkingDirectory = game.Path,
          FileName = installerFile,
          Arguments = componentConfig.Args,
          UseShellExecute = true,
          CreateNoWindow = true,
          WindowStyle = ProcessWindowStyle.Hidden
        });
        if (process == null)
          throw new GameComponentInstallerStartException("Can't start process by file $" + installerFile, componentConfig);
        process.EnableRaisingEvents = true;
        process.ErrorDataReceived += (DataReceivedEventHandler) ((_, args) => this._logger.Error("Error while component installer running " + args.Data));
        return process;
      }), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith((Action<Task<Process>>) (t =>
      {
        Process result = t.Result;
        while (!result.HasExited)
          Thread.Sleep(100);
        if (!this.IsSuccessfullInstallExitCode(result.ExitCode))
          throw new GameComponentInstallException(string.Format("Component installer has exited with error code {0}", (object) result.ExitCode), componentConfig, result.ExitCode);
      }), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
    }

    private bool IsSuccessfullInstallExitCode(int code) => code == 0 || code == -1442840576;

    private bool IsOsOfVersion(string versionRegexp) => Regex.IsMatch(this._osVersion, versionRegexp);

    private bool TryGetFileVersion(string filePath, out string version)
    {
      if (Path.IsPathRooted(filePath))
        return this.TryGetProductVersion(filePath, out version);
      List<string> stringList = new List<string>();
      stringList.Add(filePath);
      stringList.AddRange(this._environmentPathes.Select<string, string>((Func<string, string>) (p =>
      {
        try
        {
          return Path.Combine(p, filePath);
        }
        catch (Exception ex)
        {
          this._logger.Log(LogLevel.Warn, ex, "While combining [" + p + "] and [" + filePath + "]", Array.Empty<object>());
          return (string) null;
        }
      })).Where<string>((Func<string, bool>) (v => v != null)));
      foreach (string path in stringList)
      {
        if (this.TryGetProductVersion(path, out version))
          return true;
      }
      version = (string) null;
      return false;
    }

    private bool TryGetProductVersion(string path, out string version)
    {
      try
      {
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
        version = versionInfo.ProductVersion;
        return true;
      }
      catch (Exception ex)
      {
        if (!(ex is FileNotFoundException))
          this._logger.Error(ex, "Fail to get version of file " + path);
        version = (string) null;
        return false;
      }
    }
  }
}
