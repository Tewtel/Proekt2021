﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.EntityContainerMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Data.Entity.Core.Mapping.ViewGeneration.Validation;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents the Mapping metadata for the EntityContainer map in CS space.
  /// Only one EntityContainerMapping element is allowed in the MSL file for CS mapping.
  /// </summary>
  /// <example>
  ///     For Example if conceptually you could represent the CS MSL file as following
  ///     ---Mapping
  ///     --EntityContainerMapping ( CNorthwind--&gt;SNorthwind )
  ///     --EntitySetMapping
  ///     --AssociationSetMapping
  ///     The type represents the metadata for EntityContainerMapping element in the above example.
  ///     The EntitySetBaseMapping elements that are children of the EntityContainerMapping element
  ///     can be accessed through the properties on this type.
  /// </example>
  /// <remarks>
  ///     We currently assume that an Entity Container on the C side
  ///     is mapped to a single Entity Container in the S - space.
  /// </remarks>
  public class EntityContainerMapping : MappingBase
  {
    private readonly string identity;
    private readonly bool m_validate;
    private readonly bool m_generateUpdateViews;
    private readonly EntityContainer m_entityContainer;
    private readonly EntityContainer m_storageEntityContainer;
    private readonly Dictionary<string, EntitySetBaseMapping> m_entitySetMappings = new Dictionary<string, EntitySetBaseMapping>((IEqualityComparer<string>) StringComparer.Ordinal);
    private readonly Dictionary<string, EntitySetBaseMapping> m_associationSetMappings = new Dictionary<string, EntitySetBaseMapping>((IEqualityComparer<string>) StringComparer.Ordinal);
    private readonly Dictionary<EdmFunction, FunctionImportMapping> m_functionImportMappings = new Dictionary<EdmFunction, FunctionImportMapping>();
    private readonly StorageMappingItemCollection m_storageMappingItemCollection;
    private readonly Memoizer<InputForComputingCellGroups, OutputFromComputeCellGroups> m_memoizedCellGroupEvaluator;

    /// <summary>Initializes a new EntityContainerMapping instance.</summary>
    /// <param name="conceptualEntityContainer">The conceptual entity container to be mapped.</param>
    /// <param name="storeEntityContainer">The store entity container to be mapped.</param>
    /// <param name="mappingItemCollection">The parent mapping item collection.</param>
    /// <param name="generateUpdateViews">Flag indicating whether to generate update views.</param>
    public EntityContainerMapping(
      EntityContainer conceptualEntityContainer,
      EntityContainer storeEntityContainer,
      StorageMappingItemCollection mappingItemCollection,
      bool generateUpdateViews)
      : this(conceptualEntityContainer, storeEntityContainer, mappingItemCollection, true, generateUpdateViews)
    {
    }

    internal EntityContainerMapping(
      EntityContainer entityContainer,
      EntityContainer storageEntityContainer,
      StorageMappingItemCollection storageMappingItemCollection,
      bool validate,
      bool generateUpdateViews)
      : base(MetadataItem.MetadataFlags.CSSpace)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityContainer>(entityContainer, nameof (entityContainer));
      this.m_entityContainer = entityContainer;
      this.m_storageEntityContainer = storageEntityContainer;
      this.m_storageMappingItemCollection = storageMappingItemCollection;
      this.m_memoizedCellGroupEvaluator = new Memoizer<InputForComputingCellGroups, OutputFromComputeCellGroups>(new Func<InputForComputingCellGroups, OutputFromComputeCellGroups>(this.ComputeCellGroups), (IEqualityComparer<InputForComputingCellGroups>) new InputForComputingCellGroups());
      this.identity = entityContainer.Identity;
      this.m_validate = validate;
      this.m_generateUpdateViews = generateUpdateViews;
    }

    internal EntityContainerMapping(EntityContainer entityContainer)
      : this(entityContainer, (EntityContainer) null, (StorageMappingItemCollection) null, false, false)
    {
    }

    internal EntityContainerMapping()
    {
    }

    /// <summary>Gets the parent mapping item collection.</summary>
    public StorageMappingItemCollection MappingItemCollection => this.m_storageMappingItemCollection;

    internal StorageMappingItemCollection StorageMappingItemCollection => this.MappingItemCollection;

    /// <summary>Gets the type kind for this item</summary>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.MetadataItem;

    internal override MetadataItem EdmItem => (MetadataItem) this.m_entityContainer;

    internal override string Identity => this.identity;

    internal bool IsEmpty => this.m_entitySetMappings.Count == 0 && this.m_associationSetMappings.Count == 0;

    internal bool HasViews => this.HasMappingFragments() || this.AllSetMaps.Any<EntitySetBaseMapping>((Func<EntitySetBaseMapping, bool>) (setMap => setMap.QueryView != null));

    internal string SourceLocation { get; set; }

    /// <summary>Gets the conceptual entity container.</summary>
    public EntityContainer ConceptualEntityContainer => this.m_entityContainer;

    internal EntityContainer EdmEntityContainer => this.ConceptualEntityContainer;

    /// <summary>Gets the store entity container.</summary>
    public EntityContainer StoreEntityContainer => this.m_storageEntityContainer;

    internal EntityContainer StorageEntityContainer => this.StoreEntityContainer;

    internal ReadOnlyCollection<EntitySetBaseMapping> EntitySetMaps => new ReadOnlyCollection<EntitySetBaseMapping>((IList<EntitySetBaseMapping>) new List<EntitySetBaseMapping>((IEnumerable<EntitySetBaseMapping>) this.m_entitySetMappings.Values));

    /// <summary>Gets the entity set mappings.</summary>
    public virtual IEnumerable<EntitySetMapping> EntitySetMappings => this.EntitySetMaps.OfType<EntitySetMapping>();

    /// <summary>Gets the association set mappings.</summary>
    public virtual IEnumerable<AssociationSetMapping> AssociationSetMappings => this.RelationshipSetMaps.OfType<AssociationSetMapping>();

    /// <summary>Gets the function import mappings.</summary>
    public IEnumerable<FunctionImportMapping> FunctionImportMappings => (IEnumerable<FunctionImportMapping>) this.m_functionImportMappings.Values;

    internal ReadOnlyCollection<EntitySetBaseMapping> RelationshipSetMaps => new ReadOnlyCollection<EntitySetBaseMapping>((IList<EntitySetBaseMapping>) new List<EntitySetBaseMapping>((IEnumerable<EntitySetBaseMapping>) this.m_associationSetMappings.Values));

    internal IEnumerable<EntitySetBaseMapping> AllSetMaps => this.m_entitySetMappings.Values.Concat<EntitySetBaseMapping>((IEnumerable<EntitySetBaseMapping>) this.m_associationSetMappings.Values);

    internal int StartLineNumber { get; set; }

    internal int StartLinePosition { get; set; }

    internal bool Validate => this.m_validate;

    /// <summary>
    /// Gets a flag that indicates whether to generate the update views or not.
    /// </summary>
    public bool GenerateUpdateViews => this.m_generateUpdateViews;

    internal EntitySetBaseMapping GetEntitySetMapping(string setName)
    {
      EntitySetBaseMapping entitySetBaseMapping = (EntitySetBaseMapping) null;
      this.m_entitySetMappings.TryGetValue(setName, out entitySetBaseMapping);
      return entitySetBaseMapping;
    }

    internal EntitySetBaseMapping GetAssociationSetMapping(string setName)
    {
      EntitySetBaseMapping entitySetBaseMapping = (EntitySetBaseMapping) null;
      this.m_associationSetMappings.TryGetValue(setName, out entitySetBaseMapping);
      return entitySetBaseMapping;
    }

    internal IEnumerable<AssociationSetMapping> GetRelationshipSetMappingsFor(
      EntitySetBase edmEntitySet,
      EntitySetBase storeEntitySet)
    {
      return this.m_associationSetMappings.Values.Cast<AssociationSetMapping>().Where<AssociationSetMapping>((Func<AssociationSetMapping, bool>) (w => w.StoreEntitySet != null && w.StoreEntitySet == storeEntitySet)).Where<AssociationSetMapping>((Func<AssociationSetMapping, bool>) (associationSetMap => (associationSetMap.Set as AssociationSet).AssociationSetEnds.Any<AssociationSetEnd>((Func<AssociationSetEnd, bool>) (associationSetEnd => associationSetEnd.EntitySet == edmEntitySet))));
    }

    internal EntitySetBaseMapping GetSetMapping(string setName) => this.GetEntitySetMapping(setName) ?? this.GetAssociationSetMapping(setName);

    /// <summary>Adds an entity set mapping.</summary>
    /// <param name="setMapping">The entity set mapping to add.</param>
    public void AddSetMapping(EntitySetMapping setMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySetMapping>(setMapping, nameof (setMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      if (this.m_entitySetMappings.ContainsKey(setMapping.Set.Name))
        return;
      this.m_entitySetMappings.Add(setMapping.Set.Name, (EntitySetBaseMapping) setMapping);
    }

    /// <summary>Removes an association set mapping.</summary>
    /// <param name="setMapping">The association set mapping to remove.</param>
    public void RemoveSetMapping(EntitySetMapping setMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySetMapping>(setMapping, nameof (setMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      this.m_entitySetMappings.Remove(setMapping.Set.Name);
    }

    /// <summary>Adds an association set mapping.</summary>
    /// <param name="setMapping">The association set mapping to add.</param>
    public void AddSetMapping(AssociationSetMapping setMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationSetMapping>(setMapping, nameof (setMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      if (this.m_associationSetMappings.ContainsKey(setMapping.Set.Name))
        return;
      this.m_associationSetMappings.Add(setMapping.Set.Name, (EntitySetBaseMapping) setMapping);
    }

    /// <summary>Removes an association set mapping.</summary>
    /// <param name="setMapping">The association set mapping to remove.</param>
    public void RemoveSetMapping(AssociationSetMapping setMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationSetMapping>(setMapping, nameof (setMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      this.m_associationSetMappings.Remove(setMapping.Set.Name);
    }

    internal bool ContainsAssociationSetMapping(AssociationSet associationSet) => this.m_associationSetMappings.ContainsKey(associationSet.Name);

    /// <summary>Adds a function import mapping.</summary>
    /// <param name="functionImportMapping">The function import mapping to add.</param>
    public void AddFunctionImportMapping(FunctionImportMapping functionImportMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<FunctionImportMapping>(functionImportMapping, nameof (functionImportMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      this.m_functionImportMappings.Add(functionImportMapping.FunctionImport, functionImportMapping);
    }

    /// <summary>Removes a function import mapping.</summary>
    /// <param name="functionImportMapping">The function import mapping to remove.</param>
    public void RemoveFunctionImportMapping(FunctionImportMapping functionImportMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<FunctionImportMapping>(functionImportMapping, nameof (functionImportMapping));
      Util.ThrowIfReadOnly((MetadataItem) this);
      this.m_functionImportMappings.Remove(functionImportMapping.FunctionImport);
    }

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this.m_entitySetMappings.Values);
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this.m_associationSetMappings.Values);
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this.m_functionImportMappings.Values);
      base.SetReadOnly();
    }

    internal bool HasQueryViewForSetMap(string setName)
    {
      EntitySetBaseMapping setMapping = this.GetSetMapping(setName);
      return setMapping != null && setMapping.QueryView != null;
    }

    internal bool HasMappingFragments()
    {
      foreach (EntitySetBaseMapping allSetMap in this.AllSetMaps)
      {
        foreach (TypeMapping typeMapping in allSetMap.TypeMappings)
        {
          if (typeMapping.MappingFragments.Count > 0)
            return true;
        }
      }
      return false;
    }

    internal virtual bool TryGetFunctionImportMapping(
      EdmFunction functionImport,
      out FunctionImportMapping mapping)
    {
      return this.m_functionImportMappings.TryGetValue(functionImport, out mapping);
    }

    internal OutputFromComputeCellGroups GetCellgroups(
      InputForComputingCellGroups args)
    {
      return this.m_memoizedCellGroupEvaluator.Evaluate(args);
    }

    private OutputFromComputeCellGroups ComputeCellGroups(
      InputForComputingCellGroups args)
    {
      OutputFromComputeCellGroups computeCellGroups = new OutputFromComputeCellGroups();
      computeCellGroups.Success = true;
      CellCreator cellCreator = new CellCreator(args.ContainerMapping);
      computeCellGroups.Cells = cellCreator.GenerateCells();
      computeCellGroups.Identifiers = cellCreator.Identifiers;
      if (computeCellGroups.Cells.Count <= 0)
      {
        computeCellGroups.Success = false;
        return computeCellGroups;
      }
      computeCellGroups.ForeignKeyConstraints = ForeignConstraint.GetForeignConstraints(args.ContainerMapping.StorageEntityContainer);
      List<Set<Cell>> source = new CellPartitioner((IEnumerable<Cell>) computeCellGroups.Cells, (IEnumerable<ForeignConstraint>) computeCellGroups.ForeignKeyConstraints).GroupRelatedCells();
      computeCellGroups.CellGroups = source.Select<Set<Cell>, Set<Cell>>((Func<Set<Cell>, Set<Cell>>) (setOfCells => new Set<Cell>(setOfCells.Select<Cell, Cell>((Func<Cell, Cell>) (cell => new Cell(cell)))))).ToList<Set<Cell>>();
      return computeCellGroups;
    }
  }
}
