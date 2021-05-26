﻿// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SqlBuilder
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections.Generic;

namespace System.Data.SQLite.Linq
{
  internal sealed class SqlBuilder : ISqlFragment
  {
    private List<object> _sqlFragments;

    private List<object> sqlFragments
    {
      get
      {
        if (this._sqlFragments == null)
          this._sqlFragments = new List<object>();
        return this._sqlFragments;
      }
    }

    public void Append(object s) => this.sqlFragments.Add(s);

    public void AppendLine() => this.sqlFragments.Add((object) "\r\n");

    public bool IsEmpty => this._sqlFragments == null || 0 == this._sqlFragments.Count;

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      if (this._sqlFragments == null)
        return;
      foreach (object sqlFragment1 in this._sqlFragments)
      {
        switch (sqlFragment1)
        {
          case string str1:
            writer.Write(str1);
            continue;
          case ISqlFragment sqlFragment:
            sqlFragment.WriteSql(writer, sqlGenerator);
            continue;
          case char ch1:
            writer.Write(ch1);
            continue;
          default:
            throw new InvalidOperationException();
        }
      }
    }
  }
}
