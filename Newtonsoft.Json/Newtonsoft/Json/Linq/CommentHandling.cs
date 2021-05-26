// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.CommentHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Specifies how JSON comments are handled when loading JSON.
  /// </summary>
  public enum CommentHandling
  {
    /// <summary>Ignore comments.</summary>
    Ignore,
    /// <summary>
    /// Load comments as a <see cref="T:Newtonsoft.Json.Linq.JValue" /> with type <see cref="F:Newtonsoft.Json.Linq.JTokenType.Comment" />.
    /// </summary>
    Load,
  }
}
