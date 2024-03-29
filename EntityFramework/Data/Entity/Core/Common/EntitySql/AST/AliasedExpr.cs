﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.AliasedExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class AliasedExpr : Node
  {
    private readonly Node _expr;
    private readonly Identifier _alias;

    internal AliasedExpr(Node expr, Identifier alias)
    {
      if (string.IsNullOrEmpty(alias.Name))
        throw EntitySqlException.Create(alias.ErrCtx, Strings.InvalidEmptyIdentifier, (Exception) null);
      this._expr = expr;
      this._alias = alias;
    }

    internal AliasedExpr(Node expr) => this._expr = expr;

    internal Node Expr => this._expr;

    internal Identifier Alias => this._alias;
  }
}
