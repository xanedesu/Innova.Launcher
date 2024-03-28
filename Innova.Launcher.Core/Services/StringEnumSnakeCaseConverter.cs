// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.StringEnumSnakeCaseConverter
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Innova.Launcher.Core.Services
{
  public class StringEnumSnakeCaseConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        string str = string.Concat(((Enum) value).ToString("G").Select<char, string>((Func<char, int, string>) ((x, i) =>
        {
          if (!char.IsUpper(x))
            return x.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          return i != 0 ? "_" + x.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture) : x.ToString((IFormatProvider) CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture);
        })));
        writer.WriteValue(str);
      }
    }

    public virtual object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        if (!StringEnumSnakeCaseConverter.IsNullableType(objectType))
          throw new SerializationException(string.Format("Cannot convert null value to {0}.", (object) objectType));
        return (object) null;
      }
      Type enumType = StringEnumSnakeCaseConverter.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      try
      {
        if (reader.TokenType == JsonToken.String)
        {
          string str1 = reader.Value.ToString();
          StringBuilder stringBuilder = new StringBuilder();
          foreach (string str2 in str1.Split('_'))
          {
            string lowerInvariant = str2.Trim('_').ToLowerInvariant();
            if (lowerInvariant.Length > 0)
            {
              string upperInvariant = lowerInvariant[0].ToString().ToUpperInvariant();
              stringBuilder.Append(upperInvariant);
              stringBuilder.Append(lowerInvariant.Substring(1));
            }
          }
          string str3 = stringBuilder.ToString();
          return Enum.Parse(enumType, str3);
        }
      }
      catch (Exception ex)
      {
        throw new SerializationException(string.Format("Error converting value {0} to type '{1}'.", reader.Value, (object) objectType), ex);
      }
      throw new SerializationException(string.Format("Unexpected token {0} when parsing enum.", (object) reader.TokenType));
    }

    public virtual bool CanConvert(Type objectType)
    {
      if (StringEnumSnakeCaseConverter.IsNullableType(objectType))
        objectType = Nullable.GetUnderlyingType(objectType);
      return objectType != (Type) null && objectType.IsEnum;
    }

    private static bool IsNullableType(Type objectType) => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof (Nullable<>);
  }
}
