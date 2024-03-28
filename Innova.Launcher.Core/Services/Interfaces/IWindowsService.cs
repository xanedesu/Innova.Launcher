// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.IWindowsService
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public interface IWindowsService
  {
    void FocusWindow(string windowId);

    void HideToTrayWindow(string windowId);

    void HideToTrayAllWindows();

    void RiseWindow(string windowId);

    void RiseAllWindows();

    void CloseWindow(string windowId, bool force = false);

    void Reload(string windowId);

    void OpenWindow(string parentWindowId, OpenWindowData windowData);

    void OpenUrlInMainWindow(string url);

    string OpenFolderSelectDialog(string prePickedPath);

    void CloseExceptMain();

    void MaximizeOrNormalize(string windowId);

    void Normalize(string windowId);

    void Minimize(string windowId);

    bool IsMainWindowVisible();

    bool ShouldWindowHideOnClose(string viewModelId);

    void SaveWindowLocalStorage(string windowId);

    void RestoreWindowLocalStorage(string windowId, bool cleanAfterRestore);

    WindowSize GetMainWindowSize();

    List<WindowInfo> GetWindowsInfo();
  }
}
