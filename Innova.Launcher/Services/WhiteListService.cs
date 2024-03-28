// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.WhiteListService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Core.Infrastructure.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Innova.Launcher.Services
{
  public sealed class WhiteListService : IWhiteListService
  {
    private readonly HashSet<Regex> _whiteList;

    public WhiteListService(
      ILauncherConfigurationProvider configurationProvider)
    {
      this._whiteList = new HashSet<Regex>(((IEnumerable<string>) configurationProvider.WhiteList.Split(',')).Select<string, Regex>((Func<string, Regex>) (v => new Regex(v, RegexOptions.Compiled))));
    }

    public bool InWhiteList(string url)
    {
      string filteredUrl = url.StartsWith("https://") ? url.Replace("https://", string.Empty) : url;
      filteredUrl = filteredUrl.StartsWith("http://") ? filteredUrl.Replace("http://", string.Empty) : filteredUrl;
      return this._whiteList.Any<Regex>((Func<Regex, bool>) (r => r.IsMatch(filteredUrl)));
    }
  }
}
