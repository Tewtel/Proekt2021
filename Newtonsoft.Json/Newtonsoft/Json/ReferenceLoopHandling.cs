// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ReferenceLoopHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies reference loop handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public enum ReferenceLoopHandling
  {
    /// <summary>
    /// Throw a <see cref="T:Newtonsoft.Json.JsonSerializationException" /> when a loop is encountered.
    /// </summary>
    Error,
    /// <summary>Ignore loop references and do not serialize.</summary>
    Ignore,
    /// <summary>Serialize loop references.</summary>
    Serialize,
  }
}
