// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.QueryStatement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class QueryStatement : Statement
  {
    private readonly NodeList<FunctionDefinition> _functionDefList;
    private readonly Node _expr;

    internal QueryStatement(NodeList<FunctionDefinition> functionDefList, Node expr)
    {
      this._functionDefList = functionDefList;
      this._expr = expr;
    }

    internal NodeList<FunctionDefinition> FunctionDefList => this._functionDefList;

    internal Node Expr => this._expr;
  }
}
