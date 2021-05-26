// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.WhenThenExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal class WhenThenExpr : Node
  {
    private readonly Node _whenExpr;
    private readonly Node _thenExpr;

    internal WhenThenExpr(Node whenExpr, Node thenExpr)
    {
      this._whenExpr = whenExpr;
      this._thenExpr = thenExpr;
    }

    internal Node WhenExpr => this._whenExpr;

    internal Node ThenExpr => this._thenExpr;
  }
}
