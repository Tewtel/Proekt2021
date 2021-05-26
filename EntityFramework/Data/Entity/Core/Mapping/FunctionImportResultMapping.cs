// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportResultMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Represents a result mapping for a function import.</summary>
  public sealed class FunctionImportResultMapping : MappingItem
  {
    private readonly List<FunctionImportStructuralTypeMapping> _typeMappings = new List<FunctionImportStructuralTypeMapping>();

    /// <summary>Gets the type mappings.</summary>
    public ReadOnlyCollection<FunctionImportStructuralTypeMapping> TypeMappings => new ReadOnlyCollection<FunctionImportStructuralTypeMapping>((IList<FunctionImportStructuralTypeMapping>) this._typeMappings);

    /// <summary>Adds a type mapping.</summary>
    /// <param name="typeMapping">The type mapping to add.</param>
    public void AddTypeMapping(FunctionImportStructuralTypeMapping typeMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<FunctionImportStructuralTypeMapping>(typeMapping, nameof (typeMapping));
      this.ThrowIfReadOnly();
      this._typeMappings.Add(typeMapping);
    }

    /// <summary>Removes a type mapping.</summary>
    /// <param name="typeMapping">The type mapping to remove.</param>
    public void RemoveTypeMapping(FunctionImportStructuralTypeMapping typeMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<FunctionImportStructuralTypeMapping>(typeMapping, nameof (typeMapping));
      this.ThrowIfReadOnly();
      this._typeMappings.Remove(typeMapping);
    }

    internal override void SetReadOnly()
    {
      this._typeMappings.TrimExcess();
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._typeMappings);
      base.SetReadOnly();
    }

    internal List<FunctionImportStructuralTypeMapping> SourceList => this._typeMappings;
  }
}
