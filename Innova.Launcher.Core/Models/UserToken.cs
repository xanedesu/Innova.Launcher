// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.Token
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

namespace Innova.Launcher.Core.Models
{
  public class Token
  {
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public int ExpiresIn { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
  }
}
