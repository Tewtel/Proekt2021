// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.EndPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Mapping metadata for End property of an association.</summary>
  /// <example>
  /// For Example if conceptually you could represent the CS MSL file as following
  /// --Mapping
  /// --EntityContainerMapping ( CNorthwind--&gt;SNorthwind )
  /// --EntitySetMapping
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityKey
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityKey
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ComplexPropertyMap
  /// --ComplexTypeMapping
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --DiscriminatorPropertyMap ( constant value--&gt;SMemberMetadata )
  /// --ComplexTypeMapping
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --DiscriminatorPropertyMap ( constant value--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --AssociationSetMapping
  /// --AssociationTypeMapping
  /// --MappingFragment
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// This class represents the metadata for all the end property map elements in the
  /// above example. EndPropertyMaps provide mapping for each end of the association.
  /// </example>
  public class EndPropertyMapping : PropertyMapping
  {
    private AssociationEndMember _associationEnd;
    private readonly List<ScalarPropertyMapping> _properties = new List<ScalarPropertyMapping>();

    /// <summary>Creates an association end property mapping.</summary>
    /// <param name="associationEnd">An AssociationEndMember that specifies
    /// the association end to be mapped.</param>
    public EndPropertyMapping(AssociationEndMember associationEnd)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationEndMember>(associationEnd, nameof (associationEnd));
      this._associationEnd = associationEnd;
    }

    internal EndPropertyMapping()
    {
    }

    /// <summary>
    /// Gets an AssociationEndMember that specifies the mapped association end.
    /// </summary>
    public AssociationEndMember AssociationEnd
    {
      get => this._associationEnd;
      internal set => this._associationEnd = value;
    }

    /// <summary>
    /// Gets a ReadOnlyCollection of ScalarPropertyMapping that specifies the children
    /// of this association end property mapping.
    /// </summary>
    public ReadOnlyCollection<ScalarPropertyMapping> PropertyMappings => new ReadOnlyCollection<ScalarPropertyMapping>((IList<ScalarPropertyMapping>) this._properties);

    internal IEnumerable<EdmMember> StoreProperties => (IEnumerable<EdmMember>) this.PropertyMappings.Select<ScalarPropertyMapping, EdmProperty>((Func<ScalarPropertyMapping, EdmProperty>) (propertyMap => propertyMap.Column));

    /// <summary>Adds a child property-column mapping.</summary>
    /// <param name="propertyMapping">A ScalarPropertyMapping that specifies
    /// the property-column mapping to be added.</param>
    public void AddPropertyMapping(ScalarPropertyMapping propertyMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<ScalarPropertyMapping>(propertyMapping, nameof (propertyMapping));
      this.ThrowIfReadOnly();
      this._properties.Add(propertyMapping);
    }

    /// <summary>Removes a child property-column mapping.</summary>
    /// <param name="propertyMapping">A ScalarPropertyMapping that specifies
    /// the property-column mapping to be removed.</param>
    public void RemovePropertyMapping(ScalarPropertyMapping propertyMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<ScalarPropertyMapping>(propertyMapping, nameof (propertyMapping));
      this.ThrowIfReadOnly();
      this._properties.Remove(propertyMapping);
    }

    internal override void SetReadOnly()
    {
      this._properties.TrimExcess();
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._properties);
      base.SetReadOnly();
    }
  }
}
