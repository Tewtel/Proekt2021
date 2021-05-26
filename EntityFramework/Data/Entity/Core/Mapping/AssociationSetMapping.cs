// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.AssociationSetMapping
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
  /// Represents the Mapping metadata for an AssociationSet in CS space.
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
  /// This class represents the metadata for the AssociationSetMapping elements in the
  /// above example. And it is possible to access the AssociationTypeMap underneath it.
  /// There will be only one TypeMap under AssociationSetMap.
  /// </example>
  public class AssociationSetMapping : EntitySetBaseMapping
  {
    private readonly AssociationSet _associationSet;
    private AssociationTypeMapping _associationTypeMapping;
    private AssociationSetModificationFunctionMapping _modificationFunctionMapping;

    /// <summary>Initializes a new AssociationSetMapping instance.</summary>
    /// <param name="associationSet">The association set to be mapped.</param>
    /// <param name="storeEntitySet">The store entity set to be mapped.</param>
    /// <param name="containerMapping">The parent container mapping.</param>
    public AssociationSetMapping(
      AssociationSet associationSet,
      EntitySet storeEntitySet,
      EntityContainerMapping containerMapping)
      : base(containerMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationSet>(associationSet, nameof (associationSet));
      System.Data.Entity.Utilities.Check.NotNull<EntitySet>(storeEntitySet, nameof (storeEntitySet));
      this._associationSet = associationSet;
      this._associationTypeMapping = new AssociationTypeMapping(associationSet.ElementType, this);
      this._associationTypeMapping.MappingFragment = new MappingFragment(storeEntitySet, (TypeMapping) this._associationTypeMapping, false);
    }

    internal AssociationSetMapping(AssociationSet associationSet, EntitySet storeEntitySet)
      : this(associationSet, storeEntitySet, (EntityContainerMapping) null)
    {
    }

    internal AssociationSetMapping(
      AssociationSet associationSet,
      EntityContainerMapping containerMapping)
      : base(containerMapping)
    {
      this._associationSet = associationSet;
    }

    /// <summary>Gets the association set that is mapped.</summary>
    public AssociationSet AssociationSet => this._associationSet;

    internal override EntitySetBase Set => (EntitySetBase) this.AssociationSet;

    /// <summary>Gets the contained association type mapping.</summary>
    public AssociationTypeMapping AssociationTypeMapping
    {
      get => this._associationTypeMapping;
      internal set => this._associationTypeMapping = value;
    }

    internal override IEnumerable<TypeMapping> TypeMappings
    {
      get
      {
        yield return (TypeMapping) this._associationTypeMapping;
      }
    }

    /// <summary>
    /// Gets or sets the corresponding function mapping. Can be null.
    /// </summary>
    public AssociationSetModificationFunctionMapping ModificationFunctionMapping
    {
      get => this._modificationFunctionMapping;
      set
      {
        this.ThrowIfReadOnly();
        this._modificationFunctionMapping = value;
      }
    }

    /// <summary>Gets the store entity set that is mapped.</summary>
    public EntitySet StoreEntitySet
    {
      get => this.SingleFragment == null ? (EntitySet) null : this.SingleFragment.StoreEntitySet;
      internal set => this.SingleFragment.StoreEntitySet = value;
    }

    internal EntityType Table => this.StoreEntitySet == null ? (EntityType) null : this.StoreEntitySet.ElementType;

    /// <summary>Gets or sets the source end property mapping.</summary>
    public EndPropertyMapping SourceEndMapping
    {
      get => this.SingleFragment == null ? (EndPropertyMapping) null : this.SingleFragment.PropertyMappings.OfType<EndPropertyMapping>().FirstOrDefault<EndPropertyMapping>();
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<EndPropertyMapping>(value, nameof (value));
        this.ThrowIfReadOnly();
        this.SingleFragment.AddPropertyMapping((PropertyMapping) value);
      }
    }

    /// <summary>Gets or sets the target end property mapping.</summary>
    public EndPropertyMapping TargetEndMapping
    {
      get => this.SingleFragment == null ? (EndPropertyMapping) null : this.SingleFragment.PropertyMappings.OfType<EndPropertyMapping>().ElementAtOrDefault<EndPropertyMapping>(1);
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<EndPropertyMapping>(value, nameof (value));
        this.ThrowIfReadOnly();
        this.SingleFragment.AddPropertyMapping((PropertyMapping) value);
      }
    }

    /// <summary>Gets the property mapping conditions.</summary>
    public ReadOnlyCollection<ConditionPropertyMapping> Conditions => this.SingleFragment == null ? new ReadOnlyCollection<ConditionPropertyMapping>((IList<ConditionPropertyMapping>) new List<ConditionPropertyMapping>()) : this.SingleFragment.Conditions;

    private MappingFragment SingleFragment => this._associationTypeMapping == null ? (MappingFragment) null : this._associationTypeMapping.MappingFragment;

    /// <summary>Adds a property mapping condition.</summary>
    /// <param name="condition">The condition to add.</param>
    public void AddCondition(ConditionPropertyMapping condition)
    {
      System.Data.Entity.Utilities.Check.NotNull<ConditionPropertyMapping>(condition, nameof (condition));
      this.ThrowIfReadOnly();
      if (this.SingleFragment == null)
        return;
      this.SingleFragment.AddCondition(condition);
    }

    /// <summary>Removes a property mapping condition.</summary>
    /// <param name="condition">The property mapping condition to remove.</param>
    public void RemoveCondition(ConditionPropertyMapping condition)
    {
      System.Data.Entity.Utilities.Check.NotNull<ConditionPropertyMapping>(condition, nameof (condition));
      this.ThrowIfReadOnly();
      if (this.SingleFragment == null)
        return;
      this.SingleFragment.RemoveCondition(condition);
    }

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((MappingItem) this._associationTypeMapping);
      MappingItem.SetReadOnly((MappingItem) this._modificationFunctionMapping);
      base.SetReadOnly();
    }
  }
}
