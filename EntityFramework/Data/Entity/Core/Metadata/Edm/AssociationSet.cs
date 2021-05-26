// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.AssociationSet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Class for representing an Association set</summary>
  public sealed class AssociationSet : RelationshipSet
  {
    private readonly ReadOnlyMetadataCollection<AssociationSetEnd> _associationSetEnds = new ReadOnlyMetadataCollection<AssociationSetEnd>(new MetadataCollection<AssociationSetEnd>());

    internal AssociationSet(string name, AssociationType associationType)
      : base(name, (string) null, (string) null, (string) null, (RelationshipType) associationType)
    {
    }

    /// <summary>
    /// Gets the association related to this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationType" /> object that represents the association related to this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />
    /// .
    /// </returns>
    public AssociationType ElementType => (AssociationType) base.ElementType;

    /// <summary>
    /// Gets the ends of this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />.
    /// </summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1" /> that contains the ends of this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />
    /// .
    /// </returns>
    [MetadataProperty(BuiltInTypeKind.AssociationSetEnd, true)]
    public ReadOnlyMetadataCollection<AssociationSetEnd> AssociationSetEnds => this._associationSetEnds;

    internal EntitySet SourceSet
    {
      get => this.AssociationSetEnds.FirstOrDefault<AssociationSetEnd>()?.EntitySet;
      set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        AssociationSetEnd associationSetEnd = new AssociationSetEnd(value, this, this.ElementType.SourceEnd);
        if (this.AssociationSetEnds.Count == 0)
          this.AddAssociationSetEnd(associationSetEnd);
        else
          this.AssociationSetEnds.Source[0] = associationSetEnd;
      }
    }

    internal EntitySet TargetSet
    {
      get => this.AssociationSetEnds.ElementAtOrDefault<AssociationSetEnd>(1)?.EntitySet;
      set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        AssociationSetEnd associationSetEnd = new AssociationSetEnd(value, this, this.ElementType.TargetEnd);
        if (this.AssociationSetEnds.Count == 1)
          this.AddAssociationSetEnd(associationSetEnd);
        else
          this.AssociationSetEnds.Source[1] = associationSetEnd;
      }
    }

    internal AssociationEndMember SourceEnd
    {
      get
      {
        AssociationSetEnd associationSetEnd = this.AssociationSetEnds.FirstOrDefault<AssociationSetEnd>();
        return associationSetEnd == null ? (AssociationEndMember) null : this.ElementType.KeyMembers.OfType<AssociationEndMember>().SingleOrDefault<AssociationEndMember>((Func<AssociationEndMember, bool>) (e => e.Name == associationSetEnd.Name));
      }
    }

    internal AssociationEndMember TargetEnd
    {
      get
      {
        AssociationSetEnd associationSetEnd = this.AssociationSetEnds.ElementAtOrDefault<AssociationSetEnd>(1);
        return associationSetEnd == null ? (AssociationEndMember) null : this.ElementType.KeyMembers.OfType<AssociationEndMember>().SingleOrDefault<AssociationEndMember>((Func<AssociationEndMember, bool>) (e => e.Name == associationSetEnd.Name));
      }
    }

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.AssociationSet" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.AssociationSet;

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      this.AssociationSetEnds.Source.SetReadOnly();
    }

    internal void AddAssociationSetEnd(AssociationSetEnd associationSetEnd) => this.AssociationSetEnds.Source.Add(associationSetEnd);

    /// <summary>
    /// Creates a read-only AssociationSet instance from the specified parameters.
    /// </summary>
    /// <param name="name">The name of the association set.</param>
    /// <param name="type">The association type of the elements in the association set.</param>
    /// <param name="sourceSet">The entity set for the source association set end.</param>
    /// <param name="targetSet">The entity set for the target association set end.</param>
    /// <param name="metadataProperties">Metadata properties to be associated with the instance.</param>
    /// <returns>The newly created AssociationSet instance.</returns>
    /// <exception cref="T:System.ArgumentException">The specified name is null or empty.</exception>
    /// <exception cref="T:System.ArgumentNullException">The specified association type is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The entity type of one of the ends of the specified
    /// association type does not match the entity type of the corresponding entity set end.
    /// </exception>
    public static AssociationSet Create(
      string name,
      AssociationType type,
      EntitySet sourceSet,
      EntitySet targetSet,
      IEnumerable<MetadataProperty> metadataProperties)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(type, nameof (type));
      if (!AssociationSet.CheckEntitySetAgainstEndMember(sourceSet, type.SourceEnd) || !AssociationSet.CheckEntitySetAgainstEndMember(targetSet, type.TargetEnd))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.AssociationSet_EndEntityTypeMismatch);
      AssociationSet associationSet = new AssociationSet(name, type);
      if (sourceSet != null)
        associationSet.SourceSet = sourceSet;
      if (targetSet != null)
        associationSet.TargetSet = targetSet;
      if (metadataProperties != null)
        associationSet.AddMetadataProperties(metadataProperties);
      associationSet.SetReadOnly();
      return associationSet;
    }

    private static bool CheckEntitySetAgainstEndMember(
      EntitySet entitySet,
      AssociationEndMember endMember)
    {
      if (entitySet == null && endMember == null)
        return true;
      return entitySet != null && endMember != null && entitySet.ElementType == endMember.GetEntityType();
    }
  }
}
