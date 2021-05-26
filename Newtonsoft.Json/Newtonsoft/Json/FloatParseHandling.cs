// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.FloatParseHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how floating point numbers, e.g. 1.0 and 9.9, are parsed when reading JSON text.
  /// </summary>
  public enum FloatParseHandling
  {
    /// <summary>
    /// Floating point numbers are parsed to <see cref="F:Newtonsoft.Json.FloatParseHandling.Double" />.
    /// </summary>
    Double,
    /// <summary>
    /// Floating point numbers are parsed to <see cref="F:Newtonsoft.Json.FloatParseHandling.Decimal" />.
    /// </summary>
    Decimal,
  }
}
