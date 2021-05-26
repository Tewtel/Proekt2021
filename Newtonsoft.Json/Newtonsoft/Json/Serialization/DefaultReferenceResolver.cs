// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DefaultReferenceResolver
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  internal class DefaultReferenceResolver : IReferenceResolver
  {
    private int _referenceCount;

    private BidirectionalDictionary<string, object> GetMappings(
      object context)
    {
      switch (context)
      {
        case JsonSerializerInternalBase internalSerializer:
label_3:
          return internalSerializer.DefaultReferenceMappings;
        case JsonSerializerProxy jsonSerializerProxy:
          internalSerializer = jsonSerializerProxy.GetInternalSerializer();
          goto label_3;
        default:
          throw new JsonException("The DefaultReferenceResolver can only be used internally.");
      }
    }

    public object ResolveReference(object context, string reference)
    {
      object second;
      this.GetMappings(context).TryGetByFirst(reference, out second);
      return second;
    }

    public string GetReference(object context, object value)
    {
      BidirectionalDictionary<string, object> mappings = this.GetMappings(context);
      string first;
      if (!mappings.TryGetBySecond(value, out first))
      {
        ++this._referenceCount;
        first = this._referenceCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        mappings.Set(first, value);
      }
      return first;
    }

    public void AddReference(object context, string reference, object value) => this.GetMappings(context).Set(reference, value);

    public bool IsReferenced(object context, object value) => this.GetMappings(context).TryGetBySecond(value, out string _);
  }
}
