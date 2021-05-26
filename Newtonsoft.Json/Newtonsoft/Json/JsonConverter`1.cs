// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverter`1
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>Converts an object to and from JSON.</summary>
  /// <typeparam name="T">The object type to convert.</typeparam>
  public abstract class JsonConverter<T> : JsonConverter
  {
    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override sealed void WriteJson(
      JsonWriter writer,
      object? value,
      JsonSerializer serializer)
    {
      if ((value != null ? (value is T ? 1 : 0) : (ReflectionUtils.IsNullable(typeof (T)) ? 1 : 0)) == 0)
        throw new JsonSerializationException("Converter cannot write specified value to JSON. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
      this.WriteJson(writer, (T) value, serializer);
    }

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public abstract void WriteJson(JsonWriter writer, [AllowNull] T value, JsonSerializer serializer);

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override sealed object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      bool flag = existingValue == null;
      if (!flag && !(existingValue is T))
        throw new JsonSerializationException("Converter cannot read JSON with the specified existing value. {0} is required.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (T)));
      return (object) this.ReadJson(reader, objectType, flag ? default (T) : (T) existingValue, !flag, serializer);
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
    /// <param name="hasExistingValue">The existing value has a value.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public abstract T ReadJson(
      JsonReader reader,
      Type objectType,
      [AllowNull] T existingValue,
      bool hasExistingValue,
      JsonSerializer serializer);

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override sealed bool CanConvert(Type objectType) => typeof (T).IsAssignableFrom(objectType);
  }
}
