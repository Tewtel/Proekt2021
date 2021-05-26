// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.ObjectCreationHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies how object creation is handled by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public enum ObjectCreationHandling
  {
    /// <summary>
    /// Reuse existing objects, create new objects when needed.
    /// </summary>
    Auto,
    /// <summary>Only reuse existing objects.</summary>
    Reuse,
    /// <summary>Always create new objects.</summary>
    Replace,
  }
}
