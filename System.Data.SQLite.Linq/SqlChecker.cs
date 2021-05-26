// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SqlChecker
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections.Generic;
using System.Data.Common.CommandTrees;

namespace System.Data.SQLite.Linq
{
  internal sealed class SqlChecker : DbExpressionVisitor<bool>
  {
    private SqlChecker()
    {
    }

    public override bool Visit(DbAndExpression expression) => this.VisitBinaryExpression((DbBinaryExpression) expression);

    public override bool Visit(DbApplyExpression expression) => throw new NotSupportedException("apply expression");

    public override bool Visit(DbArithmeticExpression expression) => this.VisitExpressionList(expression.Arguments);

    public override bool Visit(DbCaseExpression expression)
    {
      bool flag1 = this.VisitExpressionList(expression.When);
      bool flag2 = this.VisitExpressionList(expression.Then);
      bool flag3 = this.VisitExpression(expression.Else);
      return flag1 || flag2 || flag3;
    }

    public override bool Visit(DbCastExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbComparisonExpression expression) => this.VisitBinaryExpression((DbBinaryExpression) expression);

    public override bool Visit(DbConstantExpression expression) => false;

    public override bool Visit(DbCrossJoinExpression expression) => this.VisitExpressionBindingList(expression.Inputs);

    public override bool Visit(DbDerefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbDistinctExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbElementExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbEntityRefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbExceptExpression expression)
    {
      bool flag1 = this.VisitExpression(expression.Left);
      bool flag2 = this.VisitExpression(expression.Right);
      return flag1 || flag2;
    }

    public override bool Visit(DbExpression expression) => throw new NotSupportedException(expression.GetType().FullName);

    public override bool Visit(DbFilterExpression expression)
    {
      bool flag1 = this.VisitExpressionBinding(expression.Input);
      bool flag2 = this.VisitExpression(expression.Predicate);
      return flag1 || flag2;
    }

    public override bool Visit(DbFunctionExpression expression) => this.VisitExpressionList(expression.Arguments);

    public override bool Visit(DbGroupByExpression expression)
    {
      bool flag1 = this.VisitExpression(expression.Input.Expression);
      bool flag2 = this.VisitExpressionList(expression.Keys);
      bool flag3 = this.VisitAggregateList(expression.Aggregates);
      return flag1 || flag2 || flag3;
    }

    public override bool Visit(DbIntersectExpression expression)
    {
      bool flag1 = this.VisitExpression(expression.Left);
      bool flag2 = this.VisitExpression(expression.Right);
      return flag1 || flag2;
    }

    public override bool Visit(DbIsEmptyExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbIsNullExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbIsOfExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbJoinExpression expression)
    {
      bool flag1 = this.VisitExpressionBinding(expression.Left);
      bool flag2 = this.VisitExpressionBinding(expression.Right);
      bool flag3 = this.VisitExpression(expression.JoinCondition);
      return flag1 || flag2 || flag3;
    }

    public override bool Visit(DbLikeExpression expression)
    {
      bool flag1 = this.VisitExpression(expression.Argument);
      bool flag2 = this.VisitExpression(expression.Pattern);
      bool flag3 = this.VisitExpression(expression.Escape);
      return flag1 || flag2 || flag3;
    }

    public override bool Visit(DbLimitExpression expression) => this.VisitExpression(expression.Argument);

    public override bool Visit(DbNewInstanceExpression expression) => this.VisitExpressionList(expression.Arguments);

    public override bool Visit(DbNotExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbNullExpression expression) => false;

    public override bool Visit(DbOfTypeExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbOrExpression expression) => this.VisitBinaryExpression((DbBinaryExpression) expression);

    public override bool Visit(DbParameterReferenceExpression expression) => false;

    public override bool Visit(DbProjectExpression expression)
    {
      bool flag1 = this.VisitExpressionBinding(expression.Input);
      bool flag2 = this.VisitExpression(expression.Projection);
      return flag1 || flag2;
    }

    public override bool Visit(DbPropertyExpression expression) => this.VisitExpression(expression.Instance);

    public override bool Visit(DbQuantifierExpression expression)
    {
      bool flag1 = this.VisitExpressionBinding(expression.Input);
      bool flag2 = this.VisitExpression(expression.Predicate);
      return flag1 || flag2;
    }

    public override bool Visit(DbRefExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbRefKeyExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbRelationshipNavigationExpression expression) => this.VisitExpression(expression.NavigationSource);

    public override bool Visit(DbScanExpression expression) => false;

    public override bool Visit(DbSkipExpression expression)
    {
      this.VisitExpressionBinding(expression.Input);
      this.VisitSortClauseList(expression.SortOrder);
      this.VisitExpression(expression.Count);
      return true;
    }

    public override bool Visit(DbSortExpression expression)
    {
      bool flag1 = this.VisitExpressionBinding(expression.Input);
      bool flag2 = this.VisitSortClauseList(expression.SortOrder);
      return flag1 || flag2;
    }

    public override bool Visit(DbTreatExpression expression) => this.VisitUnaryExpression((DbUnaryExpression) expression);

    public override bool Visit(DbUnionAllExpression expression) => this.VisitBinaryExpression((DbBinaryExpression) expression);

    public override bool Visit(DbVariableReferenceExpression expression) => false;

    private bool VisitAggregate(DbAggregate aggregate) => this.VisitExpressionList(aggregate.Arguments);

    private bool VisitAggregateList(IList<DbAggregate> list) => SqlChecker.VisitList<DbAggregate>(new SqlChecker.ListElementHandler<DbAggregate>(this.VisitAggregate), list);

    private bool VisitBinaryExpression(DbBinaryExpression expr)
    {
      bool flag1 = this.VisitExpression(expr.Left);
      bool flag2 = this.VisitExpression(expr.Right);
      return flag1 || flag2;
    }

    private bool VisitExpression(DbExpression expression) => expression != null && expression.Accept<bool>((DbExpressionVisitor<bool>) this);

    private bool VisitExpressionBinding(DbExpressionBinding expressionBinding) => this.VisitExpression(expressionBinding.Expression);

    private bool VisitExpressionBindingList(IList<DbExpressionBinding> list) => SqlChecker.VisitList<DbExpressionBinding>(new SqlChecker.ListElementHandler<DbExpressionBinding>(this.VisitExpressionBinding), list);

    private bool VisitExpressionList(IList<DbExpression> list) => SqlChecker.VisitList<DbExpression>(new SqlChecker.ListElementHandler<DbExpression>(this.VisitExpression), list);

    private static bool VisitList<TElementType>(
      SqlChecker.ListElementHandler<TElementType> handler,
      IList<TElementType> list)
    {
      bool flag1 = false;
      foreach (TElementType element in (IEnumerable<TElementType>) list)
      {
        bool flag2 = handler(element);
        flag1 = flag1 || flag2;
      }
      return flag1;
    }

    private bool VisitSortClause(DbSortClause sortClause) => this.VisitExpression(sortClause.Expression);

    private bool VisitSortClauseList(IList<DbSortClause> list) => SqlChecker.VisitList<DbSortClause>(new SqlChecker.ListElementHandler<DbSortClause>(this.VisitSortClause), list);

    private bool VisitUnaryExpression(DbUnaryExpression expr) => this.VisitExpression(expr.Argument);

    private delegate bool ListElementHandler<TElementType>(TElementType element);
  }
}
