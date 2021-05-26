﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ColumnOrderingConventionStrict
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to apply column ordering specified via
  /// <see cref="T:System.ComponentModel.DataAnnotations.ColumnAttribute" />
  /// or the <see cref="T:System.Data.Entity.DbModelBuilder" /> API. This convention throws if a duplicate configured column order
  /// is detected.
  /// </summary>
  public class ColumnOrderingConventionStrict : ColumnOrderingConvention
  {
    /// <summary>
    /// Validates the ordering configuration supplied for columns to ensure
    /// that the same ordinal was not supplied for two columns.
    /// </summary>
    /// <param name="table">The name of the table that the columns belong to.</param>
    /// <param name="tableName">The definition of the table.</param>
    protected override void ValidateColumns(EntityType table, string tableName)
    {
      if (table.Properties.Select<EdmProperty, int?>((Func<EdmProperty, int?>) (c => c.GetOrder())).Where<int?>((Func<int?, bool>) (o => o.HasValue)).GroupBy<int?, int?>((Func<int?, int?>) (o => o)).Any<IGrouping<int?, int?>>((Func<IGrouping<int?, int?>, bool>) (g => g.Count<int?>() > 1)))
        throw System.Data.Entity.Resources.Error.DuplicateConfiguredColumnOrder((object) tableName);
    }
  }
}
