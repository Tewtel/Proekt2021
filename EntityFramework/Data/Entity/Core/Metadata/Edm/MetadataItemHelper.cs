// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.MetadataItemHelper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class MetadataItemHelper
  {
    internal const string SchemaErrorsMetadataPropertyName = "EdmSchemaErrors";
    internal const string SchemaInvalidMetadataPropertyName = "EdmSchemaInvalid";

    public static bool IsInvalid(MetadataItem instance)
    {
      MetadataProperty metadataProperty;
      return instance.MetadataProperties.TryGetValue("EdmSchemaInvalid", false, out metadataProperty) && metadataProperty != null && (bool) metadataProperty.Value;
    }

    public static bool HasSchemaErrors(MetadataItem instance) => instance.MetadataProperties.Contains("EdmSchemaErrors");

    public static IEnumerable<EdmSchemaError> GetSchemaErrors(
      MetadataItem instance)
    {
      MetadataProperty metadataProperty;
      return !instance.MetadataProperties.TryGetValue("EdmSchemaErrors", false, out metadataProperty) || metadataProperty == null ? Enumerable.Empty<EdmSchemaError>() : (IEnumerable<EdmSchemaError>) metadataProperty.Value;
    }
  }
}
