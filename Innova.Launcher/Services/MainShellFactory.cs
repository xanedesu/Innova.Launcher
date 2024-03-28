// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.MainShellFactory
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;

namespace Innova.Launcher.Services
{
  public class MainShellFactory : IMainShellFactory
  {
    public IMainShellPresenter Create(string windowId)
    {
      MainViewModel instance1 = ServiceLocator.Current.GetInstance<MainViewModel>();
      IWindowsService instance2 = ServiceLocator.Current.GetInstance<IWindowsService>();
      instance1.Id = windowId;
      if (windowId != "main")
        instance1.SaveWindowPosition = false;
      return (IMainShellPresenter) new MainShell(instance1, instance2);
    }
  }
}
