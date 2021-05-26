// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.PrimitiveType
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

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class representing a primitive type</summary>
  public class PrimitiveType : SimpleType
  {
    private PrimitiveTypeKind _primitiveTypeKind;
    private DbProviderManifest _providerManifest;

    internal PrimitiveType()
    {
    }

    internal PrimitiveType(
      string name,
      string namespaceName,
      DataSpace dataSpace,
      PrimitiveType baseType,
      DbProviderManifest providerManifest)
      : base(name, namespaceName, dataSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<PrimitiveType>(baseType, nameof (baseType));
      System.Data.Entity.Utilities.Check.NotNull<DbProviderManifest>(providerManifest, nameof (providerManifest));
      this.BaseType = (EdmType) baseType;
      PrimitiveType.Initialize(this, baseType.PrimitiveTypeKind, providerManifest);
    }

    internal PrimitiveType(
      Type clrType,
      PrimitiveType baseType,
      DbProviderManifest providerManifest)
      : this(System.Data.Entity.Utilities.Check.NotNull<Type>(clrType, nameof (clrType)).Name, clrType.NestingNamespace(), DataSpace.OSpace, baseType, providerManifest)
    {
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.PrimitiveType;

    internal override Type ClrType => this.ClrEquivalentType;

    /// <summary>
    /// Gets a <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveTypeKind" /> enumeration value that indicates a primitive type of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveTypeKind" /> enumeration value that indicates a primitive type of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.PrimitiveTypeKind, false)]
    public virtual PrimitiveTypeKind PrimitiveTypeKind
    {
      get => this._primitiveTypeKind;
      internal set => this._primitiveTypeKind = value;
    }

    internal DbProviderManifest ProviderManifest
    {
      get => this._providerManifest;
      set => this._providerManifest = value;
    }

    /// <summary>
    /// Gets the list of facet descriptions for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />.
    /// </summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> that contains the list of facet descriptions for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    public virtual ReadOnlyCollection<FacetDescription> FacetDescriptions => this.ProviderManifest.GetFacetDescriptions((EdmType) this);

    /// <summary>
    /// Returns an equivalent common language runtime (CLR) type of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// . Note that the
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.PrimitiveType.ClrEquivalentType" />
    /// property always returns a non-nullable type value.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Type" /> object that represents an equivalent common language runtime (CLR) type of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    public Type ClrEquivalentType
    {
      get
      {
        switch (this.PrimitiveTypeKind)
        {
          case PrimitiveTypeKind.Binary:
            return typeof (byte[]);
          case PrimitiveTypeKind.Boolean:
            return typeof (bool);
          case PrimitiveTypeKind.Byte:
            return typeof (byte);
          case PrimitiveTypeKind.DateTime:
            return typeof (DateTime);
          case PrimitiveTypeKind.Decimal:
            return typeof (Decimal);
          case PrimitiveTypeKind.Double:
            return typeof (double);
          case PrimitiveTypeKind.Guid:
            return typeof (Guid);
          case PrimitiveTypeKind.Single:
            return typeof (float);
          case PrimitiveTypeKind.SByte:
            return typeof (sbyte);
          case PrimitiveTypeKind.Int16:
            return typeof (short);
          case PrimitiveTypeKind.Int32:
            return typeof (int);
          case PrimitiveTypeKind.Int64:
            return typeof (long);
          case PrimitiveTypeKind.String:
            return typeof (string);
          case PrimitiveTypeKind.Time:
            return typeof (TimeSpan);
          case PrimitiveTypeKind.DateTimeOffset:
            return typeof (DateTimeOffset);
          case PrimitiveTypeKind.Geometry:
          case PrimitiveTypeKind.GeometryPoint:
          case PrimitiveTypeKind.GeometryLineString:
          case PrimitiveTypeKind.GeometryPolygon:
          case PrimitiveTypeKind.GeometryMultiPoint:
          case PrimitiveTypeKind.GeometryMultiLineString:
          case PrimitiveTypeKind.GeometryMultiPolygon:
          case PrimitiveTypeKind.GeometryCollection:
            return typeof (DbGeometry);
          case PrimitiveTypeKind.Geography:
          case PrimitiveTypeKind.GeographyPoint:
          case PrimitiveTypeKind.GeographyLineString:
          case PrimitiveTypeKind.GeographyPolygon:
          case PrimitiveTypeKind.GeographyMultiPoint:
          case PrimitiveTypeKind.GeographyMultiLineString:
          case PrimitiveTypeKind.GeographyMultiPolygon:
          case PrimitiveTypeKind.GeographyCollection:
            return typeof (DbGeography);
          case PrimitiveTypeKind.HierarchyId:
            return typeof (HierarchyId);
          default:
            return (Type) null;
        }
      }
    }

    internal override IEnumerable<FacetDescription> GetAssociatedFacetDescriptions() => base.GetAssociatedFacetDescriptions().Concat<FacetDescription>((IEnumerable<FacetDescription>) this.FacetDescriptions);

    internal static void Initialize(
      PrimitiveType primitiveType,
      PrimitiveTypeKind primitiveTypeKind,
      DbProviderManifest providerManifest)
    {
      primitiveType._primitiveTypeKind = primitiveTypeKind;
      primitiveType._providerManifest = providerManifest;
    }

    /// <summary>
    /// Returns the equivalent <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmType" /> of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </summary>
    /// <remarks>
    /// For example if this instance is nvarchar and it's
    /// base type is Edm String then the return type is Edm String.
    /// If the type is actually already a model type then the
    /// return type is "this".
    /// </remarks>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmType" /> object that is an equivalent of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    public EdmType GetEdmPrimitiveType() => (EdmType) MetadataItem.EdmProviderManifest.GetPrimitiveType(this.PrimitiveTypeKind);

    /// <summary>Returns the list of primitive types.</summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> that contains the list of primitive types.
    /// </returns>
    public static ReadOnlyCollection<PrimitiveType> GetEdmPrimitiveTypes() => MetadataItem.EdmProviderManifest.GetStoreTypes();

    /// <summary>
    /// Returns the equivalent <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmType" /> of a
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EdmType" /> object that is an equivalent of a specified
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />
    /// .
    /// </returns>
    /// <param name="primitiveTypeKind">
    /// A value of type <see cref="T:System.Data.Entity.Core.Metadata.Edm.PrimitiveType" />.
    /// </param>
    public static PrimitiveType GetEdmPrimitiveType(PrimitiveTypeKind primitiveTypeKind) => MetadataItem.EdmProviderManifest.GetPrimitiveType(primitiveTypeKind);
  }
}
