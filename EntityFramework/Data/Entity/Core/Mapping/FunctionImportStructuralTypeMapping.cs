// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportStructuralTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>Specifies a function import structural type mapping.</summary>
  public abstract class FunctionImportStructuralTypeMapping : MappingItem
  {
    internal readonly LineInfo LineInfo;
    internal readonly Collection<FunctionImportReturnTypePropertyMapping> ColumnsRenameList;

    internal FunctionImportStructuralTypeMapping(
      Collection<FunctionImportReturnTypePropertyMapping> columnsRenameList,
      LineInfo lineInfo)
    {
      this.ColumnsRenameList = columnsRenameList;
      this.LineInfo = lineInfo;
    }

    /// <summary>
    /// Gets the property mappings for the result type of a function import.
    /// </summary>
    public ReadOnlyCollection<FunctionImportReturnTypePropertyMapping> PropertyMappings => new ReadOnlyCollection<FunctionImportReturnTypePropertyMapping>((IList<FunctionImportReturnTypePropertyMapping>) this.ColumnsRenameList);

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this.ColumnsRenameList);
      base.SetReadOnly();
    }
  }
}
