﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.DbExpressionRuleProcessingVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal abstract class DbExpressionRuleProcessingVisitor : DefaultExpressionVisitor
  {
    private bool _stopped;

    protected abstract IEnumerable<DbExpressionRule> GetRules();

    private static Tuple<DbExpression, DbExpressionRule.ProcessedAction> ProcessRules(
      DbExpression expression,
      List<DbExpressionRule> rules)
    {
      for (int index = 0; index < rules.Count; ++index)
      {
        DbExpressionRule rule = rules[index];
        DbExpression result;
        if (rule.ShouldProcess(expression) && rule.TryProcess(expression, out result))
        {
          if (rule.OnExpressionProcessed != DbExpressionRule.ProcessedAction.Continue)
            return Tuple.Create<DbExpression, DbExpressionRule.ProcessedAction>(result, rule.OnExpressionProcessed);
          expression = result;
        }
      }
      return Tuple.Create<DbExpression, DbExpressionRule.ProcessedAction>(expression, DbExpressionRule.ProcessedAction.Continue);
    }

    private DbExpression ApplyRules(DbExpression expression)
    {
      List<DbExpressionRule> list1 = this.GetRules().ToList<DbExpressionRule>();
      Tuple<DbExpression, DbExpressionRule.ProcessedAction> tuple;
      List<DbExpressionRule> list2;
      for (tuple = DbExpressionRuleProcessingVisitor.ProcessRules(expression, list1); tuple.Item2 == DbExpressionRule.ProcessedAction.Reset; tuple = DbExpressionRuleProcessingVisitor.ProcessRules(tuple.Item1, list2))
        list2 = this.GetRules().ToList<DbExpressionRule>();
      if (tuple.Item2 == DbExpressionRule.ProcessedAction.Stop)
        this._stopped = true;
      return tuple.Item1;
    }

    protected override DbExpression VisitExpression(DbExpression expression)
    {
      DbExpression expression1 = this.ApplyRules(expression);
      if (this._stopped)
        return expression1;
      DbExpression expression2 = base.VisitExpression(expression1);
      return this._stopped ? expression2 : this.ApplyRules(expression2);
    }
  }
}
