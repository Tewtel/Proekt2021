// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.FreeVariableScopeEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class FreeVariableScopeEntry : ScopeEntry
  {
    private readonly DbVariableReferenceExpression _varRef;

    internal FreeVariableScopeEntry(DbVariableReferenceExpression varRef)
      : base(ScopeEntryKind.FreeVar)
    {
      this._varRef = varRef;
    }

    internal override DbExpression GetExpression(string refName, ErrorContext errCtx) => (DbExpression) this._varRef;
  }
}
