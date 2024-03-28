// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Handlers.ForgameContextMenuHandler
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;

namespace Innova.Launcher.Handlers
{
  public class ForgameContextMenuHandler : IContextMenuHandler
  {
    public void OnBeforeContextMenu(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      IContextMenuParams parameters,
      IMenuModel model)
    {
    }

    public bool OnContextMenuCommand(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      IContextMenuParams parameters,
      CefMenuCommand commandId,
      CefEventFlags eventFlags)
    {
      return true;
    }

    public void OnContextMenuDismissed(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame)
    {
    }

    public bool RunContextMenu(
      IWebBrowser chromiumWebBrowser,
      IBrowser browser,
      IFrame frame,
      IContextMenuParams parameters,
      IMenuModel model,
      IRunContextMenuCallback callback)
    {
      return true;
    }
  }
}
