// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.BackendApi
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Localization.Helpers;
using Innova.Launcher.UI.Extensions;
using Newtonsoft.Json;
using System;
using Unity;

namespace Innova.Launcher.Services
{
  public class BackendApi : IBackendApi
  {
    private IMainShellPresenter _shell;

    [Dependency]
    public BrowserInputMessagesQueue InputMessages { get; set; }

    [Dependency]
    public IWindowsService DialogService { get; set; }

    public void InitShell(IMainShellPresenter shell) => this._shell = shell;

    private void ThrowIfNotInited()
    {
      if (this._shell == null || this.DialogService == null)
        throw new InvalidOperationException("Browser not inited");
    }

    public void send(string message)
    {
      this.ThrowIfNotInited();
      WebMessage webMessage = JsonConvert.DeserializeObject<WebMessage>(message);
      webMessage.WindowId = this._shell.ViewModel.Id;
      this.InputMessages.Add(webMessage);
    }

    public string getCulture() => LocalizationHelper.CurrentCulture.Name;

    public void dragMove()
    {
      this.ThrowIfNotInited();
      this.Gui((Action) (() => this._shell.StartNativeDrag()));
    }

    public void close()
    {
      this.ThrowIfNotInited();
      this.DialogService.CloseWindow(this._shell.ViewModel.Id);
    }

    public void maximize()
    {
      this.ThrowIfNotInited();
      this.DialogService.MaximizeOrNormalize(this._shell.ViewModel.Id);
    }

    public void minimize()
    {
      this.ThrowIfNotInited();
      this.DialogService.Minimize(this._shell.ViewModel.Id);
    }
  }
}
