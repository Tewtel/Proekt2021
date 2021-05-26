// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.AssociationEndMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents a end of a Association Type</summary>
  public sealed class AssociationEndMember : RelationshipEndMember
  {
    private Func<RelationshipManager, RelatedEnd, RelatedEnd> _getRelatedEndMethod;

    internal AssociationEndMember(
      string name,
      RefType endRefType,
      RelationshipMultiplicity multiplicity)
      : base(name, endRefType, multiplicity)
    {
    }

    internal AssociationEndMember(string name, EntityType entityType)
      : base(name, new RefType(entityType), RelationshipMultiplicity.ZeroOrOne)
    {
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationEndMember" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationEndMember" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.AssociationEndMember;

    internal Func<RelationshipManager, RelatedEnd, RelatedEnd> GetRelatedEnd
    {
      get => this._getRelatedEndMethod;
      set => Interlocked.CompareExchange<Func<RelationshipManager, RelatedEnd, RelatedEnd>>(ref this._getRelatedEndMethod, value, (Func<RelationshipManager, RelatedEnd, RelatedEnd>) null);
    }

    /// <summary>Creates a read-only AssociationEndMember instance.</summary>
    /// <param name="name">The name of the association end member.</param>
    /// <param name="endRefType">The reference type for the end.</param>
    /// <param name="multiplicity">The multiplicity of the end.</param>
    /// <param name="deleteAction">Flag that indicates the delete behavior of the end.</param>
    /// <param name="metadataProperties">Metadata properties to be associated with the instance.</param>
    /// <returns>The newly created AssociationEndMember instance.</returns>
    /// <exception cref="T:System.ArgumentException">The specified name is null or empty.</exception>
    /// <exception cref="T:System.ArgumentNullException">The specified reference type is null.</exception>
    public static AssociationEndMember Create(
      string name,
      RefType endRefType,
      RelationshipMultiplicity multiplicity,
      OperationAction deleteAction,
      IEnumerable<MetadataProperty> metadataProperties)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<RefType>(endRefType, nameof (endRefType));
      AssociationEndMember associationEndMember = new AssociationEndMember(name, endRefType, multiplicity);
      associationEndMember.DeleteBehavior = deleteAction;
      if (metadataProperties != null)
        associationEndMember.AddMetadataProperties(metadataProperties);
      associationEndMember.SetReadOnly();
      return associationEndMember;
    }
  }
}
