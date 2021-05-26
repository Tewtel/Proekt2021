// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Spatial.SpatialHelpers
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Spatial
{
  internal static class SpatialHelpers
  {
    internal static object GetSpatialValue(
      MetadataWorkspace workspace,
      DbDataReader reader,
      TypeUsage columnType,
      int columnOrdinal)
    {
      DbSpatialDataReader spatialDataReader = SpatialHelpers.CreateSpatialDataReader(workspace, reader);
      return Helper.IsGeographicType((PrimitiveType) columnType.EdmType) ? (object) spatialDataReader.GetGeography(columnOrdinal) : (object) spatialDataReader.GetGeometry(columnOrdinal);
    }

    internal static async Task<object> GetSpatialValueAsync(
      MetadataWorkspace workspace,
      DbDataReader reader,
      TypeUsage columnType,
      int columnOrdinal,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      DbSpatialDataReader spatialDataReader = SpatialHelpers.CreateSpatialDataReader(workspace, reader);
      return Helper.IsGeographicType((PrimitiveType) columnType.EdmType) ? (object) await spatialDataReader.GetGeographyAsync(columnOrdinal, cancellationToken).WithCurrentCulture<DbGeography>() : (object) await spatialDataReader.GetGeometryAsync(columnOrdinal, cancellationToken).WithCurrentCulture<DbGeometry>();
    }

    internal static DbSpatialDataReader CreateSpatialDataReader(
      MetadataWorkspace workspace,
      DbDataReader reader)
    {
      StoreItemCollection itemCollection = (StoreItemCollection) workspace.GetItemCollection(DataSpace.SSpace);
      return itemCollection.ProviderFactory.GetProviderServices().GetSpatialDataReader(reader, itemCollection.ProviderManifestToken) ?? throw new ProviderIncompatibleException(Strings.ProviderDidNotReturnSpatialServices);
    }
  }
}
