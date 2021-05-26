// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DateFormatHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how dates are formatted when writing JSON text.
  /// </summary>
  public enum DateFormatHandling
  {
    /// <summary>
    /// Dates are written in the ISO 8601 format, e.g. <c>"2012-03-21T05:40Z"</c>.
    /// </summary>
    IsoDateFormat,
    /// <summary>
    /// Dates are written in the Microsoft JSON format, e.g. <c>"\/Date(1198908717056)\/"</c>.
    /// </summary>
    MicrosoftDateFormat,
  }
}
