// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.CollectionType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Text;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the Edm Collection Type</summary>
  public class CollectionType : EdmType
  {
    private readonly TypeUsage _typeUsage;

    internal CollectionType()
    {
    }

    internal CollectionType(EdmType elementType)
      : this(TypeUsage.Create(elementType))
    {
      this.DataSpace = elementType.DataSpace;
    }

    internal CollectionType(TypeUsage elementType)
      : base(CollectionType.GetIdentity(System.Data.Entity.Utilities.Check.NotNull<TypeUsage>(elementType, nameof (elementType))), "Transient", elementType.EdmType.DataSpace)
    {
      this._typeUsage = elementType;
      this.SetReadOnly();
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.CollectionType;

    /// <summary>
    /// Gets the instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> class that contains the type of the element that this current
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" />
    /// object includes and facets for that type.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.TypeUsage" /> class that contains the type of the element that this current
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.CollectionType" />
    /// object includes and facets for that type.
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.TypeUsage, false)]
    public virtual TypeUsage TypeUsage => this._typeUsage;

    private static string GetIdentity(TypeUsage typeUsage)
    {
      StringBuilder builder = new StringBuilder(50);
      builder.Append("collection[");
      typeUsage.BuildIdentity(builder);
      builder.Append("]");
      return builder.ToString();
    }

    internal override bool EdmEquals(MetadataItem item)
    {
      if (this == item)
        return true;
      return item != null && BuiltInTypeKind.CollectionType == item.BuiltInTypeKind && this.TypeUsage.EdmEquals((MetadataItem) ((CollectionType) item).TypeUsage);
    }
  }
}
