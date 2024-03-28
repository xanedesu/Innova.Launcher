// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.LauncherLocalizationModule
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using Innova.Launcher.Shared.Localization.Interfaces;
using Innova.Launcher.Shared.Localization.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher.Shared.Localization
{
  public sealed class LauncherLocalizationModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterSingleton<ILocalizationService, LocalizationService>();

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
