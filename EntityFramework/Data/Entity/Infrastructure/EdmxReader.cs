// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.EdmxReader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal;
using System.Data.Entity.Utilities;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Utility class for reading a metadata model from .edmx.
  /// </summary>
  public static class EdmxReader
  {
    /// <summary>Reads a metadata model from .edmx.</summary>
    /// <param name="reader">XML reader for the .edmx</param>
    /// <param name="defaultSchema">Default database schema used by the model.</param>
    /// <returns>The loaded metadata model.</returns>
    public static DbCompiledModel Read(XmlReader reader, string defaultSchema)
    {
      System.Data.Entity.Utilities.Check.NotNull<XmlReader>(reader, nameof (reader));
      DbProviderInfo providerInfo;
      return new DbCompiledModel(CodeFirstCachedMetadataWorkspace.Create(XDocument.Load(reader).GetStorageMappingItemCollection(out providerInfo), providerInfo), defaultSchema);
    }
  }
}
