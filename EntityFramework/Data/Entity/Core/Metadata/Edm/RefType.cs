﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RefType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Text;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class representing a ref type</summary>
  public class RefType : EdmType
  {
    private readonly EntityTypeBase _elementType;

    internal RefType()
    {
    }

    internal RefType(EntityType entityType)
      : base(RefType.GetIdentity((EntityTypeBase) System.Data.Entity.Utilities.Check.NotNull<EntityType>(entityType, nameof (entityType))), "Transient", entityType.DataSpace)
    {
      this._elementType = (EntityTypeBase) entityType;
      this.SetReadOnly();
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.RefType" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.RefType" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.RefType;

    /// <summary>
    /// Gets the entity type referenced by this <see cref="T:System.Data.Entity.Core.Metadata.Edm.RefType" />.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntityTypeBase" /> object that represents the entity type referenced by this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.RefType" />
    /// .
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.EntityTypeBase, false)]
    public virtual EntityTypeBase ElementType => this._elementType;

    private static string GetIdentity(EntityTypeBase entityTypeBase)
    {
      StringBuilder builder = new StringBuilder(50);
      builder.Append("reference[");
      entityTypeBase.BuildIdentity(builder);
      builder.Append("]");
      return builder.ToString();
    }

    /// <inheritdoc />
    public override int GetHashCode() => this._elementType.GetHashCode() * 397 ^ typeof (RefType).GetHashCode();

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is RefType refType && refType._elementType == this._elementType;
  }
}
