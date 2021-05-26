// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportReturnTypeEntityTypeColumnsRenameBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  internal sealed class FunctionImportReturnTypeEntityTypeColumnsRenameBuilder
  {
    internal Dictionary<string, FunctionImportReturnTypeStructuralTypeColumnRenameMapping> ColumnRenameMapping;

    internal FunctionImportReturnTypeEntityTypeColumnsRenameBuilder(
      Dictionary<EntityType, Collection<FunctionImportReturnTypePropertyMapping>> isOfTypeEntityTypeColumnsRenameMapping,
      Dictionary<EntityType, Collection<FunctionImportReturnTypePropertyMapping>> entityTypeColumnsRenameMapping)
    {
      this.ColumnRenameMapping = new Dictionary<string, FunctionImportReturnTypeStructuralTypeColumnRenameMapping>();
      foreach (EntityType key in isOfTypeEntityTypeColumnsRenameMapping.Keys)
        this.SetStructuralTypeColumnsRename(key, isOfTypeEntityTypeColumnsRenameMapping[key], true);
      foreach (EntityType key in entityTypeColumnsRenameMapping.Keys)
        this.SetStructuralTypeColumnsRename(key, entityTypeColumnsRenameMapping[key], false);
    }

    private void SetStructuralTypeColumnsRename(
      EntityType entityType,
      Collection<FunctionImportReturnTypePropertyMapping> columnsRenameMapping,
      bool isTypeOf)
    {
      foreach (FunctionImportReturnTypePropertyMapping typePropertyMapping in columnsRenameMapping)
      {
        if (!this.ColumnRenameMapping.Keys.Contains<string>(typePropertyMapping.CMember))
          this.ColumnRenameMapping[typePropertyMapping.CMember] = new FunctionImportReturnTypeStructuralTypeColumnRenameMapping(typePropertyMapping.CMember);
        this.ColumnRenameMapping[typePropertyMapping.CMember].AddRename(new FunctionImportReturnTypeStructuralTypeColumn(typePropertyMapping.SColumn, (StructuralType) entityType, isTypeOf, typePropertyMapping.LineInfo));
      }
    }
  }
}
