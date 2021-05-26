// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonExtensionDataAttribute
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Instructs the <see cref="T:Newtonsoft.Json.JsonSerializer" /> to deserialize properties with no matching class member into the specified collection
  /// and write values during serialization.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class JsonExtensionDataAttribute : Attribute
  {
    /// <summary>
    /// Gets or sets a value that indicates whether to write extension data when serializing the object.
    /// </summary>
    /// <value>
    /// 	<c>true</c> to write extension data when serializing the object; otherwise, <c>false</c>. The default is <c>true</c>.
    /// </value>
    public bool WriteData { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether to read extension data when deserializing the object.
    /// </summary>
    /// <value>
    /// 	<c>true</c> to read extension data when deserializing the object; otherwise, <c>false</c>. The default is <c>true</c>.
    /// </value>
    public bool ReadData { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.JsonExtensionDataAttribute" /> class.
    /// </summary>
    public JsonExtensionDataAttribute()
    {
      this.WriteData = true;
      this.ReadData = true;
    }
  }
}
