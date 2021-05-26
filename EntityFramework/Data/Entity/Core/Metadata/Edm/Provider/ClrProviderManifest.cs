// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.Provider.ClrProviderManifest
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Hierarchy;
using System.Data.Entity.Spatial;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Threading;
using System.Xml;

namespace System.Data.Entity.Core.Metadata.Edm.Provider
{
  internal class ClrProviderManifest : DbProviderManifest
  {
    private const int s_PrimitiveTypeCount = 32;
    private ReadOnlyCollection<PrimitiveType> _primitiveTypesArray;
    private ReadOnlyCollection<PrimitiveType> _primitiveTypes;
    private static readonly ClrProviderManifest _instance = new ClrProviderManifest();

    private ClrProviderManifest()
    {
    }

    internal static ClrProviderManifest Instance => ClrProviderManifest._instance;

    public override string NamespaceName => "System";

    internal bool TryGetPrimitiveType(Type clrType, out PrimitiveType primitiveType)
    {
      primitiveType = (PrimitiveType) null;
      PrimitiveTypeKind resolvedPrimitiveTypeKind;
      if (!ClrProviderManifest.TryGetPrimitiveTypeKind(clrType, out resolvedPrimitiveTypeKind))
        return false;
      this.InitializePrimitiveTypes();
      primitiveType = this._primitiveTypesArray[(int) resolvedPrimitiveTypeKind];
      return true;
    }

    internal static bool TryGetPrimitiveTypeKind(
      Type clrType,
      out PrimitiveTypeKind resolvedPrimitiveTypeKind)
    {
      PrimitiveTypeKind? nullable = new PrimitiveTypeKind?();
      if (!clrType.IsEnum())
      {
        switch (Type.GetTypeCode(clrType))
        {
          case TypeCode.Object:
            if (typeof (byte[]) == clrType)
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Binary);
              break;
            }
            if (typeof (DateTimeOffset) == clrType)
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.DateTimeOffset);
              break;
            }
            if (typeof (DbGeography).IsAssignableFrom(clrType))
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Geography);
              break;
            }
            if (typeof (DbGeometry).IsAssignableFrom(clrType))
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Geometry);
              break;
            }
            if (typeof (Guid) == clrType)
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Guid);
              break;
            }
            if (typeof (HierarchyId) == clrType)
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.HierarchyId);
              break;
            }
            if (typeof (TimeSpan) == clrType)
            {
              nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Time);
              break;
            }
            break;
          case TypeCode.Boolean:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Boolean);
            break;
          case TypeCode.SByte:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.SByte);
            break;
          case TypeCode.Byte:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Byte);
            break;
          case TypeCode.Int16:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Int16);
            break;
          case TypeCode.Int32:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Int32);
            break;
          case TypeCode.Int64:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Int64);
            break;
          case TypeCode.Single:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Single);
            break;
          case TypeCode.Double:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Double);
            break;
          case TypeCode.Decimal:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.Decimal);
            break;
          case TypeCode.DateTime:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.DateTime);
            break;
          case TypeCode.String:
            nullable = new PrimitiveTypeKind?(PrimitiveTypeKind.String);
            break;
        }
      }
      if (nullable.HasValue)
      {
        resolvedPrimitiveTypeKind = nullable.Value;
        return true;
      }
      resolvedPrimitiveTypeKind = PrimitiveTypeKind.Binary;
      return false;
    }

    public override ReadOnlyCollection<EdmFunction> GetStoreFunctions() => Helper.EmptyEdmFunctionReadOnlyCollection;

    public override ReadOnlyCollection<FacetDescription> GetFacetDescriptions(
      EdmType type)
    {
      if (!Helper.IsPrimitiveType(type) || type.DataSpace != DataSpace.OSpace)
        return Helper.EmptyFacetDescriptionEnumerable;
      PrimitiveType baseType = (PrimitiveType) type.BaseType;
      return baseType.ProviderManifest.GetFacetDescriptions((EdmType) baseType);
    }

    private void InitializePrimitiveTypes()
    {
      if (this._primitiveTypes != null)
        return;
      PrimitiveType[] primitiveTypeArray = new PrimitiveType[32];
      primitiveTypeArray[0] = this.CreatePrimitiveType(typeof (byte[]), PrimitiveTypeKind.Binary);
      primitiveTypeArray[1] = this.CreatePrimitiveType(typeof (bool), PrimitiveTypeKind.Boolean);
      primitiveTypeArray[2] = this.CreatePrimitiveType(typeof (byte), PrimitiveTypeKind.Byte);
      primitiveTypeArray[3] = this.CreatePrimitiveType(typeof (DateTime), PrimitiveTypeKind.DateTime);
      primitiveTypeArray[13] = this.CreatePrimitiveType(typeof (TimeSpan), PrimitiveTypeKind.Time);
      primitiveTypeArray[14] = this.CreatePrimitiveType(typeof (DateTimeOffset), PrimitiveTypeKind.DateTimeOffset);
      primitiveTypeArray[4] = this.CreatePrimitiveType(typeof (Decimal), PrimitiveTypeKind.Decimal);
      primitiveTypeArray[5] = this.CreatePrimitiveType(typeof (double), PrimitiveTypeKind.Double);
      primitiveTypeArray[16] = this.CreatePrimitiveType(typeof (DbGeography), PrimitiveTypeKind.Geography);
      primitiveTypeArray[15] = this.CreatePrimitiveType(typeof (DbGeometry), PrimitiveTypeKind.Geometry);
      primitiveTypeArray[6] = this.CreatePrimitiveType(typeof (Guid), PrimitiveTypeKind.Guid);
      primitiveTypeArray[31] = this.CreatePrimitiveType(typeof (HierarchyId), PrimitiveTypeKind.HierarchyId);
      primitiveTypeArray[9] = this.CreatePrimitiveType(typeof (short), PrimitiveTypeKind.Int16);
      primitiveTypeArray[10] = this.CreatePrimitiveType(typeof (int), PrimitiveTypeKind.Int32);
      primitiveTypeArray[11] = this.CreatePrimitiveType(typeof (long), PrimitiveTypeKind.Int64);
      primitiveTypeArray[8] = this.CreatePrimitiveType(typeof (sbyte), PrimitiveTypeKind.SByte);
      primitiveTypeArray[7] = this.CreatePrimitiveType(typeof (float), PrimitiveTypeKind.Single);
      primitiveTypeArray[12] = this.CreatePrimitiveType(typeof (string), PrimitiveTypeKind.String);
      ReadOnlyCollection<PrimitiveType> readOnlyCollection1 = new ReadOnlyCollection<PrimitiveType>((IList<PrimitiveType>) primitiveTypeArray);
      ReadOnlyCollection<PrimitiveType> readOnlyCollection2 = new ReadOnlyCollection<PrimitiveType>((IList<PrimitiveType>) ((IEnumerable<PrimitiveType>) primitiveTypeArray).Where<PrimitiveType>((Func<PrimitiveType, bool>) (t => t != null)).ToList<PrimitiveType>());
      Interlocked.CompareExchange<ReadOnlyCollection<PrimitiveType>>(ref this._primitiveTypesArray, readOnlyCollection1, (ReadOnlyCollection<PrimitiveType>) null);
      Interlocked.CompareExchange<ReadOnlyCollection<PrimitiveType>>(ref this._primitiveTypes, readOnlyCollection2, (ReadOnlyCollection<PrimitiveType>) null);
    }

    private PrimitiveType CreatePrimitiveType(
      Type clrType,
      PrimitiveTypeKind primitiveTypeKind)
    {
      PrimitiveType primitiveType1 = MetadataItem.EdmProviderManifest.GetPrimitiveType(primitiveTypeKind);
      PrimitiveType primitiveType2 = new PrimitiveType(clrType, primitiveType1, (DbProviderManifest) this);
      primitiveType2.SetReadOnly();
      return primitiveType2;
    }

    public override ReadOnlyCollection<PrimitiveType> GetStoreTypes()
    {
      this.InitializePrimitiveTypes();
      return this._primitiveTypes;
    }

    public override TypeUsage GetEdmType(TypeUsage storeType)
    {
      System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(storeType, nameof (storeType));
      throw new NotImplementedException();
    }

    public override TypeUsage GetStoreType(TypeUsage edmType)
    {
      System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(edmType, nameof (edmType));
      throw new NotImplementedException();
    }

    protected override XmlReader GetDbInformation(string informationType) => throw new NotImplementedException();
  }
}
