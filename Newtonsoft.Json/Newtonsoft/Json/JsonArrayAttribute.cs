// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonArrayAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> how to serialize the collection.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
  public sealed class JsonArrayAttribute : JsonContainerAttribute
  {
    private bool _allowNullItems;

    /// <summary>
    /// Gets or sets a value indicating whether null items are allowed in the collection.
    /// </summary>
    /// <value><c>true</c> if null items are allowed in the collection; otherwise, <c>false</c>.</value>
    public bool AllowNullItems
    {
      get => this._allowNullItems;
      set => this._allowNullItems = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonArrayAttribute" /> class.
    /// </summary>
    public JsonArrayAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonObjectAttribute" /> class with a flag indicating whether the array can contain null items.
    /// </summary>
    /// <param name="allowNullItems">A flag indicating whether the array can contain null items.</param>
    public JsonArrayAttribute(bool allowNullItems) => this._allowNullItems = allowNullItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonArrayAttribute" /> class with the specified container Id.
    /// </summary>
    /// <param name="id">The container Id.</param>
    public JsonArrayAttribute(string id)
      : base(id)
    {
    }
  }
}
