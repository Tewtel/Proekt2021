// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.DuplicatePropertyNameHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Specifies how duplicate property names are handled when loading JSON.
  /// </summary>
  public enum DuplicatePropertyNameHandling
  {
    /// <summary>
    /// Replace the existing value when there is a duplicate property. The value of the last property in the JSON object will be used.
    /// </summary>
    Replace,
    /// <summary>
    /// Ignore the new value when there is a duplicate property. The value of the first property in the JSON object will be used.
    /// </summary>
    Ignore,
    /// <summary>
    /// Throw a <see cref="T:Newtonsoft.Json.JsonReaderException" /> when a duplicate property is encountered.
    /// </summary>
    Error,
  }
}
