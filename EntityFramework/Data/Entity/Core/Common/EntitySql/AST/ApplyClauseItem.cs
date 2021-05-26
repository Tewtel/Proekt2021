// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.ApplyClauseItem
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class ApplyClauseItem : Node
  {
    private readonly FromClauseItem _applyLeft;
    private readonly FromClauseItem _applyRight;
    private readonly ApplyKind _applyKind;

    internal ApplyClauseItem(
      FromClauseItem applyLeft,
      FromClauseItem applyRight,
      ApplyKind applyKind)
    {
      this._applyLeft = applyLeft;
      this._applyRight = applyRight;
      this._applyKind = applyKind;
    }

    internal FromClauseItem LeftExpr => this._applyLeft;

    internal FromClauseItem RightExpr => this._applyRight;

    internal ApplyKind ApplyKind => this._applyKind;
  }
}
