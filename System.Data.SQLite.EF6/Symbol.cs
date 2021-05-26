// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.Symbol
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.SQLite.EF6
{
  internal class Symbol : ISqlFragment
  {
    private Dictionary<string, Symbol> columns = new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private bool needsRenaming;
    private bool isUnnest;
    private string name;
    private string newName;
    private TypeUsage type;

    internal Dictionary<string, Symbol> Columns => this.columns;

    internal bool NeedsRenaming
    {
      get => this.needsRenaming;
      set => this.needsRenaming = value;
    }

    internal bool IsUnnest
    {
      get => this.isUnnest;
      set => this.isUnnest = value;
    }

    public string Name => this.name;

    public string NewName
    {
      get => this.newName;
      set => this.newName = value;
    }

    internal TypeUsage Type
    {
      get => this.type;
      set => this.type = value;
    }

    public Symbol(string name, TypeUsage type)
    {
      this.name = name;
      this.newName = name;
      this.Type = type;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      if (this.NeedsRenaming)
      {
        int allColumnName = sqlGenerator.AllColumnNames[this.NewName];
        string key;
        do
        {
          ++allColumnName;
          key = this.Name + allColumnName.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        while (sqlGenerator.AllColumnNames.ContainsKey(key));
        sqlGenerator.AllColumnNames[this.NewName] = allColumnName;
        this.NeedsRenaming = false;
        this.NewName = key;
        sqlGenerator.AllColumnNames[key] = 0;
      }
      writer.Write(SqlGenerator.QuoteIdentifier(this.NewName));
    }
  }
}
