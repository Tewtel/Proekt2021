// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SqlStringBuilder
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Text;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SqlStringBuilder
  {
    private readonly StringBuilder _sql;

    public SqlStringBuilder() => this._sql = new StringBuilder();

    public SqlStringBuilder(int capacity) => this._sql = new StringBuilder(capacity);

    public bool UpperCaseKeywords { get; set; }

    internal StringBuilder InnerBuilder => this._sql;

    public SqlStringBuilder AppendKeyword(string keyword)
    {
      this._sql.Append(this.UpperCaseKeywords ? keyword.ToUpperInvariant() : keyword.ToLowerInvariant());
      return this;
    }

    public SqlStringBuilder AppendLine()
    {
      this._sql.AppendLine();
      return this;
    }

    public SqlStringBuilder AppendLine(string s)
    {
      this._sql.AppendLine(s);
      return this;
    }

    public SqlStringBuilder Append(string s)
    {
      this._sql.Append(s);
      return this;
    }

    public int Length => this._sql.Length;

    public override string ToString() => this._sql.ToString();
  }
}
