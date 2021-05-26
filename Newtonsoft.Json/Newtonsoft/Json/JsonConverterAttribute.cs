// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonConverterAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to use the specified <see cref="T:Newtonsoft.Json.JsonConverter" /> when serializing the member or class.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonConverterAttribute : Attribute
  {
    private readonly Type _converterType;

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.JsonConverter" />.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.JsonConverter" />.</value>
    public Type ConverterType => this._converterType;

    /// <summary>
    /// The parameter list to use when constructing the <see cref="T:Newtonsoft.Json.JsonConverter" /> described by <see cref="P:Newtonsoft.Json.JsonConverterAttribute.ConverterType" />.
    /// If <c>null</c>, the default constructor is used.
    /// </summary>
    public object[]? ConverterParameters { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonConverterAttribute" /> class.
    /// </summary>
    /// <param name="converterType">Type of the <see cref="T:Newtonsoft.Json.JsonConverter" />.</param>
    public JsonConverterAttribute(Type converterType) => this._converterType = !(converterType == (Type) null) ? converterType : throw new ArgumentNullException(nameof (converterType));

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonConverterAttribute" /> class.
    /// </summary>
    /// <param name="converterType">Type of the <see cref="T:Newtonsoft.Json.JsonConverter" />.</param>
    /// <param name="converterParameters">Parameter list to use when constructing the <see cref="T:Newtonsoft.Json.JsonConverter" />. Can be <c>null</c>.</param>
    public JsonConverterAttribute(Type converterType, params object[] converterParameters)
      : this(converterType)
    {
      this.ConverterParameters = converterParameters;
    }
  }
}
