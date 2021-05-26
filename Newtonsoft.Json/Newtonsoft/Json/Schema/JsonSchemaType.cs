// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaType
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Schema
{
  /// <summary>
  /// <para>
  /// The value types allowed by the <see cref="T:Newtonsoft.Json.Schema.JsonSchema" />.
  /// </para>
  /// <note type="caution">
  /// JSON Schema validation has been moved to its own package. See <see href="https://www.newtonsoft.com/jsonschema">https://www.newtonsoft.com/jsonschema</see> for more details.
  /// </note>
  /// </summary>
  [Flags]
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public enum JsonSchemaType
  {
    /// <summary>No type specified.</summary>
    None = 0,
    /// <summary>String type.</summary>
    String = 1,
    /// <summary>Float type.</summary>
    Float = 2,
    /// <summary>Integer type.</summary>
    Integer = 4,
    /// <summary>Boolean type.</summary>
    Boolean = 8,
    /// <summary>Object type.</summary>
    Object = 16, // 0x00000010
    /// <summary>Array type.</summary>
    Array = 32, // 0x00000020
    /// <summary>Null type.</summary>
    Null = 64, // 0x00000040
    /// <summary>Any type.</summary>
    Any = Null | Array | Object | Boolean | Integer | Float | String, // 0x0000007F
  }
}
