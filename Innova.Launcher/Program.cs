// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Program
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using CefSharp.WinForms;
using Innova.Launcher.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Innova.Launcher
{
  public static class Program
  {
    [STAThread]
    public static int Main(string[] args)
    {
      Environment.SetEnvironmentVariable("GOOGLE_API_KEY", "AIzaSyAxqD4FpTTuOVtGrR2pKqZ8T5aNUcryyhg");
      Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_ID", "127193556698-8t514cl4buuoae2emltiss5df210fvvd.apps.googleusercontent.com");
      Environment.SetEnvironmentVariable("GOOGLE_DEFAULT_CLIENT_SECRET", "s2Y5kQj7kwAFnAuw8HIhpCA3");
      Cef.EnableHighDPISupport();
      string cefCacheDirectory = Program.GetCEFCacheDirectory();
      bool flag = ((IEnumerable<string>) args).Contains<string>("/d");
      CefSettings cefSettings = new CefSettings();
      cefSettings.CachePath = Path.Combine(cefCacheDirectory, "global");
      cefSettings.RemoteDebuggingPort = flag ? 8088 : 0;
      cefSettings.RootCachePath = cefCacheDirectory;
      cefSettings.Locale = CefSharpLocalHelper.GetLocale();
      cefSettings.UserAgent = "zilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36";
      cefSettings.ExternalMessagePump = false;
      cefSettings.MultiThreadedMessageLoop = true;
      CefSettings settings = cefSettings;
      settings.CefCommandLineArgs.Add("enable-media-stream");
      settings.CefCommandLineArgs.Add("enable-webgl");
      settings.CefCommandLineArgs.Add("enable-webgl2-compute-context");
      settings.CefCommandLineArgs.Add("enable-3d-apis");
      settings.CefCommandLineArgs.Add("enable-webgl-draft-extensions");
      settings.CefCommandLineArgs.Add("enable-gpu");
      settings.CefCommandLineArgs.Add("enable-webgl-image-chromium");
      settings.CefCommandLineArgs.Add("enable-gpu-rasterization");
      int num = Array.IndexOf<string>(args, "/sw");
      if (num >= 0)
      {
        foreach ((string, string) tuple in ((IEnumerable<string>) File.ReadAllLines(args[num + 1])).Select<string, string[]>((Func<string, string[]>) (v => v.Split(' '))).Select<string[], (string, string)>((Func<string[], (string, string)>) (v => (v[0].TrimStart('-'), string.Join<string>(' ', ((IEnumerable<string>) v).Skip<string>(1).DefaultIfEmpty<string>(string.Empty))))))
          ((Dictionary<string, string>) settings.CefCommandLineArgs).Add(tuple.Item1, tuple.Item2);
      }
      Cef.Initialize((CefSettingsBase) settings, false, (IBrowserProcessHandler) null);
      App app = new App();
      app.InitializeComponent();
      return app.Run();
    }

    private static string GetCEFCacheDirectory()
    {
      try
      {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Innova", "4game", "CEF");
        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);
        return path;
      }
      catch (Exception ex)
      {
        return Path.GetFullPath("CEF");
      }
    }
  }
}
