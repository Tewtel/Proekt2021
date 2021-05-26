// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.TopClause
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Globalization;

namespace System.Data.SQLite.Linq
{
  internal class TopClause : ISqlFragment
  {
    private ISqlFragment topCount;
    private bool withTies;

    internal bool WithTies => this.withTies;

    internal ISqlFragment TopCount => this.topCount;

    internal TopClause(ISqlFragment topCount, bool withTies)
    {
      this.topCount = topCount;
      this.withTies = withTies;
    }

    internal TopClause(int topCount, bool withTies)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) topCount.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.topCount = (ISqlFragment) sqlBuilder;
      this.withTies = withTies;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      writer.Write(" LIMIT ");
      this.TopCount.WriteSql(writer, sqlGenerator);
      if (this.WithTies)
        throw new NotSupportedException("WITH TIES");
    }
  }
}
