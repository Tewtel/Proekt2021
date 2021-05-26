// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.UndefinedSchemaIdHandling
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Schema
{
  /// <summary>
  /// <para>
  /// Specifies undefined schema Id handling options for the <see cref="T:Newtonsoft.Json.Schema.JsonSchemaGenerator" />.
  /// </para>
  /// <note type="caution">
  /// JSON Schema validation has been moved to its own package. See <see href="https://www.newtonsoft.com/jsonschema">https://www.newtonsoft.com/jsonschema</see> for more details.
  /// </note>
  /// </summary>
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public enum UndefinedSchemaIdHandling
  {
    /// <summary>Do not infer a schema Id.</summary>
    None,
    /// <summary>Use the .NET type name as the schema Id.</summary>
    UseTypeName,
    /// <summary>
    /// Use the assembly qualified .NET type name as the schema Id.
    /// </summary>
    UseAssemblyQualifiedName,
  }
}
