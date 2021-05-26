// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.NullValueHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies null value handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeNullValueHandlingObject" title="NullValueHandling Class" />
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="ReducingSerializedJsonSizeNullValueHandlingExample" title="NullValueHandling Ignore Example" />
  /// </example>
  public enum NullValueHandling
  {
    /// <summary>
    /// Include null values when serializing and deserializing objects.
    /// </summary>
    Include,
    /// <summary>
    /// Ignore null values when serializing and deserializing objects.
    /// </summary>
    Ignore,
  }
}
