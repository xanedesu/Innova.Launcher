// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Interfaces.IGameInSystemRegistrator
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Models;
using System.Collections.Generic;

namespace Innova.Launcher.Shared.Services.Interfaces
{
  public interface IGameInSystemRegistrator
  {
    void Register(GameRegistrationData registrationData);

    void Unregister(GameUnregistrationData unregistrationData);

    bool IsRegistered(string launcherKey, string gameRegistrationKey);

    List<InstalledGameInfo> GetInstalledGames(string launcherKey);
  }
}
