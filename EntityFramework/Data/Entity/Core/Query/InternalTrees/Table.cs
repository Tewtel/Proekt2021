﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Table
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class Table
  {
    private readonly TableMD m_tableMetadata;
    private readonly VarList m_columns;
    private readonly VarVec m_referencedColumns;
    private readonly VarVec m_keys;
    private readonly VarVec m_nonnullableColumns;
    private readonly int m_tableId;

    internal Table(Command command, TableMD tableMetadata, int tableId)
    {
      this.m_tableMetadata = tableMetadata;
      this.m_columns = Command.CreateVarList();
      this.m_keys = command.CreateVarVec();
      this.m_nonnullableColumns = command.CreateVarVec();
      this.m_tableId = tableId;
      Dictionary<string, ColumnVar> dictionary = new Dictionary<string, ColumnVar>();
      foreach (ColumnMD column in tableMetadata.Columns)
      {
        ColumnVar columnVar = command.CreateColumnVar(this, column);
        dictionary[column.Name] = columnVar;
        if (!column.IsNullable)
          this.m_nonnullableColumns.Set((Var) columnVar);
      }
      foreach (ColumnMD key in tableMetadata.Keys)
        this.m_keys.Set((Var) dictionary[key.Name]);
      this.m_referencedColumns = command.CreateVarVec((IEnumerable<Var>) this.m_columns);
    }

    internal TableMD TableMetadata => this.m_tableMetadata;

    internal VarList Columns => this.m_columns;

    internal VarVec ReferencedColumns => this.m_referencedColumns;

    internal VarVec NonNullableColumns => this.m_nonnullableColumns;

    internal VarVec Keys => this.m_keys;

    internal int TableId => this.m_tableId;

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}::{1}", (object) this.m_tableMetadata, (object) this.TableId);
  }
}
