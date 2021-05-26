// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.EntityTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Mapping metadata for Entity type.
  /// If an EntitySet represents entities of more than one type, than we will have
  /// more than one EntityTypeMapping for an EntitySet( For ex : if
  /// PersonSet Entity extent represents entities of types Person and Customer,
  /// than we will have two EntityType Mappings under mapping for PersonSet).
  /// </summary>
  /// <example>
  /// For Example if conceptually you could represent the CS MSL file as following
  /// --Mapping
  /// --EntityContainerMapping ( CNorthwind--&gt;SNorthwind )
  /// --EntitySetMapping
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityKey
  /// --ScalarPropertyMap
  /// --ScalarPropertyMap
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityKey
  /// --ScalarPropertyMap
  /// --ComplexPropertyMap
  /// --ScalarPropertyMap
  /// --ScalarPropertyMap
  /// --ScalarPropertyMap
  /// --AssociationSetMapping
  /// --AssociationTypeMapping
  /// --MappingFragment
  /// --EndPropertyMap
  /// --ScalarPropertyMap
  /// --ScalarPropertyMap
  /// --EndPropertyMap
  /// --ScalarPropertyMap
  /// This class represents the metadata for all entity Type map elements in the
  /// above example. Users can access the table mapping fragments under the
  /// entity type mapping through this class.
  /// </example>
  public class EntityTypeMapping : TypeMapping
  {
    private readonly EntitySetMapping _entitySetMapping;
    private readonly List<MappingFragment> _fragments;
    private readonly Dictionary<string, EntityType> m_entityTypes = new Dictionary<string, EntityType>((IEqualityComparer<string>) StringComparer.Ordinal);
    private readonly Dictionary<string, EntityType> m_isOfEntityTypes = new Dictionary<string, EntityType>((IEqualityComparer<string>) StringComparer.Ordinal);
    private EntityType _entityType;

    /// <summary>Creates an EntityTypeMapping instance.</summary>
    /// <param name="entitySetMapping">The EntitySetMapping that contains this EntityTypeMapping.</param>
    public EntityTypeMapping(EntitySetMapping entitySetMapping)
    {
      this._entitySetMapping = entitySetMapping;
      this._fragments = new List<MappingFragment>();
    }

    /// <summary>
    /// Gets the EntitySetMapping that contains this EntityTypeMapping.
    /// </summary>
    public EntitySetMapping EntitySetMapping => this._entitySetMapping;

    internal override EntitySetBaseMapping SetMapping => (EntitySetBaseMapping) this.EntitySetMapping;

    /// <summary>
    /// Gets the single EntityType being mapped. Throws exception in case of hierarchy type mapping.
    /// </summary>
    public EntityType EntityType => this._entityType ?? (this._entityType = this.m_entityTypes.Values.SingleOrDefault<EntityType>());

    /// <summary>
    /// Gets a flag that indicates whether this is a type hierarchy mapping.
    /// </summary>
    public bool IsHierarchyMapping => this.m_isOfEntityTypes.Count > 0 || this.m_entityTypes.Count > 1;

    /// <summary>Gets a read-only collection of mapping fragments.</summary>
    public ReadOnlyCollection<MappingFragment> Fragments => new ReadOnlyCollection<MappingFragment>((IList<MappingFragment>) this._fragments);

    internal override ReadOnlyCollection<MappingFragment> MappingFragments => this.Fragments;

    /// <summary>Gets the mapped entity types.</summary>
    public ReadOnlyCollection<EntityTypeBase> EntityTypes => new ReadOnlyCollection<EntityTypeBase>((IList<EntityTypeBase>) new List<EntityTypeBase>((IEnumerable<EntityTypeBase>) this.m_entityTypes.Values));

    internal override ReadOnlyCollection<EntityTypeBase> Types => this.EntityTypes;

    /// <summary>Gets the mapped base types for a hierarchy mapping.</summary>
    public ReadOnlyCollection<EntityTypeBase> IsOfEntityTypes => new ReadOnlyCollection<EntityTypeBase>((IList<EntityTypeBase>) new List<EntityTypeBase>((IEnumerable<EntityTypeBase>) this.m_isOfEntityTypes.Values));

    internal override ReadOnlyCollection<EntityTypeBase> IsOfTypes => this.IsOfEntityTypes;

    /// <summary>Adds an entity type to the mapping.</summary>
    /// <param name="type">The EntityType to be added.</param>
    public void AddType(EntityType type)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(type, nameof (type));
      this.ThrowIfReadOnly();
      this.m_entityTypes.Add(type.FullName, type);
    }

    /// <summary>Removes an entity type from the mapping.</summary>
    /// <param name="type">The EntityType to be removed.</param>
    public void RemoveType(EntityType type)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(type, nameof (type));
      this.ThrowIfReadOnly();
      this.m_entityTypes.Remove(type.FullName);
    }

    /// <summary>
    /// Adds an entity type hierarchy to the mapping.
    /// The hierarchy is represented by the specified root entity type.
    /// </summary>
    /// <param name="type">The root EntityType of the hierarchy to be added.</param>
    public void AddIsOfType(EntityType type)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(type, nameof (type));
      this.ThrowIfReadOnly();
      this.m_isOfEntityTypes.Add(type.FullName, type);
    }

    /// <summary>
    /// Removes an entity type hierarchy from the mapping.
    /// The hierarchy is represented by the specified root entity type.
    /// </summary>
    /// <param name="type">The root EntityType of the hierarchy to be removed.</param>
    public void RemoveIsOfType(EntityType type)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(type, nameof (type));
      this.ThrowIfReadOnly();
      this.m_isOfEntityTypes.Remove(type.FullName);
    }

    /// <summary>Adds a mapping fragment.</summary>
    /// <param name="fragment">The mapping fragment to be added.</param>
    public void AddFragment(MappingFragment fragment)
    {
      System.Data.Entity.Utilities.Check.NotNull<MappingFragment>(fragment, nameof (fragment));
      this.ThrowIfReadOnly();
      this._fragments.Add(fragment);
    }

    /// <summary>Removes a mapping fragment.</summary>
    /// <param name="fragment">The mapping fragment to be removed.</param>
    public void RemoveFragment(MappingFragment fragment)
    {
      System.Data.Entity.Utilities.Check.NotNull<MappingFragment>(fragment, nameof (fragment));
      this.ThrowIfReadOnly();
      this._fragments.Remove(fragment);
    }

    internal override void SetReadOnly()
    {
      this._fragments.TrimExcess();
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._fragments);
      base.SetReadOnly();
    }

    internal EntityType GetContainerType(string memberName)
    {
      foreach (EntityType entityType in this.m_entityTypes.Values)
      {
        if (entityType.Properties.Contains(memberName))
          return entityType;
      }
      foreach (EntityType entityType in this.m_isOfEntityTypes.Values)
      {
        if (entityType.Properties.Contains(memberName))
          return entityType;
      }
      return (EntityType) null;
    }
  }
}
