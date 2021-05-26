// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.Symbol
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class Symbol : ISqlFragment
  {
    private Dictionary<string, Symbol> columns;
    private Dictionary<string, Symbol> outputColumns;
    private readonly string name;

    internal Dictionary<string, Symbol> Columns
    {
      get
      {
        if (this.columns == null)
          this.columns = new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        return this.columns;
      }
    }

    internal Dictionary<string, Symbol> OutputColumns
    {
      get
      {
        if (this.outputColumns == null)
          this.outputColumns = new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        return this.outputColumns;
      }
    }

    internal bool NeedsRenaming { get; set; }

    internal bool OutputColumnsRenamed { get; set; }

    public string Name => this.name;

    public string NewName { get; set; }

    internal TypeUsage Type { get; set; }

    public Symbol(string name, TypeUsage type)
    {
      this.name = name;
      this.NewName = name;
      this.Type = type;
    }

    public Symbol(
      string name,
      TypeUsage type,
      Dictionary<string, Symbol> outputColumns,
      bool outputColumnsRenamed)
    {
      this.name = name;
      this.NewName = name;
      this.Type = type;
      this.outputColumns = outputColumns;
      this.OutputColumnsRenamed = outputColumnsRenamed;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      if (this.NeedsRenaming)
      {
        int num;
        if (sqlGenerator.AllColumnNames.TryGetValue(this.NewName, out num))
        {
          string key;
          do
          {
            ++num;
            key = this.NewName + num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          while (sqlGenerator.AllColumnNames.ContainsKey(key));
          sqlGenerator.AllColumnNames[this.NewName] = num;
          this.NewName = key;
        }
        sqlGenerator.AllColumnNames[this.NewName] = 0;
        this.NeedsRenaming = false;
      }
      writer.Write(SqlGenerator.QuoteIdentifier(this.NewName));
    }
  }
}
