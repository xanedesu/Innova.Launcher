// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Helpers.DotnetBrowserDateTimeHelper
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;

namespace Innova.Launcher.Helpers
{
  public static class DotnetBrowserDateTimeHelper
  {
    private static readonly DateTime _defaultFileDate = new DateTime(1601, 1, 1);

    public static DateTime FromMicroseconds(long value) => DotnetBrowserDateTimeHelper._defaultFileDate + TimeSpan.FromMilliseconds((double) (value / 1000L));
  }
}
