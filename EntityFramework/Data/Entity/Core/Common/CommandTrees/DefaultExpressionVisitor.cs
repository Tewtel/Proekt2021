// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DefaultExpressionVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary> Visits each element of an expression tree from a given root expression. If any element changes, the tree is rebuilt back to the root and the new root expression is returned; otherwise the original root expression is returned. </summary>
  public class DefaultExpressionVisitor : DbExpressionVisitor<DbExpression>
  {
    private readonly Dictionary<DbVariableReferenceExpression, DbVariableReferenceExpression> varMappings = new Dictionary<DbVariableReferenceExpression, DbVariableReferenceExpression>();

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DefaultExpressionVisitor" />
    /// class.
    /// </summary>
    protected DefaultExpressionVisitor()
    {
    }

    /// <summary>Replaces an old expression with a new one for the expression visitor.</summary>
    /// <param name="oldExpression">The old expression.</param>
    /// <param name="newExpression">The new expression.</param>
    protected virtual void OnExpressionReplaced(
      DbExpression oldExpression,
      DbExpression newExpression)
    {
    }

    /// <summary>Represents an event when the variable is rebound for the expression visitor.</summary>
    /// <param name="fromVarRef">The location of the variable.</param>
    /// <param name="toVarRef">The reference of the variable where it is rebounded.</param>
    protected virtual void OnVariableRebound(
      DbVariableReferenceExpression fromVarRef,
      DbVariableReferenceExpression toVarRef)
    {
    }

    /// <summary>Represents an event when entering the scope for the expression visitor with specified scope variables.</summary>
    /// <param name="scopeVariables">The collection of scope variables.</param>
    protected virtual void OnEnterScope(
      IEnumerable<DbVariableReferenceExpression> scopeVariables)
    {
    }

    /// <summary>Exits the scope for the expression visitor.</summary>
    protected virtual void OnExitScope()
    {
    }

    /// <summary>Implements the visitor pattern for the expression.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="expression">The expression.</param>
    protected virtual DbExpression VisitExpression(DbExpression expression)
    {
      DbExpression dbExpression = (DbExpression) null;
      if (expression != null)
        dbExpression = expression.Accept<DbExpression>((DbExpressionVisitor<DbExpression>) this);
      return dbExpression;
    }

    /// <summary>Implements the visitor pattern for the expression list.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="list">The expression list.</param>
    protected virtual IList<DbExpression> VisitExpressionList(
      IList<DbExpression> list)
    {
      return DefaultExpressionVisitor.VisitList<DbExpression>(list, new Func<DbExpression, DbExpression>(this.VisitExpression));
    }

    /// <summary>Implements the visitor pattern for expression binding.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="binding">The expression binding.</param>
    protected virtual DbExpressionBinding VisitExpressionBinding(
      DbExpressionBinding binding)
    {
      DbExpressionBinding expressionBinding = binding;
      if (binding != null)
      {
        DbExpression input = this.VisitExpression(binding.Expression);
        if (binding.Expression != input)
        {
          expressionBinding = input.BindAs(binding.VariableName);
          this.RebindVariable(binding.Variable, expressionBinding.Variable);
        }
      }
      return expressionBinding;
    }

    /// <summary>Implements the visitor pattern for the expression binding list.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="list">The expression binding list.</param>
    protected virtual IList<DbExpressionBinding> VisitExpressionBindingList(
      IList<DbExpressionBinding> list)
    {
      return DefaultExpressionVisitor.VisitList<DbExpressionBinding>(list, new Func<DbExpressionBinding, DbExpressionBinding>(this.VisitExpressionBinding));
    }

    /// <summary>Implements the visitor pattern for the group expression binding.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="binding">The binding.</param>
    protected virtual DbGroupExpressionBinding VisitGroupExpressionBinding(
      DbGroupExpressionBinding binding)
    {
      DbGroupExpressionBinding expressionBinding = binding;
      if (binding != null)
      {
        DbExpression input = this.VisitExpression(binding.Expression);
        if (binding.Expression != input)
        {
          expressionBinding = input.GroupBindAs(binding.VariableName, binding.GroupVariableName);
          this.RebindVariable(binding.Variable, expressionBinding.Variable);
          this.RebindVariable(binding.GroupVariable, expressionBinding.GroupVariable);
        }
      }
      return expressionBinding;
    }

    /// <summary>Implements the visitor pattern for the sort clause.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="clause">The sort clause.</param>
    protected virtual DbSortClause VisitSortClause(DbSortClause clause)
    {
      DbSortClause dbSortClause = clause;
      if (clause != null)
      {
        DbExpression key = this.VisitExpression(clause.Expression);
        if (clause.Expression != key)
          dbSortClause = string.IsNullOrEmpty(clause.Collation) ? (clause.Ascending ? key.ToSortClause() : key.ToSortClauseDescending()) : (clause.Ascending ? key.ToSortClause(clause.Collation) : key.ToSortClauseDescending(clause.Collation));
      }
      return dbSortClause;
    }

    /// <summary>Implements the visitor pattern for the sort order.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="sortOrder">The sort order.</param>
    protected virtual IList<DbSortClause> VisitSortOrder(
      IList<DbSortClause> sortOrder)
    {
      return DefaultExpressionVisitor.VisitList<DbSortClause>(sortOrder, new Func<DbSortClause, DbSortClause>(this.VisitSortClause));
    }

    /// <summary>Implements the visitor pattern for the aggregate.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="aggregate">The aggregate.</param>
    protected virtual DbAggregate VisitAggregate(DbAggregate aggregate) => aggregate is DbFunctionAggregate aggregate1 ? (DbAggregate) this.VisitFunctionAggregate(aggregate1) : (DbAggregate) this.VisitGroupAggregate((DbGroupAggregate) aggregate);

    /// <summary>Implements the visitor pattern for the function aggregate.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="aggregate">The aggregate.</param>
    protected virtual DbFunctionAggregate VisitFunctionAggregate(
      DbFunctionAggregate aggregate)
    {
      DbFunctionAggregate functionAggregate = aggregate;
      if (aggregate != null)
      {
        EdmFunction function = this.VisitFunction(aggregate.Function);
        IList<DbExpression> dbExpressionList = this.VisitExpressionList(aggregate.Arguments);
        if (aggregate.Function != function || aggregate.Arguments != dbExpressionList)
          functionAggregate = !aggregate.Distinct ? function.Aggregate((IEnumerable<DbExpression>) dbExpressionList) : function.AggregateDistinct((IEnumerable<DbExpression>) dbExpressionList);
      }
      return functionAggregate;
    }

    /// <summary>Implements the visitor pattern for the group aggregate.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="aggregate">The aggregate.</param>
    protected virtual DbGroupAggregate VisitGroupAggregate(
      DbGroupAggregate aggregate)
    {
      DbGroupAggregate dbGroupAggregate = aggregate;
      if (aggregate != null)
      {
        IList<DbExpression> dbExpressionList = this.VisitExpressionList(aggregate.Arguments);
        if (aggregate.Arguments != dbExpressionList)
          dbGroupAggregate = DbExpressionBuilder.GroupAggregate(dbExpressionList[0]);
      }
      return dbGroupAggregate;
    }

    /// <summary>Implements the visitor pattern for the Lambda function.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="lambda">The lambda function.</param>
    protected virtual DbLambda VisitLambda(DbLambda lambda)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambda>(lambda, nameof (lambda));
      DbLambda dbLambda = lambda;
      IList<DbVariableReferenceExpression> source = DefaultExpressionVisitor.VisitList<DbVariableReferenceExpression>(lambda.Variables, (Func<DbVariableReferenceExpression, DbVariableReferenceExpression>) (varRef =>
      {
        TypeUsage type = this.VisitTypeUsage(varRef.ResultType);
        return varRef.ResultType != type ? type.Variable(varRef.VariableName) : varRef;
      }));
      this.EnterScope(source.ToArray<DbVariableReferenceExpression>());
      DbExpression body = this.VisitExpression(lambda.Body);
      this.ExitScope();
      if (lambda.Variables != source || lambda.Body != body)
        dbLambda = DbExpressionBuilder.Lambda(body, (IEnumerable<DbVariableReferenceExpression>) source);
      return dbLambda;
    }

    /// <summary>Implements the visitor pattern for the type.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="type">The type.</param>
    protected virtual EdmType VisitType(EdmType type) => type;

    /// <summary>Implements the visitor pattern for the type usage.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="type">The type.</param>
    protected virtual TypeUsage VisitTypeUsage(TypeUsage type) => type;

    /// <summary>Implements the visitor pattern for the entity set.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="entitySet">The entity set.</param>
    protected virtual EntitySetBase VisitEntitySet(EntitySetBase entitySet) => entitySet;

    /// <summary>Implements the visitor pattern for the function.</summary>
    /// <returns>The implemented visitor pattern.</returns>
    /// <param name="functionMetadata">The function metadata.</param>
    protected virtual EdmFunction VisitFunction(EdmFunction functionMetadata) => functionMetadata;

    private void NotifyIfChanged(DbExpression originalExpression, DbExpression newExpression)
    {
      if (originalExpression == newExpression)
        return;
      this.OnExpressionReplaced(originalExpression, newExpression);
    }

    private static IList<TElement> VisitList<TElement>(
      IList<TElement> list,
      Func<TElement, TElement> map)
    {
      IList<TElement> elementList1 = list;
      if (list != null)
      {
        List<TElement> elementList2 = (List<TElement>) null;
        for (int index = 0; index < list.Count; ++index)
        {
          TElement element = map(list[index]);
          if (elementList2 == null && (object) list[index] != (object) element)
          {
            elementList2 = new List<TElement>((IEnumerable<TElement>) list);
            elementList1 = (IList<TElement>) elementList2;
          }
          if (elementList2 != null)
            elementList2[index] = element;
        }
      }
      return elementList1;
    }

    private DbExpression VisitUnary(
      DbUnaryExpression expression,
      Func<DbExpression, DbExpression> callback)
    {
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression = this.VisitExpression(expression.Argument);
      if (expression.Argument != dbExpression)
        newExpression = callback(dbExpression);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    private DbExpression VisitTypeUnary(
      DbUnaryExpression expression,
      TypeUsage type,
      Func<DbExpression, TypeUsage, DbExpression> callback)
    {
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression = this.VisitExpression(expression.Argument);
      TypeUsage typeUsage = this.VisitTypeUsage(type);
      if (expression.Argument != dbExpression || type != typeUsage)
        newExpression = callback(dbExpression, typeUsage);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    private DbExpression VisitBinary(
      DbBinaryExpression expression,
      Func<DbExpression, DbExpression, DbExpression> callback)
    {
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression1 = this.VisitExpression(expression.Left);
      DbExpression dbExpression2 = this.VisitExpression(expression.Right);
      if (expression.Left != dbExpression1 || expression.Right != dbExpression2)
        newExpression = callback(dbExpression1, dbExpression2);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    private DbRelatedEntityRef VisitRelatedEntityRef(DbRelatedEntityRef entityRef)
    {
      RelationshipEndMember newSource;
      RelationshipEndMember newTarget;
      this.VisitRelationshipEnds(entityRef.SourceEnd, entityRef.TargetEnd, out newSource, out newTarget);
      DbExpression targetEntity = this.VisitExpression(entityRef.TargetEntityReference);
      return entityRef.SourceEnd != newSource || entityRef.TargetEnd != newTarget || entityRef.TargetEntityReference != targetEntity ? DbExpressionBuilder.CreateRelatedEntityRef(newSource, newTarget, targetEntity) : entityRef;
    }

    private void VisitRelationshipEnds(
      RelationshipEndMember source,
      RelationshipEndMember target,
      out RelationshipEndMember newSource,
      out RelationshipEndMember newTarget)
    {
      RelationshipType relationshipType = (RelationshipType) this.VisitType((EdmType) target.DeclaringType);
      newSource = relationshipType.RelationshipEndMembers[source.Name];
      newTarget = relationshipType.RelationshipEndMembers[target.Name];
    }

    private DbExpression VisitTerminal(
      DbExpression expression,
      Func<TypeUsage, DbExpression> reconstructor)
    {
      DbExpression newExpression = expression;
      TypeUsage typeUsage = this.VisitTypeUsage(expression.ResultType);
      if (expression.ResultType != typeUsage)
        newExpression = reconstructor(typeUsage);
      this.NotifyIfChanged(expression, newExpression);
      return newExpression;
    }

    private void RebindVariable(
      DbVariableReferenceExpression from,
      DbVariableReferenceExpression to)
    {
      if (from.VariableName.Equals(to.VariableName, StringComparison.Ordinal) && from.ResultType.EdmType == to.ResultType.EdmType && from.ResultType.EdmEquals((MetadataItem) to.ResultType))
        return;
      this.varMappings[from] = to;
      this.OnVariableRebound(from, to);
    }

    private DbExpressionBinding VisitExpressionBindingEnterScope(
      DbExpressionBinding binding)
    {
      DbExpressionBinding expressionBinding = this.VisitExpressionBinding(binding);
      this.OnEnterScope((IEnumerable<DbVariableReferenceExpression>) new DbVariableReferenceExpression[1]
      {
        expressionBinding.Variable
      });
      return expressionBinding;
    }

    private void EnterScope(params DbVariableReferenceExpression[] scopeVars) => this.OnEnterScope((IEnumerable<DbVariableReferenceExpression>) scopeVars);

    private void ExitScope() => this.OnExitScope();

    /// <summary>Implements the visitor pattern for the basic functionality required by expression types.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(expression, nameof (expression));
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.Cqt_General_UnsupportedExpression((object) expression.GetType().FullName));
    }

    /// <summary>Implements the visitor pattern for the different kinds of constants.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The constant expression.</param>
    public override DbExpression Visit(DbConstantExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(expression, nameof (expression));
      return this.VisitTerminal((DbExpression) expression, (Func<TypeUsage, DbExpression>) (newType => (DbExpression) newType.Constant(expression.GetValue())));
    }

    /// <summary>Implements the visitor pattern for a reference to a typed null literal.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbNullExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(expression, nameof (expression));
      return this.VisitTerminal((DbExpression) expression, new Func<TypeUsage, DbExpression>(DbExpressionBuilder.Null));
    }

    /// <summary>Implements the visitor pattern for a reference to a variable that is currently in scope.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbVariableReferenceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbVariableReferenceExpression referenceExpression;
      if (this.varMappings.TryGetValue(expression, out referenceExpression))
        newExpression = (DbExpression) referenceExpression;
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a reference to a parameter declared on the command tree that contains this expression.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbParameterReferenceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(expression, nameof (expression));
      return this.VisitTerminal((DbExpression) expression, (Func<TypeUsage, DbExpression>) (newType => (DbExpression) newType.Parameter(expression.ParameterName)));
    }

    /// <summary>Implements the visitor pattern for an invocation of a function.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The function expression.</param>
    public override DbExpression Visit(DbFunctionExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      IList<DbExpression> dbExpressionList = this.VisitExpressionList(expression.Arguments);
      EdmFunction function = this.VisitFunction(expression.Function);
      if (expression.Arguments != dbExpressionList || expression.Function != function)
        newExpression = (DbExpression) function.Invoke((IEnumerable<DbExpression>) dbExpressionList);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the application of a lambda function to arguments represented by DbExpression objects.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbLambdaExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      IList<DbExpression> dbExpressionList = this.VisitExpressionList(expression.Arguments);
      DbLambda lambda = this.VisitLambda(expression.Lambda);
      if (expression.Arguments != dbExpressionList || expression.Lambda != lambda)
        newExpression = (DbExpression) lambda.Invoke((IEnumerable<DbExpression>) dbExpressionList);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for retrieving an instance property.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbPropertyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpression instance = this.VisitExpression(expression.Instance);
      if (expression.Instance != instance)
        newExpression = (DbExpression) instance.Property(expression.Property.Name);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the comparison operation applied to two arguments.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The cast expression.</param>
    public override DbExpression Visit(DbComparisonExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(expression, nameof (expression));
      switch (expression.ExpressionKind)
      {
        case DbExpressionKind.Equals:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.Equal));
        case DbExpressionKind.GreaterThan:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.GreaterThan));
        case DbExpressionKind.GreaterThanOrEquals:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.GreaterThanOrEqual));
        case DbExpressionKind.LessThan:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.LessThan));
        case DbExpressionKind.LessThanOrEquals:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.LessThanOrEqual));
        case DbExpressionKind.NotEquals:
          return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.NotEqual));
        default:
          throw new NotSupportedException();
      }
    }

    /// <summary>Implements the visitor pattern for a string comparison against the specified pattern with an optional escape string.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbLikeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression = this.VisitExpression(expression.Argument);
      DbExpression pattern = this.VisitExpression(expression.Pattern);
      DbExpression escape = this.VisitExpression(expression.Escape);
      if (expression.Argument != dbExpression || expression.Pattern != pattern || expression.Escape != escape)
        newExpression = (DbExpression) dbExpression.Like(pattern, escape);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the restriction of the number of elements in the argument collection to the specified limit value.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbLimitExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression = this.VisitExpression(expression.Argument);
      DbExpression count = this.VisitExpression(expression.Limit);
      if (expression.Argument != dbExpression || expression.Limit != count)
        newExpression = (DbExpression) dbExpression.Limit(count);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the null determination applied to a single argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbIsNullExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.IsNull));
    }

    /// <summary>Implements the visitor pattern for the arithmetic operation applied to numeric arguments.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The arithmetic expression.</param>
    public override DbExpression Visit(DbArithmeticExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      IList<DbExpression> dbExpressionList = this.VisitExpressionList(expression.Arguments);
      if (expression.Arguments != dbExpressionList)
      {
        switch (expression.ExpressionKind)
        {
          case DbExpressionKind.Divide:
            newExpression = (DbExpression) dbExpressionList[0].Divide(dbExpressionList[1]);
            break;
          case DbExpressionKind.Minus:
            newExpression = (DbExpression) dbExpressionList[0].Minus(dbExpressionList[1]);
            break;
          case DbExpressionKind.Modulo:
            newExpression = (DbExpression) dbExpressionList[0].Modulo(dbExpressionList[1]);
            break;
          case DbExpressionKind.Multiply:
            newExpression = (DbExpression) dbExpressionList[0].Multiply(dbExpressionList[1]);
            break;
          case DbExpressionKind.Plus:
            newExpression = (DbExpression) dbExpressionList[0].Plus(dbExpressionList[1]);
            break;
          case DbExpressionKind.UnaryMinus:
            newExpression = (DbExpression) dbExpressionList[0].UnaryMinus();
            break;
          default:
            throw new NotSupportedException();
        }
      }
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the logical AND expression.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The logical AND expression.</param>
    public override DbExpression Visit(DbAndExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(expression, nameof (expression));
      return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.And));
    }

    /// <summary>Implements the visitor pattern for the logical OR of two Boolean arguments.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbOrExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(expression, nameof (expression));
      return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.Or));
    }

    /// <summary>Implements the visitor pattern for the DbInExpression.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The DbInExpression that is being visited.</param>
    public override DbExpression Visit(DbInExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpression dbExpression = this.VisitExpression(expression.Item);
      IList<DbExpression> list = this.VisitExpressionList(expression.List);
      if (expression.Item != dbExpression || expression.List != list)
        newExpression = (DbExpression) DbExpressionBuilder.CreateInExpression(dbExpression, list);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the logical NOT of a single Boolean argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbNotExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.Not));
    }

    /// <summary>Implements the visitor pattern for the removed duplicate elements from the specified set argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The distinct expression.</param>
    public override DbExpression Visit(DbDistinctExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.Distinct));
    }

    /// <summary>Implements the visitor pattern for the conversion of the specified set argument to a singleton the conversion of the specified set argument to a singleton.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The element expression.</param>
    public override DbExpression Visit(DbElementExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, expression.IsSinglePropertyUnwrapped ? new Func<DbExpression, DbExpression>(DbExpressionBuilder.CreateElementExpressionUnwrapSingleProperty) : new Func<DbExpression, DbExpression>(DbExpressionBuilder.Element));
    }

    /// <summary>Implements the visitor pattern for an empty set determination applied to a single set argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbIsEmptyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.IsEmpty));
    }

    /// <summary>Implements the visitor pattern for the set union operation between the left and right operands.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbUnionAllExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(expression, nameof (expression));
      return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.UnionAll));
    }

    /// <summary>Implements the visitor pattern for the set intersection operation between the left and right operands.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbIntersectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(expression, nameof (expression));
      return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.Intersect));
    }

    /// <summary>Implements the visitor pattern for the set subtraction operation between the left and right operands.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbExceptExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(expression, nameof (expression));
      return this.VisitBinary((DbBinaryExpression) expression, new Func<DbExpression, DbExpression, DbExpression>(DbExpressionBuilder.Except));
    }

    /// <summary>Implements the visitor pattern for a type conversion operation applied to a polymorphic argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbTreatExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(expression, nameof (expression));
      return this.VisitTypeUnary((DbUnaryExpression) expression, expression.ResultType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.TreatAs));
    }

    /// <summary>Implements the visitor pattern for the type comparison of a single argument against the specified type.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbIsOfExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(expression, nameof (expression));
      return expression.ExpressionKind == DbExpressionKind.IsOfOnly ? this.VisitTypeUnary((DbUnaryExpression) expression, expression.OfType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.IsOfOnly)) : this.VisitTypeUnary((DbUnaryExpression) expression, expression.OfType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.IsOf));
    }

    /// <summary>Implements the visitor pattern for the type conversion of a single argument to the specified type.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The cast expression.</param>
    public override DbExpression Visit(DbCastExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(expression, nameof (expression));
      return this.VisitTypeUnary((DbUnaryExpression) expression, expression.ResultType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.CastTo));
    }

    /// <summary>Implements the visitor pattern for the When, Then, and Else clauses.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The case expression.</param>
    public override DbExpression Visit(DbCaseExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      IList<DbExpression> dbExpressionList1 = this.VisitExpressionList(expression.When);
      IList<DbExpression> dbExpressionList2 = this.VisitExpressionList(expression.Then);
      DbExpression elseExpression = this.VisitExpression(expression.Else);
      if (expression.When != dbExpressionList1 || expression.Then != dbExpressionList2 || expression.Else != elseExpression)
        newExpression = (DbExpression) DbExpressionBuilder.Case((IEnumerable<DbExpression>) dbExpressionList1, (IEnumerable<DbExpression>) dbExpressionList2, elseExpression);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the retrieval of elements of the specified type from the given set argument.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbOfTypeExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(expression, nameof (expression));
      return expression.ExpressionKind == DbExpressionKind.OfTypeOnly ? this.VisitTypeUnary((DbUnaryExpression) expression, expression.OfType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.OfTypeOnly)) : this.VisitTypeUnary((DbUnaryExpression) expression, expression.OfType, new Func<DbExpression, TypeUsage, DbExpression>(DbExpressionBuilder.OfType));
    }

    /// <summary>Implements the visitor pattern for the construction of a new instance of a given type, including set and record types.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbNewInstanceExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      TypeUsage instanceType = this.VisitTypeUsage(expression.ResultType);
      IList<DbExpression> dbExpressionList = this.VisitExpressionList(expression.Arguments);
      bool flag = expression.ResultType == instanceType && expression.Arguments == dbExpressionList;
      if (expression.HasRelatedEntityReferences)
      {
        IList<DbRelatedEntityRef> relationships = DefaultExpressionVisitor.VisitList<DbRelatedEntityRef>((IList<DbRelatedEntityRef>) expression.RelatedEntityReferences, new Func<DbRelatedEntityRef, DbRelatedEntityRef>(this.VisitRelatedEntityRef));
        if (!flag || expression.RelatedEntityReferences != relationships)
          newExpression = (DbExpression) DbExpressionBuilder.CreateNewEntityWithRelationshipsExpression((EntityType) instanceType.EdmType, dbExpressionList, relationships);
      }
      else if (!flag)
        newExpression = (DbExpression) instanceType.New(dbExpressionList.ToArray<DbExpression>());
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a strongly typed reference to a specific instance within an entity set.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      EntityType elementType = (EntityType) TypeHelpers.GetEdmType<RefType>(expression.ResultType).ElementType;
      DbExpression keyRow = this.VisitExpression(expression.Argument);
      EntityType entityType = (EntityType) this.VisitType((EdmType) elementType);
      EntitySet entitySet = (EntitySet) this.VisitEntitySet((EntitySetBase) expression.EntitySet);
      if (expression.Argument != keyRow || elementType != entityType || expression.EntitySet != entitySet)
        newExpression = (DbExpression) entitySet.RefFromKey(keyRow, entityType);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the navigation of a relationship.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbRelationshipNavigationExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      RelationshipEndMember newSource;
      RelationshipEndMember newTarget;
      this.VisitRelationshipEnds(expression.NavigateFrom, expression.NavigateTo, out newSource, out newTarget);
      DbExpression navigateFrom = this.VisitExpression(expression.NavigationSource);
      if (expression.NavigateFrom != newSource || expression.NavigateTo != newTarget || expression.NavigationSource != navigateFrom)
        newExpression = (DbExpression) navigateFrom.Navigate(newSource, newTarget);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the expression that retrieves an entity based on the specified reference.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The DEREF expression.</param>
    public override DbExpression Visit(DbDerefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.Deref));
    }

    /// <summary>Implements the visitor pattern for the retrieval of the key value from the underlying reference value.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbRefKeyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.GetRefKey));
    }

    /// <summary>Implements the visitor pattern for the expression that extracts a reference from the underlying entity instance.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The entity reference expression.</param>
    public override DbExpression Visit(DbEntityRefExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(expression, nameof (expression));
      return this.VisitUnary((DbUnaryExpression) expression, new Func<DbExpression, DbExpression>(DbExpressionBuilder.GetEntityRef));
    }

    /// <summary>Implements the visitor pattern for a scan over an entity set or relationship set, as indicated by the Target property.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbScanExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      EntitySetBase targetSet = this.VisitEntitySet(expression.Target);
      if (expression.Target != targetSet)
        newExpression = (DbExpression) targetSet.Scan();
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a predicate applied to filter an input set.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The filter expression.</param>
    public override DbExpression Visit(DbFilterExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      DbExpression predicate = this.VisitExpression(expression.Predicate);
      this.ExitScope();
      if (expression.Input != input || expression.Predicate != predicate)
        newExpression = (DbExpression) input.Filter(predicate);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the projection of a given input set over the specified expression.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbProjectExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      DbExpression projection = this.VisitExpression(expression.Projection);
      this.ExitScope();
      if (expression.Input != input || expression.Projection != projection)
        newExpression = (DbExpression) input.Project(projection);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the unconditional join operation between the given collection arguments.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The join expression.</param>
    public override DbExpression Visit(DbCrossJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      IList<DbExpressionBinding> expressionBindingList = this.VisitExpressionBindingList(expression.Inputs);
      if (expression.Inputs != expressionBindingList)
        newExpression = (DbExpression) DbExpressionBuilder.CrossJoin((IEnumerable<DbExpressionBinding>) expressionBindingList);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for an inner, left outer, or full outer join operation between the given collection arguments on the specified join condition.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbJoinExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding left = this.VisitExpressionBinding(expression.Left);
      DbExpressionBinding right = this.VisitExpressionBinding(expression.Right);
      this.EnterScope(left.Variable, right.Variable);
      DbExpression joinCondition = this.VisitExpression(expression.JoinCondition);
      this.ExitScope();
      if (expression.Left != left || expression.Right != right || expression.JoinCondition != joinCondition)
        newExpression = DbExpressionKind.InnerJoin != expression.ExpressionKind ? (DbExpressionKind.LeftOuterJoin != expression.ExpressionKind ? (DbExpression) left.FullOuterJoin(right, joinCondition) : (DbExpression) left.LeftOuterJoin(right, joinCondition)) : (DbExpression) left.InnerJoin(right, joinCondition);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the invocation of the specified function for each element in the specified input set.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The APPLY expression.</param>
    public override DbExpression Visit(DbApplyExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      DbExpressionBinding apply = this.VisitExpressionBinding(expression.Apply);
      this.ExitScope();
      if (expression.Input != input || expression.Apply != apply)
        newExpression = DbExpressionKind.CrossApply != expression.ExpressionKind ? (DbExpression) input.OuterApply(apply) : (DbExpression) input.CrossApply(apply);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a group by operation.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbGroupByExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbGroupExpressionBinding input = this.VisitGroupExpressionBinding(expression.Input);
      this.EnterScope(input.Variable);
      IList<DbExpression> dbExpressionList = this.VisitExpressionList(expression.Keys);
      this.ExitScope();
      this.EnterScope(input.GroupVariable);
      IList<DbAggregate> dbAggregateList = DefaultExpressionVisitor.VisitList<DbAggregate>(expression.Aggregates, new Func<DbAggregate, DbAggregate>(this.VisitAggregate));
      this.ExitScope();
      if (expression.Input != input || expression.Keys != dbExpressionList || expression.Aggregates != dbAggregateList)
      {
        RowType edmType = TypeHelpers.GetEdmType<RowType>(TypeHelpers.GetEdmType<CollectionType>(expression.ResultType).TypeUsage);
        List<KeyValuePair<string, DbExpression>> list1 = edmType.Properties.Take<EdmProperty>(dbExpressionList.Count).Select<EdmProperty, string>((Func<EdmProperty, string>) (p => p.Name)).Zip<string, DbExpression>((IEnumerable<DbExpression>) dbExpressionList).ToList<KeyValuePair<string, DbExpression>>();
        List<KeyValuePair<string, DbAggregate>> list2 = edmType.Properties.Skip<EdmProperty>(dbExpressionList.Count).Select<EdmProperty, string>((Func<EdmProperty, string>) (p => p.Name)).Zip<string, DbAggregate>((IEnumerable<DbAggregate>) dbAggregateList).ToList<KeyValuePair<string, DbAggregate>>();
        newExpression = (DbExpression) input.GroupBy((IEnumerable<KeyValuePair<string, DbExpression>>) list1, (IEnumerable<KeyValuePair<string, DbAggregate>>) list2);
      }
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for the skip expression.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbSkipExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      IList<DbSortClause> dbSortClauseList = this.VisitSortOrder(expression.SortOrder);
      this.ExitScope();
      DbExpression count = this.VisitExpression(expression.Count);
      if (expression.Input != input || expression.SortOrder != dbSortClauseList || expression.Count != count)
        newExpression = (DbExpression) input.Skip((IEnumerable<DbSortClause>) dbSortClauseList, count);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a sort key that can be used as part of the sort order.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbSortExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      IList<DbSortClause> dbSortClauseList = this.VisitSortOrder(expression.SortOrder);
      this.ExitScope();
      if (expression.Input != input || expression.SortOrder != dbSortClauseList)
        newExpression = (DbExpression) input.Sort((IEnumerable<DbSortClause>) dbSortClauseList);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }

    /// <summary>Implements the visitor pattern for a quantifier operation of the specified kind over the elements of the specified input set.</summary>
    /// <returns>The implemented visitor.</returns>
    /// <param name="expression">The expression.</param>
    public override DbExpression Visit(DbQuantifierExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(expression, nameof (expression));
      DbExpression newExpression = (DbExpression) expression;
      DbExpressionBinding input = this.VisitExpressionBindingEnterScope(expression.Input);
      DbExpression predicate = this.VisitExpression(expression.Predicate);
      this.ExitScope();
      if (expression.Input != input || expression.Predicate != predicate)
        newExpression = expression.ExpressionKind != DbExpressionKind.All ? (DbExpression) input.Any(predicate) : (DbExpression) input.All(predicate);
      this.NotifyIfChanged((DbExpression) expression, newExpression);
      return newExpression;
    }
  }
}
