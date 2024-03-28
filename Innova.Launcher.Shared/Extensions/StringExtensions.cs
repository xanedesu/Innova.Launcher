// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Extensions.StringExtensions
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Innova.Launcher.Shared.Extensions
{
  public static class StringExtensions
  {
    public static string RemoveLeadingSymbols(this string str, char[] symbols)
    {
      if (str == null)
        return (string) null;
      foreach (char symbol in symbols)
      {
        if (str.Length > 0 && (int) str[0] == (int) symbol)
          str = str.Substring(1);
      }
      return str;
    }

    public static string RemoveTrailingSymbols(this string str, char[] symbols)
    {
      if (str == null)
        return (string) null;
      foreach (char symbol in symbols)
      {
        if (str.Length > 0 && (int) str[str.Length - 1] == (int) symbol)
          str = str.Substring(0, str.Length - 1);
      }
      return str;
    }

    public static string RemoveLeadingSlash(this string url) => url.RemoveLeadingChar('/');

    public static string RemoveTrailingSlash(this string url) => url.RemoveTrailingChar('/');

    public static string EnsureLeadingSlash(this string url)
    {
      if (string.IsNullOrEmpty(url))
        return "/";
      return url[0] != '/' ? "/" + url : url;
    }

    public static string EnsureTrailingSlash(this string url) => url.EnsureTrailingChar('/');

    public static string EnsureTrailingChar(this string url, char symbol) => string.IsNullOrEmpty(url) || url.Length > 0 && (int) url[url.Length - 1] == (int) symbol ? url : url + symbol.ToString();

    public static string RemoveLast(this string value, int length) => value.Substring(0, value.Length - length);

    public static string RemoveLast(this string value, string remove) => value.EndsWith(remove) ? value.Substring(0, value.Length - remove.Length) : value;

    public static string EscapeCmdInQuotesPart(this string part) => part == null ? (string) null : part.Replace("\"", "\\\"").RemoveTrailingSlash();

    public static string RemoveLeadingChar(this string url, char chr)
    {
      if (!string.IsNullOrEmpty(url) && (int) url[0] == (int) chr)
        url = url.Substring(1);
      return url;
    }

    public static string RemoveTrailingChar(this string url, char chr)
    {
      if (!string.IsNullOrEmpty(url) && (int) url[url.Length - 1] == (int) chr)
        url = url.Substring(0, url.Length - 1);
      return url;
    }

    public static string SubstringOrSelf(this string str, int startIndex, int length) => str?.Substring(startIndex, Math.Min(length, str.Length));

    public static string ReplaceNewLineWith(this string str, string replace) => string.Join(replace, Regex.Split(str, "\\r?\\n|\\r"));

    public static string AddOrUpdateParameterToUrl(this string url, string name, string value)
    {
      try
      {
        UriBuilder uriBuilder = new UriBuilder(url);
        string str1 = uriBuilder.Query.RemoveLeadingChar('?');
        StringBuilder stringBuilder = new StringBuilder();
        bool flag = false;
        int num = 0;
        if (!string.IsNullOrWhiteSpace(str1))
        {
          foreach (string str2 in str1.Split('&'))
          {
            string str3;
            if ((str3 = str2).StartsWith(name + "="))
            {
              flag = true;
              if (value != null)
                str3 = name + "=" + Uri.EscapeUriString(value);
              else
                continue;
            }
            if (num > 0)
              stringBuilder.Append("&");
            stringBuilder.Append(str3);
            ++num;
          }
        }
        if (!flag && value != null)
        {
          if (num > 0)
            stringBuilder.Append("&");
          stringBuilder.Append(name + "=" + Uri.EscapeUriString(value));
        }
        uriBuilder.Query = stringBuilder.ToString();
        return uriBuilder.Uri.ToString();
      }
      catch (UriFormatException ex)
      {
        throw new Exception("StartPage config parameter had bad format", (Exception) ex);
      }
    }
  }
}
