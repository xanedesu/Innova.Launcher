// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.Interfaces.ILauncherInSystemRegistrator
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Utils;

namespace Innova.Launcher.Shared.Services.Interfaces
{
  public interface ILauncherInSystemRegistrator
  {
    void Register(LauncherRegistrationData registrationData, bool createShortcut);

    void Unregister(LauncherUnregistrationData unregistrationData);

    bool IsRegistered(string registrationKey);

    void SetLauncherIdIfNotExist(string launcherKey, string launcherId);

    void Update(LauncherRegistrationDataUpdate updateData);

    string GetInstallPath(string registrationKey);

    RegisterLauncherSoftwareInfo GetLauncherSoftwareInfo(string launcherKey);

    void UpdateSoftwareInfo(RegisterLauncherSoftwareInfo updateData);
  }
}
