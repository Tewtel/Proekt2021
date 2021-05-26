// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.MissingMemberHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies missing member handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public enum MissingMemberHandling
  {
    /// <summary>
    /// Ignore a missing member and do not attempt to deserialize it.
    /// </summary>
    Ignore,
    /// <summary>
    /// Throw a <see cref="T:Newtonsoft.Json.JsonSerializationException" /> when a missing member is encountered during deserialization.
    /// </summary>
    Error,
  }
}
