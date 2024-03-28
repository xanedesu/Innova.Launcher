// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Extensions.LauncherVersionExtentions
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Common;
using Innova.Launcher.Core.Models;

namespace Innova.Launcher.Core.Extensions
{
  public static class LauncherVersionExtentions
  {
    public static bool IsGreaterThen(
      this LauncherVersionInfo version1,
      LauncherVersionInfo version2)
    {
      return version1.IsGreaterThen(version2.Version);
    }

    public static bool IsGreaterThen(this LauncherVersionInfo version1, string version2) => new VersionComparer().Compare(version1.Version, version2) > 0;

    public static bool IsLessThen(this LauncherVersionInfo version1, LauncherVersionInfo version2) => version1.IsLessThen(version2.Version);

    public static bool IsLessThen(this LauncherVersionInfo version1, string version2) => new VersionComparer().Compare(version1.Version, version2) < 0;
  }
}
