// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonObjectAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> how to serialize the object.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonObjectAttribute : JsonContainerAttribute
  {
    private MemberSerialization _memberSerialization;
    internal MissingMemberHandling? _missingMemberHandling;
    internal Required? _itemRequired;
    internal NullValueHandling? _itemNullValueHandling;

    /// <summary>Gets or sets the member serialization.</summary>
    /// <value>The member serialization.</value>
    public MemberSerialization MemberSerialization
    {
      get => this._memberSerialization;
      set => this._memberSerialization = value;
    }

    /// <summary>
    /// Gets or sets the missing member handling used when deserializing this object.
    /// </summary>
    /// <value>The missing member handling.</value>
    public MissingMemberHandling MissingMemberHandling
    {
      get => this._missingMemberHandling.GetValueOrDefault();
      set => this._missingMemberHandling = new MissingMemberHandling?(value);
    }

    /// <summary>
    /// Gets or sets how the object's properties with null values are handled during serialization and deserialization.
    /// </summary>
    /// <value>How the object's properties with null values are handled during serialization and deserialization.</value>
    public NullValueHandling ItemNullValueHandling
    {
      get => this._itemNullValueHandling.GetValueOrDefault();
      set => this._itemNullValueHandling = new NullValueHandling?(value);
    }

    /// <summary>
    /// Gets or sets a value that indicates whether the object's properties are required.
    /// </summary>
    /// <value>
    /// 	A value indicating whether the object's properties are required.
    /// </value>
    public Required ItemRequired
    {
      get => this._itemRequired.GetValueOrDefault();
      set => this._itemRequired = new Required?(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonObjectAttribute" /> class.
    /// </summary>
    public JsonObjectAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonObjectAttribute" /> class with the specified member serialization.
    /// </summary>
    /// <param name="memberSerialization">The member serialization.</param>
    public JsonObjectAttribute(MemberSerialization memberSerialization) => this.MemberSerialization = memberSerialization;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonObjectAttribute" /> class with the specified container Id.
    /// </summary>
    /// <param name="id">The container Id.</param>
    public JsonObjectAttribute(string id)
      : base(id)
    {
    }
  }
}
