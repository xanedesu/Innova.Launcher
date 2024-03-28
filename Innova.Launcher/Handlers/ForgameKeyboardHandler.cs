// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Handlers.ForgameKeyboardHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Properties;
using Innova.Launcher.Services.Interfaces;
using System;
using System.Windows.Forms;

namespace Innova.Launcher.Handlers
{
  public sealed class ForgameKeyboardHandler : IKeyboardHandler
  {
    private readonly IWindowsService _windowsService;
    private readonly ISwitchGamesEnvironmentService _switchGamesEnvironmentService;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;
    private readonly string _id;

    public ForgameKeyboardHandler(
      IWindowsService windowsService,
      ISwitchGamesEnvironmentService switchGamesEnvironmentService,
      string id,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._windowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));
      this._switchGamesEnvironmentService = switchGamesEnvironmentService ?? throw new ArgumentNullException(nameof (switchGamesEnvironmentService));
      this._id = id ?? throw new ArgumentNullException(nameof (id));
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider ?? throw new ArgumentNullException(nameof (outputMessageDispatcherProvider));
    }

    public bool OnPreKeyEvent(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      KeyType type,
      int windowsKeyCode,
      int nativeKeyCode,
      CefEventFlags modifiers,
      bool isSystemKey,
      ref bool isKeyboardShortcut)
    {
      Keys keys = (Keys) windowsKeyCode;
      if (keys == Keys.Q && ((Enum) modifiers).HasFlag((Enum) CefEventFlags.AltDown))
      {
        this._outputMessageDispatcherProvider.Get(this._id)?.Dispatch(new LauncherMessage("logout"));
        return true;
      }
      if (type != KeyType.KeyUp)
        return false;
      if (keys == Keys.R && ((Enum) modifiers).HasFlag((Enum) CefEventFlags.ControlDown))
      {
        if (((Enum) modifiers).HasFlag((Enum) CefEventFlags.ShiftDown))
          browser.Reload(true);
        else
          browser.Reload();
        return true;
      }
      if (keys == Keys.F4 && ((Enum) modifiers).HasFlag((Enum) CefEventFlags.AltDown))
      {
        if (((Enum) modifiers).HasFlag((Enum) CefEventFlags.AltDown))
          this._windowsService.CloseWindow(this._id, true);
        else
          this._windowsService.CloseWindow(this._id);
        return true;
      }
      if (keys == Keys.Oemplus && ((Enum) modifiers).HasFlag((Enum) CefEventFlags.ControlDown))
      {
        double zoomLevel = this.GetZoomLevel(browser);
        this.SetZoomLevel(browser, zoomLevel + 0.2);
        return true;
      }
      if (keys == Keys.OemMinus && ((Enum) modifiers).HasFlag((Enum) CefEventFlags.ControlDown))
      {
        double zoomLevel = this.GetZoomLevel(browser);
        this.SetZoomLevel(browser, zoomLevel * 0.7);
        return true;
      }
      if (keys != Keys.E || !((Enum) modifiers).HasFlag((Enum) CefEventFlags.ControlDown) || !((Enum) modifiers).HasFlag((Enum) CefEventFlags.ShiftDown))
        return false;
      this._switchGamesEnvironmentService.SwitchEnvironment();
      return true;
    }

    public bool OnKeyEvent(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      KeyType type,
      int windowsKeyCode,
      int nativeKeyCode,
      CefEventFlags modifiers,
      bool isSystemKey)
    {
      return false;
    }

    private double GetZoomLevel(IBrowser browser)
    {
      double result = WebBrowserExtensions.GetZoomLevelAsync(browser).GetAwaiter().GetResult();
      return result >= double.Epsilon ? result : 1.0;
    }

    private void SetZoomLevel(IBrowser browser, double zoom)
    {
      zoom = Math.Max(0.001, Math.Min(2.0, zoom));
      browser.SetZoomLevel(zoom);
      Settings.Default.ZoomLevel = zoom;
      Settings.Default.Save();
    }
  }
}
