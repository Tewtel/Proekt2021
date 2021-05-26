// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SkipClause
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Globalization;

namespace System.Data.SQLite.Linq
{
  internal class SkipClause : ISqlFragment
  {
    private ISqlFragment skipCount;

    internal ISqlFragment SkipCount => this.skipCount;

    internal SkipClause(ISqlFragment skipCount) => this.skipCount = skipCount;

    internal SkipClause(int skipCount)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) skipCount.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.skipCount = (ISqlFragment) sqlBuilder;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
      writer.Write(" OFFSET ");
      this.SkipCount.WriteSql(writer, sqlGenerator);
    }
  }
}
