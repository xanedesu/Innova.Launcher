// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Tracking.LauncherTrackingModule
// Assembly: Innova.Launcher.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: DA384C86-6E9B-47C9-B483-AED3A5709C44
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Tracking.dll

using Innova.Launcher.Shared.Tracking.Interfaces;
using Innova.Launcher.Tracking.Services;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher.Tracking
{
  public sealed class LauncherTrackingModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterSingleton<ITrackingService, TrackingService>();

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
