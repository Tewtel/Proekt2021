﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ColumnOrderingConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to apply column ordering specified via
  /// <see cref="T:System.ComponentModel.DataAnnotations.ColumnAttribute" />
  /// or the <see cref="T:System.Data.Entity.DbModelBuilder" /> API.
  /// </summary>
  public class ColumnOrderingConvention : IStoreModelConvention<EntityType>, IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(EntityType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      this.ValidateColumns(item, model.StoreModel.GetEntitySet(item).Table);
      ColumnOrderingConvention.OrderColumns((IEnumerable<EdmProperty>) item.Properties).Each<EdmProperty>((Action<EdmProperty>) (c =>
      {
        int num = c.IsPrimaryKeyColumn ? 1 : 0;
        item.RemoveMember((EdmMember) c);
        item.AddMember((EdmMember) c);
        if (num == 0)
          return;
        item.AddKeyMember((EdmMember) c);
      }));
      item.ForeignKeyBuilders.Each<ForeignKeyBuilder, IEnumerable<EdmProperty>>((Func<ForeignKeyBuilder, IEnumerable<EdmProperty>>) (fk => fk.DependentColumns = ColumnOrderingConvention.OrderColumns(fk.DependentColumns)));
    }

    /// <summary>
    /// Validates the ordering configuration supplied for columns.
    /// This base implementation is a no-op.
    /// </summary>
    /// <param name="table">The name of the table that the columns belong to.</param>
    /// <param name="tableName">The definition of the table.</param>
    protected virtual void ValidateColumns(EntityType table, string tableName)
    {
    }

    private static IEnumerable<EdmProperty> OrderColumns(
      IEnumerable<EdmProperty> columns)
    {
      return (IEnumerable<EdmProperty>) columns.Select(c => new
      {
        Column = c,
        Order = c.GetOrder() ?? int.MaxValue
      }).OrderBy(c => c.Order).Select(c => c.Column).ToList<EdmProperty>();
    }
  }
}
