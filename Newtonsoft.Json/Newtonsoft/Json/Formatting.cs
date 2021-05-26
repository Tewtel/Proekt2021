// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Formatting
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies formatting options for the <see cref="T:Newtonsoft.Json.JsonTextWriter" />.
  /// </summary>
  public enum Formatting
  {
    /// <summary>
    /// No special formatting is applied. This is the default.
    /// </summary>
    None,
    /// <summary>
    /// Causes child objects to be indented according to the <see cref="P:Newtonsoft.Json.JsonTextWriter.Indentation" /> and <see cref="P:Newtonsoft.Json.JsonTextWriter.IndentChar" /> settings.
    /// </summary>
    Indented,
  }
}
