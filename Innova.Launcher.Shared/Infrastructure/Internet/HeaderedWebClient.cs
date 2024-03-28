// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.Internet.HeaderedWebClient
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System.Net;

namespace Innova.Launcher.Shared.Infrastructure.Internet
{
  public class HeaderedWebClient : WebClient
  {
    public HeaderedWebClient()
    {
      WebHeaderCollection headerCollection = new WebHeaderCollection();
      headerCollection["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.119 Safari/537.36";
      headerCollection["Accept"] = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
      headerCollection["Accept-Encoding"] = "gzip, deflate";
      headerCollection["Accept-Language"] = "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7";
      this.Headers = headerCollection;
    }
  }
}
