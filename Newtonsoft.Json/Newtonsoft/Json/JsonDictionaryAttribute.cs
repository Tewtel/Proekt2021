// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonDictionaryAttribute
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
  public sealed class JsonDictionaryAttribute : JsonContainerAttribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonDictionaryAttribute" /> class.
    /// </summary>
    public JsonDictionaryAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonDictionaryAttribute" /> class with the specified container Id.
    /// </summary>
    /// <param name="id">The container Id.</param>
    public JsonDictionaryAttribute(string id)
      : base(id)
    {
    }
  }
}
