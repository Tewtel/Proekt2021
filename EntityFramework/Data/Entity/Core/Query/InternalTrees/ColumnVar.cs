// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ColumnVar
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class ColumnVar : Var
  {
    private readonly ColumnMD m_columnMetadata;
    private readonly Table m_table;

    internal ColumnVar(int id, Table table, ColumnMD columnMetadata)
      : base(id, VarType.Column, columnMetadata.Type)
    {
      this.m_table = table;
      this.m_columnMetadata = columnMetadata;
    }

    internal Table Table => this.m_table;

    internal ColumnMD ColumnMetadata => this.m_columnMetadata;

    internal override bool TryGetName(out string name)
    {
      name = this.m_columnMetadata.Name;
      return true;
    }
  }
}
