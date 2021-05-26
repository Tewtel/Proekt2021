// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportEntityTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Represents a function import entity type mapping.</summary>
  public sealed class FunctionImportEntityTypeMapping : FunctionImportStructuralTypeMapping
  {
    private readonly ReadOnlyCollection<EntityType> _entityTypes;
    private readonly ReadOnlyCollection<EntityType> _isOfTypeEntityTypes;
    private readonly ReadOnlyCollection<FunctionImportEntityTypeMappingCondition> _conditions;

    /// <summary>
    /// Initializes a new FunctionImportEntityTypeMapping instance.
    /// </summary>
    /// <param name="isOfTypeEntityTypes">The entity types at the base of
    /// the type hierarchies to be mapped.</param>
    /// <param name="entityTypes">The entity types to be mapped.</param>
    /// <param name="properties">The property mappings for the result types of a function import.</param>
    /// <param name="conditions">The mapping conditions.</param>
    public FunctionImportEntityTypeMapping(
      IEnumerable<EntityType> isOfTypeEntityTypes,
      IEnumerable<EntityType> entityTypes,
      Collection<FunctionImportReturnTypePropertyMapping> properties,
      IEnumerable<FunctionImportEntityTypeMappingCondition> conditions)
      : this(System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EntityType>>(isOfTypeEntityTypes, nameof (isOfTypeEntityTypes)), System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EntityType>>(entityTypes, nameof (entityTypes)), System.Data.Entity.Utilities.Check.NotNull<IEnumerable<FunctionImportEntityTypeMappingCondition>>(conditions, nameof (conditions)), System.Data.Entity.Utilities.Check.NotNull<Collection<FunctionImportReturnTypePropertyMapping>>(properties, nameof (properties)), LineInfo.Empty)
    {
    }

    internal FunctionImportEntityTypeMapping(
      IEnumerable<EntityType> isOfTypeEntityTypes,
      IEnumerable<EntityType> entityTypes,
      IEnumerable<FunctionImportEntityTypeMappingCondition> conditions,
      Collection<FunctionImportReturnTypePropertyMapping> columnsRenameList,
      LineInfo lineInfo)
      : base(columnsRenameList, lineInfo)
    {
      this._isOfTypeEntityTypes = new ReadOnlyCollection<EntityType>((IList<EntityType>) isOfTypeEntityTypes.ToList<EntityType>());
      this._entityTypes = new ReadOnlyCollection<EntityType>((IList<EntityType>) entityTypes.ToList<EntityType>());
      this._conditions = new ReadOnlyCollection<FunctionImportEntityTypeMappingCondition>((IList<FunctionImportEntityTypeMappingCondition>) conditions.ToList<FunctionImportEntityTypeMappingCondition>());
    }

    /// <summary>Gets the entity types being mapped.</summary>
    public ReadOnlyCollection<EntityType> EntityTypes => this._entityTypes;

    /// <summary>
    /// Gets the entity types at the base of the hierarchies being mapped.
    /// </summary>
    public ReadOnlyCollection<EntityType> IsOfTypeEntityTypes => this._isOfTypeEntityTypes;

    /// <summary>Gets the mapping conditions.</summary>
    public ReadOnlyCollection<FunctionImportEntityTypeMappingCondition> Conditions => this._conditions;

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._conditions);
      base.SetReadOnly();
    }

    internal IEnumerable<EntityType> GetMappedEntityTypes(
      ItemCollection itemCollection)
    {
      return this.EntityTypes.Concat<EntityType>(this.IsOfTypeEntityTypes.SelectMany<EntityType, EntityType>((Func<EntityType, IEnumerable<EntityType>>) (entityType => MetadataHelper.GetTypeAndSubtypesOf((EdmType) entityType, itemCollection, false).Cast<EntityType>())));
    }

    internal IEnumerable<string> GetDiscriminatorColumns() => this.Conditions.Select<FunctionImportEntityTypeMappingCondition, string>((Func<FunctionImportEntityTypeMappingCondition, string>) (condition => condition.ColumnName));
  }
}
