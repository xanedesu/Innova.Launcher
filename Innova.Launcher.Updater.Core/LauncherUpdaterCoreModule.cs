// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.LauncherUpdaterCoreModule
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using Innova.Launcher.Updater.Core.Services;
using Innova.Launcher.Updater.Core.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using System.Text;

namespace Innova.Launcher.Updater.Core
{
  public class LauncherUpdaterCoreModule : IModule
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      containerRegistry.RegisterSingleton<IBinaryUpdater, ForgameUpdater>();
      containerRegistry.RegisterSingleton<IForgameUpdaterProgressHandler, ForgameUpdaterProgressHandler>();
      containerRegistry.RegisterSingleton<IExternalGameInstaller, GameInstallerByBinaryUpdater>();
      containerRegistry.Register<LauncherBinaryUpdater>();
      containerRegistry.Register<IProcessManager, ProcessManager>();
    }

    public void OnInitialized(IContainerProvider containerProvider)
    {
    }
  }
}
