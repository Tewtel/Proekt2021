// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SqlSelectStatement
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections.Generic;
using System.Globalization;

namespace System.Data.SQLite.Linq
{
  internal sealed class SqlSelectStatement : ISqlFragment
  {
    private bool isDistinct;
    private List<Symbol> allJoinExtents;
    private List<Symbol> fromExtents;
    private Dictionary<Symbol, bool> outerExtents;
    private TopClause top;
    private SkipClause skip;
    private SqlBuilder select = new SqlBuilder();
    private SqlBuilder from = new SqlBuilder();
    private SqlBuilder where;
    private SqlBuilder groupBy;
    private SqlBuilder orderBy;
    private bool isTopMost;

    internal bool IsDistinct
    {
      get => this.isDistinct;
      set => this.isDistinct = value;
    }

    internal List<Symbol> AllJoinExtents
    {
      get => this.allJoinExtents;
      set => this.allJoinExtents = value;
    }

    internal List<Symbol> FromExtents
    {
      get
      {
        if (this.fromExtents == null)
          this.fromExtents = new List<Symbol>();
        return this.fromExtents;
      }
    }

    internal Dictionary<Symbol, bool> OuterExtents
    {
      get
      {
        if (this.outerExtents == null)
          this.outerExtents = new Dictionary<Symbol, bool>();
        return this.outerExtents;
      }
    }

    internal TopClause Top
    {
      get => this.top;
      set => this.top = value;
    }

    internal SkipClause Skip
    {
      get => this.skip;
      set => this.skip = value;
    }

    internal SqlBuilder Select => this.select;

    internal SqlBuilder From => this.from;

    internal SqlBuilder Where
    {
      get
      {
        if (this.where == null)
          this.where = new SqlBuilder();
        return this.where;
      }
    }

    internal SqlBuilder GroupBy
    {
      get
      {
        if (this.groupBy == null)
          this.groupBy = new SqlBuilder();
        return this.groupBy;
      }
    }

    public SqlBuilder OrderBy
    {
      get
      {
        if (this.orderBy == null)
          this.orderBy = new SqlBuilder();
        return this.orderBy;
      }
    }

    internal bool IsTopMost
    {
      get => this.isTopMost;
      set => this.isTopMost = value;
    }

    public bool HaveOrderByLimitOrOffset() => this.orderBy != null && !this.orderBy.IsEmpty || (this.top != null || this.skip != null);

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      List<string> stringList = (List<string>) null;
      if (this.outerExtents != null && 0 < this.outerExtents.Count)
      {
        foreach (Symbol key in this.outerExtents.Keys)
        {
          if (key is JoinSymbol joinSymbol2)
          {
            foreach (Symbol flattenedExtent in joinSymbol2.FlattenedExtentList)
            {
              if (stringList == null)
                stringList = new List<string>();
              stringList.Add(flattenedExtent.NewName);
            }
          }
          else
          {
            if (stringList == null)
              stringList = new List<string>();
            stringList.Add(key.NewName);
          }
        }
      }
      List<Symbol> symbolList = this.AllJoinExtents ?? this.fromExtents;
      if (symbolList != null)
      {
        foreach (Symbol symbol in symbolList)
        {
          if (stringList != null && stringList.Contains(symbol.Name))
          {
            int allExtentName = sqlGenerator.AllExtentNames[symbol.Name];
            string key;
            do
            {
              ++allExtentName;
              key = symbol.Name + allExtentName.ToString((IFormatProvider) CultureInfo.InvariantCulture);
            }
            while (sqlGenerator.AllExtentNames.ContainsKey(key));
            sqlGenerator.AllExtentNames[symbol.Name] = allExtentName;
            symbol.NewName = key;
            sqlGenerator.AllExtentNames[key] = 0;
          }
          if (stringList == null)
            stringList = new List<string>();
          stringList.Add(symbol.NewName);
        }
      }
      ++writer.Indent;
      writer.Write("SELECT ");
      if (this.IsDistinct)
        writer.Write("DISTINCT ");
      if (this.select == null || this.Select.IsEmpty)
        writer.Write("*");
      else
        this.Select.WriteSql(writer, sqlGenerator);
      writer.WriteLine();
      writer.Write("FROM ");
      this.From.WriteSql(writer, sqlGenerator);
      if (this.where != null && !this.Where.IsEmpty)
      {
        writer.WriteLine();
        writer.Write("WHERE ");
        this.Where.WriteSql(writer, sqlGenerator);
      }
      if (this.groupBy != null && !this.GroupBy.IsEmpty)
      {
        writer.WriteLine();
        writer.Write("GROUP BY ");
        this.GroupBy.WriteSql(writer, sqlGenerator);
      }
      if (this.orderBy != null && !this.OrderBy.IsEmpty && (this.IsTopMost || this.Top != null))
      {
        writer.WriteLine();
        writer.Write("ORDER BY ");
        this.OrderBy.WriteSql(writer, sqlGenerator);
      }
      if (this.Top != null)
        this.Top.WriteSql(writer, sqlGenerator);
      if (this.skip != null)
        this.Skip.WriteSql(writer, sqlGenerator);
      --writer.Indent;
    }
  }
}
