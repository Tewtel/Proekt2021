// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SkipClause
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Globalization;

namespace System.Data.SQLite.EF6
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
