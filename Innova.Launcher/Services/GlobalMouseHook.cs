// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.GlobalMouseHook
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Views;
using NLog;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.Services
{
  public class GlobalMouseHook : IGlobalMouseHook
  {
    private readonly ILogger _logger;
    private IntPtr _globalMouseHook;
    private NativeMethods.CBTProc _mouseHookHandlerRef;

    public GlobalMouseHook(ILifetimeManager lifetimeManager, ILoggerFactory loggerFactory)
    {
      if (lifetimeManager == null)
        throw new ArgumentNullException(nameof (lifetimeManager));
      lifetimeManager.TimeToDie += new EventHandler(this.LifetimeManagerTimeToDie);
      this._logger = loggerFactory.GetCurrentClassLogger<GlobalMouseHook>();
    }

    public void Setup()
    {
      try
      {
        this._mouseHookHandlerRef = new NativeMethods.CBTProc(this.GlobalMouseHookHandler);
        this._globalMouseHook = NativeMethods.SetWindowsHookEx(HookType.WH_MOUSE_LL, this._mouseHookHandlerRef, IntPtr.Zero, 0U);
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, "Error while trying to setup global mouse hook", Array.Empty<object>());
      }
    }

    private void LifetimeManagerTimeToDie(object sender, EventArgs ea)
    {
      try
      {
        if (!(this._globalMouseHook != IntPtr.Zero))
          return;
        NativeMethods.UnhookWindowsHookEx(this._globalMouseHook);
      }
      catch (Exception ex)
      {
        this._logger.Log(LogLevel.Error, ex, "Error while trying to release global mouse hook", Array.Empty<object>());
      }
    }

    private IntPtr GlobalMouseHookHandler(int code, IntPtr wParam, IntPtr lParam)
    {
      if ((int) wParam == 513)
      {
        PushNotificatorWindow notificatorWindow = ((IEnumerable) Application.Current.Windows).OfType<PushNotificatorWindow>().FirstOrDefault<PushNotificatorWindow>();
        if (notificatorWindow != null)
        {
          Button closeButton = notificatorWindow.PushNotification.CloseButton;
          Button primaryButton1 = notificatorWindow.PushNotification.PrimaryButton;
          Button primaryButton2 = notificatorWindow.PushNotification.PrimaryButton;
          if (closeButton.IsMouseOver)
          {
            closeButton.Command?.Execute((object) null);
            return IntPtr.Zero;
          }
          if (primaryButton1.IsMouseOver)
          {
            primaryButton1.Command?.Execute((object) null);
            closeButton.Command?.Execute((object) null);
          }
          else if (primaryButton2.IsMouseOver)
          {
            primaryButton2.Command?.Execute((object) null);
            closeButton.Command?.Execute((object) null);
          }
        }
      }
      return NativeMethods.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
    }
  }
}
