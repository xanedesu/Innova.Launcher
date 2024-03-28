// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.Interfaces.RegionUpdatedEventArgs
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using System;

namespace Innova.Launcher.Core.Services.Interfaces
{
  public class RegionUpdatedEventArgs : EventArgs
  {
    public string Region { get; }

    public string UrlAfterUpdate { get; }

    public string Culture { get; }

    public RegionUpdatedEventArgs(string region, string urlAfterUpdate, string culture)
    {
      this.Region = region ?? throw new ArgumentNullException(nameof (region));
      this.UrlAfterUpdate = urlAfterUpdate;
      this.Culture = culture;
    }
  }
}
