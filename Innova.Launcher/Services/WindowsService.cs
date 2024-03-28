// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.WindowsService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Session;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.UI.Extensions;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Innova.Launcher.Services
{
  public class WindowsService : IWindowsService
  {
    private readonly ILogger _logger;
    private readonly IMainShellFactory _mainShellFactory;
    private readonly IBrowserDispatcher _browserDispatcher;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private readonly ILauncherTrackingService _launcherTrackingService;
    private readonly SessionEndedMessageHandler _sessionEndedMessageHandler;
    private readonly IWindowsStorageManager _windowsStorageManager;
    private readonly ConcurrentDictionary<string, Lazy<IMainShellPresenter>> _windows = new ConcurrentDictionary<string, Lazy<IMainShellPresenter>>();
    private readonly double _defaultMinWidth = 715.0;
    private readonly double _defaultMinHeight = 450.0;

    public WindowsService(
      ILoggerFactory loggerFactory,
      IMainShellFactory mainShellFactory,
      IBrowserDispatcher browserDispatcher,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider,
      ILauncherTrackingService launcherTrackingService,
      SessionEndedMessageHandler sessionEndedMessageHandler,
      ILauncherStateService launcherStateService,
      IWindowsStorageManager windowsStorageManager)
    {
      this._logger = loggerFactory.GetCurrentClassLogger<WindowsService>();
      this._mainShellFactory = mainShellFactory;
      this._browserDispatcher = browserDispatcher;
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider;
      this._launcherTrackingService = launcherTrackingService;
      this._sessionEndedMessageHandler = sessionEndedMessageHandler;
      this._windowsStorageManager = windowsStorageManager;
      launcherStateService.RegionUpdating += new EventHandler(this.OnLauncherRegionUpdating);
    }

    public void RegisterMainWindow(IMainShellPresenter window) => this._windows["main"] = new Lazy<IMainShellPresenter>((Func<IMainShellPresenter>) (() => window));

    public void CloseExceptMain() => this.GuiNoWait((Action) (() =>
    {
      foreach (KeyValuePair<string, Lazy<IMainShellPresenter>> keyValuePair in this._windows.Where<KeyValuePair<string, Lazy<IMainShellPresenter>>>((Func<KeyValuePair<string, Lazy<IMainShellPresenter>>, bool>) (v => v.Key != "main")))
      {
        try
        {
          if (keyValuePair.Value.IsValueCreated)
            keyValuePair.Value.Value.HideAndClose();
        }
        catch
        {
        }
      }
      this._browserDispatcher.CloseAll();
    }));

    public void MaximizeOrNormalize(string windowId) => this.Gui((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.MaximizeOrNormalize();
    }));

    public void Minimize(string windowId) => this.Gui((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.Minimize();
    }));

    public void Normalize(string windowId) => this.Gui((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.Normalize();
    }));

    public void Reload(string windowId) => this.GuiNoWait((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (windowId != "main" || !this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.ViewModel.Reload();
    }));

    public void FocusWindow(string windowId) => this.Gui((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.Rise();
    }));

    public void HideToTrayWindow(string windowId) => this.GuiNoWait((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      lazy.Value.HideToTray();
    }));

    public void HideToTrayAllWindows() => this.GuiNoWait((Action) (() =>
    {
      this._launcherTrackingService.SendLauncherHidedToTray();
      this._sessionEndedMessageHandler.SendSessionEndedMessage();
      foreach (KeyValuePair<string, Lazy<IMainShellPresenter>> keyValuePair in this._windows.ToArray())
        keyValuePair.Value.Value.HideToTray();
      this._browserDispatcher.HideToTrayAll();
    }));

    public void RiseWindow(string windowId)
    {
      if (windowId == "main")
        this.RiseAllWindows();
      else
        this.GuiNoWait((Action) (() =>
        {
          Lazy<IMainShellPresenter> lazy;
          if (!this._windows.TryGetValue(windowId, out lazy))
            return;
          lazy.Value.Rise();
        }));
    }

    public void RiseAllWindows()
    {
      this._launcherTrackingService.SendLauncherRestoredFromTray();
      this.GuiNoWait((Action) (() =>
      {
        this._browserDispatcher.RiseAll();
        foreach (KeyValuePair<string, Lazy<IMainShellPresenter>> keyValuePair in this._windows.ToArray())
          keyValuePair.Value.Value.Rise();
      }));
    }

    public void CloseWindow(string windowId, bool force = false)
    {
      if (!force && windowId == "main")
      {
        this.HideToTrayAllWindows();
      }
      else
      {
        if (force && windowId == "main")
        {
          this.CloseExceptMain();
          this._sessionEndedMessageHandler.SendSessionEndedMessage();
        }
        this.GuiNoWait((Action) (() =>
        {
          Lazy<IMainShellPresenter> lazy;
          if (!this._windows.TryGetValue(windowId, out lazy))
            return;
          try
          {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            lazy.Value.HideAndClose();
          }
          finally
          {
            Mouse.OverrideCursor = (System.Windows.Input.Cursor) null;
          }
        }));
      }
    }

    public void OpenUrlInMainWindow(string url) => this.GuiNoWait((Action) (() =>
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue("main", out lazy))
        return;
      this._logger.Trace("Opening url: {url}", url);
      lazy.Value.GoToUrl(url);
    }));

    public void OpenWindow(string parentWindowId, OpenWindowData windowData) => this.GuiNoWait((Action) (() =>
    {
      string windowId = windowData.WindowId;
      string url = windowData.Url;
      IMainShellPresenter mainShellPresenter1 = this._windows.GetOrAdd(windowId, (Func<string, Lazy<IMainShellPresenter>>) (key => new Lazy<IMainShellPresenter>((Func<IMainShellPresenter>) (() =>
      {
        IMainShellPresenter mainShellPresenter2 = this._mainShellFactory.Create(key);
        mainShellPresenter2.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        mainShellPresenter2.Closed += (EventHandler) ((_param1, _param2) =>
        {
          this._windows.TryRemove(key, out Lazy<IMainShellPresenter> _);
          this._outputMessageDispatcherProvider.Remove(key);
          this._outputMessageDispatcherProvider.Get(parentWindowId).Dispatch(new LauncherMessage("closed", (object) new
          {
            WindowId = key
          }));
        });
        this._outputMessageDispatcherProvider.Get(key).SetWindowId(key);
        return mainShellPresenter2;
      })))).Value;
      if (!string.IsNullOrEmpty(url) && !mainShellPresenter1.IsLoaded)
        mainShellPresenter1.GoToUrl(url);
      if (windowData.CanResize)
      {
        IMainShellPresenter mainShellPresenter3 = mainShellPresenter1;
        double? width = windowData.Width;
        double? height = windowData.Height;
        double? nullable = windowData.MinWidth;
        double? minWidth = new double?(nullable ?? this._defaultMinWidth);
        nullable = windowData.MinHeight;
        double? minHeight = new double?(nullable ?? this._defaultMinHeight);
        mainShellPresenter3.MakeResizable(width, height, minWidth, minHeight);
        if (windowData.IsMaximized.HasValue && windowData.IsMaximized.Value)
          mainShellPresenter1.Maximize();
      }
      else
      {
        mainShellPresenter1.MakeNoResizable(windowData.Width, windowData.Height);
        mainShellPresenter1.Normalize();
      }
      mainShellPresenter1.Rise();
    }));

    public string OpenFolderSelectDialog(string prePickedPath)
    {
      string result = (string) null;
      this.Gui((Action) (() =>
      {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
        {
          SelectedPath = prePickedPath,
          RootFolder = System.Environment.SpecialFolder.MyComputer
        };
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
          result = folderBrowserDialog.SelectedPath;
        ((Component) folderBrowserDialog).Dispose();
      }));
      return result;
    }

    public bool IsMainWindowVisible()
    {
      Lazy<IMainShellPresenter> lazy;
      return this._windows.TryGetValue("main", out lazy) && lazy.Value.IsVisible;
    }

    public bool ShouldWindowHideOnClose(string windowId) => windowId == "main";

    public void SaveWindowLocalStorage(string windowId)
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      WindowStorage windowStorage = lazy.Value.GetWindowStorage();
      this._windowsStorageManager.Save(windowId, windowStorage);
    }

    public void RestoreWindowLocalStorage(string windowId, bool cleanAfterRestore)
    {
      Lazy<IMainShellPresenter> lazy;
      if (!this._windows.TryGetValue(windowId, out lazy))
        return;
      WindowStorage storage = this._windowsStorageManager.Get(windowId);
      if (storage == null)
        return;
      lazy.Value.RestoreWindowStorage(storage);
      if (!cleanAfterRestore)
        return;
      this._windowsStorageManager.Save(windowId, (WindowStorage) null);
    }

    public WindowSize GetMainWindowSize()
    {
      WindowSize result = new WindowSize();
      Lazy<IMainShellPresenter> lazy;
      if (this._windows.TryGetValue("main", out lazy))
      {
        Window shellWindow = (Window) lazy.Value;
        ((object) shellWindow.Dispatcher).Gui((Action) (() =>
        {
          result.Width = shellWindow.ActualWidth;
          result.Height = shellWindow.ActualHeight;
          result.IsMaximized = shellWindow.WindowState == WindowState.Maximized;
        }));
      }
      return result;
    }

    public List<WindowInfo> GetWindowsInfo()
    {
      List<WindowInfo> windowsInfo = new List<WindowInfo>();
      foreach (KeyValuePair<string, Lazy<IMainShellPresenter>> window in this._windows)
      {
        WindowInfo windowInfo = new WindowInfo()
        {
          Id = window.Key
        };
        Window openedWindow = (Window) window.Value.Value;
        ((object) openedWindow).Gui((Action) (() =>
        {
          windowInfo.IsMinimized = openedWindow.WindowState == WindowState.Minimized;
          windowInfo.IsActive = openedWindow.IsActive;
          windowInfo.Title = openedWindow.Title;
        }));
        windowsInfo.Add(windowInfo);
      }
      return windowsInfo;
    }

    private void OnLauncherRegionUpdating(object sender, EventArgs e) => this.SaveWindowLocalStorage("main");
  }
}
