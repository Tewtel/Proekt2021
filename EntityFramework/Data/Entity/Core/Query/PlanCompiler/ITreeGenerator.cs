// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ITreeGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class ITreeGenerator : DbExpressionVisitor<System.Data.Entity.Core.Query.InternalTrees.Node>
  {
    private static readonly Dictionary<DbExpressionKind, OpType> _opMap = ITreeGenerator.InitializeExpressionKindToOpTypeMap();
    private readonly bool _useDatabaseNullSemantics;
    private readonly Command _iqtCommand;
    private readonly Stack<ITreeGenerator.CqtVariableScope> _varScopes = new Stack<ITreeGenerator.CqtVariableScope>();
    private readonly Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, Var> _varMap = new Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, Var>();
    private readonly Stack<EdmFunction> _functionExpansions = new Stack<EdmFunction>();
    private readonly Dictionary<DbExpression, bool> _functionsIsPredicateFlag = new Dictionary<DbExpression, bool>();
    private readonly HashSet<DbFilterExpression> _processedIsOfFilters = new HashSet<DbFilterExpression>();
    private readonly HashSet<DbTreatExpression> _fakeTreats = new HashSet<DbTreatExpression>();
    private readonly DiscriminatorMap _discriminatorMap;
    private readonly DbProjectExpression _discriminatedViewTopProject;

    private static Dictionary<DbExpressionKind, OpType> InitializeExpressionKindToOpTypeMap() => new Dictionary<DbExpressionKind, OpType>(12)
    {
      [DbExpressionKind.Plus] = OpType.Plus,
      [DbExpressionKind.Minus] = OpType.Minus,
      [DbExpressionKind.Multiply] = OpType.Multiply,
      [DbExpressionKind.Divide] = OpType.Divide,
      [DbExpressionKind.Modulo] = OpType.Modulo,
      [DbExpressionKind.UnaryMinus] = OpType.UnaryMinus,
      [DbExpressionKind.Equals] = OpType.EQ,
      [DbExpressionKind.NotEquals] = OpType.NE,
      [DbExpressionKind.LessThan] = OpType.LT,
      [DbExpressionKind.GreaterThan] = OpType.GT,
      [DbExpressionKind.LessThanOrEquals] = OpType.LE,
      [DbExpressionKind.GreaterThanOrEquals] = OpType.GE
    };

    internal Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, Var> VarMap => this._varMap;

    public static Command Generate(DbQueryCommandTree ctree) => ITreeGenerator.Generate(ctree, (DiscriminatorMap) null);

    internal static Command Generate(
      DbQueryCommandTree ctree,
      DiscriminatorMap discriminatorMap)
    {
      return new ITreeGenerator(ctree, discriminatorMap)._iqtCommand;
    }

    private ITreeGenerator(DbQueryCommandTree ctree, DiscriminatorMap discriminatorMap)
    {
      this._useDatabaseNullSemantics = ctree.UseDatabaseNullSemantics;
      this._iqtCommand = new Command(ctree.MetadataWorkspace);
      if (discriminatorMap != null)
      {
        this._discriminatorMap = discriminatorMap;
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(ctree.Query.ExpressionKind == DbExpressionKind.Project, "top level QMV expression must be project to match discriminator pattern");
        this._discriminatedViewTopProject = (DbProjectExpression) ctree.Query;
      }
      foreach (KeyValuePair<string, TypeUsage> parameter in ctree.Parameters)
      {
        if (!ITreeGenerator.ValidateParameterType(parameter.Value))
          throw new NotSupportedException(System.Data.Entity.Resources.Strings.ParameterTypeNotSupported((object) parameter.Key, (object) parameter.Value.ToString()));
        this._iqtCommand.CreateParameterVar(parameter.Key, parameter.Value);
      }
      this._iqtCommand.Root = this.VisitExpr(ctree.Query);
      if (!this._iqtCommand.Root.Op.IsRelOp)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node scalarOpTree = this.ConvertToScalarOpTree(this._iqtCommand.Root, ctree.Query);
        System.Data.Entity.Core.Query.InternalTrees.Node node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateSingleRowTableOp());
        Var computedVar;
        System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(scalarOpTree, out computedVar);
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar), node1, varDefListNode);
        if (TypeSemantics.IsCollectionType(this._iqtCommand.Root.Op.Type))
        {
          UnnestOp unnestOp = this._iqtCommand.CreateUnnestOp(computedVar);
          node2 = this._iqtCommand.CreateNode((Op) unnestOp, varDefListNode.Child0);
          computedVar = unnestOp.Table.Columns[0];
        }
        this._iqtCommand.Root = node2;
        this._varMap[this._iqtCommand.Root] = computedVar;
      }
      this._iqtCommand.Root = this.CapWithPhysicalProject(this._iqtCommand.Root);
    }

    private static bool ValidateParameterType(TypeUsage paramType)
    {
      if (paramType == null || paramType.EdmType == null)
        return false;
      return TypeSemantics.IsPrimitiveType(paramType) || paramType.EdmType is EnumType;
    }

    private static RowType ExtractElementRowType(TypeUsage typeUsage) => TypeHelpers.GetEdmType<RowType>(TypeHelpers.GetEdmType<CollectionType>(typeUsage).TypeUsage);

    private bool IsPredicate(DbExpression expr)
    {
      if (!TypeSemantics.IsPrimitiveType(expr.ResultType, PrimitiveTypeKind.Boolean))
        return false;
      switch (expr.ExpressionKind)
      {
        case DbExpressionKind.All:
        case DbExpressionKind.And:
        case DbExpressionKind.Any:
        case DbExpressionKind.Equals:
        case DbExpressionKind.GreaterThan:
        case DbExpressionKind.GreaterThanOrEquals:
        case DbExpressionKind.IsEmpty:
        case DbExpressionKind.IsNull:
        case DbExpressionKind.IsOf:
        case DbExpressionKind.IsOfOnly:
        case DbExpressionKind.LessThan:
        case DbExpressionKind.LessThanOrEquals:
        case DbExpressionKind.Like:
        case DbExpressionKind.Not:
        case DbExpressionKind.NotEquals:
        case DbExpressionKind.Or:
        case DbExpressionKind.In:
          return true;
        case DbExpressionKind.Function:
          if (!((DbFunctionExpression) expr).Function.HasUserDefinedBody)
            return false;
          bool flag1;
          if (this._functionsIsPredicateFlag.TryGetValue(expr, out flag1))
            return flag1;
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "IsPredicate must be called on a visited function expression");
          return false;
        case DbExpressionKind.VariableReference:
          DbVariableReferenceExpression e = (DbVariableReferenceExpression) expr;
          return this.ResolveScope(e).IsPredicate(e.VariableName);
        case DbExpressionKind.Lambda:
          bool flag2;
          if (this._functionsIsPredicateFlag.TryGetValue(expr, out flag2))
            return flag2;
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "IsPredicate must be called on a visited lambda expression");
          return false;
        default:
          return false;
      }
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitExpr(DbExpression e) => e?.Accept<System.Data.Entity.Core.Query.InternalTrees.Node>((DbExpressionVisitor<System.Data.Entity.Core.Query.InternalTrees.Node>) this);

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitExprAsScalar(DbExpression expr) => expr == null ? (System.Data.Entity.Core.Query.InternalTrees.Node) null : this.ConvertToScalarOpTree(this.VisitExpr(expr), expr);

    private System.Data.Entity.Core.Query.InternalTrees.Node ConvertToScalarOpTree(
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      DbExpression expr)
    {
      if (node.Op.IsRelOp)
        node = this.ConvertRelOpToScalarOpTree(node, expr.ResultType);
      else if (this.IsPredicate(expr))
        node = this.ConvertPredicateToScalarOpTree(node, expr);
      return node;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ConvertRelOpToScalarOpTree(
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      TypeUsage resultType)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(resultType), "RelOp with non-Collection result type");
      node = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateCollectOp(resultType), this.CapWithPhysicalProject(node));
      return node;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ConvertPredicateToScalarOpTree(
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      DbExpression expr)
    {
      CaseOp caseOp = this._iqtCommand.CreateCaseOp(this._iqtCommand.BooleanType);
      int num = this.IsNullable(expr) ? 1 : 0;
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(num != 0 ? 5 : 3);
      args.Add(node);
      args.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateInternalConstantOp(this._iqtCommand.BooleanType, (object) true)));
      if (num != 0)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExpr(expr);
        args.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Not), node1));
      }
      args.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateInternalConstantOp(this._iqtCommand.BooleanType, (object) false)));
      if (num != 0)
        args.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNullOp(this._iqtCommand.BooleanType)));
      node = this._iqtCommand.CreateNode((Op) caseOp, args);
      return node;
    }

    private bool IsNullable(DbExpression expression)
    {
      switch (expression.ExpressionKind)
      {
        case DbExpressionKind.All:
        case DbExpressionKind.Any:
        case DbExpressionKind.IsEmpty:
        case DbExpressionKind.IsNull:
          return false;
        case DbExpressionKind.And:
        case DbExpressionKind.Or:
          DbBinaryExpression binaryExpression = (DbBinaryExpression) expression;
          return this.IsNullable(binaryExpression.Left) || this.IsNullable(binaryExpression.Right);
        case DbExpressionKind.Not:
          return this.IsNullable(((DbUnaryExpression) expression).Argument);
        default:
          return true;
      }
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitExprAsPredicate(
      DbExpression expr)
    {
      if (expr == null)
        return (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExpr(expr);
      if (!this.IsPredicate(expr))
      {
        ComparisonOp comparisonOp = this._iqtCommand.CreateComparisonOp(OpType.EQ);
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateInternalConstantOp(this._iqtCommand.BooleanType, (object) true));
        node1 = this._iqtCommand.CreateNode((Op) comparisonOp, node1, node2);
      }
      else
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(!node1.Op.IsRelOp, "unexpected relOp as predicate?");
      return node1;
    }

    private static IList<System.Data.Entity.Core.Query.InternalTrees.Node> VisitExpr(
      IList<DbExpression> exprs,
      ITreeGenerator.VisitExprDelegate exprDelegate)
    {
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      for (int index = 0; index < exprs.Count; ++index)
        nodeList.Add(exprDelegate(exprs[index]));
      return (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList;
    }

    private IList<System.Data.Entity.Core.Query.InternalTrees.Node> VisitExprAsScalar(
      IList<DbExpression> exprs)
    {
      return ITreeGenerator.VisitExpr(exprs, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitUnary(
      DbUnaryExpression e,
      Op op,
      ITreeGenerator.VisitExprDelegate exprDelegate)
    {
      return this._iqtCommand.CreateNode(op, exprDelegate(e.Argument));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitBinary(
      DbBinaryExpression e,
      Op op,
      ITreeGenerator.VisitExprDelegate exprDelegate)
    {
      return this._iqtCommand.CreateNode(op, exprDelegate(e.Left), exprDelegate(e.Right));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node EnsureRelOp(System.Data.Entity.Core.Query.InternalTrees.Node inputNode)
    {
      Op op = inputNode.Op;
      if (op.IsRelOp)
        return inputNode;
      ScalarOp scalarOp = op as ScalarOp;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(scalarOp != null, "An expression in a CQT produced a non-ScalarOp and non-RelOp output Op");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(scalarOp.Type), "An expression used as a RelOp argument was neither a RelOp or a collection");
      if (op is CollectOp)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(inputNode.HasChild0, "CollectOp without argument");
        if (inputNode.Child0.Op is PhysicalProjectOp)
        {
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(inputNode.Child0.HasChild0, "PhysicalProjectOp without argument");
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(inputNode.Child0.Child0.Op.IsRelOp, "PhysicalProjectOp applied to non-RelOp input");
          return inputNode.Child0.Child0;
        }
      }
      Var computedVar1;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefNode = this._iqtCommand.CreateVarDefNode(inputNode, out computedVar1);
      UnnestOp unnestOp = this._iqtCommand.CreateUnnestOp(computedVar1);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(unnestOp.Table.Columns.Count == 1, "Un-nest of collection ScalarOp produced unexpected number of columns (1 expected)");
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this._iqtCommand.CreateNode((Op) unnestOp, varDefNode);
      this._varMap[node1] = unnestOp.Table.Columns[0];
      Var computedVar2;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp(unnestOp.Table.Columns[0])), out computedVar2);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar2), node1, varDefListNode);
      this._varMap[node2] = computedVar2;
      return node2;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node CapWithProject(System.Data.Entity.Core.Query.InternalTrees.Node input)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(input.Op.IsRelOp, "unexpected non-RelOp?");
      if (input.Op.OpType == OpType.Project)
        return input;
      Var var = this._varMap[input];
      System.Data.Entity.Core.Query.InternalTrees.Node node = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(var), input, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp()));
      this._varMap[node] = var;
      return node;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node CapWithPhysicalProject(System.Data.Entity.Core.Query.InternalTrees.Node input)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(input.Op.IsRelOp, "unexpected non-RelOp?");
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreatePhysicalProjectOp(this._varMap[input]), input);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node EnterExpressionBinding(
      DbExpressionBinding binding)
    {
      return this.VisitBoundExpressionPushBindingScope(binding.Expression, binding.VariableName);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node EnterGroupExpressionBinding(
      DbGroupExpressionBinding binding)
    {
      return this.VisitBoundExpressionPushBindingScope(binding.Expression, binding.VariableName);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitBoundExpressionPushBindingScope(
      DbExpression boundExpression,
      string bindingName)
    {
      Var boundVar;
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitBoundExpression(boundExpression, out boundVar);
      this.PushBindingScope(boundVar, bindingName);
      return node;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitBoundExpression(
      DbExpression boundExpression,
      out Var boundVar)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node inputNode = this.VisitExpr(boundExpression);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(inputNode != null, "DbExpressionBinding.Expression produced null conversion");
      System.Data.Entity.Core.Query.InternalTrees.Node key = this.EnsureRelOp(inputNode);
      boundVar = this._varMap[key];
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(boundVar != null, "No Var found for Input Op");
      return key;
    }

    private void PushBindingScope(Var boundVar, string bindingName) => this._varScopes.Push((ITreeGenerator.CqtVariableScope) new ITreeGenerator.ExpressionBindingScope(this._iqtCommand, bindingName, boundVar));

    private ITreeGenerator.ExpressionBindingScope ExitExpressionBinding()
    {
      ITreeGenerator.ExpressionBindingScope expressionBindingScope = this._varScopes.Pop() as ITreeGenerator.ExpressionBindingScope;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(expressionBindingScope != null, "ExitExpressionBinding called without ExpressionBindingScope on top of scope stack");
      return expressionBindingScope;
    }

    private void ExitGroupExpressionBinding() => System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this._varScopes.Pop() is ITreeGenerator.ExpressionBindingScope, "ExitGroupExpressionBinding called without ExpressionBindingScope on top of scope stack");

    private void EnterLambdaFunction(
      DbLambda lambda,
      List<Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>> argumentValues,
      EdmFunction expandingEdmFunction)
    {
      IList<DbVariableReferenceExpression> variables = lambda.Variables;
      Dictionary<string, Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>> args = new Dictionary<string, Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>>();
      int index = 0;
      foreach (Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool> argumentValue in argumentValues)
      {
        args.Add(variables[index].VariableName, argumentValue);
        ++index;
      }
      if (expandingEdmFunction != null)
      {
        if (this._functionExpansions.Contains(expandingEdmFunction))
          throw new EntityCommandCompilationException(System.Data.Entity.Resources.Strings.Cqt_UDF_FunctionDefinitionWithCircularReference((object) expandingEdmFunction.FullName), (Exception) null);
        this._functionExpansions.Push(expandingEdmFunction);
      }
      this._varScopes.Push((ITreeGenerator.CqtVariableScope) new ITreeGenerator.LambdaScope(this, this._iqtCommand, args));
    }

    private ITreeGenerator.LambdaScope ExitLambdaFunction(
      EdmFunction expandingEdmFunction)
    {
      ITreeGenerator.LambdaScope lambdaScope = this._varScopes.Pop() as ITreeGenerator.LambdaScope;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(lambdaScope != null, "ExitLambdaFunction called without LambdaScope on top of scope stack");
      if (expandingEdmFunction == null)
        return lambdaScope;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this._functionExpansions.Pop() == expandingEdmFunction, "Function expansion stack corruption: unexpected function at the top of the stack");
      return lambdaScope;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ProjectNewRecord(
      System.Data.Entity.Core.Query.InternalTrees.Node inputNode,
      RowType recType,
      IEnumerable<Var> colVars)
    {
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      foreach (Var colVar in colVars)
        args.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp(colVar)));
      Var computedVar;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNewRecordOp(recType), args), out computedVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar), inputNode, varDefListNode);
      this._varMap[node] = computedVar;
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(e, nameof (e));
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.Cqt_General_UnsupportedExpression((object) e.GetType().FullName));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbConstantExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConstantExpression>(e, nameof (e));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConstantOp(e.ResultType, e.GetValue()));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbNullExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNullExpression>(e, nameof (e));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNullOp(e.ResultType));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbVariableReferenceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbVariableReferenceExpression>(e, nameof (e));
      return this.ResolveScope(e)[e.VariableName];
    }

    private ITreeGenerator.CqtVariableScope ResolveScope(
      DbVariableReferenceExpression e)
    {
      foreach (ITreeGenerator.CqtVariableScope varScope in this._varScopes)
      {
        if (varScope.Contains(e.VariableName))
          return varScope;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "CQT VarRef could not be resolved in the variable scope stack");
      return (ITreeGenerator.CqtVariableScope) null;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbParameterReferenceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbParameterReferenceExpression>(e, nameof (e));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp((Var) this._iqtCommand.GetParameter(e.ParameterName)));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbFunctionExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFunctionExpression>(e, nameof (e));
      System.Data.Entity.Core.Query.InternalTrees.Node node;
      if (e.Function.IsModelDefinedFunction)
      {
        DbLambda functionDefinition;
        try
        {
          functionDefinition = this._iqtCommand.MetadataWorkspace.GetGeneratedFunctionDefinition(e.Function);
        }
        catch (Exception ex)
        {
          if (ex.IsCatchableExceptionType())
            throw new EntityCommandCompilationException(System.Data.Entity.Resources.Strings.Cqt_UDF_FunctionDefinitionGenerationFailed((object) e.Function.FullName), ex);
          throw;
        }
        node = this.VisitLambdaExpression(functionDefinition, e.Arguments, (DbExpression) e, e.Function);
      }
      else
      {
        List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(e.Arguments.Count);
        for (int index = 0; index < e.Arguments.Count; ++index)
          args.Add(this.BuildSoftCast(this.VisitExprAsScalar(e.Arguments[index]), e.Function.Parameters[index].TypeUsage));
        node = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateFunctionOp(e.Function), args);
      }
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbLambdaExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLambdaExpression>(e, nameof (e));
      return this.VisitLambdaExpression(e.Lambda, e.Arguments, (DbExpression) e, (EdmFunction) null);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitLambdaExpression(
      DbLambda lambda,
      IList<DbExpression> arguments,
      DbExpression applicationExpr,
      EdmFunction expandingEdmFunction)
    {
      List<Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>> argumentValues = new List<Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>>(arguments.Count);
      foreach (DbExpression dbExpression in (IEnumerable<DbExpression>) arguments)
        argumentValues.Add(Tuple.Create<System.Data.Entity.Core.Query.InternalTrees.Node, bool>(this.VisitExpr(dbExpression), this.IsPredicate(dbExpression)));
      this.EnterLambdaFunction(lambda, argumentValues, expandingEdmFunction);
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitExpr(lambda.Body);
      this._functionsIsPredicateFlag[applicationExpr] = this.IsPredicate(lambda.Body);
      this.ExitLambdaFunction(expandingEdmFunction);
      return node;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildSoftCast(
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      TypeUsage targetType)
    {
      if (node.Op.IsRelOp)
      {
        targetType = TypeHelpers.GetEdmType<CollectionType>(targetType).TypeUsage;
        Var var = this._varMap[node];
        if (Command.EqualTypes(targetType, var.Type))
          return node;
        System.Data.Entity.Core.Query.InternalTrees.Node node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp(var));
        Var computedVar;
        System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateSoftCastOp(targetType), node1), out computedVar);
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar), node, varDefListNode);
        this._varMap[node2] = computedVar;
        return node2;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(node.Op.IsScalarOp, "I want a scalar op");
      return Command.EqualTypes(node.Op.Type, targetType) ? node : this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateSoftCastOp(targetType), node);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildSoftCast(
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      EdmType targetType)
    {
      return this.BuildSoftCast(node, TypeUsage.Create(targetType));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildEntityRef(
      System.Data.Entity.Core.Query.InternalTrees.Node arg,
      TypeUsage entityType)
    {
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateGetEntityRefOp(TypeHelpers.CreateReferenceTypeUsage((EntityType) entityType.EdmType)), arg);
    }

    private static bool TryRewriteKeyPropertyAccess(
      DbPropertyExpression propertyExpression,
      out DbExpression rewritten)
    {
      if (propertyExpression.Instance.ExpressionKind == DbExpressionKind.Property && Helper.IsEntityType(propertyExpression.Instance.ResultType.EdmType))
      {
        EntityType edmType = (EntityType) propertyExpression.Instance.ResultType.EdmType;
        DbPropertyExpression instance = (DbPropertyExpression) propertyExpression.Instance;
        if (Helper.IsNavigationProperty(instance.Property) && edmType.KeyMembers.Contains(propertyExpression.Property))
        {
          NavigationProperty property = (NavigationProperty) instance.Property;
          DbExpression dbExpression = (DbExpression) instance.Instance.GetEntityRef().Navigate(property.FromEndMember, property.ToEndMember);
          rewritten = (DbExpression) dbExpression.GetRefKey();
          rewritten = (DbExpression) rewritten.Property(propertyExpression.Property.Name);
          return true;
        }
      }
      rewritten = (DbExpression) null;
      return false;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbPropertyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbPropertyExpression>(e, nameof (e));
      if (BuiltInTypeKind.EdmProperty != e.Property.BuiltInTypeKind && e.Property.BuiltInTypeKind != BuiltInTypeKind.AssociationEndMember && BuiltInTypeKind.NavigationProperty != e.Property.BuiltInTypeKind)
        throw new NotSupportedException();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(e.Instance != null, "Static properties are not supported");
      DbExpression rewritten;
      System.Data.Entity.Core.Query.InternalTrees.Node node1;
      if (ITreeGenerator.TryRewriteKeyPropertyAccess(e, out rewritten))
      {
        node1 = this.VisitExpr(rewritten);
      }
      else
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.VisitExpr(e.Instance);
        if (e.Instance.ExpressionKind == DbExpressionKind.NewInstance && Helper.IsStructuralType(e.Instance.ResultType.EdmType))
        {
          IList structuralMembers = Helper.GetAllStructuralMembers(e.Instance.ResultType.EdmType);
          int index1 = -1;
          for (int index2 = 0; index2 < structuralMembers.Count; ++index2)
          {
            if (string.Equals(e.Property.Name, ((EdmMember) structuralMembers[index2]).Name, StringComparison.Ordinal))
            {
              index1 = index2;
              break;
            }
          }
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(index1 > -1, "The specified property was not found");
          node1 = this.BuildSoftCast(node2.Children[index1], e.ResultType);
        }
        else
          node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreatePropertyOp(e.Property), this.BuildSoftCast(node2, (EdmType) e.Property.DeclaringType));
      }
      return node1;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbComparisonExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbComparisonExpression>(e, nameof (e));
      Op comparisonOp = (Op) this._iqtCommand.CreateComparisonOp(ITreeGenerator._opMap[e.ExpressionKind]);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExprAsScalar(e.Left);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.VisitExprAsScalar(e.Right);
      TypeUsage commonTypeUsage = TypeHelpers.GetCommonTypeUsage(e.Left.ResultType, e.Right.ResultType);
      if (!Command.EqualTypes(e.Left.ResultType, e.Right.ResultType))
      {
        node1 = this.BuildSoftCast(node1, commonTypeUsage);
        node2 = this.BuildSoftCast(node2, commonTypeUsage);
      }
      if (TypeSemantics.IsEntityType(commonTypeUsage) && (e.ExpressionKind == DbExpressionKind.Equals || e.ExpressionKind == DbExpressionKind.NotEquals))
      {
        node1 = this.BuildEntityRef(node1, commonTypeUsage);
        node2 = this.BuildEntityRef(node2, commonTypeUsage);
      }
      return this._iqtCommand.CreateNode(comparisonOp, node1, node2);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbLikeExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLikeExpression>(e, nameof (e));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateLikeOp(), this.VisitExpr(e.Argument), this.VisitExpr(e.Pattern), this.VisitExpr(e.Escape));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node CreateLimitNode(
      System.Data.Entity.Core.Query.InternalTrees.Node inputNode,
      System.Data.Entity.Core.Query.InternalTrees.Node limitNode,
      bool withTies)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node;
      if (OpType.ConstrainedSort == inputNode.Op.OpType && OpType.Null == inputNode.Child2.Op.OpType)
      {
        inputNode.Child2 = limitNode;
        if (withTies)
          ((ConstrainedSortOp) inputNode.Op).WithTies = true;
        node = inputNode;
      }
      else
        node = OpType.Sort != inputNode.Op.OpType ? this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConstrainedSortOp(new List<SortKey>(), withTies), inputNode, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNullOp(this._iqtCommand.IntegerType)), limitNode) : this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConstrainedSortOp(((SortBaseOp) inputNode.Op).Keys, withTies), inputNode.Child0, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNullOp(this._iqtCommand.IntegerType)), limitNode);
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbLimitExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbLimitExpression>(expression, nameof (expression));
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.EnsureRelOp(this.VisitExpr(expression.Argument));
      Var var = this._varMap[node];
      System.Data.Entity.Core.Query.InternalTrees.Node limitNode = this.VisitExprAsScalar(expression.Limit);
      System.Data.Entity.Core.Query.InternalTrees.Node key;
      if (OpType.Project == node.Op.OpType && (node.Child0.Op.OpType == OpType.Sort || node.Child0.Op.OpType == OpType.ConstrainedSort))
      {
        node.Child0 = this.CreateLimitNode(node.Child0, limitNode, expression.WithTies);
        key = node;
      }
      else
        key = this.CreateLimitNode(node, limitNode, expression.WithTies);
      if (key != node)
        this._varMap[key] = var;
      return key;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbIsNullExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsNullExpression>(e, nameof (e));
      bool flag = false;
      if (e.Argument.ExpressionKind == DbExpressionKind.IsNull)
        flag = true;
      else if (e.Argument.ExpressionKind == DbExpressionKind.Not && ((DbUnaryExpression) e.Argument).Argument.ExpressionKind == DbExpressionKind.IsNull)
        flag = true;
      Op conditionalOp = (Op) this._iqtCommand.CreateConditionalOp(OpType.IsNull);
      if (flag)
        return this._iqtCommand.CreateNode(conditionalOp, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateInternalConstantOp(this._iqtCommand.BooleanType, (object) true)));
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitExprAsScalar(e.Argument);
      if (TypeSemantics.IsEntityType(e.Argument.ResultType))
        node = this.BuildEntityRef(node, e.Argument.ResultType);
      return this._iqtCommand.CreateNode(conditionalOp, node);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbArithmeticExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbArithmeticExpression>(e, nameof (e));
      Op arithmeticOp = (Op) this._iqtCommand.CreateArithmeticOp(ITreeGenerator._opMap[e.ExpressionKind], e.ResultType);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      foreach (DbExpression expr in (IEnumerable<DbExpression>) e.Arguments)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitExprAsScalar(expr);
        args.Add(this.BuildSoftCast(node, e.ResultType));
      }
      return this._iqtCommand.CreateNode(arithmeticOp, args);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbAndExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbAndExpression>(e, nameof (e));
      Op conditionalOp = (Op) this._iqtCommand.CreateConditionalOp(OpType.And);
      return this.VisitBinary((DbBinaryExpression) e, conditionalOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsPredicate));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbOrExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOrExpression>(e, nameof (e));
      Op conditionalOp = (Op) this._iqtCommand.CreateConditionalOp(OpType.Or);
      return this.VisitBinary((DbBinaryExpression) e, conditionalOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsPredicate));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbInExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInExpression>(e, nameof (e));
      Op conditionalOp = (Op) this._iqtCommand.CreateConditionalOp(OpType.In);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(1 + e.List.Count)
      {
        this.VisitExpr(e.Item)
      };
      args.AddRange(e.List.Select<DbExpression, System.Data.Entity.Core.Query.InternalTrees.Node>(new Func<DbExpression, System.Data.Entity.Core.Query.InternalTrees.Node>(this.VisitExpr)));
      return this._iqtCommand.CreateNode(conditionalOp, args);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbNotExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNotExpression>(e, nameof (e));
      Op conditionalOp = (Op) this._iqtCommand.CreateConditionalOp(OpType.Not);
      return this.VisitUnary((DbUnaryExpression) e, conditionalOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsPredicate));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbDistinctExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDistinctExpression>(e, nameof (e));
      System.Data.Entity.Core.Query.InternalTrees.Node key = this.EnsureRelOp(this.VisitExpr(e.Argument));
      Var var = this._varMap[key];
      System.Data.Entity.Core.Query.InternalTrees.Node node = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateDistinctOp(var), key);
      this._varMap[node] = var;
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbElementExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbElementExpression>(e, nameof (e));
      Op elementOp = (Op) this._iqtCommand.CreateElementOp(e.ResultType);
      System.Data.Entity.Core.Query.InternalTrees.Node key = this.BuildSoftCast(this.EnsureRelOp(this.VisitExpr(e.Argument)), TypeHelpers.CreateCollectionTypeUsage(e.ResultType));
      Var var = this._varMap[key];
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateSingleRowOp(), key);
      this._varMap[node1] = var;
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.CapWithProject(node1);
      return this._iqtCommand.CreateNode(elementOp, node2);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbIsEmptyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsEmptyExpression>(e, nameof (e));
      Op existsOp = (Op) this._iqtCommand.CreateExistsOp();
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.EnsureRelOp(this.VisitExpr(e.Argument));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Not), this._iqtCommand.CreateNode(existsOp, node));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitSetOpExpression(
      DbBinaryExpression expression)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(DbExpressionKind.Except == expression.ExpressionKind || DbExpressionKind.Intersect == expression.ExpressionKind || DbExpressionKind.UnionAll == expression.ExpressionKind, "Non-SetOp DbExpression used as argument to VisitSetOpExpression");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(expression.ResultType), "SetOp DbExpression does not have collection result type?");
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.EnsureRelOp(this.VisitExpr(expression.Left));
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.EnsureRelOp(this.VisitExpr(expression.Right));
      System.Data.Entity.Core.Query.InternalTrees.Node key1 = this.BuildSoftCast(node1, expression.ResultType);
      System.Data.Entity.Core.Query.InternalTrees.Node key2 = this.BuildSoftCast(node2, expression.ResultType);
      Var setOpVar = (Var) this._iqtCommand.CreateSetOpVar(TypeHelpers.GetEdmType<CollectionType>(expression.ResultType).TypeUsage);
      System.Data.Entity.Core.Query.InternalTrees.VarMap leftMap = new System.Data.Entity.Core.Query.InternalTrees.VarMap();
      leftMap.Add(setOpVar, this._varMap[key1]);
      System.Data.Entity.Core.Query.InternalTrees.VarMap rightMap = new System.Data.Entity.Core.Query.InternalTrees.VarMap();
      rightMap.Add(setOpVar, this._varMap[key2]);
      Op op = (Op) null;
      switch (expression.ExpressionKind)
      {
        case DbExpressionKind.Except:
          op = (Op) this._iqtCommand.CreateExceptOp(leftMap, rightMap);
          break;
        case DbExpressionKind.Intersect:
          op = (Op) this._iqtCommand.CreateIntersectOp(leftMap, rightMap);
          break;
        case DbExpressionKind.UnionAll:
          op = (Op) this._iqtCommand.CreateUnionAllOp(leftMap, rightMap);
          break;
      }
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this._iqtCommand.CreateNode(op, key1, key2);
      this._varMap[node3] = setOpVar;
      return node3;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbUnionAllExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbUnionAllExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbIntersectExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIntersectExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbExceptExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExceptExpression>(e, nameof (e));
      return this.VisitSetOpExpression((DbBinaryExpression) e);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbTreatExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbTreatExpression>(e, nameof (e));
      Op op = !this._fakeTreats.Contains(e) ? (Op) this._iqtCommand.CreateTreatOp(e.ResultType) : (Op) this._iqtCommand.CreateFakeTreatOp(e.ResultType);
      return this.VisitUnary((DbUnaryExpression) e, op, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbIsOfExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbIsOfExpression>(e, nameof (e));
      Op op = DbExpressionKind.IsOfOnly != e.ExpressionKind ? (Op) this._iqtCommand.CreateIsOfOp(e.OfType) : (Op) this._iqtCommand.CreateIsOfOnlyOp(e.OfType);
      return this.VisitUnary((DbUnaryExpression) e, op, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbCastExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCastExpression>(e, nameof (e));
      Op castOp = (Op) this._iqtCommand.CreateCastOp(e.ResultType);
      return this.VisitUnary((DbUnaryExpression) e, castOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbCaseExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCaseExpression>(e, nameof (e));
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      for (int index = 0; index < e.When.Count; ++index)
      {
        args.Add(this.VisitExprAsPredicate(e.When[index]));
        args.Add(this.BuildSoftCast(this.VisitExprAsScalar(e.Then[index]), e.ResultType));
      }
      args.Add(this.BuildSoftCast(this.VisitExprAsScalar(e.Else), e.ResultType));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateCaseOp(e.ResultType), args);
    }

    private DbFilterExpression CreateIsOfFilterExpression(
      DbExpression input,
      ITreeGenerator.IsOfFilter typeFilter)
    {
      DbExpressionBinding resultBinding = input.Bind();
      DbExpression predicate = Helpers.BuildBalancedTreeInPlace<DbExpression>((IList<DbExpression>) new List<DbExpression>((IEnumerable<DbExpression>) typeFilter.ToEnumerable().Select<KeyValuePair<TypeUsage, bool>, DbIsOfExpression>((Func<KeyValuePair<TypeUsage, bool>, DbIsOfExpression>) (tf => !tf.Value ? resultBinding.Variable.IsOf(tf.Key) : resultBinding.Variable.IsOfOnly(tf.Key))).ToList<DbIsOfExpression>()), (Func<DbExpression, DbExpression, DbExpression>) ((left, right) => (DbExpression) left.And(right)));
      DbFilterExpression filterExpression = resultBinding.Filter(predicate);
      this._processedIsOfFilters.Add(filterExpression);
      return filterExpression;
    }

    private static bool IsIsOfFilter(DbFilterExpression filter)
    {
      if (filter.Predicate.ExpressionKind != DbExpressionKind.IsOf && filter.Predicate.ExpressionKind != DbExpressionKind.IsOfOnly)
        return false;
      DbExpression dbExpression = ((DbUnaryExpression) filter.Predicate).Argument;
      return dbExpression.ExpressionKind == DbExpressionKind.VariableReference && ((DbVariableReferenceExpression) dbExpression).VariableName == filter.Input.VariableName;
    }

    private DbExpression ApplyIsOfFilter(
      DbExpression current,
      ITreeGenerator.IsOfFilter typeFilter)
    {
      DbExpression dbExpression;
      switch (current.ExpressionKind)
      {
        case DbExpressionKind.Distinct:
          dbExpression = (DbExpression) this.ApplyIsOfFilter(((DbUnaryExpression) current).Argument, typeFilter).Distinct();
          break;
        case DbExpressionKind.Filter:
          DbFilterExpression filter = (DbFilterExpression) current;
          if (ITreeGenerator.IsIsOfFilter(filter))
          {
            DbIsOfExpression predicate = (DbIsOfExpression) filter.Predicate;
            typeFilter = typeFilter.Merge(predicate);
            dbExpression = this.ApplyIsOfFilter(filter.Input.Expression, typeFilter);
            break;
          }
          dbExpression = (DbExpression) this.ApplyIsOfFilter(filter.Input.Expression, typeFilter).BindAs(filter.Input.VariableName).Filter(filter.Predicate);
          break;
        case DbExpressionKind.OfType:
        case DbExpressionKind.OfTypeOnly:
          DbOfTypeExpression other = (DbOfTypeExpression) current;
          typeFilter = typeFilter.Merge(other);
          DbExpressionBinding input = this.ApplyIsOfFilter(other.Argument, typeFilter).Bind();
          DbTreatExpression dbTreatExpression = input.Variable.TreatAs(other.OfType);
          this._fakeTreats.Add(dbTreatExpression);
          dbExpression = (DbExpression) input.Project((DbExpression) dbTreatExpression);
          break;
        case DbExpressionKind.Project:
          DbProjectExpression projectExpression = (DbProjectExpression) current;
          dbExpression = projectExpression.Projection.ExpressionKind != DbExpressionKind.VariableReference || !(((DbVariableReferenceExpression) projectExpression.Projection).VariableName == projectExpression.Input.VariableName) ? (DbExpression) this.CreateIsOfFilterExpression(current, typeFilter) : this.ApplyIsOfFilter(projectExpression.Input.Expression, typeFilter);
          break;
        case DbExpressionKind.Sort:
          DbSortExpression dbSortExpression = (DbSortExpression) current;
          dbExpression = (DbExpression) this.ApplyIsOfFilter(dbSortExpression.Input.Expression, typeFilter).BindAs(dbSortExpression.Input.VariableName).Sort((IEnumerable<DbSortClause>) dbSortExpression.SortOrder);
          break;
        default:
          dbExpression = (DbExpression) this.CreateIsOfFilterExpression(current, typeFilter);
          break;
      }
      return dbExpression;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbOfTypeExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbOfTypeExpression>(e, nameof (e));
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(e.Argument.ResultType), "Non-Collection Type Argument in DbOfTypeExpression");
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.EnsureRelOp(this.VisitExpr(this.ApplyIsOfFilter(e.Argument, new ITreeGenerator.IsOfFilter(e))));
      Var var = this._varMap[node];
      Var resultVar;
      System.Data.Entity.Core.Query.InternalTrees.Node key = this._iqtCommand.BuildFakeTreatProject(node, var, e.OfType, out resultVar);
      this._varMap[key] = resultVar;
      return key;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbNewInstanceExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbNewInstanceExpression>(e, nameof (e));
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = (List<System.Data.Entity.Core.Query.InternalTrees.Node>) null;
      Op op;
      if (TypeSemantics.IsCollectionType(e.ResultType))
        op = (Op) this._iqtCommand.CreateNewMultisetOp(e.ResultType);
      else if (TypeSemantics.IsRowType(e.ResultType))
        op = (Op) this._iqtCommand.CreateNewRecordOp(e.ResultType);
      else if (TypeSemantics.IsEntityType(e.ResultType))
      {
        List<RelProperty> relProperties = new List<RelProperty>();
        nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        if (e.HasRelatedEntityReferences)
        {
          foreach (DbRelatedEntityRef relatedEntityReference in e.RelatedEntityReferences)
          {
            RelProperty relProperty = new RelProperty((RelationshipType) relatedEntityReference.TargetEnd.DeclaringType, relatedEntityReference.SourceEnd, relatedEntityReference.TargetEnd);
            relProperties.Add(relProperty);
            System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitExprAsScalar(relatedEntityReference.TargetEntityReference);
            nodeList.Add(node);
          }
        }
        op = (Op) this._iqtCommand.CreateNewEntityOp(e.ResultType, relProperties);
      }
      else
        op = (Op) this._iqtCommand.CreateNewInstanceOp(e.ResultType);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      if (TypeSemantics.IsStructuralType(e.ResultType))
      {
        StructuralType edmType = TypeHelpers.GetEdmType<StructuralType>(e.ResultType);
        int index = 0;
        foreach (EdmMember structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers((EdmType) edmType))
        {
          System.Data.Entity.Core.Query.InternalTrees.Node node = this.BuildSoftCast(this.VisitExprAsScalar(e.Arguments[index]), Helper.GetModelTypeUsage(structuralMember));
          args.Add(node);
          ++index;
        }
      }
      else
      {
        TypeUsage typeUsage = TypeHelpers.GetEdmType<CollectionType>(e.ResultType).TypeUsage;
        foreach (DbExpression expr in (IEnumerable<DbExpression>) e.Arguments)
        {
          System.Data.Entity.Core.Query.InternalTrees.Node node = this.BuildSoftCast(this.VisitExprAsScalar(expr), typeUsage);
          args.Add(node);
        }
      }
      if (nodeList != null)
        args.AddRange((IEnumerable<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList);
      return this._iqtCommand.CreateNode(op, args);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbRefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefExpression>(e, nameof (e));
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateRefOp(e.EntitySet, e.ResultType), this.BuildSoftCast(this.VisitExprAsScalar(e.Argument), (EdmType) TypeHelpers.CreateKeyRowType((EntityTypeBase) e.EntitySet.ElementType)));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbRelationshipNavigationExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRelationshipNavigationExpression>(e, nameof (e));
      RelProperty relProperty = new RelProperty(e.Relationship, e.NavigateFrom, e.NavigateTo);
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNavigateOp(e.ResultType, relProperty), this.VisitExprAsScalar(e.NavigationSource));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbDerefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbDerefExpression>(e, nameof (e));
      Op derefOp = (Op) this._iqtCommand.CreateDerefOp(e.ResultType);
      return this.VisitUnary((DbUnaryExpression) e, derefOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbRefKeyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbRefKeyExpression>(e, nameof (e));
      Op getRefKeyOp = (Op) this._iqtCommand.CreateGetRefKeyOp(e.ResultType);
      return this.VisitUnary((DbUnaryExpression) e, getRefKeyOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbEntityRefExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbEntityRefExpression>(e, nameof (e));
      Op getEntityRefOp = (Op) this._iqtCommand.CreateGetEntityRefOp(e.ResultType);
      return this.VisitUnary((DbUnaryExpression) e, getEntityRefOp, new ITreeGenerator.VisitExprDelegate(this.VisitExprAsScalar));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbScanExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbScanExpression>(e, nameof (e));
      ScanTableOp scanTableOp = this._iqtCommand.CreateScanTableOp(Command.CreateTableDefinition(e.Target));
      System.Data.Entity.Core.Query.InternalTrees.Node node = this._iqtCommand.CreateNode((Op) scanTableOp);
      Var column = scanTableOp.Table.Columns[0];
      this._varMap[node] = column;
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbFilterExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbFilterExpression>(e, nameof (e));
      if (!ITreeGenerator.IsIsOfFilter(e) || this._processedIsOfFilters.Contains(e))
      {
        System.Data.Entity.Core.Query.InternalTrees.Node key = this.EnterExpressionBinding(e.Input);
        System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExprAsPredicate(e.Predicate);
        this.ExitExpressionBinding();
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateFilterOp(), key, node1);
        this._varMap[node2] = this._varMap[key];
        return node2;
      }
      DbIsOfExpression predicate = (DbIsOfExpression) e.Predicate;
      return this.VisitExpr(this.ApplyIsOfFilter(e.Input.Expression, new ITreeGenerator.IsOfFilter(predicate)));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbProjectExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbProjectExpression>(e, nameof (e));
      return e == this._discriminatedViewTopProject ? this.GenerateDiscriminatedProject(e) : this.GenerateStandardProject(e);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node GenerateDiscriminatedProject(
      DbProjectExpression e)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this._discriminatedViewTopProject != null, "if a project matches the pattern, there must be a corresponding discriminator map");
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.EnterExpressionBinding(e.Input);
      List<RelProperty> relProperties = new List<RelProperty>();
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      foreach (KeyValuePair<RelProperty, DbExpression> relProperty in this._discriminatorMap.RelPropertyMap)
      {
        relProperties.Add(relProperty.Key);
        nodeList.Add(this.VisitExprAsScalar(relProperty.Value));
      }
      DiscriminatedNewEntityOp discriminatedNewEntityOp = this._iqtCommand.CreateDiscriminatedNewEntityOp(e.Projection.ResultType, new ExplicitDiscriminatorMap(this._discriminatorMap), this._discriminatorMap.EntitySet, relProperties);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(this._discriminatorMap.PropertyMap.Count + 1);
      args.Add(this.CreateNewInstanceArgument(this._discriminatorMap.Discriminator.Property, (DbExpression) this._discriminatorMap.Discriminator));
      foreach (KeyValuePair<EdmProperty, DbExpression> property in this._discriminatorMap.PropertyMap)
      {
        DbExpression dbExpression = property.Value;
        System.Data.Entity.Core.Query.InternalTrees.Node instanceArgument = this.CreateNewInstanceArgument((EdmMember) property.Key, dbExpression);
        args.Add(instanceArgument);
      }
      args.AddRange((IEnumerable<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) discriminatedNewEntityOp, args);
      this.ExitExpressionBinding();
      Var computedVar;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(node2, out computedVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar), node1, varDefListNode);
      this._varMap[node3] = computedVar;
      return node3;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node CreateNewInstanceArgument(
      EdmMember property,
      DbExpression value)
    {
      return this.BuildSoftCast(this.VisitExprAsScalar(value), Helper.GetModelTypeUsage(property));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node GenerateStandardProject(
      DbProjectExpression e)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.EnterExpressionBinding(e.Input);
      System.Data.Entity.Core.Query.InternalTrees.Node definingExpr = this.VisitExprAsScalar(e.Projection);
      this.ExitExpressionBinding();
      Var computedVar;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this._iqtCommand.CreateVarDefListNode(definingExpr, out computedVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(computedVar), node1, varDefListNode);
      this._varMap[node2] = computedVar;
      return node2;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbCrossJoinExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbCrossJoinExpression>(e, nameof (e));
      return this.VisitJoin((DbExpression) e, e.Inputs, (DbExpression) null);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbJoinExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbJoinExpression>(e, nameof (e));
      return this.VisitJoin((DbExpression) e, (IList<DbExpressionBinding>) new List<DbExpressionBinding>()
      {
        e.Left,
        e.Right
      }, e.JoinCondition);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitJoin(
      DbExpression e,
      IList<DbExpressionBinding> inputs,
      DbExpression joinCond)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(DbExpressionKind.CrossJoin == e.ExpressionKind || DbExpressionKind.InnerJoin == e.ExpressionKind || DbExpressionKind.LeftOuterJoin == e.ExpressionKind || DbExpressionKind.FullOuterJoin == e.ExpressionKind, "Unrecognized JoinType specified in DbJoinExpression");
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      List<Var> varList = new List<Var>();
      for (int index = 0; index < inputs.Count; ++index)
      {
        Var boundVar;
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitBoundExpression(inputs[index].Expression, out boundVar);
        args.Add(node);
        varList.Add(boundVar);
      }
      for (int index = 0; index < args.Count; ++index)
        this.PushBindingScope(varList[index], inputs[index].VariableName);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExprAsPredicate(joinCond);
      for (int index = 0; index < args.Count; ++index)
        this.ExitExpressionBinding();
      JoinBaseOp joinBaseOp = (JoinBaseOp) null;
      switch (e.ExpressionKind)
      {
        case DbExpressionKind.CrossJoin:
          joinBaseOp = (JoinBaseOp) this._iqtCommand.CreateCrossJoinOp();
          break;
        case DbExpressionKind.FullOuterJoin:
          joinBaseOp = (JoinBaseOp) this._iqtCommand.CreateFullOuterJoinOp();
          break;
        case DbExpressionKind.InnerJoin:
          joinBaseOp = (JoinBaseOp) this._iqtCommand.CreateInnerJoinOp();
          break;
        case DbExpressionKind.LeftOuterJoin:
          joinBaseOp = (JoinBaseOp) this._iqtCommand.CreateLeftOuterJoinOp();
          break;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(joinBaseOp != null, "Unrecognized JoinOp specified in DbJoinExpression, no JoinOp was produced");
      if (e.ExpressionKind != DbExpressionKind.CrossJoin)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(node1 != null, "Non CrossJoinOps must specify a join condition");
        args.Add(node1);
      }
      return this.ProjectNewRecord(this._iqtCommand.CreateNode((Op) joinBaseOp, args), ITreeGenerator.ExtractElementRowType(e.ResultType), (IEnumerable<Var>) varList);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbApplyExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbApplyExpression>(e, nameof (e));
      System.Data.Entity.Core.Query.InternalTrees.Node key1 = this.EnterExpressionBinding(e.Input);
      System.Data.Entity.Core.Query.InternalTrees.Node key2 = this.EnterExpressionBinding(e.Apply);
      this.ExitExpressionBinding();
      this.ExitExpressionBinding();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(DbExpressionKind.CrossApply == e.ExpressionKind || DbExpressionKind.OuterApply == e.ExpressionKind, "Unrecognized DbExpressionKind specified in DbApplyExpression");
      return this.ProjectNewRecord(this._iqtCommand.CreateNode(DbExpressionKind.CrossApply != e.ExpressionKind ? (Op) this._iqtCommand.CreateOuterApplyOp() : (Op) this._iqtCommand.CreateCrossApplyOp(), key1, key2), ITreeGenerator.ExtractElementRowType(e.ResultType), (IEnumerable<Var>) new Var[2]
      {
        this._varMap[key1],
        this._varMap[key2]
      });
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbGroupByExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGroupByExpression>(e, nameof (e));
      VarVec varVec1 = this._iqtCommand.CreateVarVec();
      VarVec varVec2 = this._iqtCommand.CreateVarVec();
      System.Data.Entity.Core.Query.InternalTrees.Node inputNode1;
      List<System.Data.Entity.Core.Query.InternalTrees.Node> keyVarDefNodes1;
      ITreeGenerator.ExpressionBindingScope scope;
      this.ExtractKeys(e, varVec1, varVec2, out inputNode1, out keyVarDefNodes1, out scope);
      int num = -1;
      for (int index = 0; index < e.Aggregates.Count; ++index)
      {
        if (e.Aggregates[index].GetType() == typeof (DbGroupAggregate))
        {
          num = index;
          break;
        }
      }
      System.Data.Entity.Core.Query.InternalTrees.Node inputNode2 = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      List<System.Data.Entity.Core.Query.InternalTrees.Node> keyVarDefNodes2 = (List<System.Data.Entity.Core.Query.InternalTrees.Node>) null;
      VarVec varVec3 = this._iqtCommand.CreateVarVec();
      VarVec varVec4 = this._iqtCommand.CreateVarVec();
      if (num >= 0)
        this.ExtractKeys(e, varVec4, varVec3, out inputNode2, out keyVarDefNodes2, out ITreeGenerator.ExpressionBindingScope _);
      this._varScopes.Push((ITreeGenerator.CqtVariableScope) new ITreeGenerator.ExpressionBindingScope(this._iqtCommand, e.Input.GroupVariableName, scope.ScopeVar));
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args1 = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      System.Data.Entity.Core.Query.InternalTrees.Node node = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      for (int index = 0; index < e.Aggregates.Count; ++index)
      {
        DbAggregate aggregate = e.Aggregates[index];
        IList<System.Data.Entity.Core.Query.InternalTrees.Node> argNodes = this.VisitExprAsScalar(aggregate.Arguments);
        Var v;
        if (index != num)
        {
          DbFunctionAggregate funcAgg = aggregate as DbFunctionAggregate;
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(funcAgg != null, "Unrecognized DbAggregate used in DbGroupByExpression");
          args1.Add(this.ProcessFunctionAggregate(funcAgg, argNodes, out v));
        }
        else
          node = this.ProcessGroupAggregate(keyVarDefNodes1, inputNode2, keyVarDefNodes2, varVec4, e.Input.Expression.ResultType, out v);
        varVec2.Set(v);
      }
      this.ExitGroupExpressionBinding();
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args2 = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      args2.Add(inputNode1);
      args2.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp(), keyVarDefNodes1));
      args2.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp(), args1));
      GroupByBaseOp groupByBaseOp;
      if (num >= 0)
      {
        args2.Add(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp(), node));
        groupByBaseOp = (GroupByBaseOp) this._iqtCommand.CreateGroupByIntoOp(varVec1, this._iqtCommand.CreateVarVec(this._varMap[inputNode1]), varVec2);
      }
      else
        groupByBaseOp = (GroupByBaseOp) this._iqtCommand.CreateGroupByOp(varVec1, varVec2);
      return this.ProjectNewRecord(this._iqtCommand.CreateNode((Op) groupByBaseOp, args2), ITreeGenerator.ExtractElementRowType(e.ResultType), (IEnumerable<Var>) varVec2);
    }

    private void ExtractKeys(
      DbGroupByExpression e,
      VarVec keyVarSet,
      VarVec outputVarSet,
      out System.Data.Entity.Core.Query.InternalTrees.Node inputNode,
      out List<System.Data.Entity.Core.Query.InternalTrees.Node> keyVarDefNodes,
      out ITreeGenerator.ExpressionBindingScope scope)
    {
      inputNode = this.EnterGroupExpressionBinding(e.Input);
      keyVarDefNodes = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      for (int index = 0; index < e.Keys.Count; ++index)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node definingExpr = this.VisitExprAsScalar(e.Keys[index]);
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(definingExpr.Op is ScalarOp, "GroupBy Key is not a ScalarOp");
        Var computedVar;
        keyVarDefNodes.Add(this._iqtCommand.CreateVarDefNode(definingExpr, out computedVar));
        outputVarSet.Set(computedVar);
        keyVarSet.Set(computedVar);
      }
      scope = this.ExitExpressionBinding();
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ProcessFunctionAggregate(
      DbFunctionAggregate funcAgg,
      IList<System.Data.Entity.Core.Query.InternalTrees.Node> argNodes,
      out Var aggVar)
    {
      return this._iqtCommand.CreateVarDefNode(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateAggregateOp(funcAgg.Function, funcAgg.Distinct), argNodes), out aggVar);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ProcessGroupAggregate(
      List<System.Data.Entity.Core.Query.InternalTrees.Node> keyVarDefNodes,
      System.Data.Entity.Core.Query.InternalTrees.Node copyOfInput,
      List<System.Data.Entity.Core.Query.InternalTrees.Node> copyOfkeyVarDefNodes,
      VarVec copyKeyVarSet,
      TypeUsage inputResultType,
      out Var groupAggVar)
    {
      Var var1 = this._varMap[copyOfInput];
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = copyOfInput;
      if (keyVarDefNodes.Count > 0)
      {
        VarVec varVec = this._iqtCommand.CreateVarVec();
        varVec.Set(var1);
        varVec.Or(copyKeyVarSet);
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(varVec), node1, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp(), copyOfkeyVarDefNodes));
        List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList1 = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList2 = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        for (int index = 0; index < keyVarDefNodes.Count; ++index)
        {
          System.Data.Entity.Core.Query.InternalTrees.Node keyVarDefNode = keyVarDefNodes[index];
          System.Data.Entity.Core.Query.InternalTrees.Node copyOfkeyVarDefNode = copyOfkeyVarDefNodes[index];
          Var var2 = ((VarDefOp) keyVarDefNode.Op).Var;
          Var var3 = ((VarDefOp) copyOfkeyVarDefNode.Op).Var;
          this.FlattenProperties(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp(var2)), (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList1);
          this.FlattenProperties(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarRefOp(var3)), (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList2);
        }
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(nodeList1.Count == nodeList2.Count, "The flattened keys lists should have the same number of elements");
        System.Data.Entity.Core.Query.InternalTrees.Node node3 = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
        for (int index = 0; index < nodeList1.Count; ++index)
        {
          System.Data.Entity.Core.Query.InternalTrees.Node n1 = nodeList1[index];
          System.Data.Entity.Core.Query.InternalTrees.Node n2 = nodeList2[index];
          System.Data.Entity.Core.Query.InternalTrees.Node node4 = !this._useDatabaseNullSemantics ? this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateComparisonOp(OpType.EQ), n1, n2) : this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Or), this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateComparisonOp(OpType.EQ), n1, n2), this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.And), this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.IsNull), OpCopier.Copy(this._iqtCommand, n1)), this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.IsNull), OpCopier.Copy(this._iqtCommand, n2))));
          node3 = node3 != null ? this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.And), node3, node4) : node4;
        }
        node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateFilterOp(), node2, node3);
      }
      this._varMap[node1] = var1;
      return this._iqtCommand.CreateVarDefNode(this.ConvertRelOpToScalarOpTree(node1, inputResultType), out groupAggVar);
    }

    private void FlattenProperties(System.Data.Entity.Core.Query.InternalTrees.Node input, IList<System.Data.Entity.Core.Query.InternalTrees.Node> flattenedProperties)
    {
      if (input.Op.Type.EdmType.BuiltInTypeKind == BuiltInTypeKind.RowType)
      {
        IList<EdmProperty> properties = (IList<EdmProperty>) TypeHelpers.GetProperties(input.Op.Type);
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert((uint) properties.Count > 0U, "No nested properties for RowType");
        for (int index = 0; index < properties.Count; ++index)
        {
          System.Data.Entity.Core.Query.InternalTrees.Node node = index == 0 ? input : OpCopier.Copy(this._iqtCommand, input);
          this.FlattenProperties(this._iqtCommand.CreateNode((Op) this._iqtCommand.CreatePropertyOp((EdmMember) properties[index]), node), flattenedProperties);
        }
      }
      else
        flattenedProperties.Add(input);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitSortArguments(
      DbExpressionBinding input,
      IList<DbSortClause> sortOrder,
      List<SortKey> sortKeys,
      out Var inputVar)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node key = this.EnterExpressionBinding(input);
      inputVar = this._varMap[key];
      VarVec varVec = this._iqtCommand.CreateVarVec();
      varVec.Set(inputVar);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(sortKeys.Count == 0, "Non-empty SortKey list before adding converted SortClauses");
      for (int index = 0; index < sortOrder.Count; ++index)
      {
        DbSortClause dbSortClause = sortOrder[index];
        System.Data.Entity.Core.Query.InternalTrees.Node definingExpr = this.VisitExprAsScalar(dbSortClause.Expression);
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(definingExpr.Op is ScalarOp, "DbSortClause Expression converted to non-ScalarOp");
        Var computedVar;
        args.Add(this._iqtCommand.CreateVarDefNode(definingExpr, out computedVar));
        varVec.Set(computedVar);
        SortKey sortKey = !string.IsNullOrEmpty(dbSortClause.Collation) ? Command.CreateSortKey(computedVar, dbSortClause.Ascending, dbSortClause.Collation) : Command.CreateSortKey(computedVar, dbSortClause.Ascending);
        sortKeys.Add(sortKey);
      }
      this.ExitExpressionBinding();
      return this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateProjectOp(varVec), key, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateVarDefListOp(), args));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbSkipExpression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSkipExpression>(expression, nameof (expression));
      List<SortKey> sortKeys = new List<SortKey>();
      Var inputVar;
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitSortArguments(expression.Input, expression.SortOrder, sortKeys, out inputVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.VisitExprAsScalar(expression.Count);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConstrainedSortOp(sortKeys), node1, node2, this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateNullOp(this._iqtCommand.IntegerType)));
      this._varMap[node3] = inputVar;
      return node3;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DbSortExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbSortExpression>(e, nameof (e));
      List<SortKey> sortKeys = new List<SortKey>();
      Var inputVar;
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitSortArguments(e.Input, e.SortOrder, sortKeys, out inputVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateSortOp(sortKeys), node1);
      this._varMap[node2] = inputVar;
      return node2;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DbQuantifierExpression e)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbQuantifierExpression>(e, nameof (e));
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(DbExpressionKind.Any == e.ExpressionKind || e.ExpressionKind == DbExpressionKind.All, "Invalid DbExpressionKind in DbQuantifierExpression");
      System.Data.Entity.Core.Query.InternalTrees.Node key = this.EnterExpressionBinding(e.Input);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.VisitExprAsPredicate(e.Predicate);
      if (e.ExpressionKind == DbExpressionKind.All)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node2 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Not), node1);
        System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.VisitExprAsScalar(e.Predicate);
        System.Data.Entity.Core.Query.InternalTrees.Node node4 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.IsNull), node3);
        node1 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Or), node2, node4);
      }
      this.ExitExpressionBinding();
      Var var = this._varMap[key];
      System.Data.Entity.Core.Query.InternalTrees.Node node5 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateFilterOp(), key, node1);
      this._varMap[node5] = var;
      System.Data.Entity.Core.Query.InternalTrees.Node node6 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateExistsOp(), node5);
      if (e.ExpressionKind == DbExpressionKind.All)
        node6 = this._iqtCommand.CreateNode((Op) this._iqtCommand.CreateConditionalOp(OpType.Not), node6);
      return node6;
    }

    private abstract class CqtVariableScope
    {
      internal abstract bool Contains(string varName);

      internal abstract System.Data.Entity.Core.Query.InternalTrees.Node this[string varName] { get; }

      internal abstract bool IsPredicate(string varName);
    }

    private class ExpressionBindingScope : ITreeGenerator.CqtVariableScope
    {
      private readonly Command _tree;
      private readonly string _varName;
      private readonly Var _var;

      internal ExpressionBindingScope(Command iqtTree, string name, Var iqtVar)
      {
        this._tree = iqtTree;
        this._varName = name;
        this._var = iqtVar;
      }

      internal override bool Contains(string name) => this._varName == name;

      internal override System.Data.Entity.Core.Query.InternalTrees.Node this[string name]
      {
        get
        {
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(name == this._varName, "huh?");
          return this._tree.CreateNode((Op) this._tree.CreateVarRefOp(this._var));
        }
      }

      internal override bool IsPredicate(string varName) => false;

      internal Var ScopeVar => this._var;
    }

    private sealed class LambdaScope : ITreeGenerator.CqtVariableScope
    {
      private readonly ITreeGenerator _treeGen;
      private readonly Command _command;
      private readonly Dictionary<string, Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>> _arguments;
      private readonly Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, bool> _referencedArgs;

      internal LambdaScope(
        ITreeGenerator treeGen,
        Command command,
        Dictionary<string, Tuple<System.Data.Entity.Core.Query.InternalTrees.Node, bool>> args)
      {
        this._treeGen = treeGen;
        this._command = command;
        this._arguments = args;
        this._referencedArgs = new Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, bool>(this._arguments.Count);
      }

      internal override bool Contains(string name) => this._arguments.ContainsKey(name);

      internal override System.Data.Entity.Core.Query.InternalTrees.Node this[string name]
      {
        get
        {
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this._arguments.ContainsKey(name), "LambdaScope indexer called for invalid Var");
          System.Data.Entity.Core.Query.InternalTrees.Node node1 = this._arguments[name].Item1;
          if (this._referencedArgs.ContainsKey(node1))
          {
            System.Data.Entity.Core.Query.InternalTrees.VarMap varMap = (System.Data.Entity.Core.Query.InternalTrees.VarMap) null;
            System.Data.Entity.Core.Query.InternalTrees.Node node2 = OpCopier.Copy(this._command, node1, out varMap);
            if (varMap.Count > 0)
              this.MapCopiedNodeVars((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) new List<System.Data.Entity.Core.Query.InternalTrees.Node>(1)
              {
                node1
              }, (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) new List<System.Data.Entity.Core.Query.InternalTrees.Node>(1)
              {
                node2
              }, (IDictionary<Var, Var>) varMap);
            node1 = node2;
          }
          else
            this._referencedArgs[node1] = true;
          return node1;
        }
      }

      internal override bool IsPredicate(string name)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this._arguments.ContainsKey(name), "LambdaScope indexer called for invalid Var");
        return this._arguments[name].Item2;
      }

      private void MapCopiedNodeVars(
        IList<System.Data.Entity.Core.Query.InternalTrees.Node> sources,
        IList<System.Data.Entity.Core.Query.InternalTrees.Node> copies,
        IDictionary<Var, Var> varMappings)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(sources.Count == copies.Count, "Source/Copy Node count mismatch");
        for (int index = 0; index < sources.Count; ++index)
        {
          System.Data.Entity.Core.Query.InternalTrees.Node source = sources[index];
          System.Data.Entity.Core.Query.InternalTrees.Node copy = copies[index];
          if (source.Children.Count > 0)
            this.MapCopiedNodeVars((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) source.Children, (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) copy.Children, varMappings);
          Var key = (Var) null;
          if (this._treeGen.VarMap.TryGetValue(source, out key))
          {
            System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(varMappings.ContainsKey(key), "No mapping found for Var in Var to Var map from OpCopier");
            this._treeGen.VarMap[copy] = varMappings[key];
          }
        }
      }
    }

    private delegate System.Data.Entity.Core.Query.InternalTrees.Node VisitExprDelegate(
      DbExpression e);

    private class IsOfFilter
    {
      private readonly TypeUsage requiredType;
      private readonly bool isExact;
      private ITreeGenerator.IsOfFilter next;

      internal IsOfFilter(DbIsOfExpression template)
      {
        this.requiredType = template.OfType;
        this.isExact = template.ExpressionKind == DbExpressionKind.IsOfOnly;
      }

      internal IsOfFilter(DbOfTypeExpression template)
      {
        this.requiredType = template.OfType;
        this.isExact = template.ExpressionKind == DbExpressionKind.OfTypeOnly;
      }

      private IsOfFilter(TypeUsage required, bool exact)
      {
        this.requiredType = required;
        this.isExact = exact;
      }

      private ITreeGenerator.IsOfFilter Merge(
        TypeUsage otherRequiredType,
        bool otherIsExact)
      {
        bool flag = this.requiredType.EdmEquals((MetadataItem) otherRequiredType);
        ITreeGenerator.IsOfFilter isOfFilter;
        if (flag && this.isExact == otherIsExact)
          isOfFilter = this;
        else if (this.isExact & otherIsExact)
        {
          isOfFilter = new ITreeGenerator.IsOfFilter(otherRequiredType, otherIsExact);
          isOfFilter.next = this;
        }
        else if (!this.isExact && !otherIsExact)
        {
          if (otherRequiredType.IsSubtypeOf(this.requiredType))
          {
            isOfFilter = new ITreeGenerator.IsOfFilter(otherRequiredType, false);
            isOfFilter.next = this.next;
          }
          else if (this.requiredType.IsSubtypeOf(otherRequiredType))
          {
            isOfFilter = this;
          }
          else
          {
            isOfFilter = new ITreeGenerator.IsOfFilter(otherRequiredType, otherIsExact);
            isOfFilter.next = this;
          }
        }
        else if (flag)
        {
          isOfFilter = new ITreeGenerator.IsOfFilter(otherRequiredType, true);
          isOfFilter.next = this.next;
        }
        else
        {
          TypeUsage required = this.isExact ? this.requiredType : otherRequiredType;
          TypeUsage typeUsage = this.isExact ? otherRequiredType : this.requiredType;
          if (required.IsSubtypeOf(typeUsage))
          {
            if (required == this.requiredType && this.isExact)
            {
              isOfFilter = this;
            }
            else
            {
              isOfFilter = new ITreeGenerator.IsOfFilter(required, true);
              isOfFilter.next = this.next;
            }
          }
          else
          {
            isOfFilter = new ITreeGenerator.IsOfFilter(otherRequiredType, otherIsExact);
            isOfFilter.next = this;
          }
        }
        return isOfFilter;
      }

      internal ITreeGenerator.IsOfFilter Merge(DbIsOfExpression other) => this.Merge(other.OfType, other.ExpressionKind == DbExpressionKind.IsOfOnly);

      internal ITreeGenerator.IsOfFilter Merge(DbOfTypeExpression other) => this.Merge(other.OfType, other.ExpressionKind == DbExpressionKind.OfTypeOnly);

      internal IEnumerable<KeyValuePair<TypeUsage, bool>> ToEnumerable()
      {
        for (ITreeGenerator.IsOfFilter currentFilter = this; currentFilter != null; currentFilter = currentFilter.next)
          yield return new KeyValuePair<TypeUsage, bool>(currentFilter.requiredType, currentFilter.isExact);
      }
    }
  }
}
