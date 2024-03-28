// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.BrowserDispatcher
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Innova.Launcher.Services
{
  public class BrowserDispatcher : IBrowserDispatcher
  {
    private InternalBrowser _browser;

    public void Open(string url) => this.GuiNoWait((Action) (() =>
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        if (this._browser == null || !this._browser.IsLoaded)
        {
          BrowserViewModel instance = ServiceLocator.Current.GetInstance<BrowserViewModel>();
          this._browser = new InternalBrowser()
          {
            ViewModel = instance,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
          };
          this._browser.ViewModel.Open(this._browser.ViewModel.CreateTab(url));
          this._browser.Closing += (CancelEventHandler) ((_param1, _param2) => this._browser = (InternalBrowser) null);
          this._browser.Show();
        }
        else
        {
          this._browser.ViewModel.Open(url);
          this.ShowBrowser((Window) this._browser);
        }
      }
      finally
      {
        Mouse.OverrideCursor = (Cursor) null;
      }
    }));

    public void CloseAll() => this._browser?.Close();

    public void HideToTrayAll() => this._browser?.Hide();

    public void RiseAll()
    {
      if (this._browser == null)
        return;
      this.ShowBrowser((Window) this._browser);
    }

    private void ShowBrowser(Window browser)
    {
      if (!browser.IsVisible)
        browser.Show();
      if (browser.WindowState == WindowState.Minimized)
        browser.WindowState = WindowState.Normal;
      browser.Activate();
    }
  }
}
