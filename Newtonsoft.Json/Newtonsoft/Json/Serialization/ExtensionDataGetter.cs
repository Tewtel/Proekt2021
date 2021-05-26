// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ExtensionDataGetter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Gets extension data for an object during serialization.
  /// </summary>
  /// <param name="o">The object to set extension data on.</param>
  public delegate IEnumerable<KeyValuePair<object, object>>? ExtensionDataGetter(
    object o);
}
