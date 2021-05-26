// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.AssociationTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents the Mapping metadata for an association type map in CS space.
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
  /// --ComplexTypeMap
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
  /// This class represents the metadata for all association Type map elements in the
  /// above example. Users can access the table mapping fragments under the
  /// association type mapping through this class.
  /// </example>
  public class AssociationTypeMapping : TypeMapping
  {
    private readonly AssociationSetMapping _associationSetMapping;
    private MappingFragment _mappingFragment;
    private readonly AssociationType m_relation;

    /// <summary>Creates an AssociationTypeMapping instance.</summary>
    /// <param name="associationSetMapping">The AssociationSetMapping that
    /// the contains this AssociationTypeMapping.</param>
    public AssociationTypeMapping(AssociationSetMapping associationSetMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationSetMapping>(associationSetMapping, nameof (associationSetMapping));
      this._associationSetMapping = associationSetMapping;
      this.m_relation = associationSetMapping.AssociationSet.ElementType;
    }

    internal AssociationTypeMapping(
      AssociationType relation,
      AssociationSetMapping associationSetMapping)
    {
      this._associationSetMapping = associationSetMapping;
      this.m_relation = relation;
    }

    /// <summary>
    /// Gets the AssociationSetMapping that contains this AssociationTypeMapping.
    /// </summary>
    public AssociationSetMapping AssociationSetMapping => this._associationSetMapping;

    internal override EntitySetBaseMapping SetMapping => (EntitySetBaseMapping) this.AssociationSetMapping;

    /// <summary>Gets the association type being mapped.</summary>
    public AssociationType AssociationType => this.m_relation;

    /// <summary>Gets the single mapping fragment.</summary>
    public MappingFragment MappingFragment
    {
      get => this._mappingFragment;
      internal set => this._mappingFragment = value;
    }

    internal override ReadOnlyCollection<MappingFragment> MappingFragments
    {
      get
      {
        if (this._mappingFragment == null)
          return new ReadOnlyCollection<MappingFragment>((IList<MappingFragment>) new MappingFragment[0]);
        return new ReadOnlyCollection<MappingFragment>((IList<MappingFragment>) new MappingFragment[1]
        {
          this._mappingFragment
        });
      }
    }

    internal override ReadOnlyCollection<EntityTypeBase> Types => new ReadOnlyCollection<EntityTypeBase>((IList<EntityTypeBase>) new AssociationType[1]
    {
      this.m_relation
    });

    internal override ReadOnlyCollection<EntityTypeBase> IsOfTypes => new ReadOnlyCollection<EntityTypeBase>((IList<EntityTypeBase>) new List<EntityTypeBase>());

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((MappingItem) this._mappingFragment);
      base.SetReadOnly();
    }
  }
}
