// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Required
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>Indicating whether a property is required.</summary>
  public enum Required
  {
    /// <summary>The property is not required. The default state.</summary>
    Default,
    /// <summary>
    /// The property must be defined in JSON but can be a null value.
    /// </summary>
    AllowNull,
    /// <summary>
    /// The property must be defined in JSON and cannot be a null value.
    /// </summary>
    Always,
    /// <summary>
    /// The property is not required but it cannot be a null value.
    /// </summary>
    DisallowNull,
  }
}
