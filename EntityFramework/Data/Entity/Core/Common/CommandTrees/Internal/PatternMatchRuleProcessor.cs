// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.PatternMatchRuleProcessor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal class PatternMatchRuleProcessor : DbExpressionRuleProcessingVisitor
  {
    private readonly ReadOnlyCollection<PatternMatchRule> ruleSet;

    private PatternMatchRuleProcessor(ReadOnlyCollection<PatternMatchRule> rules) => this.ruleSet = rules;

    private DbExpression Process(DbExpression expression)
    {
      expression = this.VisitExpression(expression);
      return expression;
    }

    protected override IEnumerable<DbExpressionRule> GetRules() => (IEnumerable<DbExpressionRule>) this.ruleSet;

    internal static Func<DbExpression, DbExpression> Create(
      params PatternMatchRule[] rules)
    {
      return new Func<DbExpression, DbExpression>(new PatternMatchRuleProcessor(new ReadOnlyCollection<PatternMatchRule>((IList<PatternMatchRule>) rules)).Process);
    }
  }
}
