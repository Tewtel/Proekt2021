// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.StringEnumConverter
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
  /// Converts an <see cref="T:System.Enum" /> to and from its name string value.
  /// </summary>
  public class StringEnumConverter : JsonConverter
  {
    /// <summary>
    /// Gets or sets a value indicating whether the written enum text should be camel case.
    /// The default value is <c>false</c>.
    /// </summary>
    /// <value><c>true</c> if the written enum text will be camel case; otherwise, <c>false</c>.</value>
    [Obsolete("StringEnumConverter.CamelCaseText is obsolete. Set StringEnumConverter.NamingStrategy with CamelCaseNamingStrategy instead.")]
    public bool CamelCaseText
    {
      get => this.NamingStrategy is CamelCaseNamingStrategy;
      set
      {
        if (value)
        {
          if (this.NamingStrategy is CamelCaseNamingStrategy)
            return;
          this.NamingStrategy = (NamingStrategy) new CamelCaseNamingStrategy();
        }
        else
        {
          if (!(this.NamingStrategy is CamelCaseNamingStrategy))
            return;
          this.NamingStrategy = (NamingStrategy) null;
        }
      }
    }

    /// <summary>
    /// Gets or sets the naming strategy used to resolve how enum text is written.
    /// </summary>
    /// <value>The naming strategy used to resolve how enum text is written.</value>
    public NamingStrategy? NamingStrategy { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether integer values are allowed when serializing and deserializing.
    /// The default value is <c>true</c>.
    /// </summary>
    /// <value><c>true</c> if integers are allowed when serializing and deserializing; otherwise, <c>false</c>.</value>
    public bool AllowIntegerValues { get; set; } = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    public StringEnumConverter()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    /// <param name="camelCaseText"><c>true</c> if the written enum text will be camel case; otherwise, <c>false</c>.</param>
    [Obsolete("StringEnumConverter(bool) is obsolete. Create a converter with StringEnumConverter(NamingStrategy, bool) instead.")]
    public StringEnumConverter(bool camelCaseText)
    {
      if (!camelCaseText)
        return;
      this.NamingStrategy = (NamingStrategy) new CamelCaseNamingStrategy();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    /// <param name="namingStrategy">The naming strategy used to resolve how enum text is written.</param>
    /// <param name="allowIntegerValues"><c>true</c> if integers are allowed when serializing and deserializing; otherwise, <c>false</c>.</param>
    public StringEnumConverter(NamingStrategy namingStrategy, bool allowIntegerValues = true)
    {
      this.NamingStrategy = namingStrategy;
      this.AllowIntegerValues = allowIntegerValues;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    /// <param name="namingStrategyType">The <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> used to write enum text.</param>
    public StringEnumConverter(Type namingStrategyType)
    {
      ValidationUtils.ArgumentNotNull((object) namingStrategyType, nameof (namingStrategyType));
      this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, (object[]) null);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    /// <param name="namingStrategyType">The <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> used to write enum text.</param>
    /// <param name="namingStrategyParameters">
    /// The parameter list to use when constructing the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> described by <paramref name="namingStrategyType" />.
    /// If <c>null</c>, the default constructor is used.
    /// When non-<c>null</c>, there must be a constructor defined in the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> that exactly matches the number,
    /// order, and type of these parameters.
    /// </param>
    public StringEnumConverter(Type namingStrategyType, object[] namingStrategyParameters)
    {
      ValidationUtils.ArgumentNotNull((object) namingStrategyType, nameof (namingStrategyType));
      this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Converters.StringEnumConverter" /> class.
    /// </summary>
    /// <param name="namingStrategyType">The <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> used to write enum text.</param>
    /// <param name="namingStrategyParameters">
    /// The parameter list to use when constructing the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> described by <paramref name="namingStrategyType" />.
    /// If <c>null</c>, the default constructor is used.
    /// When non-<c>null</c>, there must be a constructor defined in the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> that exactly matches the number,
    /// order, and type of these parameters.
    /// </param>
    /// <param name="allowIntegerValues"><c>true</c> if integers are allowed when serializing and deserializing; otherwise, <c>false</c>.</param>
    public StringEnumConverter(
      Type namingStrategyType,
      object[] namingStrategyParameters,
      bool allowIntegerValues)
    {
      ValidationUtils.ArgumentNotNull((object) namingStrategyType, nameof (namingStrategyType));
      this.NamingStrategy = JsonTypeReflector.CreateNamingStrategyInstance(namingStrategyType, namingStrategyParameters);
      this.AllowIntegerValues = allowIntegerValues;
    }

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
        Enum @enum = (Enum) value;
        string name;
        if (!EnumUtils.TryToString(@enum.GetType(), value, this.NamingStrategy, out name))
        {
          if (!this.AllowIntegerValues)
            throw JsonSerializationException.Create((IJsonLineInfo) null, writer.ContainerPath, "Integer value {0} is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) @enum.ToString("D")), (Exception) null);
          writer.WriteValue(value);
        }
        else
          writer.WriteValue(name);
      }
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
        if (!ReflectionUtils.IsNullableType(objectType))
          throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) objectType));
        return (object) null;
      }
      bool flag = ReflectionUtils.IsNullableType(objectType);
      Type type = flag ? Nullable.GetUnderlyingType(objectType) : objectType;
      try
      {
        if (reader.TokenType == JsonToken.String)
        {
          string str = reader.Value?.ToString();
          return StringUtils.IsNullOrEmpty(str) & flag ? (object) null : EnumUtils.ParseEnum(type, this.NamingStrategy, str, !this.AllowIntegerValues);
        }
        if (reader.TokenType == JsonToken.Integer)
        {
          if (!this.AllowIntegerValues)
            throw JsonSerializationException.Create(reader, "Integer value {0} is not allowed.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, reader.Value));
          return ConvertUtils.ConvertOrCast(reader.Value, CultureInfo.InvariantCulture, type);
        }
      }
      catch (Exception ex)
      {
        throw JsonSerializationException.Create(reader, "Error converting value {0} to type '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(reader.Value), (object) objectType), ex);
      }
      throw JsonSerializationException.Create(reader, "Unexpected token {0} when parsing enum.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType) => (ReflectionUtils.IsNullableType(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum();
  }
}
