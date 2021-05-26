// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.EntitySetBaseMapping
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
  /// <summary>
  /// Represents the Mapping metadata for an Extent in CS space.
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
  /// This class represents the metadata for all the extent map elements in the
  /// above example namely EntitySetMapping, AssociationSetMapping and CompositionSetMapping.
  /// The EntitySetBaseMapping elements that are children of the EntityContainerMapping element
  /// can be accessed through the properties on this type.
  /// </example>
  public abstract class EntitySetBaseMapping : MappingItem
  {
    private readonly EntityContainerMapping _containerMapping;
    private string _queryView;
    private readonly Dictionary<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>, string> _typeSpecificQueryViews = new Dictionary<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>, string>((IEqualityComparer<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>>) Pair<EntitySetBase, Pair<EntityTypeBase, bool>>.PairComparer.Instance);

    internal EntitySetBaseMapping(EntityContainerMapping containerMapping) => this._containerMapping = containerMapping;

    /// <summary>Gets the parent container mapping.</summary>
    public EntityContainerMapping ContainerMapping => this._containerMapping;

    internal EntityContainerMapping EntityContainerMapping => this.ContainerMapping;

    /// <summary>
    /// Gets or sets the query view associated with this mapping.
    /// </summary>
    public string QueryView
    {
      get => this._queryView;
      set
      {
        this.ThrowIfReadOnly();
        this._queryView = value;
      }
    }

    internal abstract EntitySetBase Set { get; }

    internal abstract IEnumerable<TypeMapping> TypeMappings { get; }

    internal virtual bool HasNoContent
    {
      get
      {
        if (this.QueryView != null)
          return false;
        foreach (TypeMapping typeMapping in this.TypeMappings)
        {
          foreach (MappingFragment mappingFragment in typeMapping.MappingFragments)
          {
            if (mappingFragment.AllProperties.Any<PropertyMapping>())
              return false;
          }
        }
        return true;
      }
    }

    internal int StartLineNumber { get; set; }

    internal int StartLinePosition { get; set; }

    internal bool HasModificationFunctionMapping { get; set; }

    internal bool ContainsTypeSpecificQueryView(
      Pair<EntitySetBase, Pair<EntityTypeBase, bool>> key)
    {
      return this._typeSpecificQueryViews.ContainsKey(key);
    }

    internal void AddTypeSpecificQueryView(
      Pair<EntitySetBase, Pair<EntityTypeBase, bool>> key,
      string viewString)
    {
      this._typeSpecificQueryViews.Add(key, viewString);
    }

    internal ReadOnlyCollection<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>> GetTypeSpecificQVKeys() => new ReadOnlyCollection<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>>((IList<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>>) this._typeSpecificQueryViews.Keys.ToList<Pair<EntitySetBase, Pair<EntityTypeBase, bool>>>());

    internal string GetTypeSpecificQueryView(
      Pair<EntitySetBase, Pair<EntityTypeBase, bool>> key)
    {
      return this._typeSpecificQueryViews[key];
    }
  }
}
