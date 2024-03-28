// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Converters.HtmlDecodeJsonConverter
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Newtonsoft.Json;
using System;
using System.Net;

namespace Innova.Launcher.Converters
{
  public sealed class HtmlDecodeJsonConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        string str = value.ToString();
        writer.WriteValue(WebUtility.HtmlEncode(str));
      }
    }

    public virtual object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      return reader.TokenType == JsonToken.String ? (object) WebUtility.HtmlDecode(reader.Value.ToString()).Replace("<br/>", Environment.NewLine) : existingValue;
    }

    public virtual bool CanConvert(Type objectType) => objectType == typeof (string);
  }
}
