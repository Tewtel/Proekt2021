// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.UpdateExpressionVisitor`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  internal abstract class UpdateExpressionVisitor<TReturn> : DbExpressionVisitor<TReturn>
  {
    protected abstract string VisitorName { get; }

    protected NotSupportedException ConstructNotSupportedException(
      DbExpression node)
    {
      return new NotSupportedException(Strings.Update_UnsupportedExpressionKind(node == null ? (object) (string) null : (object) node.ExpressionKind.ToString(), (object) this.VisitorName));
    }

    public override TReturn Visit(DbExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
      return expression != null ? expression.Accept<TReturn>((DbExpressionVisitor<TReturn>) this) : throw this.ConstructNotSupportedException(expression);
    }

    public override TReturn Visit(DbAndExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbApplyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbArithmeticExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbCaseExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbCastExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbComparisonExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbConstantExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbCrossJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbDerefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbDistinctExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbElementExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbExceptExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbFilterExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbFunctionExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbEntityRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbRefKeyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbGroupByExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbIntersectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbIsEmptyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbIsNullExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbIsOfExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbLikeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbLimitExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbNewInstanceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbNotExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbNullExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbOfTypeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbOrExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbInExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbParameterReferenceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbProjectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbPropertyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbQuantifierExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbRelationshipNavigationExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbSkipExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbSortExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbTreatExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbUnionAllExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbVariableReferenceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }

    public override TReturn Visit(DbScanExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));
      throw this.ConstructNotSupportedException((DbExpression) expression);
    }
  }
}
