// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RelationshipEndMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Initializes a new instance of the RelationshipEndMember class
  /// </summary>
  public abstract class RelationshipEndMember : EdmMember
  {
    private OperationAction _deleteBehavior;
    private RelationshipMultiplicity _relationshipMultiplicity;

    internal RelationshipEndMember(
      string name,
      RefType endRefType,
      RelationshipMultiplicity multiplicity)
      : base(name, TypeUsage.Create((EdmType) endRefType, new FacetValues()
      {
        Nullable = (FacetValueContainer<bool?>) new bool?(false)
      }))
    {
      this._relationshipMultiplicity = multiplicity;
      this._deleteBehavior = OperationAction.None;
    }

    /// <summary>Gets the operational behavior of this relationship end member.</summary>
    /// <returns>
    /// One of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.OperationAction" /> values. The default is
    /// <see cref="P:System.Data.Entity.Core.Metadata.Edm.OperationAction.None" />
    /// .
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.OperationAction, true)]
    public OperationAction DeleteBehavior
    {
      get => this._deleteBehavior;
      set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._deleteBehavior = value;
      }
    }

    /// <summary>Gets the multiplicity of this relationship end member.</summary>
    /// <returns>
    /// One of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.RelationshipMultiplicity" /> values.
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.RelationshipMultiplicity, false)]
    public RelationshipMultiplicity RelationshipMultiplicity
    {
      get => this._relationshipMultiplicity;
      set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._relationshipMultiplicity = value;
      }
    }

    /// <summary>Access the EntityType of the EndMember in an association.</summary>
    /// <returns>The EntityType of the EndMember in an association.</returns>
    public EntityType GetEntityType() => this.TypeUsage == null ? (EntityType) null : (EntityType) ((RefType) this.TypeUsage.EdmType).ElementType;
  }
}
