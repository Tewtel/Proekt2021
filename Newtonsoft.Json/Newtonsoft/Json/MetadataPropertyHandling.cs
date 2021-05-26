// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.MetadataPropertyHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies metadata property handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public enum MetadataPropertyHandling
  {
    /// <summary>
    /// Read metadata properties located at the start of a JSON object.
    /// </summary>
    Default,
    /// <summary>
    /// Read metadata properties located anywhere in a JSON object. Note that this setting will impact performance.
    /// </summary>
    ReadAhead,
    /// <summary>Do not try to read metadata properties.</summary>
    Ignore,
  }
}
