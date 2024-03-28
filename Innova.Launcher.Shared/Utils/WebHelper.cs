// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.WebHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Infrastructure.Internet;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;

namespace Innova.Launcher.Shared.Utils
{
  public class WebHelper
  {
    public static T TryToLoadJsonData<T>(string configPath, int retryCount)
    {
      using (HeaderedWebClient headeredWebClient = new HeaderedWebClient())
      {
        int num = 0;
        while (num < retryCount)
        {
          ++num;
          try
          {
            return JsonConvert.DeserializeObject<T>(headeredWebClient.DownloadString(configPath));
          }
          catch (WebException ex)
          {
            if (num == retryCount)
              throw;
            else
              Thread.Sleep(300);
          }
        }
        throw new Exception(configPath + ". Not loaded.");
      }
    }
  }
}
