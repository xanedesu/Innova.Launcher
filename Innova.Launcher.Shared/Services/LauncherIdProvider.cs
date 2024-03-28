// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.LauncherIdProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Threading;

namespace Innova.Launcher.Shared.Services
{
  public class LauncherIdProvider : ILauncherIdProvider
  {
    private readonly ILauncherInSystemRegistrator _launcherInSystemRegistrator;
    private readonly ILogger _logger;
    private readonly ILauncherIdGenerator _launcherIdGenerator;
    private Lazy<string> _launcherId;

    public LauncherIdProvider(
      ILoggerFactory loggerFactory,
      ILauncherInSystemRegistrator launcherInSystemRegistrator,
      ILauncherIdGenerator launcherIdGenerator)
    {
      this._launcherIdGenerator = launcherIdGenerator;
      this._launcherInSystemRegistrator = launcherInSystemRegistrator;
      this._logger = loggerFactory.GetCurrentClassLogger<ComputerNameProvider>();
      this.RefreshValue();
    }

    public string Get()
    {
      if (string.IsNullOrWhiteSpace(this._launcherId.Value))
        this.RefreshValue();
      return this._launcherId.Value;
    }

    private void RefreshValue() => this._launcherId = new Lazy<string>(new Func<string>(this.GetCore), LazyThreadSafetyMode.ExecutionAndPublication);

    private string GetCore()
    {
      try
      {
        string launcherId = this._launcherInSystemRegistrator.GetLauncherSoftwareInfo("4game2.0").LauncherId;
        if (string.IsNullOrEmpty(launcherId))
        {
          launcherId = this._launcherIdGenerator.GenegateNewId();
          this._launcherInSystemRegistrator.SetLauncherIdIfNotExist("4game2.0", launcherId);
        }
        return launcherId;
      }
      catch (Exception ex)
      {
        this._logger.Error<Exception>(ex);
        return string.Empty;
      }
    }
  }
}
