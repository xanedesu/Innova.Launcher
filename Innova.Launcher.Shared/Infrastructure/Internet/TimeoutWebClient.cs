// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.Internet.TimeoutWebClient
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Net;

namespace Innova.Launcher.Shared.Infrastructure.Internet
{
  public class TimeoutWebClient : HeaderedWebClient
  {
    public int Timeout { get; }

    public TimeoutWebClient()
      : this(60000)
    {
    }

    public TimeoutWebClient(int timeout) => this.Timeout = timeout;

    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest webRequest = base.GetWebRequest(address);
      if (webRequest != null)
        webRequest.Timeout = this.Timeout;
      return webRequest;
    }
  }
}
