﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.BinaryConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a binary value to and from a base 64 string value.
  /// </summary>
  public class BinaryConverter : JsonConverter
  {
    private const string BinaryTypeName = "System.Data.Linq.Binary";
    private const string BinaryToArrayName = "ToArray";
    private static ReflectionObject? _reflectionObject;

    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        byte[] byteArray = this.GetByteArray(value);
        writer.WriteValue(byteArray);
      }
    }

    private byte[] GetByteArray(object value)
    {
      if (value.GetType().FullName == "System.Data.Linq.Binary")
      {
        BinaryConverter.EnsureReflectionObject(value.GetType());
        return (byte[]) BinaryConverter._reflectionObject.GetValue(value, "ToArray");
      }
      return value is SqlBinary sqlBinary ? sqlBinary.Value : throw new JsonSerializationException("Unexpected value type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    private static void EnsureReflectionObject(Type t)
    {
      if (BinaryConverter._reflectionObject != null)
        return;
      BinaryConverter._reflectionObject = ReflectionObject.Create(t, (MethodBase) t.GetConstructor(new Type[1]
      {
        typeof (byte[])
      }), "ToArray");
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
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
      byte[] numArray;
      if (reader.TokenType == JsonToken.StartArray)
        numArray = this.ReadByteArray(reader);
      else
        numArray = reader.TokenType == JsonToken.String ? Convert.FromBase64String(reader.Value.ToString()) : throw JsonSerializationException.Create(reader, "Unexpected token parsing binary. Expected String or StartArray, got {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      Type t = ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType;
      if (t.FullName == "System.Data.Linq.Binary")
      {
        BinaryConverter.EnsureReflectionObject(t);
        return BinaryConverter._reflectionObject.Creator(new object[1]
        {
          (object) numArray
        });
      }
      if (t == typeof (SqlBinary))
        return (object) new SqlBinary(numArray);
      throw JsonSerializationException.Create(reader, "Unexpected object type when writing binary: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
    }

    private byte[] ReadByteArray(JsonReader reader)
    {
      List<byte> byteList = new List<byte>();
      while (reader.Read())
      {
        switch (reader.TokenType)
        {
          case JsonToken.Comment:
            continue;
          case JsonToken.Integer:
            byteList.Add(Convert.ToByte(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            continue;
          case JsonToken.EndArray:
            return byteList.ToArray();
          default:
            throw JsonSerializationException.Create(reader, "Unexpected token when reading bytes: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
        }
      }
      throw JsonSerializationException.Create(reader, "Unexpected end when reading bytes.");
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType) => objectType.FullName == "System.Data.Linq.Binary" || objectType == typeof (SqlBinary) || objectType == typeof (SqlBinary?);
  }
}
