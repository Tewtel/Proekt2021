// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.Internal.Patterns
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Common.CommandTrees.Internal
{
  internal static class Patterns
  {
    internal static Func<DbExpression, bool> And(
      Func<DbExpression, bool> pattern1,
      Func<DbExpression, bool> pattern2)
    {
      return (Func<DbExpression, bool>) (e => pattern1(e) && pattern2(e));
    }

    internal static Func<DbExpression, bool> And(
      Func<DbExpression, bool> pattern1,
      Func<DbExpression, bool> pattern2,
      Func<DbExpression, bool> pattern3)
    {
      return (Func<DbExpression, bool>) (e => pattern1(e) && pattern2(e) && pattern3(e));
    }

    internal static Func<DbExpression, bool> Or(
      Func<DbExpression, bool> pattern1,
      Func<DbExpression, bool> pattern2)
    {
      return (Func<DbExpression, bool>) (e => pattern1(e) || pattern2(e));
    }

    internal static Func<DbExpression, bool> Or(
      Func<DbExpression, bool> pattern1,
      Func<DbExpression, bool> pattern2,
      Func<DbExpression, bool> pattern3)
    {
      return (Func<DbExpression, bool>) (e => pattern1(e) || pattern2(e) || pattern3(e));
    }

    internal static Func<DbExpression, bool> AnyExpression => (Func<DbExpression, bool>) (e => true);

    internal static Func<IEnumerable<DbExpression>, bool> AnyExpressions => (Func<IEnumerable<DbExpression>, bool>) (elems => true);

    internal static Func<DbExpression, bool> MatchComplexType => (Func<DbExpression, bool>) (e => TypeSemantics.IsComplexType(e.ResultType));

    internal static Func<DbExpression, bool> MatchEntityType => (Func<DbExpression, bool>) (e => TypeSemantics.IsEntityType(e.ResultType));

    internal static Func<DbExpression, bool> MatchRowType => (Func<DbExpression, bool>) (e => TypeSemantics.IsRowType(e.ResultType));

    internal static Func<DbExpression, bool> MatchKind(DbExpressionKind kindToMatch) => (Func<DbExpression, bool>) (e => e.ExpressionKind == kindToMatch);

    internal static Func<IEnumerable<DbExpression>, bool> MatchForAll(
      Func<DbExpression, bool> elementPattern)
    {
      return (Func<IEnumerable<DbExpression>, bool>) (elems => elems.FirstOrDefault<DbExpression>((Func<DbExpression, bool>) (e => !elementPattern(e))) == null);
    }

    internal static Func<DbExpression, bool> MatchBinary() => (Func<DbExpression, bool>) (e => e is DbBinaryExpression);

    internal static Func<DbExpression, bool> MatchFilter(
      Func<DbExpression, bool> inputPattern,
      Func<DbExpression, bool> predicatePattern)
    {
      return (Func<DbExpression, bool>) (e =>
      {
        if (e.ExpressionKind != DbExpressionKind.Filter)
          return false;
        DbFilterExpression filterExpression = (DbFilterExpression) e;
        return inputPattern(filterExpression.Input.Expression) && predicatePattern(filterExpression.Predicate);
      });
    }

    internal static Func<DbExpression, bool> MatchProject(
      Func<DbExpression, bool> inputPattern,
      Func<DbExpression, bool> projectionPattern)
    {
      return (Func<DbExpression, bool>) (e =>
      {
        if (e.ExpressionKind != DbExpressionKind.Project)
          return false;
        DbProjectExpression projectExpression = (DbProjectExpression) e;
        return inputPattern(projectExpression.Input.Expression) && projectionPattern(projectExpression.Projection);
      });
    }

    internal static Func<DbExpression, bool> MatchCase(
      Func<IEnumerable<DbExpression>, bool> whenPattern,
      Func<IEnumerable<DbExpression>, bool> thenPattern,
      Func<DbExpression, bool> elsePattern)
    {
      return (Func<DbExpression, bool>) (e =>
      {
        if (e.ExpressionKind != DbExpressionKind.Case)
          return false;
        DbCaseExpression dbCaseExpression = (DbCaseExpression) e;
        return whenPattern((IEnumerable<DbExpression>) dbCaseExpression.When) && thenPattern((IEnumerable<DbExpression>) dbCaseExpression.Then) && elsePattern(dbCaseExpression.Else);
      });
    }

    internal static Func<DbExpression, bool> MatchNewInstance() => (Func<DbExpression, bool>) (e => e.ExpressionKind == DbExpressionKind.NewInstance);

    internal static Func<DbExpression, bool> MatchNewInstance(
      Func<IEnumerable<DbExpression>, bool> argumentsPattern)
    {
      return (Func<DbExpression, bool>) (e => e.ExpressionKind == DbExpressionKind.NewInstance && argumentsPattern((IEnumerable<DbExpression>) ((DbNewInstanceExpression) e).Arguments));
    }
  }
}
