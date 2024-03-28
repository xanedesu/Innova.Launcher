// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.FrostUpdaterFactory
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Updater.Core.Services.Interfaces;

namespace Innova.Launcher.Core.Services
{
  public class FrostUpdaterFactory
  {
    private readonly ILauncherConfigurationProvider _launcherConfigurationProvider;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IBinaryUpdater _binaryUpdater;

    public FrostUpdaterFactory(
      ILauncherConfigurationProvider launcherConfigurationProvider,
      ILoggerFactory loggerFactory,
      IBinaryUpdater binaryUpdater)
    {
      this._launcherConfigurationProvider = launcherConfigurationProvider;
      this._loggerFactory = loggerFactory;
      this._binaryUpdater = binaryUpdater;
    }

    public IFrostUpdater Get() => (IFrostUpdater) new FrostUpdater(this._launcherConfigurationProvider, this._loggerFactory, this._binaryUpdater);
  }
}
