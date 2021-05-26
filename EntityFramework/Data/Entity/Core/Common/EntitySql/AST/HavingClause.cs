// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.HavingClause
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class HavingClause : Node
  {
    private readonly Node _havingExpr;
    private readonly uint _methodCallCount;

    internal HavingClause(Node havingExpr, uint methodCallCounter)
    {
      this._havingExpr = havingExpr;
      this._methodCallCount = methodCallCounter;
    }

    internal Node HavingPredicate => this._havingExpr;

    internal bool HasMethodCall => this._methodCallCount > 0U;
  }
}
