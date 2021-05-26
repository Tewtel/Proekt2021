// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Schema.JsonSchemaNodeCollection
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using System;
using System.Collections.ObjectModel;

namespace Newtonsoft.Json.Schema
{
  [Obsolete("JSON Schema validation has been moved to its own package. See https://www.newtonsoft.com/jsonschema for more details.")]
  internal class JsonSchemaNodeCollection : KeyedCollection<string, JsonSchemaNode>
  {
    protected override string GetKeyForItem(JsonSchemaNode item) => item.Id;
  }
}
