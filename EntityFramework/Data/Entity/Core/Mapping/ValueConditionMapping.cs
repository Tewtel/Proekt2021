// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ValueConditionMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Specifies a mapping condition evaluated by comparing the value of
  /// a property or column with a given value.
  /// </summary>
  public class ValueConditionMapping : ConditionPropertyMapping
  {
    /// <summary>Creates a ValueConditionMapping instance.</summary>
    /// <param name="propertyOrColumn">An EdmProperty that specifies a property or column.</param>
    /// <param name="value">An object that specifies the value to compare with.</param>
    public ValueConditionMapping(EdmProperty propertyOrColumn, object value)
      : base(System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(propertyOrColumn, nameof (propertyOrColumn)), System.Data.Entity.Utilities.Check.NotNull<object>(value, nameof (value)), new bool?())
    {
    }

    /// <summary>
    /// Gets an object that specifies the value to check against.
    /// </summary>
    public new object Value => base.Value;
  }
}
