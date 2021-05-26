// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.StructuralTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Specifies a structural type mapping.</summary>
  public abstract class StructuralTypeMapping : MappingItem
  {
    /// <summary>Gets a read-only collection of property mappings.</summary>
    public abstract ReadOnlyCollection<PropertyMapping> PropertyMappings { get; }

    /// <summary>
    /// Gets a read-only collection of property mapping conditions.
    /// </summary>
    public abstract ReadOnlyCollection<ConditionPropertyMapping> Conditions { get; }

    /// <summary>Adds a property mapping.</summary>
    /// <param name="propertyMapping">The property mapping to be added.</param>
    public abstract void AddPropertyMapping(PropertyMapping propertyMapping);

    /// <summary>Removes a property mapping.</summary>
    /// <param name="propertyMapping">The property mapping to be removed.</param>
    public abstract void RemovePropertyMapping(PropertyMapping propertyMapping);

    /// <summary>Adds a property mapping condition.</summary>
    /// <param name="condition">The property mapping condition to be added.</param>
    public abstract void AddCondition(ConditionPropertyMapping condition);

    /// <summary>Removes a property mapping condition.</summary>
    /// <param name="condition">The property mapping condition to be removed.</param>
    public abstract void RemoveCondition(ConditionPropertyMapping condition);
  }
}
