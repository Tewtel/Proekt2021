﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.EntityKeyMemberConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts an Entity Framework <see cref="T:System.Data.EntityKeyMember" /> to and from JSON.
  /// </summary>
  public class EntityKeyMemberConverter : JsonConverter
  {
    private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";
    private const string KeyPropertyName = "Key";
    private const string TypePropertyName = "Type";
    private const string ValuePropertyName = "Value";
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
        EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
        DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
        string str = (string) EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
        object obj = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
        Type type = obj?.GetType();
        writer.WriteStartObject();
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Key") : "Key");
        writer.WriteValue(str);
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Type") : "Type");
        writer.WriteValue(type?.FullName);
        writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName("Value") : "Value");
        if (type != (Type) null)
        {
          string s;
          if (JsonSerializerInternalWriter.TryConvertToString(obj, type, out s))
            writer.WriteValue(s);
          else
            writer.WriteValue(obj);
        }
        else
          writer.WriteNull();
        writer.WriteEndObject();
      }
    }

    private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
    {
      reader.ReadAndAssert();
      if (reader.TokenType != JsonToken.PropertyName || !string.Equals(reader.Value?.ToString(), propertyName, StringComparison.OrdinalIgnoreCase))
        throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) propertyName));
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
      EntityKeyMemberConverter.EnsureReflectionObject(objectType);
      object target = EntityKeyMemberConverter._reflectionObject.Creator(new object[0]);
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
      reader.ReadAndAssert();
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Key", (object) reader.Value?.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
      reader.ReadAndAssert();
      Type type = Type.GetType(reader.Value?.ToString());
      EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
      reader.ReadAndAssert();
      EntityKeyMemberConverter._reflectionObject.SetValue(target, "Value", serializer.Deserialize(reader, type));
      reader.ReadAndAssert();
      return target;
    }

    private static void EnsureReflectionObject(Type objectType)
    {
      if (EntityKeyMemberConverter._reflectionObject != null)
        return;
      EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, "Key", "Value");
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType) => objectType.AssignableToTypeName("System.Data.EntityKeyMember", false);
  }
}
