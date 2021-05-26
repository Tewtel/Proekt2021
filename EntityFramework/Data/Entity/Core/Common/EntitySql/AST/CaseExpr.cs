// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.CaseExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class CaseExpr : Node
  {
    private readonly NodeList<WhenThenExpr> _whenThenExpr;
    private readonly Node _elseExpr;

    internal CaseExpr(NodeList<WhenThenExpr> whenThenExpr)
      : this(whenThenExpr, (Node) null)
    {
    }

    internal CaseExpr(NodeList<WhenThenExpr> whenThenExpr, Node elseExpr)
    {
      this._whenThenExpr = whenThenExpr;
      this._elseExpr = elseExpr;
    }

    internal NodeList<WhenThenExpr> WhenThenExprList => this._whenThenExpr;

    internal Node ElseExpr => this._elseExpr;
  }
}
