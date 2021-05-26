// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ScalarPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Mapping metadata for scalar properties.</summary>
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
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --AssociationSetMapping
  /// --AssociationTypeMapping
  /// --MappingFragment
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// This class represents the metadata for all the scalar property map elements in the
  /// above example.
  /// </example>
  public class ScalarPropertyMapping : PropertyMapping
  {
    private EdmProperty _column;

    /// <summary>
    /// Creates a mapping between a simple property and a column.
    /// </summary>
    /// <param name="property">The property to be mapped.</param>
    /// <param name="column">The column to be mapped.</param>
    public ScalarPropertyMapping(EdmProperty property, EdmProperty column)
      : base(property)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(property, nameof (property));
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(column, nameof (column));
      if (!Helper.IsScalarType(property.TypeUsage.EdmType) || !Helper.IsPrimitiveType(column.TypeUsage.EdmType))
        throw new ArgumentException(Strings.StorageScalarPropertyMapping_OnlyScalarPropertiesAllowed);
      this._column = column;
    }

    /// <summary>Gets an EdmProperty that specifies the mapped column.</summary>
    public EdmProperty Column
    {
      get => this._column;
      internal set => this._column = value;
    }
  }
}
