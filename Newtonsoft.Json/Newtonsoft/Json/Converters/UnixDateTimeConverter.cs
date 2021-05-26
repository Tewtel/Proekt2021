// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.UnixDateTimeConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a <see cref="T:System.DateTime" /> to and from Unix epoch time
  /// </summary>
  public class UnixDateTimeConverter : DateTimeConverterBase
  {
    internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      long totalSeconds;
      switch (value)
      {
        case DateTime dateTime:
          totalSeconds = (long) (dateTime.ToUniversalTime() - UnixDateTimeConverter.UnixEpoch).TotalSeconds;
          break;
        case DateTimeOffset dateTimeOffset:
          totalSeconds = (long) (dateTimeOffset.ToUniversalTime() - (DateTimeOffset) UnixDateTimeConverter.UnixEpoch).TotalSeconds;
          break;
        default:
          throw new JsonSerializationException("Expected date object value.");
      }
      if (totalSeconds < 0L)
        throw new JsonSerializationException("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.");
      writer.WriteValue(totalSeconds);
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing property value of the JSON that is being converted.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      bool flag = ReflectionUtils.IsNullable(objectType);
      if (reader.TokenType == JsonToken.Null)
      {
        if (!flag)
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      long result;
      if (reader.TokenType == JsonToken.Integer)
      {
        result = (long) reader.Value;
      }
      else
      {
        if (reader.TokenType != JsonToken.String)
          throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer or String, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        if (!long.TryParse((string) reader.Value, out result))
          throw JsonSerializationException.Create(reader, "Cannot convert invalid value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      }
      DateTime dateTime = result >= 0L ? UnixDateTimeConverter.UnixEpoch.AddSeconds((double) result) : throw JsonSerializationException.Create(reader, "Cannot convert value that is before Unix epoch of 00:00:00 UTC on 1 January 1970 to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
      return (flag ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof (DateTimeOffset) ? (object) new DateTimeOffset(dateTime, TimeSpan.Zero) : (object) dateTime;
    }
  }
}
