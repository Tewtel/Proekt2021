﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.DateParseHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed when reading JSON text.
  /// </summary>
  public enum DateParseHandling
  {
    /// <summary>
    /// Date formatted strings are not parsed to a date type and are read as strings.
    /// </summary>
    None,
    /// <summary>
    /// Date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed to <see cref="T:System.DateTime" />.
    /// </summary>
    DateTime,
    /// <summary>
    /// Date formatted strings, e.g. <c>"\/Date(1198908717056)\/"</c> and <c>"2012-03-21T05:40Z"</c>, are parsed to <see cref="T:System.DateTimeOffset" />.
    /// </summary>
    DateTimeOffset,
  }
}
