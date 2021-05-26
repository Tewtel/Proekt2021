// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.SourceScopeEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class SourceScopeEntry : 
    ScopeEntry,
    IGroupExpressionExtendedInfo,
    IGetAlternativeName
  {
    private readonly string[] _alternativeName;
    private List<string> _propRefs;
    private DbExpression _varBasedExpression;
    private DbExpression _groupVarBasedExpression;
    private DbExpression _groupAggBasedExpression;

    internal SourceScopeEntry(DbVariableReferenceExpression varRef)
      : this(varRef, (string[]) null)
    {
    }

    internal SourceScopeEntry(DbVariableReferenceExpression varRef, string[] alternativeName)
      : base(ScopeEntryKind.SourceVar)
    {
      this._varBasedExpression = (DbExpression) varRef;
      this._alternativeName = alternativeName;
    }

    internal override DbExpression GetExpression(string refName, ErrorContext errCtx) => this._varBasedExpression;

    DbExpression IGroupExpressionExtendedInfo.GroupVarBasedExpression => this._groupVarBasedExpression;

    DbExpression IGroupExpressionExtendedInfo.GroupAggBasedExpression => this._groupAggBasedExpression;

    internal bool IsJoinClauseLeftExpr { get; set; }

    string[] IGetAlternativeName.AlternativeName => this._alternativeName;

    internal SourceScopeEntry AddParentVar(
      DbVariableReferenceExpression parentVarRef)
    {
      if (this._propRefs == null)
      {
        this._propRefs = new List<string>(2);
        this._propRefs.Add(((DbVariableReferenceExpression) this._varBasedExpression).VariableName);
      }
      this._varBasedExpression = (DbExpression) parentVarRef;
      for (int index = this._propRefs.Count - 1; index >= 0; --index)
        this._varBasedExpression = (DbExpression) this._varBasedExpression.Property(this._propRefs[index]);
      this._propRefs.Add(parentVarRef.VariableName);
      return this;
    }

    internal void ReplaceParentVar(DbVariableReferenceExpression parentVarRef)
    {
      if (this._propRefs == null)
      {
        this._varBasedExpression = (DbExpression) parentVarRef;
      }
      else
      {
        this._propRefs.RemoveAt(this._propRefs.Count - 1);
        this.AddParentVar(parentVarRef);
      }
    }

    internal void AdjustToGroupVar(
      DbVariableReferenceExpression parentVarRef,
      DbVariableReferenceExpression parentGroupVarRef,
      DbVariableReferenceExpression groupAggRef)
    {
      this.ReplaceParentVar(parentVarRef);
      this._groupVarBasedExpression = (DbExpression) parentGroupVarRef;
      this._groupAggBasedExpression = (DbExpression) groupAggRef;
      if (this._propRefs == null)
        return;
      for (int index = this._propRefs.Count - 2; index >= 0; --index)
      {
        this._groupVarBasedExpression = (DbExpression) this._groupVarBasedExpression.Property(this._propRefs[index]);
        this._groupAggBasedExpression = (DbExpression) this._groupAggBasedExpression.Property(this._propRefs[index]);
      }
    }

    internal void RollbackAdjustmentToGroupVar(DbVariableReferenceExpression pregroupParentVarRef)
    {
      this._groupVarBasedExpression = (DbExpression) null;
      this._groupAggBasedExpression = (DbExpression) null;
      this.ReplaceParentVar(pregroupParentVarRef);
    }
  }
}
