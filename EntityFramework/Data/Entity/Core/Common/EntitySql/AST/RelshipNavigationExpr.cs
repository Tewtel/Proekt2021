// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.RelshipNavigationExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class RelshipNavigationExpr : Node
  {
    private readonly Node _refExpr;
    private readonly Node _relshipTypeName;
    private readonly Identifier _toEndIdentifier;
    private readonly Identifier _fromEndIdentifier;

    internal RelshipNavigationExpr(
      Node refExpr,
      Node relshipTypeName,
      Identifier toEndIdentifier,
      Identifier fromEndIdentifier)
    {
      this._refExpr = refExpr;
      this._relshipTypeName = relshipTypeName;
      this._toEndIdentifier = toEndIdentifier;
      this._fromEndIdentifier = fromEndIdentifier;
    }

    internal Node RefExpr => this._refExpr;

    internal Node TypeName => this._relshipTypeName;

    internal Identifier ToEndIdentifier => this._toEndIdentifier;

    internal Identifier FromEndIdentifier => this._fromEndIdentifier;
  }
}
