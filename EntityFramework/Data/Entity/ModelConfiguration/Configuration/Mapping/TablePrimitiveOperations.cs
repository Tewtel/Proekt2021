// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.TablePrimitiveOperations
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  internal static class TablePrimitiveOperations
  {
    public static void AddColumn(EntityType table, EdmProperty column)
    {
      if (table.Properties.Contains(column))
        return;
      if (!(column.GetConfiguration() is System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration configuration) || string.IsNullOrWhiteSpace(configuration.ColumnName))
      {
        string name = column.GetPreferredName() ?? column.Name;
        column.SetUnpreferredUniqueName(column.Name);
        column.Name = ((IEnumerable<INamedDataModelItem>) table.Properties).UniquifyName(name);
      }
      table.AddMember((EdmMember) column);
    }

    public static EdmProperty RemoveColumn(EntityType table, EdmProperty column)
    {
      if (!column.IsPrimaryKeyColumn)
        table.RemoveMember((EdmMember) column);
      return column;
    }

    public static EdmProperty IncludeColumn(
      EntityType table,
      EdmProperty templateColumn,
      Func<EdmProperty, bool> isCompatible,
      bool useExisting)
    {
      EdmProperty edmProperty = table.Properties.FirstOrDefault<EdmProperty>(isCompatible);
      templateColumn = edmProperty != null ? (useExisting || edmProperty.IsPrimaryKeyColumn ? edmProperty : templateColumn.Clone()) : templateColumn.Clone();
      TablePrimitiveOperations.AddColumn(table, templateColumn);
      return templateColumn;
    }

    public static Func<EdmProperty, bool> GetNameMatcher(string name) => (Func<EdmProperty, bool>) (c => string.Equals(c.Name, name, StringComparison.Ordinal));
  }
}
