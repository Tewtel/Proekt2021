// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ConditionPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Mapping metadata for Conditional property mapping on a type.
  /// Condition Property Mapping specifies a Condition either on the C side property or S side property.
  /// </summary>
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
  /// --ConditionPropertyMap ( constant value--&gt;SMemberMetadata )
  /// --EntityTypeMapping
  /// --MappingFragment
  /// --EntityKey
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ComplexPropertyMap
  /// --ComplexTypeMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ConditionPropertyMap ( constant value--&gt;SMemberMetadata )
  /// --AssociationSetMapping
  /// --AssociationTypeMapping
  /// --MappingFragment
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// --EndPropertyMap
  /// --ScalarPropertyMap ( CMemberMetadata--&gt;SMemberMetadata )
  /// This class represents the metadata for all the condition property map elements in the
  /// above example.
  /// </example>
  public class ConditionPropertyMapping : PropertyMapping
  {
    private EdmProperty _column;
    private readonly object _value;
    private readonly bool? _isNull;

    internal ConditionPropertyMapping(EdmProperty propertyOrColumn, object value, bool? isNull)
    {
      DataSpace dataSpace = propertyOrColumn.TypeUsage.EdmType.DataSpace;
      switch (dataSpace)
      {
        case DataSpace.CSpace:
          base.Property = propertyOrColumn;
          break;
        case DataSpace.SSpace:
          this._column = propertyOrColumn;
          break;
        default:
          throw new ArgumentException(Strings.MetadataItem_InvalidDataSpace((object) dataSpace, (object) typeof (EdmProperty).Name), nameof (propertyOrColumn));
      }
      this._value = value;
      this._isNull = isNull;
    }

    internal ConditionPropertyMapping(
      EdmProperty property,
      EdmProperty column,
      object value,
      bool? isNull)
      : base(property)
    {
      this._column = column;
      this._value = value;
      this._isNull = isNull;
    }

    internal object Value => this._value;

    internal bool? IsNull => this._isNull;

    /// <summary>
    /// Gets an EdmProperty that specifies the mapped property.
    /// </summary>
    public override EdmProperty Property
    {
      get => base.Property;
      internal set => base.Property = value;
    }

    /// <summary>Gets an EdmProperty that specifies the mapped column.</summary>
    public EdmProperty Column
    {
      get => this._column;
      internal set => this._column = value;
    }
  }
}
