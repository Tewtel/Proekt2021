// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DateTimeConverterBase
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Provides a base class for converting a <see cref="T:System.DateTime" /> to and from JSON.
  /// </summary>
  public abstract class DateTimeConverterBase : JsonConverter
  {
    /// <summary>
    /// Determines whether this instance can convert the specified object type.
    /// </summary>
    /// <param name="objectType">Type of the object.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type objectType) => objectType == typeof (DateTime) || objectType == typeof (DateTime?) || (objectType == typeof (DateTimeOffset) || objectType == typeof (DateTimeOffset?));
  }
}
