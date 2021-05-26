// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.PreserveReferencesHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json
{
  /// <summary>
  /// Specifies reference handling options for the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// Note that references cannot be preserved when a value is set via a non-default constructor such as types that implement <see cref="T:System.Runtime.Serialization.ISerializable" />.
  /// </summary>
  /// <example>
  ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="PreservingObjectReferencesOn" title="Preserve Object References" />
  /// </example>
  [Flags]
  public enum PreserveReferencesHandling
  {
    /// <summary>Do not preserve references when serializing types.</summary>
    None = 0,
    /// <summary>
    /// Preserve references when serializing into a JSON object structure.
    /// </summary>
    Objects = 1,
    /// <summary>
    /// Preserve references when serializing into a JSON array structure.
    /// </summary>
    Arrays = 2,
    /// <summary>Preserve references when serializing.</summary>
    All = Arrays | Objects, // 0x00000003
  }
}
