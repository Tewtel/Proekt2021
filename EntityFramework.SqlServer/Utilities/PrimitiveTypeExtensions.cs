// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.PrimitiveTypeExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class PrimitiveTypeExtensions
  {
    internal static bool IsSpatialType(this PrimitiveType type)
    {
      PrimitiveTypeKind primitiveTypeKind = type.PrimitiveTypeKind;
      return primitiveTypeKind >= PrimitiveTypeKind.Geometry && primitiveTypeKind <= PrimitiveTypeKind.GeographyCollection;
    }

    internal static bool IsHierarchyIdType(this PrimitiveType type) => type.PrimitiveTypeKind == PrimitiveTypeKind.HierarchyId;
  }
}
