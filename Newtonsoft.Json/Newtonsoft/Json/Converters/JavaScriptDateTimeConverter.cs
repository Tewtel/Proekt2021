// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.JavaScriptDateTimeConverter
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
  /// Converts a <see cref="T:System.DateTime" /> to and from a JavaScript <c>Date</c> constructor (e.g. <c>new Date(52231943)</c>).
  /// </summary>
  public class JavaScriptDateTimeConverter : DateTimeConverterBase
  {
    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      long javaScriptTicks;
      switch (value)
      {
        case DateTime dateTime:
          javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTime.ToUniversalTime());
          break;
        case DateTimeOffset dateTimeOffset:
          javaScriptTicks = DateTimeUtils.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.ToUniversalTime().UtcDateTime);
          break;
        default:
          throw new JsonSerializationException("Expected date object value.");
      }
      writer.WriteStartConstructor("Date");
      writer.WriteValue(javaScriptTicks);
      writer.WriteEndConstructor();
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
      if (reader.TokenType == JsonToken.Null)
      {
        if (!ReflectionUtils.IsNullable(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      if (reader.TokenType != JsonToken.StartConstructor || !string.Equals(reader.Value?.ToString(), "Date", StringComparison.Ordinal))
        throw JsonSerializationException.Create(reader, "Unexpected token or value when parsing date. Token: {0}, Value: {1}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType, reader.Value));
      DateTime dateTime;
      string errorMessage;
      if (!JavaScriptUtils.TryGetDateFromConstructorJson(reader, out dateTime, out errorMessage))
        throw JsonSerializationException.Create(reader, errorMessage);
      return (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof (DateTimeOffset) ? (object) new DateTimeOffset(dateTime) : (object) dateTime;
    }
  }
}
