﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class representing a AssociationSet End</summary>
  public sealed class AssociationSetEnd : MetadataItem
  {
    private readonly EntitySet _entitySet;
    private readonly AssociationSet _parentSet;
    private readonly AssociationEndMember _endMember;

    internal AssociationSetEnd(
      EntitySet entitySet,
      AssociationSet parentSet,
      AssociationEndMember endMember)
    {
      this._entitySet = System.Data.Entity.Utilities.Check.NotNull<EntitySet>(entitySet, nameof (entitySet));
      this._parentSet = System.Data.Entity.Utilities.Check.NotNull<AssociationSet>(parentSet, nameof (parentSet));
      this._endMember = System.Data.Entity.Utilities.Check.NotNull<AssociationEndMember>(endMember, nameof (endMember));
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.AssociationSetEnd;

    /// <summary>
    /// Gets the parent association set of this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" /> object that represents the parent association set of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />
    /// .
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if Setter is called when the AssociationSetEnd instance is in ReadOnly state</exception>
    [MetadataProperty(BuiltInTypeKind.AssociationSet, false)]
    public AssociationSet ParentAssociationSet => this._parentSet;

    /// <summary>
    /// Gets the End member that this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" /> object corresponds to.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationEndMember" /> object that represents the End member that this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />
    /// object corresponds to.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if Setter is called when the AssociationSetEnd instance is in ReadOnly state</exception>
    [MetadataProperty(BuiltInTypeKind.AssociationEndMember, false)]
    public AssociationEndMember CorrespondingAssociationEndMember => this._endMember;

    /// <summary>
    /// Gets the name of the End for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </summary>
    /// <returns>
    /// The name of the End for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </returns>
    [MetadataProperty(PrimitiveTypeKind.String, false)]
    public string Name => this.CorrespondingAssociationEndMember.Name;

    /// <summary>
    /// Gets the name of the End role for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </summary>
    /// <returns>
    /// The name of the End role for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">Thrown if Setter is called when the AssociationSetEnd instance is in ReadOnly state</exception>
    [MetadataProperty(PrimitiveTypeKind.String, false)]
    [Obsolete("This property is going away, please use the Name property instead")]
    public string Role => this.Name;

    /// <summary>Gets the entity set referenced by this End role. </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.EntitySet" /> object that represents the entity set referred by this End role.
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.EntitySet, false)]
    public EntitySet EntitySet => this._entitySet;

    internal override string Identity => this.Name;

    /// <summary>
    /// Returns the name of the End role for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </summary>
    /// <returns>
    /// The name of the End role for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSetEnd" />.
    /// </returns>
    public override string ToString() => this.Name;

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      this.ParentAssociationSet?.SetReadOnly();
      this.CorrespondingAssociationEndMember?.SetReadOnly();
      this.EntitySet?.SetReadOnly();
    }
  }
}
