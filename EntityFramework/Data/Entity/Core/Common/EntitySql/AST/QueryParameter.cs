// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.QueryParameter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class QueryParameter : Node
  {
    private readonly string _name;

    internal QueryParameter(string parameterName, string query, int inputPos)
      : base(query, inputPos)
    {
      this._name = parameterName.Substring(1);
      if (this._name.StartsWith("_", StringComparison.OrdinalIgnoreCase) || char.IsDigit(this._name, 0))
        throw EntitySqlException.Create(this.ErrCtx, Strings.InvalidParameterFormat((object) this._name), (Exception) null);
    }

    internal string Name => this._name;
  }
}
