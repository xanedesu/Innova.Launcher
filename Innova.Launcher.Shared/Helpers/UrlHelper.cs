// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Helpers.UrlHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.IO;
using System.Web;

namespace Innova.Launcher.Shared.Helpers
{
  public static class UrlHelper
  {
    public static bool IsAbsoluteUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out Uri _);

    public static string ConcatPathWithParams(string url, string path)
    {
      UriBuilder uriBuilder1 = new UriBuilder(url);
      string path2 = HttpUtility.UrlDecode(path);
      int num1 = path2.IndexOf('?');
      if (num1 != -1)
      {
        uriBuilder1.Path = Path.Combine(uriBuilder1.Path, path2.Substring(0, num1 - 0));
        UriBuilder uriBuilder2 = uriBuilder1;
        string query = uriBuilder2.Query;
        string str1;
        if (uriBuilder1.Query.Length <= 0)
        {
          string str2 = path2;
          int length1 = str2.Length;
          int startIndex = num1;
          int num2 = startIndex;
          int length2 = length1 - num2;
          str1 = str2.Substring(startIndex, length2);
        }
        else
        {
          string str3 = path2;
          int length3 = str3.Length;
          int startIndex = num1;
          int num3 = startIndex;
          int length4 = length3 - num3;
          str1 = str3.Substring(startIndex, length4).Replace('?', '&');
        }
        uriBuilder2.Query = query + str1;
      }
      else
        uriBuilder1.Path = Path.Combine(uriBuilder1.Path, path2);
      return uriBuilder1.ToString();
    }
  }
}
