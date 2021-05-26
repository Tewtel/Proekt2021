// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.PatternMatchRule
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal class PatternMatchRule : DbExpressionRule
  {
    private readonly Func<DbExpression, bool> isMatch;
    private readonly Func<DbExpression, DbExpression> process;
    private readonly DbExpressionRule.ProcessedAction processed;

    private PatternMatchRule(
      Func<DbExpression, bool> matchFunc,
      Func<DbExpression, DbExpression> processor,
      DbExpressionRule.ProcessedAction onProcessed)
    {
      this.isMatch = matchFunc;
      this.process = processor;
      this.processed = onProcessed;
    }

    internal override bool ShouldProcess(DbExpression expression) => this.isMatch(expression);

    internal override bool TryProcess(DbExpression expression, out DbExpression result)
    {
      result = this.process(expression);
      return result != null;
    }

    internal override DbExpressionRule.ProcessedAction OnExpressionProcessed => this.processed;

    internal static PatternMatchRule Create(
      Func<DbExpression, bool> matchFunc,
      Func<DbExpression, DbExpression> processor)
    {
      return PatternMatchRule.Create(matchFunc, processor, DbExpressionRule.ProcessedAction.Reset);
    }

    internal static PatternMatchRule Create(
      Func<DbExpression, bool> matchFunc,
      Func<DbExpression, DbExpression> processor,
      DbExpressionRule.ProcessedAction onProcessed)
    {
      return new PatternMatchRule(matchFunc, processor, onProcessed);
    }
  }
}
