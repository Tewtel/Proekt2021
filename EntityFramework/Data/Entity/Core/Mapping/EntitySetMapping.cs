// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.EntitySetMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents the Mapping metadata for an EntitySet in CS space.
  /// </summary>
  /// <example>
  /// For Example if conceptually you could represent the CS MSL file as following
  /// --Mapping
  /// --EntityContainerMapping ( CNorthwind--&gt;SNorthwind )
  /// --EntitySetMapping
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --AssociationSetMapping
  /// --AssociationTypeMapping
  /// --MappingFragment
  /// This class represents the metadata for the EntitySetMapping elements in the
  /// above example. And it is possible to access the EntityTypeMaps underneath it.
  /// </example>
  public class EntitySetMapping : EntitySetBaseMapping
  {
    private readonly EntitySet _entitySet;
    private readonly List<EntityTypeMapping> _entityTypeMappings;
    private readonly List<EntityTypeModificationFunctionMapping> _modificationFunctionMappings;
    private Lazy<List<AssociationSetEnd>> _implicitlyMappedAssociationSetEnds;

    /// <summary>Initializes a new EntitySetMapping instance.</summary>
    /// <param name="entitySet">The entity set to be mapped.</param>
    /// <param name="containerMapping">The parent container mapping.</param>
    public EntitySetMapping(EntitySet entitySet, EntityContainerMapping containerMapping)
      : base(containerMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySet>(entitySet, nameof (entitySet));
      this._entitySet = entitySet;
      this._entityTypeMappings = new List<EntityTypeMapping>();
      this._modificationFunctionMappings = new List<EntityTypeModificationFunctionMapping>();
      this._implicitlyMappedAssociationSetEnds = new Lazy<List<AssociationSetEnd>>(new Func<List<AssociationSetEnd>>(this.InitializeImplicitlyMappedAssociationSetEnds));
    }

    /// <summary>Gets the entity set that is mapped.</summary>
    public EntitySet EntitySet => this._entitySet;

    internal override EntitySetBase Set => (EntitySetBase) this.EntitySet;

    /// <summary>Gets the contained entity type mappings.</summary>
    public ReadOnlyCollection<EntityTypeMapping> EntityTypeMappings => new ReadOnlyCollection<EntityTypeMapping>((IList<EntityTypeMapping>) this._entityTypeMappings);

    internal override IEnumerable<TypeMapping> TypeMappings => (IEnumerable<TypeMapping>) this._entityTypeMappings;

    /// <summary>Gets the corresponding function mappings.</summary>
    public ReadOnlyCollection<EntityTypeModificationFunctionMapping> ModificationFunctionMappings => new ReadOnlyCollection<EntityTypeModificationFunctionMapping>((IList<EntityTypeModificationFunctionMapping>) this._modificationFunctionMappings);

    internal IEnumerable<AssociationSetEnd> ImplicitlyMappedAssociationSetEnds => (IEnumerable<AssociationSetEnd>) this._implicitlyMappedAssociationSetEnds.Value;

    internal override bool HasNoContent => this._modificationFunctionMappings.Count == 0 && base.HasNoContent;

    /// <summary>Adds a type mapping.</summary>
    /// <param name="typeMapping">The type mapping to add.</param>
    public void AddTypeMapping(EntityTypeMapping typeMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityTypeMapping>(typeMapping, nameof (typeMapping));
      this.ThrowIfReadOnly();
      this._entityTypeMappings.Add(typeMapping);
    }

    /// <summary>Removes a type mapping.</summary>
    /// <param name="typeMapping">The type mapping to remove.</param>
    public void RemoveTypeMapping(EntityTypeMapping typeMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityTypeMapping>(typeMapping, nameof (typeMapping));
      this.ThrowIfReadOnly();
      this._entityTypeMappings.Remove(typeMapping);
    }

    internal void ClearModificationFunctionMappings() => this._modificationFunctionMappings.Clear();

    /// <summary>Adds a function mapping.</summary>
    /// <param name="modificationFunctionMapping">The function mapping to add.</param>
    public void AddModificationFunctionMapping(
      EntityTypeModificationFunctionMapping modificationFunctionMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityTypeModificationFunctionMapping>(modificationFunctionMapping, nameof (modificationFunctionMapping));
      this.ThrowIfReadOnly();
      this._modificationFunctionMappings.Add(modificationFunctionMapping);
      if (!this._implicitlyMappedAssociationSetEnds.IsValueCreated)
        return;
      this._implicitlyMappedAssociationSetEnds = new Lazy<List<AssociationSetEnd>>(new Func<List<AssociationSetEnd>>(this.InitializeImplicitlyMappedAssociationSetEnds));
    }

    /// <summary>Removes a function mapping.</summary>
    /// <param name="modificationFunctionMapping">The function mapping to remove.</param>
    public void RemoveModificationFunctionMapping(
      EntityTypeModificationFunctionMapping modificationFunctionMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityTypeModificationFunctionMapping>(modificationFunctionMapping, nameof (modificationFunctionMapping));
      this.ThrowIfReadOnly();
      this._modificationFunctionMappings.Remove(modificationFunctionMapping);
      if (!this._implicitlyMappedAssociationSetEnds.IsValueCreated)
        return;
      this._implicitlyMappedAssociationSetEnds = new Lazy<List<AssociationSetEnd>>(new Func<List<AssociationSetEnd>>(this.InitializeImplicitlyMappedAssociationSetEnds));
    }

    internal override void SetReadOnly()
    {
      this._entityTypeMappings.TrimExcess();
      this._modificationFunctionMappings.TrimExcess();
      if (this._implicitlyMappedAssociationSetEnds.IsValueCreated)
        this._implicitlyMappedAssociationSetEnds.Value.TrimExcess();
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._entityTypeMappings);
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._modificationFunctionMappings);
      base.SetReadOnly();
    }

    [Conditional("DEBUG")]
    private void AssertModificationFunctionMappingInvariants(
      EntityTypeModificationFunctionMapping modificationFunctionMapping)
    {
      foreach (EntityTypeModificationFunctionMapping modificationFunctionMapping1 in this._modificationFunctionMappings)
        ;
    }

    private List<AssociationSetEnd> InitializeImplicitlyMappedAssociationSetEnds()
    {
      List<AssociationSetEnd> associationSetEndList = new List<AssociationSetEnd>();
      foreach (EntityTypeModificationFunctionMapping modificationFunctionMapping in this._modificationFunctionMappings)
      {
        if (modificationFunctionMapping.DeleteFunctionMapping != null)
          associationSetEndList.AddRange((IEnumerable<AssociationSetEnd>) modificationFunctionMapping.DeleteFunctionMapping.CollocatedAssociationSetEnds);
        if (modificationFunctionMapping.InsertFunctionMapping != null)
          associationSetEndList.AddRange((IEnumerable<AssociationSetEnd>) modificationFunctionMapping.InsertFunctionMapping.CollocatedAssociationSetEnds);
        if (modificationFunctionMapping.UpdateFunctionMapping != null)
          associationSetEndList.AddRange((IEnumerable<AssociationSetEnd>) modificationFunctionMapping.UpdateFunctionMapping.CollocatedAssociationSetEnds);
      }
      if (this.IsReadOnly)
        associationSetEndList.TrimExcess();
      return associationSetEndList;
    }
  }
}
