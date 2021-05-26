// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbProviderManifestExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class DbProviderManifestExtensions
  {
    public static PrimitiveType GetStoreTypeFromName(
      this DbProviderManifest providerManifest,
      string name)
    {
      return providerManifest.GetStoreTypes().SingleOrDefault<PrimitiveType>((Func<PrimitiveType, bool>) (p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))) ?? throw System.Data.Entity.Resources.Error.StoreTypeNotFound((object) name, (object) providerManifest.NamespaceName);
    }
  }
}
