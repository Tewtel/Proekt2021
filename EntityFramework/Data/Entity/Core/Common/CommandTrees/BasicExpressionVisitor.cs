// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.BasicExpressionVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// An abstract base type for types that implement the IExpressionVisitor interface to derive from.
  /// </summary>
  public abstract class BasicExpressionVisitor : DbExpressionVisitor
  {
    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbUnaryExpression" />.
    /// </summary>
    /// <param name="expression"> The DbUnaryExpression to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    protected virtual void VisitUnaryExpression(DbUnaryExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnaryExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Argument);
    }

    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbBinaryExpression" />.
    /// </summary>
    /// <param name="expression"> The DbBinaryExpression to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    protected virtual void VisitBinaryExpression(DbBinaryExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbBinaryExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Left);
      this.VisitExpression(expression.Right);
    }

    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpressionBinding" />.
    /// </summary>
    /// <param name="binding"> The DbExpressionBinding to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="binding" />
    /// is null
    /// </exception>
    protected virtual void VisitExpressionBindingPre(DbExpressionBinding binding)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpressionBinding>(binding, nameof (binding));
      this.VisitExpression(binding.Expression);
    }

    /// <summary>
    /// Convenience method for post-processing after a DbExpressionBinding has been visited.
    /// </summary>
    /// <param name="binding"> The previously visited DbExpressionBinding. </param>
    protected virtual void VisitExpressionBindingPost(DbExpressionBinding binding)
    {
    }

    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbGroupExpressionBinding" />.
    /// </summary>
    /// <param name="binding"> The DbGroupExpressionBinding to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="binding" />
    /// is null
    /// </exception>
    protected virtual void VisitGroupExpressionBindingPre(DbGroupExpressionBinding binding)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupExpressionBinding>(binding, nameof (binding));
      this.VisitExpression(binding.Expression);
    }

    /// <summary>
    /// Convenience method indicating that the grouping keys of a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbGroupByExpression" /> have been visited and the aggregates are now about to be visited.
    /// </summary>
    /// <param name="binding"> The DbGroupExpressionBinding of the DbGroupByExpression </param>
    protected virtual void VisitGroupExpressionBindingMid(DbGroupExpressionBinding binding)
    {
    }

    /// <summary>
    /// Convenience method for post-processing after a DbGroupExpressionBinding has been visited.
    /// </summary>
    /// <param name="binding"> The previously visited DbGroupExpressionBinding. </param>
    protected virtual void VisitGroupExpressionBindingPost(DbGroupExpressionBinding binding)
    {
    }

    /// <summary>
    /// Convenience method indicating that the body of a Lambda <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> is now about to be visited.
    /// </summary>
    /// <param name="lambda"> The DbLambda that is about to be visited </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="lambda" />
    /// is null
    /// </exception>
    protected virtual void VisitLambdaPre(DbLambda lambda) => System.Data.Entity.Utilities.Check.NotNull<DbLambda>(lambda, nameof (lambda));

    /// <summary>
    /// Convenience method for post-processing after a DbLambda has been visited.
    /// </summary>
    /// <param name="lambda"> The previously visited DbLambda. </param>
    protected virtual void VisitLambdaPost(DbLambda lambda)
    {
    }

    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" />, if non-null.
    /// </summary>
    /// <param name="expression"> The expression to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public virtual void VisitExpression(DbExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
      expression.Accept((DbExpressionVisitor) this);
    }

    /// <summary>
    /// Convenience method to visit each <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> in the given list, if the list is non-null.
    /// </summary>
    /// <param name="expressionList"> The list of expressions to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expressionList" />
    /// is null
    /// </exception>
    public virtual void VisitExpressionList(IList<DbExpression> expressionList)
    {
      System.Data.Entity.Utilities.Check.NotNull<IList<DbExpression>>(expressionList, nameof (expressionList));
      for (int index = 0; index < expressionList.Count; ++index)
        this.VisitExpression(expressionList[index]);
    }

    /// <summary>
    /// Convenience method to visit each <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" /> in the list, if the list is non-null.
    /// </summary>
    /// <param name="aggregates"> The list of aggregates to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="aggregates" />
    /// is null
    /// </exception>
    public virtual void VisitAggregateList(IList<DbAggregate> aggregates)
    {
      System.Data.Entity.Utilities.Check.NotNull<IList<DbAggregate>>(aggregates, nameof (aggregates));
      for (int index = 0; index < aggregates.Count; ++index)
        this.VisitAggregate(aggregates[index]);
    }

    /// <summary>
    /// Convenience method to visit the specified <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" />.
    /// </summary>
    /// <param name="aggregate"> The aggregate to visit. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="aggregate" />
    /// is null
    /// </exception>
    public virtual void VisitAggregate(DbAggregate aggregate)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAggregate>(aggregate, nameof (aggregate));
      this.VisitExpressionList(aggregate.Arguments);
    }

    internal virtual void VisitRelatedEntityReferenceList(
      IList<DbRelatedEntityRef> relatedEntityReferences)
    {
      for (int index = 0; index < relatedEntityReferences.Count; ++index)
        this.VisitRelatedEntityReference(relatedEntityReferences[index]);
    }

    internal virtual void VisitRelatedEntityReference(DbRelatedEntityRef relatedEntityRef) => this.VisitExpression(relatedEntityRef.TargetEntityReference);

    /// <summary>
    /// Called when an <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> of an otherwise unrecognized type is encountered.
    /// </summary>
    /// <param name="expression"> The expression </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    /// <exception cref="T:System.NotSupportedException">
    /// Always thrown if this method is called, since it indicates that
    /// <paramref name="expression" />
    /// is of an unsupported type
    /// </exception>
    public override void Visit(DbExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
      throw new NotSupportedException(Strings.Cqt_General_UnsupportedExpression((object) expression.GetType().FullName));
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbConstantExpression" />.
    /// </summary>
    /// <param name="expression"> The DbConstantExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbConstantExpression expression) => System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(expression, nameof (expression));

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbNullExpression" />.
    /// </summary>
    /// <param name="expression"> The DbNullExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbNullExpression expression) => System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(expression, nameof (expression));

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbVariableReferenceExpression" />.
    /// </summary>
    /// <param name="expression"> The DbVariableReferenceExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbVariableReferenceExpression expression) => System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(expression, nameof (expression));

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbParameterReferenceExpression" />.
    /// </summary>
    /// <param name="expression"> The DbParameterReferenceExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbParameterReferenceExpression expression) => System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" />.
    /// </summary>
    /// <param name="expression"> The DbFunctionExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbFunctionExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
      this.VisitExpressionList(expression.Arguments);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLambdaExpression" />.
    /// </summary>
    /// <param name="expression"> The DbLambdaExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      this.VisitExpressionList(expression.Arguments);
      this.VisitLambdaPre(expression.Lambda);
      this.VisitExpression(expression.Lambda.Body);
      this.VisitLambdaPost(expression.Lambda);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbPropertyExpression" />.
    /// </summary>
    /// <param name="expression"> The DbPropertyExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbPropertyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
      if (expression.Instance == null)
        return;
      this.VisitExpression(expression.Instance);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbComparisonExpression" />.
    /// </summary>
    /// <param name="expression"> The DbComparisonExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbComparisonExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLikeExpression" />.
    /// </summary>
    /// <param name="expression"> The DbLikeExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbLikeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Argument);
      this.VisitExpression(expression.Pattern);
      this.VisitExpression(expression.Escape);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLimitExpression" />.
    /// </summary>
    /// <param name="expression"> The DbLimitExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbLimitExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Argument);
      this.VisitExpression(expression.Limit);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbIsNullExpression" />.
    /// </summary>
    /// <param name="expression"> The DbIsNullExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbIsNullExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbArithmeticExpression" />.
    /// </summary>
    /// <param name="expression"> The DbArithmeticExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbArithmeticExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(expression, nameof (expression));
      this.VisitExpressionList(expression.Arguments);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAndExpression" />.
    /// </summary>
    /// <param name="expression"> The DbAndExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbAndExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbOrExpression" />.
    /// </summary>
    /// <param name="expression"> The DbOrExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbOrExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbInExpression" />.
    /// </summary>
    /// <param name="expression"> The DbInExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbInExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(expression, nameof (expression));
      this.VisitExpression(expression.Item);
      this.VisitExpressionList(expression.List);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbNotExpression" />.
    /// </summary>
    /// <param name="expression"> The DbNotExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbNotExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbDistinctExpression" />.
    /// </summary>
    /// <param name="expression"> The DbDistinctExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbDistinctExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbElementExpression" />.
    /// </summary>
    /// <param name="expression"> The DbElementExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbElementExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbIsEmptyExpression" />.
    /// </summary>
    /// <param name="expression"> The DbIsEmptyExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbIsEmptyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbUnionAllExpression" />.
    /// </summary>
    /// <param name="expression"> The DbUnionAllExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbUnionAllExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbIntersectExpression" />.
    /// </summary>
    /// <param name="expression"> The DbIntersectExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbIntersectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExceptExpression" />.
    /// </summary>
    /// <param name="expression"> The DbExceptExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbExceptExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(expression, nameof (expression));
      this.VisitBinaryExpression((DbBinaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbOfTypeExpression" />.
    /// </summary>
    /// <param name="expression"> The DbOfTypeExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbOfTypeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbTreatExpression" />.
    /// </summary>
    /// <param name="expression"> The DbTreatExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbTreatExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCastExpression" />.
    /// </summary>
    /// <param name="expression"> The DbCastExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbCastExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbIsOfExpression" />.
    /// </summary>
    /// <param name="expression"> The DbIsOfExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbIsOfExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCaseExpression" />.
    /// </summary>
    /// <param name="expression"> The DbCaseExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbCaseExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(expression, nameof (expression));
      this.VisitExpressionList(expression.When);
      this.VisitExpressionList(expression.Then);
      this.VisitExpression(expression.Else);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbNewInstanceExpression" />.
    /// </summary>
    /// <param name="expression"> The DbNewInstanceExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbNewInstanceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
      this.VisitExpressionList(expression.Arguments);
      if (!expression.HasRelatedEntityReferences)
        return;
      this.VisitRelatedEntityReferenceList((IList<DbRelatedEntityRef>) expression.RelatedEntityReferences);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbRefExpression" />.
    /// </summary>
    /// <param name="expression"> The DbRefExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbRelationshipNavigationExpression" />.
    /// </summary>
    /// <param name="expression"> The DbRelationshipNavigationExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbRelationshipNavigationExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(expression, nameof (expression));
      this.VisitExpression(expression.NavigationSource);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbDerefExpression" />.
    /// </summary>
    /// <param name="expression"> The DeRefExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbDerefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbRefKeyExpression" />.
    /// </summary>
    /// <param name="expression"> The DbRefKeyExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbRefKeyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbEntityRefExpression" />.
    /// </summary>
    /// <param name="expression"> The DbEntityRefExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbEntityRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(expression, nameof (expression));
      this.VisitUnaryExpression((DbUnaryExpression) expression);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbScanExpression" />.
    /// </summary>
    /// <param name="expression"> The DbScanExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbScanExpression expression) => System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFilterExpression" />.
    /// </summary>
    /// <param name="expression"> The DbFilterExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbFilterExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      this.VisitExpression(expression.Predicate);
      this.VisitExpressionBindingPost(expression.Input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbProjectExpression" />.
    /// </summary>
    /// <param name="expression"> The DbProjectExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbProjectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      this.VisitExpression(expression.Projection);
      this.VisitExpressionBindingPost(expression.Input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCrossJoinExpression" />.
    /// </summary>
    /// <param name="expression"> The DbCrossJoinExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbCrossJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(expression, nameof (expression));
      foreach (DbExpressionBinding input in (IEnumerable<DbExpressionBinding>) expression.Inputs)
        this.VisitExpressionBindingPre(input);
      foreach (DbExpressionBinding input in (IEnumerable<DbExpressionBinding>) expression.Inputs)
        this.VisitExpressionBindingPost(input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbJoinExpression" />.
    /// </summary>
    /// <param name="expression"> The DbJoinExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Left);
      this.VisitExpressionBindingPre(expression.Right);
      this.VisitExpression(expression.JoinCondition);
      this.VisitExpressionBindingPost(expression.Left);
      this.VisitExpressionBindingPost(expression.Right);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbApplyExpression" />.
    /// </summary>
    /// <param name="expression"> The DbApplyExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbApplyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      if (expression.Apply != null)
        this.VisitExpression(expression.Apply.Expression);
      this.VisitExpressionBindingPost(expression.Input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbGroupByExpression" />.
    /// </summary>
    /// <param name="expression"> The DbExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbGroupByExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(expression, nameof (expression));
      this.VisitGroupExpressionBindingPre(expression.Input);
      this.VisitExpressionList(expression.Keys);
      this.VisitGroupExpressionBindingMid(expression.Input);
      this.VisitAggregateList(expression.Aggregates);
      this.VisitGroupExpressionBindingPost(expression.Input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbSkipExpression" />.
    /// </summary>
    /// <param name="expression"> The DbSkipExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbSkipExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      foreach (DbSortClause dbSortClause in (IEnumerable<DbSortClause>) expression.SortOrder)
        this.VisitExpression(dbSortClause.Expression);
      this.VisitExpressionBindingPost(expression.Input);
      this.VisitExpression(expression.Count);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbSortExpression" />.
    /// </summary>
    /// <param name="expression"> The DbSortExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbSortExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      for (int index = 0; index < expression.SortOrder.Count; ++index)
        this.VisitExpression(expression.SortOrder[index].Expression);
      this.VisitExpressionBindingPost(expression.Input);
    }

    /// <summary>
    /// Visitor pattern method for <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbQuantifierExpression" />.
    /// </summary>
    /// <param name="expression"> The DbQuantifierExpression that is being visited. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="expression" />
    /// is null
    /// </exception>
    public override void Visit(DbQuantifierExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(expression, nameof (expression));
      this.VisitExpressionBindingPre(expression.Input);
      this.VisitExpression(expression.Predicate);
      this.VisitExpressionBindingPost(expression.Input);
    }
  }
}
