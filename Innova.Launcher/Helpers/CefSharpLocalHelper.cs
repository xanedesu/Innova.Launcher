// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Helpers.CefSharpLocalHelper
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System.IO;
using System.Threading;

namespace Innova.Launcher.Helpers
{
  public static class CefSharpLocalHelper
  {
    public static string GetLocale()
    {
      string locale = Thread.CurrentThread.CurrentUICulture.Parent.Name.ToLowerInvariant();
      if (!File.Exists(Path.Combine(Path.GetFullPath("locales"), locale + ".pak")))
        locale = "ru";
      return locale;
    }
  }
}
