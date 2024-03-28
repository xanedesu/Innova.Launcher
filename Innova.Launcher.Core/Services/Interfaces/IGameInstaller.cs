// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.IGameInstaller
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface IGameInstaller
  {
    Task InstallAsync(string gameKey, string path, string extendedStatus, string culture);

    Task UpdateAsync(string gameKey);

    Task RepairAsync(string gameKey);

    Task ResumeAsync(string gameKey);

    void Pause(string gameKey);

    void Cancel(string gameKey);

    void Register(Innova.Launcher.Shared.Models.GameConfig.GameConfig gameConfig);
  }
}
