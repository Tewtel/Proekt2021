﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.ValidationEventArgs
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Schema
{
  /// <summary>
  /// <para>
  /// Returns detailed information related to the <see cref="T:Newtonsoft.Json.Schema.ValidationEventHandler" />.
  /// </para>
  /// <note type="caution">
  /// JSON Schema validation has been moved to its own package. See <see href="https://www.newtonsoft.com/jsonschema">https://www.newtonsoft.com/jsonschema</see> for more details.
  /// </note>
  /// </summary>
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  public class ValidationEventArgs : EventArgs
  {
    private readonly JsonSchemaException _ex;

    internal ValidationEventArgs(JsonSchemaException ex)
    {
      ValidationUtils.ArgumentNotNull((object) ex, nameof (ex));
      this._ex = ex;
    }

    /// <summary>
    /// Gets the <see cref="T:Newtonsoft.Json.Schema.JsonSchemaException" /> associated with the validation error.
    /// </summary>
    /// <value>The JsonSchemaException associated with the validation error.</value>
    public JsonSchemaException Exception => this._ex;

    /// <summary>
    /// Gets the path of the JSON location where the validation error occurred.
    /// </summary>
    /// <value>The path of the JSON location where the validation error occurred.</value>
    public string Path => this._ex.Path;

    /// <summary>
    /// Gets the text description corresponding to the validation error.
    /// </summary>
    /// <value>The text description.</value>
    public string Message => this._ex.Message;
  }
}
