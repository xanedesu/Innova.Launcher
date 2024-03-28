// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.ApplicationUrlLoadService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.Shared.Localization.Helpers;
using Innova.Launcher.Shared.Utils;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.ViewModels;
using Innova.Launcher.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Innova.Launcher.Services
{
  public class ApplicationUrlLoadService : IApplicationUrlLoadService
  {
    private readonly HashSet<string> _httpSchemes = new HashSet<string>()
    {
      "http",
      "https",
      "about",
      "default",
      "browser"
    };

    public OpenApplicationResult Handle(string url)
    {
      string applicationName = this.GetApplicationNameByUrl(url);
      if (applicationName == null)
        return OpenApplicationResult.Ignore;
      if (!this.Gui<bool>((Func<bool>) (() =>
      {
        bool? nullable = new DialogWindow(new DialogViewModel()
        {
          Message = Localize.It("Dialog.ApplicationUrlOpen") + " \"" + applicationName + "\"?"
        }).ShowDialog();
        bool flag = true;
        return nullable.GetValueOrDefault() == flag & nullable.HasValue;
      })))
        return OpenApplicationResult.Cancel;
      Process.Start(new ProcessStartInfo()
      {
        FileName = url,
        UseShellExecute = true
      });
      return OpenApplicationResult.Ok;
    }

    private string GetApplicationNameByUrl(string url)
    {
      Uri result;
      if (!Uri.TryCreate(url, UriKind.Absolute, out result))
        return (string) null;
      if (!this._httpSchemes.Contains(result.Scheme))
      {
        try
        {
          UrlSchemeInfo urlSchemeInfo = RegistryHelper.GetUrlSchemeInfo(result.Scheme);
          if (urlSchemeInfo != null)
            return urlSchemeInfo.ApplicationName;
        }
        catch
        {
          return (string) null;
        }
      }
      return (string) null;
    }
  }
}
