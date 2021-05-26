// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.Sql8ConformanceChecker
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.SqlServer.Resources;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class Sql8ConformanceChecker : DbExpressionVisitor<bool>
  {
    internal static bool NeedsRewrite(DbExpression expr)
    {
      Sql8ConformanceChecker conformanceChecker = new Sql8ConformanceChecker();
      return expr.Accept<bool>((DbExpressionVisitor<bool>) conformanceChecker);
    }

    private Sql8ConformanceChecker()
    {
    }

    private bool VisitUnaryExpression(DbUnaryExpression expr) => this.VisitExpression(expr.Argument);

    private bool VisitBinaryExpression(DbBinaryExpression expr) => this.VisitExpression(expr.Left) | this.VisitExpression(expr.Right);

    private bool VisitAggregate(DbAggregate aggregate) => this.VisitExpressionList(aggregate.Arguments);

    private bool VisitExpressionBinding(DbExpressionBinding expressionBinding) => this.VisitExpression(expressionBinding.Expression);

    private bool VisitExpression(DbExpression expression) => expression != null && expression.Accept<bool>((DbExpressionVisitor<bool>) this);

    private bool VisitSortClause(DbSortClause sortClause) => this.VisitExpression(sortClause.Expression);

    private static bool VisitList<TElementType>(
      Sql8ConformanceChecker.ListElementHandler<TElementType> handler,
      IList<TElementType> list)
    {
      bool flag1 = false;
      foreach (TElementType element in (IEnumerable<TElementType>) list)
      {
        bool flag2 = handler(element);
        flag1 |= flag2;
      }
      return flag1;
    }

    private bool VisitAggregateList(IList<DbAggregate> list) => Sql8ConformanceChecker.VisitList<DbAggregate>(new Sql8ConformanceChecker.ListElementHandler<DbAggregate>(this.VisitAggregate), list);

    private bool VisitExpressionBindingList(IList<DbExpressionBinding> list) => Sql8ConformanceChecker.VisitList<DbExpressionBinding>(new Sql8ConformanceChecker.ListElementHandler<DbExpressionBinding>(this.VisitExpressionBinding), list);

    private bool VisitExpressionList(IList<DbExpression> list) => Sql8ConformanceChecker.VisitList<DbExpression>(new Sql8ConformanceChecker.ListElementHandler<DbExpression>(this.VisitExpression), list);

    private bool VisitSortClauseList(IList<DbSortClause> list) => Sql8ConformanceChecker.VisitList<DbSortClause>(new Sql8ConformanceChecker.ListElementHandler<DbSortClause>(this.VisitSortClause), list);

    public override bool Visit(DbExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
      throw new NotSupportedException(Strings.Cqt_General_UnsupportedExpression((object) expression.GetType().FullName));
    }

    public override bool Visit(DbAndExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbAndExpression>(expression, nameof (expression));
      return this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    public override bool Visit(DbApplyExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbApplyExpression>(expression, nameof (expression));
      throw new NotSupportedException(Strings.SqlGen_ApplyNotSupportedOnSql8);
    }

    public override bool Visit(DbArithmeticExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbArithmeticExpression>(expression, nameof (expression));
      return this.VisitExpressionList(expression.Arguments);
    }

    public override bool Visit(DbCaseExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCaseExpression>(expression, nameof (expression));
      int num1 = this.VisitExpressionList(expression.When) ? 1 : 0;
      bool flag1 = this.VisitExpressionList(expression.Then);
      bool flag2 = this.VisitExpression(expression.Else);
      int num2 = flag1 ? 1 : 0;
      return (num1 | num2 | (flag2 ? 1 : 0)) != 0;
    }

    public override bool Visit(DbCastExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCastExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbComparisonExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbComparisonExpression>(expression, nameof (expression));
      return this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    public override bool Visit(DbConstantExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbConstantExpression>(expression, nameof (expression));
      return false;
    }

    public override bool Visit(DbCrossJoinExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbCrossJoinExpression>(expression, nameof (expression));
      return this.VisitExpressionBindingList(expression.Inputs);
    }

    public override bool Visit(DbDerefExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbDerefExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbDistinctExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbDistinctExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbElementExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbElementExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbEntityRefExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbEntityRefExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbExceptExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbExceptExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Left);
      this.VisitExpression(expression.Right);
      return true;
    }

    public override bool Visit(DbFilterExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbFilterExpression>(expression, nameof (expression));
      return this.VisitExpressionBinding(expression.Input) | this.VisitExpression(expression.Predicate);
    }

    public override bool Visit(DbFunctionExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
      return this.VisitExpressionList(expression.Arguments);
    }

    public override bool Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      return this.VisitExpressionList(expression.Arguments) | this.VisitExpression(expression.Lambda.Body);
    }

    public override bool Visit(DbGroupByExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbGroupByExpression>(expression, nameof (expression));
      int num1 = this.VisitExpression(expression.Input.Expression) ? 1 : 0;
      bool flag1 = this.VisitExpressionList(expression.Keys);
      bool flag2 = this.VisitAggregateList(expression.Aggregates);
      int num2 = flag1 ? 1 : 0;
      return (num1 | num2 | (flag2 ? 1 : 0)) != 0;
    }

    public override bool Visit(DbIntersectExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIntersectExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Left);
      this.VisitExpression(expression.Right);
      return true;
    }

    public override bool Visit(DbIsEmptyExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsEmptyExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbIsNullExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsNullExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbIsOfExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbIsOfExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbJoinExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbJoinExpression>(expression, nameof (expression));
      int num1 = this.VisitExpressionBinding(expression.Left) ? 1 : 0;
      bool flag1 = this.VisitExpressionBinding(expression.Right);
      bool flag2 = this.VisitExpression(expression.JoinCondition);
      int num2 = flag1 ? 1 : 0;
      return (num1 | num2 | (flag2 ? 1 : 0)) != 0;
    }

    public override bool Visit(DbLikeExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLikeExpression>(expression, nameof (expression));
      int num1 = this.VisitExpression(expression.Argument) ? 1 : 0;
      bool flag1 = this.VisitExpression(expression.Pattern);
      bool flag2 = this.VisitExpression(expression.Escape);
      int num2 = flag1 ? 1 : 0;
      return (num1 | num2 | (flag2 ? 1 : 0)) != 0;
    }

    public override bool Visit(DbLimitExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
      return !(expression.Limit is DbParameterReferenceExpression) ? this.VisitExpression(expression.Argument) : throw new NotSupportedException(Strings.SqlGen_ParameterForLimitNotSupportedOnSql8);
    }

    public override bool Visit(DbNewInstanceExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
      return this.VisitExpressionList(expression.Arguments);
    }

    public override bool Visit(DbNotExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNotExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbNullExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbNullExpression>(expression, nameof (expression));
      return false;
    }

    public override bool Visit(DbOfTypeExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbOfTypeExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbOrExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbOrExpression>(expression, nameof (expression));
      return this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    public override bool Visit(DbInExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbInExpression>(expression, nameof (expression));
      return this.VisitExpression(expression.Item) || this.VisitExpressionList(expression.List);
    }

    public override bool Visit(DbParameterReferenceExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));
      return false;
    }

    public override bool Visit(DbProjectExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbProjectExpression>(expression, nameof (expression));
      return this.VisitExpressionBinding(expression.Input) | this.VisitExpression(expression.Projection);
    }

    public override bool Visit(DbPropertyExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
      return this.VisitExpression(expression.Instance);
    }

    public override bool Visit(DbQuantifierExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbQuantifierExpression>(expression, nameof (expression));
      return this.VisitExpressionBinding(expression.Input) | this.VisitExpression(expression.Predicate);
    }

    public override bool Visit(DbRefExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRefExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbRefKeyExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRefKeyExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbRelationshipNavigationExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(expression, nameof (expression));
      return this.VisitExpression(expression.NavigationSource);
    }

    public override bool Visit(DbScanExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));
      return false;
    }

    public override bool Visit(DbSkipExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
      if (expression.Count is DbParameterReferenceExpression)
        throw new NotSupportedException(Strings.SqlGen_ParameterForSkipNotSupportedOnSql8);
      this.VisitExpressionBinding(expression.Input);
      this.VisitSortClauseList(expression.SortOrder);
      this.VisitExpression(expression.Count);
      return true;
    }

    public override bool Visit(DbSortExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbSortExpression>(expression, nameof (expression));
      return this.VisitExpressionBinding(expression.Input) | this.VisitSortClauseList(expression.SortOrder);
    }

    public override bool Visit(DbTreatExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbTreatExpression>(expression, nameof (expression));
      return this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    public override bool Visit(DbUnionAllExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbUnionAllExpression>(expression, nameof (expression));
      return this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    public override bool Visit(DbVariableReferenceExpression expression)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<DbVariableReferenceExpression>(expression, nameof (expression));
      return false;
    }

    private delegate bool ListElementHandler<TElementType>(TElementType element);
  }
}
