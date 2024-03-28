// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.GameRepositoryFactory
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Data.Interfaces;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;

namespace Innova.Launcher.Core.Services
{
  public class GameRepositoryFactory : IGameRepositoryFactory
  {
    private readonly ILoggerFactory _loggerFactory;
    private readonly IDatabaseLocationSelectionStrategy _databaseLocationSelectionStrategy;

    public GameRepositoryFactory(
      ILoggerFactory loggerFactory,
      IDatabaseByEnvironmentLocationStrategy databaseLocationSelectionStrategy)
    {
      this._loggerFactory = loggerFactory;
      this._databaseLocationSelectionStrategy = (IDatabaseLocationSelectionStrategy) databaseLocationSelectionStrategy;
    }

    public IGameRepository Get() => (IGameRepository) new GameRepository(this._loggerFactory, this._databaseLocationSelectionStrategy.GetDatabaseLocation("games"));
  }
}
