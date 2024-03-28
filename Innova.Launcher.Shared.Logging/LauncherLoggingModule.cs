// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Logging.LauncherLoggingModule
// Assembly: Innova.Launcher.Shared.Logging, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 00910835-5246-4CB6-87E9-1F840D471FFA
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Logging.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace Innova.Launcher.Shared.Logging
{
  public sealed class LauncherLoggingModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry) => containerRegistry.RegisterSingleton<ILoggerFactory, LoggerFactory>();

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
