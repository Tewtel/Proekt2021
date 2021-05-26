// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.IDbSpatialValueExtensionMethods
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Spatial;

namespace System.Data.Entity.SqlServer
{
  internal static class IDbSpatialValueExtensionMethods
  {
    internal static IDbSpatialValue AsSpatialValue(this DbGeography geographyValue) => (IDbSpatialValue) new DbGeographyAdapter(geographyValue);

    internal static IDbSpatialValue AsSpatialValue(this DbGeometry geometryValue) => (IDbSpatialValue) new DbGeometryAdapter(geometryValue);
  }
}
