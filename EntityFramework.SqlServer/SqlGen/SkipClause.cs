// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SkipClause
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Globalization;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SkipClause : ISqlFragment
  {
    private readonly ISqlFragment skipCount;

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
      writer.Write("OFFSET ");
      this.SkipCount.WriteSql(writer, sqlGenerator);
      writer.Write(" ROWS ");
    }
  }
}
