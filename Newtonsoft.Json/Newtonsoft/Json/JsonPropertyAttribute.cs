// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonPropertyAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to always serialize the member with the specified name.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
  public sealed class JsonPropertyAttribute : Attribute
  {
    internal NullValueHandling? _nullValueHandling;
    internal DefaultValueHandling? _defaultValueHandling;
    internal ReferenceLoopHandling? _referenceLoopHandling;
    internal ObjectCreationHandling? _objectCreationHandling;
    internal TypeNameHandling? _typeNameHandling;
    internal bool? _isReference;
    internal int? _order;
    internal Required? _required;
    internal bool? _itemIsReference;
    internal ReferenceLoopHandling? _itemReferenceLoopHandling;
    internal TypeNameHandling? _itemTypeNameHandling;

    /// <summary>
    /// Gets or sets the <see cref="T:Newtonsoft.Json.JsonConverter" /> type used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items <see cref="T:Newtonsoft.Json.JsonConverter" /> type.</value>
    public Type? ItemConverterType { get; set; }

    /// <summary>
    /// The parameter list to use when constructing the <see cref="T:Newtonsoft.Json.JsonConverter" /> described by <see cref="P:Newtonsoft.Json.JsonPropertyAttribute.ItemConverterType" />.
    /// If <c>null</c>, the default constructor is used.
    /// When non-<c>null</c>, there must be a constructor defined in the <see cref="T:Newtonsoft.Json.JsonConverter" /> that exactly matches the number,
    /// order, and type of these parameters.
    /// </summary>
    /// <example>
    /// <code>
    /// [JsonProperty(ItemConverterType = typeof(MyContainerConverter), ItemConverterParameters = new object[] { 123, "Four" })]
    /// </code>
    /// </example>
    public object[]? ItemConverterParameters { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" />.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" />.</value>
    public Type? NamingStrategyType { get; set; }

    /// <summary>
    /// The parameter list to use when constructing the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> described by <see cref="P:Newtonsoft.Json.JsonPropertyAttribute.NamingStrategyType" />.
    /// If <c>null</c>, the default constructor is used.
    /// When non-<c>null</c>, there must be a constructor defined in the <see cref="T:Newtonsoft.Json.Serialization.NamingStrategy" /> that exactly matches the number,
    /// order, and type of these parameters.
    /// </summary>
    /// <example>
    /// <code>
    /// [JsonProperty(NamingStrategyType = typeof(MyNamingStrategy), NamingStrategyParameters = new object[] { 123, "Four" })]
    /// </code>
    /// </example>
    public object[]? NamingStrategyParameters { get; set; }

    /// <summary>
    /// Gets or sets the null value handling used when serializing this property.
    /// </summary>
    /// <value>The null value handling.</value>
    public NullValueHandling NullValueHandling
    {
      get => this._nullValueHandling.GetValueOrDefault();
      set => this._nullValueHandling = new NullValueHandling?(value);
    }

    /// <summary>
    /// Gets or sets the default value handling used when serializing this property.
    /// </summary>
    /// <value>The default value handling.</value>
    public DefaultValueHandling DefaultValueHandling
    {
      get => this._defaultValueHandling.GetValueOrDefault();
      set => this._defaultValueHandling = new DefaultValueHandling?(value);
    }

    /// <summary>
    /// Gets or sets the reference loop handling used when serializing this property.
    /// </summary>
    /// <value>The reference loop handling.</value>
    public ReferenceLoopHandling ReferenceLoopHandling
    {
      get => this._referenceLoopHandling.GetValueOrDefault();
      set => this._referenceLoopHandling = new ReferenceLoopHandling?(value);
    }

    /// <summary>
    /// Gets or sets the object creation handling used when deserializing this property.
    /// </summary>
    /// <value>The object creation handling.</value>
    public ObjectCreationHandling ObjectCreationHandling
    {
      get => this._objectCreationHandling.GetValueOrDefault();
      set => this._objectCreationHandling = new ObjectCreationHandling?(value);
    }

    /// <summary>
    /// Gets or sets the type name handling used when serializing this property.
    /// </summary>
    /// <value>The type name handling.</value>
    public TypeNameHandling TypeNameHandling
    {
      get => this._typeNameHandling.GetValueOrDefault();
      set => this._typeNameHandling = new TypeNameHandling?(value);
    }

    /// <summary>
    /// Gets or sets whether this property's value is serialized as a reference.
    /// </summary>
    /// <value>Whether this property's value is serialized as a reference.</value>
    public bool IsReference
    {
      get => this._isReference.GetValueOrDefault();
      set => this._isReference = new bool?(value);
    }

    /// <summary>Gets or sets the order of serialization of a member.</summary>
    /// <value>The numeric order of serialization.</value>
    public int Order
    {
      get => this._order.GetValueOrDefault();
      set => this._order = new int?(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this property is required.
    /// </summary>
    /// <value>A value indicating whether this property is required.</value>
    public Required Required
    {
      get => this._required.GetValueOrDefault();
      set => this._required = new Required?(value);
    }

    /// <summary>Gets or sets the name of the property.</summary>
    /// <value>The name of the property.</value>
    public string? PropertyName { get; set; }

    /// <summary>
    /// Gets or sets the reference loop handling used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items reference loop handling.</value>
    public ReferenceLoopHandling ItemReferenceLoopHandling
    {
      get => this._itemReferenceLoopHandling.GetValueOrDefault();
      set => this._itemReferenceLoopHandling = new ReferenceLoopHandling?(value);
    }

    /// <summary>
    /// Gets or sets the type name handling used when serializing the property's collection items.
    /// </summary>
    /// <value>The collection's items type name handling.</value>
    public TypeNameHandling ItemTypeNameHandling
    {
      get => this._itemTypeNameHandling.GetValueOrDefault();
      set => this._itemTypeNameHandling = new TypeNameHandling?(value);
    }

    /// <summary>
    /// Gets or sets whether this property's collection items are serialized as a reference.
    /// </summary>
    /// <value>Whether this property's collection items are serialized as a reference.</value>
    public bool ItemIsReference
    {
      get => this._itemIsReference.GetValueOrDefault();
      set => this._itemIsReference = new bool?(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" /> class.
    /// </summary>
    public JsonPropertyAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonPropertyAttribute" /> class with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    public JsonPropertyAttribute(string propertyName) => this.PropertyName = propertyName;
  }
}
