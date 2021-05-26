// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.MergeNullValueHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies how null value properties are merged.</summary>
  [Flags]
  public enum MergeNullValueHandling
  {
    /// <summary>
    /// The content's null value properties will be ignored during merging.
    /// </summary>
    Ignore = 0,
    /// <summary>The content's null value properties will be merged.</summary>
    Merge = 1,
  }
}
