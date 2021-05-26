// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SqlBuilder
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SqlBuilder : ISqlFragment
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

    public virtual bool IsEmpty => this._sqlFragments == null || this._sqlFragments.Count == 0;

    public virtual void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
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
          default:
            throw new InvalidOperationException();
        }
      }
    }
  }
}
