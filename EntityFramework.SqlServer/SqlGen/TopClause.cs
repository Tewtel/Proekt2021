// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.TopClause
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Globalization;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class TopClause : ISqlFragment
  {
    private readonly ISqlFragment topCount;
    private readonly bool withTies;

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
      writer.Write("TOP ");
      if (sqlGenerator.SqlVersion != SqlVersion.Sql8)
        writer.Write("(");
      this.TopCount.WriteSql(writer, sqlGenerator);
      if (sqlGenerator.SqlVersion != SqlVersion.Sql8)
        writer.Write(")");
      writer.Write(" ");
      if (!this.WithTies)
        return;
      writer.Write("WITH TIES ");
    }
  }
}
