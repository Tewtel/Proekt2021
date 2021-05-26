// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.ValidationEventHandler
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Schema
{
  /// <summary>
  /// <para>
  /// Represents the callback method that will handle JSON schema validation events and the <see cref="T:Newtonsoft.Json.Schema.ValidationEventArgs" />.
  /// </para>
  /// <note type="caution">
  /// JSON Schema validation has been moved to its own package. See <see href="https://www.newtonsoft.com/jsonschema">https://www.newtonsoft.com/jsonschema</see> for more details.
  /// </note>
  /// </summary>
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public delegate void ValidationEventHandler(object sender, ValidationEventArgs e);
}
