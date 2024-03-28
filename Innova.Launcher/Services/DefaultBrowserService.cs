// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.DefaultBrowserService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Services.Interfaces;
using System.Diagnostics;

namespace Innova.Launcher.Services
{
  public sealed class DefaultBrowserService : IDefaultBrowserService
  {
    private const string _defaultSchema = "default://";
    private const string _httpsSchema = "https://";

    public bool CanHandle(string url) => DefaultBrowserService.IsDefaultUrl(url);

    public void Start(string url)
    {
      url = DefaultBrowserService.CleanUrl(url);
      Process.Start(new ProcessStartInfo()
      {
        FileName = url,
        UseShellExecute = true
      });
    }

    private static bool IsDefaultUrl(string url) => url.StartsWith("default://");

    private static string CleanUrl(string url)
    {
      string str = DefaultBrowserService.IsDefaultUrl(url) ? url.Substring("default://".Length) : url;
      return (str.StartsWith("http") ? string.Empty : "https://") + str;
    }
  }
}
