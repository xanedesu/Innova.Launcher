// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.Interfaces.IMainShellPresenter
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace Innova.Launcher.Services.Interfaces
{
  public interface IMainShellPresenter
  {
    WindowStartupLocation WindowStartupLocation { get; set; }

    bool IsLoaded { get; }

    bool IsVisible { get; }

    MainViewModel ViewModel { get; }

    bool SaveWindowPosition { get; set; }

    WindowState WindowState { get; set; }

    event EventHandler StateChanged;

    event EventHandler Closed;

    event CancelEventHandler Closing;

    void MakeResizable(double? width, double? height, double? minWidth, double? minHeight);

    void MakeNoResizable(double? width, double? height);

    void GoToUrl(string url);

    void Rise();

    void StartNativeDrag();

    void MaximizeOrNormalize();

    void Maximize();

    void Normalize();

    void HideToTray();

    void HideAndClose();

    bool Activate();

    void Minimize();

    WindowStorage GetWindowStorage();

    void RestoreWindowStorage(WindowStorage storage);
  }
}
