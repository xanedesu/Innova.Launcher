// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.Interfaces.IScriptExecutorProvider
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;

namespace Innova.Launcher.Services.Interfaces
{
  public interface IScriptExecutorProvider
  {
    void Add(string windowId, ChromiumWebBrowser browser);

    IScriptExecutor Get(string windowId);
  }
}
