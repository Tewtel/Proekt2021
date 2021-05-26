// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.PropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Mapping metadata for all types of property mappings.</summary>
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
  /// This class represents the metadata for all property map elements in the
  /// above example. This includes the scalar property maps, complex property maps
  /// and end property maps.
  /// </example>
  public abstract class PropertyMapping : MappingItem
  {
    private EdmProperty _property;

    internal PropertyMapping(EdmProperty property) => this._property = property;

    internal PropertyMapping()
    {
    }

    /// <summary>
    /// Gets an EdmProperty that specifies the mapped property.
    /// </summary>
    public virtual EdmProperty Property
    {
      get => this._property;
      internal set => this._property = value;
    }
  }
}
