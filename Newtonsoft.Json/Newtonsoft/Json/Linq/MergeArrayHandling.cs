// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.MergeArrayHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Linq
{
  /// <summary>Specifies how JSON arrays are merged together.</summary>
  public enum MergeArrayHandling
  {
    /// <summary>Concatenate arrays.</summary>
    Concat,
    /// <summary>Union arrays, skipping items that already exist.</summary>
    Union,
    /// <summary>Replace all array items.</summary>
    Replace,
    /// <summary>Merge array items together, matched by index.</summary>
    Merge,
  }
}
