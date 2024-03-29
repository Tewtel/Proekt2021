﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.SemanticAnalyzer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.EntitySql.AST;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class SemanticAnalyzer
  {
    private readonly SemanticResolver _sr;
    private static readonly DbExpressionKind[] _joinMap = new DbExpressionKind[4]
    {
      DbExpressionKind.CrossJoin,
      DbExpressionKind.InnerJoin,
      DbExpressionKind.LeftOuterJoin,
      DbExpressionKind.FullOuterJoin
    };
    private static readonly DbExpressionKind[] _applyMap = new DbExpressionKind[2]
    {
      DbExpressionKind.CrossApply,
      DbExpressionKind.OuterApply
    };
    private static readonly Dictionary<Type, SemanticAnalyzer.AstExprConverter> _astExprConverters = SemanticAnalyzer.CreateAstExprConverters();
    private static readonly Dictionary<BuiltInKind, SemanticAnalyzer.BuiltInExprConverter> _builtInExprConverter = SemanticAnalyzer.CreateBuiltInExprConverter();

    internal SemanticAnalyzer(SemanticResolver sr) => this._sr = sr;

    internal ParseResult AnalyzeCommand(System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr)
    {
      Command command = SemanticAnalyzer.ValidateQueryCommandAst(astExpr);
      SemanticAnalyzer.ConvertAndRegisterNamespaceImports(command.NamespaceImportList, command.ErrCtx, this._sr);
      return SemanticAnalyzer.ConvertStatement(command.Statement, this._sr);
    }

    internal DbLambda AnalyzeQueryCommand(System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr)
    {
      Command command = SemanticAnalyzer.ValidateQueryCommandAst(astExpr);
      SemanticAnalyzer.ConvertAndRegisterNamespaceImports(command.NamespaceImportList, command.ErrCtx, this._sr);
      return DbExpressionBuilder.Lambda(SemanticAnalyzer.ConvertQueryStatementToDbExpression(command.Statement, this._sr, out List<FunctionDefinition> _), (IEnumerable<DbVariableReferenceExpression>) this._sr.Variables.Values);
    }

    private static Command ValidateQueryCommandAst(System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr)
    {
      if (!(astExpr is Command command))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.UnknownAstCommandExpression);
      return command.Statement is QueryStatement ? command : throw new ArgumentException(System.Data.Entity.Resources.Strings.UnknownAstExpressionType);
    }

    private static void ConvertAndRegisterNamespaceImports(
      NodeList<NamespaceImport> nsImportList,
      ErrorContext cmdErrCtx,
      SemanticResolver sr)
    {
      List<Tuple<string, MetadataNamespace, ErrorContext>> tupleList1 = new List<Tuple<string, MetadataNamespace, ErrorContext>>();
      List<Tuple<MetadataNamespace, ErrorContext>> tupleList2 = new List<Tuple<MetadataNamespace, ErrorContext>>();
      if (nsImportList != null)
      {
        foreach (NamespaceImport nsImport in (IEnumerable<NamespaceImport>) nsImportList)
        {
          string[] names = (string[]) null;
          if (nsImport.NamespaceName is Identifier namespaceName7)
            names = new string[1]{ namespaceName7.Name };
          if (nsImport.NamespaceName is DotExpr namespaceName8)
            namespaceName8.IsMultipartIdentifier(out names);
          if (names == null)
            throw EntitySqlException.Create(nsImport.NamespaceName.ErrCtx, System.Data.Entity.Resources.Strings.InvalidMetadataMemberName, (Exception) null);
          string str = nsImport.Alias != null ? nsImport.Alias.Name : (string) null;
          MetadataMember metadataMember = sr.ResolveMetadataMemberName(names, nsImport.NamespaceName.ErrCtx);
          MetadataNamespace metadataNamespace = metadataMember.MetadataMemberClass == MetadataMemberClass.Namespace ? (MetadataNamespace) metadataMember : throw EntitySqlException.Create(nsImport.NamespaceName.ErrCtx, System.Data.Entity.Resources.Strings.InvalidMetadataMemberClassResolution((object) metadataMember.Name, (object) metadataMember.MetadataMemberClassName, (object) MetadataNamespace.NamespaceClassName), (Exception) null);
          if (str != null)
            tupleList1.Add(Tuple.Create<string, MetadataNamespace, ErrorContext>(str, metadataNamespace, nsImport.ErrCtx));
          else
            tupleList2.Add(Tuple.Create<MetadataNamespace, ErrorContext>(metadataNamespace, nsImport.ErrCtx));
        }
      }
      sr.TypeResolver.AddNamespaceImport(new MetadataNamespace("Edm"), nsImportList != null ? nsImportList.ErrCtx : cmdErrCtx);
      foreach (Tuple<string, MetadataNamespace, ErrorContext> tuple in tupleList1)
        sr.TypeResolver.AddAliasedNamespaceImport(tuple.Item1, tuple.Item2, tuple.Item3);
      foreach (Tuple<MetadataNamespace, ErrorContext> tuple in tupleList2)
        sr.TypeResolver.AddNamespaceImport(tuple.Item1, tuple.Item2);
    }

    private static ParseResult ConvertStatement(
      Statement astStatement,
      SemanticResolver sr)
    {
      if (astStatement is QueryStatement)
        return new SemanticAnalyzer.StatementConverter(SemanticAnalyzer.ConvertQueryStatementToDbCommandTree)(astStatement, sr);
      throw new ArgumentException(System.Data.Entity.Resources.Strings.UnknownAstExpressionType);
    }

    private static ParseResult ConvertQueryStatementToDbCommandTree(
      Statement astStatement,
      SemanticResolver sr)
    {
      List<FunctionDefinition> functionDefs;
      DbExpression dbExpression = SemanticAnalyzer.ConvertQueryStatementToDbExpression(astStatement, sr, out functionDefs);
      return new ParseResult((DbCommandTree) DbQueryCommandTree.FromValidExpression(sr.TypeResolver.Perspective.MetadataWorkspace, sr.TypeResolver.Perspective.TargetDataspace, dbExpression, true, false), functionDefs);
    }

    private static DbExpression ConvertQueryStatementToDbExpression(
      Statement astStatement,
      SemanticResolver sr,
      out List<FunctionDefinition> functionDefs)
    {
      if (!(astStatement is QueryStatement queryStatement))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.UnknownAstExpressionType);
      functionDefs = SemanticAnalyzer.ConvertInlineFunctionDefinitions(queryStatement.FunctionDefList, sr);
      DbExpression input1 = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(queryStatement.Expr, sr);
      if (input1 == null)
        throw EntitySqlException.Create(queryStatement.Expr.ErrCtx, System.Data.Entity.Resources.Strings.ResultingExpressionTypeCannotBeNull, (Exception) null);
      if (input1 is DbScanExpression)
      {
        DbExpressionBinding input2 = input1.BindAs(sr.GenerateInternalName("extent"));
        input1 = (DbExpression) input2.Project((DbExpression) input2.Variable);
      }
      if (sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.NormalMode)
        SemanticAnalyzer.ValidateQueryResultType(input1.ResultType, queryStatement.Expr.ErrCtx);
      return input1;
    }

    private static void ValidateQueryResultType(TypeUsage resultType, ErrorContext errCtx)
    {
      if (Helper.IsCollectionType((GlobalItem) resultType.EdmType))
        SemanticAnalyzer.ValidateQueryResultType(((CollectionType) resultType.EdmType).TypeUsage, errCtx);
      else if (Helper.IsRowType((GlobalItem) resultType.EdmType))
      {
        foreach (EdmMember property in ((RowType) resultType.EdmType).Properties)
          SemanticAnalyzer.ValidateQueryResultType(property.TypeUsage, errCtx);
      }
      else if (Helper.IsAssociationType(resultType.EdmType))
      {
        string errorMessage = System.Data.Entity.Resources.Strings.InvalidQueryResultType((object) resultType.EdmType.FullName);
        throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
      }
    }

    private static List<FunctionDefinition> ConvertInlineFunctionDefinitions(
      NodeList<System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition> functionDefList,
      SemanticResolver sr)
    {
      List<FunctionDefinition> functionDefinitionList = new List<FunctionDefinition>();
      if (functionDefList != null)
      {
        List<InlineFunctionInfo> inlineFunctionInfoList = new List<InlineFunctionInfo>();
        foreach (System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition functionDef in (IEnumerable<System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition>) functionDefList)
        {
          string name = functionDef.Name;
          InlineFunctionInfo functionInfo = (InlineFunctionInfo) new SemanticAnalyzer.InlineFunctionInfoImpl(functionDef, SemanticAnalyzer.ConvertInlineFunctionParameterDefs(functionDef.Parameters, sr));
          inlineFunctionInfoList.Add(functionInfo);
          sr.TypeResolver.DeclareInlineFunction(name, functionInfo);
        }
        foreach (InlineFunctionInfo inlineFunctionInfo in inlineFunctionInfoList)
          functionDefinitionList.Add(new FunctionDefinition(inlineFunctionInfo.FunctionDefAst.Name, inlineFunctionInfo.GetLambda(sr), inlineFunctionInfo.FunctionDefAst.StartPosition, inlineFunctionInfo.FunctionDefAst.EndPosition));
      }
      return functionDefinitionList;
    }

    private static List<DbVariableReferenceExpression> ConvertInlineFunctionParameterDefs(
      NodeList<PropDefinition> parameterDefs,
      SemanticResolver sr)
    {
      List<DbVariableReferenceExpression> referenceExpressionList = new List<DbVariableReferenceExpression>();
      if (parameterDefs != null)
      {
        foreach (PropDefinition parameterDef in (IEnumerable<PropDefinition>) parameterDefs)
        {
          string name = parameterDef.Name.Name;
          if (referenceExpressionList.Exists((Predicate<DbVariableReferenceExpression>) (arg => sr.NameComparer.Compare(arg.VariableName, name) == 0)))
            throw EntitySqlException.Create(parameterDef.ErrCtx, System.Data.Entity.Resources.Strings.MultipleDefinitionsOfParameter((object) name), (Exception) null);
          DbVariableReferenceExpression referenceExpression = new DbVariableReferenceExpression(SemanticAnalyzer.ConvertTypeDefinition(parameterDef.Type, sr), name);
          referenceExpressionList.Add(referenceExpression);
        }
      }
      return referenceExpressionList;
    }

    private static DbLambda ConvertInlineFunctionDefinition(
      InlineFunctionInfo functionInfo,
      SemanticResolver sr)
    {
      sr.EnterScope();
      functionInfo.Parameters.Each<DbVariableReferenceExpression, Scope>((Func<DbVariableReferenceExpression, Scope>) (p => sr.CurrentScope.Add(p.VariableName, (ScopeEntry) new FreeVariableScopeEntry(p))));
      DbExpression body = SemanticAnalyzer.ConvertValueExpression(functionInfo.FunctionDefAst.Body, sr);
      sr.LeaveScope();
      List<DbVariableReferenceExpression> parameters = functionInfo.Parameters;
      return DbExpressionBuilder.Lambda(body, (IEnumerable<DbVariableReferenceExpression>) parameters);
    }

    private static ExpressionResolution Convert(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      SemanticAnalyzer.AstExprConverter astExprConverter = SemanticAnalyzer._astExprConverters[astExpr.GetType()];
      if (astExprConverter == null)
        throw new EntitySqlException(System.Data.Entity.Resources.Strings.UnknownAstExpressionType);
      return astExprConverter(astExpr, sr);
    }

    private static DbExpression ConvertValueExpression(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      return SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(astExpr, sr) ?? throw EntitySqlException.Create(astExpr.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionCannotBeNull, (Exception) null);
    }

    private static DbExpression ConvertValueExpressionAllowUntypedNulls(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      ExpressionResolution expressionResolution = SemanticAnalyzer.Convert(astExpr, sr);
      if (expressionResolution.ExpressionClass == ExpressionResolutionClass.Value)
        return ((ValueExpression) expressionResolution).Value;
      if (expressionResolution.ExpressionClass == ExpressionResolutionClass.MetadataMember)
      {
        MetadataMember metadataMember = (MetadataMember) expressionResolution;
        if (metadataMember.MetadataMemberClass == MetadataMemberClass.EnumMember)
        {
          MetadataEnumMember metadataEnumMember = (MetadataEnumMember) metadataMember;
          return (DbExpression) metadataEnumMember.EnumType.Constant(metadataEnumMember.EnumMember.Value);
        }
      }
      string errorMessage = System.Data.Entity.Resources.Strings.InvalidExpressionResolutionClass((object) expressionResolution.ExpressionClassName, (object) ValueExpression.ValueClassName);
      if (astExpr is Identifier identifier)
        errorMessage = System.Data.Entity.Resources.Strings.CouldNotResolveIdentifier((object) identifier.Name);
      string[] names;
      if (astExpr is DotExpr dotExpr && dotExpr.IsMultipartIdentifier(out names))
        errorMessage = System.Data.Entity.Resources.Strings.CouldNotResolveIdentifier((object) TypeResolver.GetFullName(names));
      throw EntitySqlException.Create(astExpr.ErrCtx, errorMessage, (Exception) null);
    }

    private static Pair<DbExpression, DbExpression> ConvertValueExpressionsWithUntypedNulls(
      System.Data.Entity.Core.Common.EntitySql.AST.Node leftAst,
      System.Data.Entity.Core.Common.EntitySql.AST.Node rightAst,
      ErrorContext errCtx,
      Func<string> formatMessage,
      SemanticResolver sr)
    {
      DbExpression left = leftAst != null ? SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(leftAst, sr) : (DbExpression) null;
      DbExpression right = rightAst != null ? SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(rightAst, sr) : (DbExpression) null;
      if (left == null)
      {
        if (right == null)
        {
          string errorMessage = formatMessage();
          throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
        }
        left = (DbExpression) right.ResultType.Null();
      }
      else if (right == null)
        right = (DbExpression) left.ResultType.Null();
      return new Pair<DbExpression, DbExpression>(left, right);
    }

    private static ExpressionResolution ConvertLiteral(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      Literal literal = (Literal) expr;
      return literal.IsNullLiteral ? (ExpressionResolution) new ValueExpression((DbExpression) null) : (ExpressionResolution) new ValueExpression((DbExpression) SemanticAnalyzer.GetLiteralTypeUsage(literal).Constant(literal.Value));
    }

    private static TypeUsage GetLiteralTypeUsage(Literal literal)
    {
      PrimitiveType primitiveType = (PrimitiveType) null;
      if (!ClrProviderManifest.Instance.TryGetPrimitiveType(literal.Type, out primitiveType))
        throw EntitySqlException.Create(literal.ErrCtx, System.Data.Entity.Resources.Strings.LiteralTypeNotFoundInMetadata((object) literal.OriginalValue), (Exception) null);
      return TypeHelpers.GetLiteralTypeUsage(primitiveType.PrimitiveTypeKind, literal.IsUnicodeString);
    }

    private static ExpressionResolution ConvertIdentifier(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      return SemanticAnalyzer.ConvertIdentifier((Identifier) expr, false, sr);
    }

    private static ExpressionResolution ConvertIdentifier(
      Identifier identifier,
      bool leftHandSideOfMemberAccess,
      SemanticResolver sr)
    {
      return sr.ResolveSimpleName(identifier.Name, leftHandSideOfMemberAccess, identifier.ErrCtx);
    }

    private static ExpressionResolution ConvertDotExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      DotExpr dotExpr = (DotExpr) expr;
      ValueExpression groupKeyResolution;
      if (sr.TryResolveDotExprAsGroupKeyAlternativeName(dotExpr, out groupKeyResolution))
        return (ExpressionResolution) groupKeyResolution;
      ExpressionResolution expressionResolution = !(dotExpr.Left is Identifier left) ? SemanticAnalyzer.Convert(dotExpr.Left, sr) : SemanticAnalyzer.ConvertIdentifier(left, true, sr);
      switch (expressionResolution.ExpressionClass)
      {
        case ExpressionResolutionClass.Value:
          return (ExpressionResolution) sr.ResolvePropertyAccess(((ValueExpression) expressionResolution).Value, dotExpr.Identifier.Name, dotExpr.Identifier.ErrCtx);
        case ExpressionResolutionClass.EntityContainer:
          return sr.ResolveEntityContainerMemberAccess(((EntityContainerExpression) expressionResolution).EntityContainer, dotExpr.Identifier.Name, dotExpr.Identifier.ErrCtx);
        case ExpressionResolutionClass.MetadataMember:
          return (ExpressionResolution) sr.ResolveMetadataMemberAccess((MetadataMember) expressionResolution, dotExpr.Identifier.Name, dotExpr.Identifier.ErrCtx);
        default:
          throw EntitySqlException.Create(dotExpr.Left.ErrCtx, System.Data.Entity.Resources.Strings.UnknownExpressionResolutionClass((object) expressionResolution.ExpressionClass), (Exception) null);
      }
    }

    private static ExpressionResolution ConvertParenExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      return (ExpressionResolution) new ValueExpression(SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(((ParenExpr) astExpr).Expr, sr));
    }

    private static ExpressionResolution ConvertGroupPartitionExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      GroupPartitionExpr groupPartitionExpr = (GroupPartitionExpr) astExpr;
      DbExpression converted = (DbExpression) null;
      if (!SemanticAnalyzer.TryConvertAsResolvedGroupAggregate((GroupAggregateExpr) groupPartitionExpr, sr, out converted))
      {
        if (!sr.IsInAnyGroupScope())
          throw EntitySqlException.Create(astExpr.ErrCtx, System.Data.Entity.Resources.Strings.GroupPartitionOutOfContext, (Exception) null);
        GroupPartitionInfo aggregateInfo;
        DbExpression projection;
        using (sr.EnterGroupPartition(groupPartitionExpr, groupPartitionExpr.ErrCtx, out aggregateInfo))
          projection = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(groupPartitionExpr.ArgExpr, sr);
        if (projection == null)
          throw EntitySqlException.Create(groupPartitionExpr.ArgExpr.ErrCtx, System.Data.Entity.Resources.Strings.ResultingExpressionTypeCannotBeNull, (Exception) null);
        DbExpression aggregateDefinition = (DbExpression) aggregateInfo.EvaluatingScopeRegion.GroupAggregateBinding.Project(projection);
        if (groupPartitionExpr.DistinctKind == DistinctKind.Distinct)
        {
          SemanticAnalyzer.ValidateDistinctProjection(aggregateDefinition.ResultType, groupPartitionExpr.ArgExpr.ErrCtx, (List<ErrorContext>) null);
          aggregateDefinition = (DbExpression) aggregateDefinition.Distinct();
        }
        aggregateInfo.AttachToAstNode(sr.GenerateInternalName("groupPartition"), aggregateDefinition);
        aggregateInfo.EvaluatingScopeRegion.GroupAggregateInfos.Add((GroupAggregateInfo) aggregateInfo);
        converted = (DbExpression) aggregateInfo.AggregateStubExpression;
      }
      return (ExpressionResolution) new ValueExpression(converted);
    }

    private static ExpressionResolution ConvertMethodExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      return SemanticAnalyzer.ConvertMethodExpr((MethodExpr) expr, true, sr);
    }

    private static ExpressionResolution ConvertMethodExpr(
      MethodExpr methodExpr,
      bool includeInlineFunctions,
      SemanticResolver sr)
    {
      ExpressionResolution expressionResolution;
      using (sr.TypeResolver.EnterFunctionNameResolution(includeInlineFunctions))
      {
        if (methodExpr.Expr is Identifier expr2)
        {
          expressionResolution = (ExpressionResolution) sr.ResolveSimpleFunctionName(expr2.Name, expr2.ErrCtx);
        }
        else
        {
          DotExpr expr = methodExpr.Expr as DotExpr;
          using (SemanticAnalyzer.ConvertMethodExpr_TryEnterIgnoreEntityContainerNameResolution(expr, sr))
          {
            using (SemanticAnalyzer.ConvertMethodExpr_TryEnterV1ViewGenBackwardCompatibilityResolution(expr, sr))
              expressionResolution = SemanticAnalyzer.Convert(methodExpr.Expr, sr);
          }
        }
      }
      MetadataMember metadataMember = expressionResolution.ExpressionClass == ExpressionResolutionClass.MetadataMember ? (MetadataMember) expressionResolution : throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.MethodInvocationNotSupported, (Exception) null);
      if (metadataMember.MetadataMemberClass == MetadataMemberClass.InlineFunctionGroup)
      {
        methodExpr.ErrCtx.ErrorContextInfo = System.Data.Entity.Resources.Strings.CtxFunction((object) metadataMember.Name);
        methodExpr.ErrCtx.UseContextInfoAsResourceIdentifier = false;
        ValueExpression inlineFunctionCall;
        return SemanticAnalyzer.TryConvertInlineFunctionCall((InlineFunctionGroup) metadataMember, methodExpr, sr, out inlineFunctionCall) ? (ExpressionResolution) inlineFunctionCall : SemanticAnalyzer.ConvertMethodExpr(methodExpr, false, sr);
      }
      switch (metadataMember.MetadataMemberClass)
      {
        case MetadataMemberClass.Type:
          methodExpr.ErrCtx.ErrorContextInfo = System.Data.Entity.Resources.Strings.CtxTypeCtor((object) metadataMember.Name);
          methodExpr.ErrCtx.UseContextInfoAsResourceIdentifier = false;
          return (ExpressionResolution) SemanticAnalyzer.ConvertTypeConstructorCall((MetadataType) metadataMember, methodExpr, sr);
        case MetadataMemberClass.FunctionGroup:
          methodExpr.ErrCtx.ErrorContextInfo = System.Data.Entity.Resources.Strings.CtxFunction((object) metadataMember.Name);
          methodExpr.ErrCtx.UseContextInfoAsResourceIdentifier = false;
          return (ExpressionResolution) SemanticAnalyzer.ConvertModelFunctionCall((MetadataFunctionGroup) metadataMember, methodExpr, sr);
        default:
          throw EntitySqlException.Create(methodExpr.Expr.ErrCtx, System.Data.Entity.Resources.Strings.CannotResolveNameToTypeOrFunction((object) metadataMember.Name), (Exception) null);
      }
    }

    private static IDisposable ConvertMethodExpr_TryEnterIgnoreEntityContainerNameResolution(
      DotExpr leftExpr,
      SemanticResolver sr)
    {
      return leftExpr == null || !(leftExpr.Left is Identifier) ? (IDisposable) null : sr.EnterIgnoreEntityContainerNameResolution();
    }

    private static IDisposable ConvertMethodExpr_TryEnterV1ViewGenBackwardCompatibilityResolution(
      DotExpr leftExpr,
      SemanticResolver sr)
    {
      return leftExpr != null && leftExpr.Left is Identifier && (sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.RestrictedViewGenerationMode || sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.UserViewGenerationMode) && (sr.TypeResolver.Perspective.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace) as StorageMappingItemCollection).MappingVersion < 2.0 ? sr.TypeResolver.EnterBackwardCompatibilityResolution() : (IDisposable) null;
    }

    private static bool TryConvertInlineFunctionCall(
      InlineFunctionGroup inlineFunctionGroup,
      MethodExpr methodExpr,
      SemanticResolver sr,
      out ValueExpression inlineFunctionCall)
    {
      inlineFunctionCall = (ValueExpression) null;
      if (methodExpr.DistinctKind != DistinctKind.None)
        return false;
      List<TypeUsage> argTypes;
      List<DbExpression> args = SemanticAnalyzer.ConvertFunctionArguments(methodExpr.Args, sr, out argTypes);
      bool isAmbiguous = false;
      InlineFunctionInfo inlineFunctionInfo = SemanticResolver.ResolveFunctionOverloads<InlineFunctionInfo, DbVariableReferenceExpression>(inlineFunctionGroup.FunctionMetadata, (IList<TypeUsage>) argTypes, (Func<InlineFunctionInfo, IList<DbVariableReferenceExpression>>) (lambdaOverload => (IList<DbVariableReferenceExpression>) lambdaOverload.Parameters), (Func<DbVariableReferenceExpression, TypeUsage>) (varRef => varRef.ResultType), (Func<DbVariableReferenceExpression, ParameterMode>) (varRef => ParameterMode.In), false, out isAmbiguous);
      if (isAmbiguous)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.AmbiguousFunctionArguments, (Exception) null);
      if (inlineFunctionInfo == null)
        return false;
      SemanticAnalyzer.ConvertUntypedNullsInArguments<DbVariableReferenceExpression>(args, (IList<DbVariableReferenceExpression>) inlineFunctionInfo.Parameters, (Func<DbVariableReferenceExpression, TypeUsage>) (formal => formal.ResultType));
      inlineFunctionCall = new ValueExpression((DbExpression) inlineFunctionInfo.GetLambda(sr).Invoke((IEnumerable<DbExpression>) args));
      return true;
    }

    private static ValueExpression ConvertTypeConstructorCall(
      MetadataType metadataType,
      MethodExpr methodExpr,
      SemanticResolver sr)
    {
      if (!TypeSemantics.IsComplexType(metadataType.TypeUsage) && !TypeSemantics.IsEntityType(metadataType.TypeUsage) && !TypeSemantics.IsRelationshipType(metadataType.TypeUsage))
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCtorUseOnType((object) metadataType.TypeUsage.EdmType.FullName), (Exception) null);
      if (metadataType.TypeUsage.EdmType.Abstract)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.CannotInstantiateAbstractType((object) metadataType.TypeUsage.EdmType.FullName), (Exception) null);
      if (methodExpr.DistinctKind != DistinctKind.None)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidDistinctArgumentInCtor, (Exception) null);
      List<DbRelatedEntityRef> relshipExprList = (List<DbRelatedEntityRef>) null;
      if (methodExpr.HasRelationships)
      {
        if (sr.ParserOptions.ParserCompilationMode != ParserOptions.CompilationMode.RestrictedViewGenerationMode && sr.ParserOptions.ParserCompilationMode != ParserOptions.CompilationMode.UserViewGenerationMode)
          throw EntitySqlException.Create(methodExpr.Relationships.ErrCtx, System.Data.Entity.Resources.Strings.InvalidModeForWithRelationshipClause, (Exception) null);
        if (!(metadataType.TypeUsage.EdmType is EntityType edmType2))
          throw EntitySqlException.Create(methodExpr.Relationships.ErrCtx, System.Data.Entity.Resources.Strings.InvalidTypeForWithRelationshipClause, (Exception) null);
        HashSet<string> stringSet = new HashSet<string>();
        relshipExprList = new List<DbRelatedEntityRef>(methodExpr.Relationships.Count);
        for (int index = 0; index < methodExpr.Relationships.Count; ++index)
        {
          RelshipNavigationExpr relationship = methodExpr.Relationships[index];
          DbRelatedEntityRef relatedEntityRef = SemanticAnalyzer.ConvertRelatedEntityRef(relationship, edmType2, sr);
          string str = string.Join(":", new string[2]
          {
            relatedEntityRef.TargetEnd.DeclaringType.Identity,
            relatedEntityRef.TargetEnd.Identity
          });
          if (stringSet.Contains(str))
            throw EntitySqlException.Create(relationship.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipTargetMustBeUnique((object) str), (Exception) null);
          stringSet.Add(str);
          relshipExprList.Add(relatedEntityRef);
        }
      }
      return new ValueExpression(SemanticAnalyzer.CreateConstructorCallExpression(methodExpr, metadataType.TypeUsage, SemanticAnalyzer.ConvertFunctionArguments(methodExpr.Args, sr, out List<TypeUsage> _), relshipExprList, sr));
    }

    private static ValueExpression ConvertModelFunctionCall(
      MetadataFunctionGroup metadataFunctionGroup,
      MethodExpr methodExpr,
      SemanticResolver sr)
    {
      if (metadataFunctionGroup.FunctionMetadata.Any<EdmFunction>((Func<EdmFunction, bool>) (f => !f.IsComposableAttribute)))
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.CannotCallNoncomposableFunction((object) metadataFunctionGroup.Name), (Exception) null);
      return TypeSemantics.IsAggregateFunction(metadataFunctionGroup.FunctionMetadata[0]) && sr.IsInAnyGroupScope() ? new ValueExpression(SemanticAnalyzer.ConvertAggregateFunctionInGroupScope(methodExpr, metadataFunctionGroup, sr)) : new ValueExpression((DbExpression) SemanticAnalyzer.CreateModelFunctionCallExpression(methodExpr, metadataFunctionGroup, sr));
    }

    private static DbExpression ConvertAggregateFunctionInGroupScope(
      MethodExpr methodExpr,
      MetadataFunctionGroup metadataFunctionGroup,
      SemanticResolver sr)
    {
      DbExpression converted = (DbExpression) null;
      if (SemanticAnalyzer.TryConvertAsResolvedGroupAggregate((GroupAggregateExpr) methodExpr, sr, out converted))
        return converted;
      ScopeRegion scopeRegion = sr.CurrentGroupAggregateInfo != null ? sr.CurrentGroupAggregateInfo.InnermostReferencedScopeRegion : (ScopeRegion) null;
      List<TypeUsage> argTypes;
      if (SemanticAnalyzer.TryConvertAsCollectionFunction(methodExpr, metadataFunctionGroup, sr, out argTypes, out converted))
        return converted;
      if (sr.CurrentGroupAggregateInfo != null)
        sr.CurrentGroupAggregateInfo.InnermostReferencedScopeRegion = scopeRegion;
      if (SemanticAnalyzer.TryConvertAsFunctionAggregate(methodExpr, metadataFunctionGroup, argTypes, sr, out converted))
        return converted;
      throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.FailedToResolveAggregateFunction((object) metadataFunctionGroup.Name), (Exception) null);
    }

    private static bool TryConvertAsResolvedGroupAggregate(
      GroupAggregateExpr groupAggregateExpr,
      SemanticResolver sr,
      out DbExpression converted)
    {
      converted = (DbExpression) null;
      if (groupAggregateExpr.AggregateInfo == null)
        return false;
      groupAggregateExpr.AggregateInfo.SetContainingAggregate(sr.CurrentGroupAggregateInfo);
      if (!sr.TryResolveInternalAggregateName(groupAggregateExpr.AggregateInfo.AggregateName, groupAggregateExpr.AggregateInfo.ErrCtx, out converted))
        converted = (DbExpression) groupAggregateExpr.AggregateInfo.AggregateStubExpression;
      return true;
    }

    private static bool TryConvertAsCollectionFunction(
      MethodExpr methodExpr,
      MetadataFunctionGroup metadataFunctionGroup,
      SemanticResolver sr,
      out List<TypeUsage> argTypes,
      out DbExpression converted)
    {
      List<DbExpression> args = SemanticAnalyzer.ConvertFunctionArguments(methodExpr.Args, sr, out argTypes);
      bool isAmbiguous = false;
      EdmFunction function = SemanticResolver.ResolveFunctionOverloads(metadataFunctionGroup.FunctionMetadata, (IList<TypeUsage>) argTypes, false, out isAmbiguous);
      if (isAmbiguous)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.AmbiguousFunctionArguments, (Exception) null);
      if (function != null)
      {
        SemanticAnalyzer.ConvertUntypedNullsInArguments<FunctionParameter>(args, (IList<FunctionParameter>) function.Parameters, (Func<FunctionParameter, TypeUsage>) (parameter => parameter.TypeUsage));
        converted = (DbExpression) function.Invoke((IEnumerable<DbExpression>) args);
        return true;
      }
      converted = (DbExpression) null;
      return false;
    }

    private static bool TryConvertAsFunctionAggregate(
      MethodExpr methodExpr,
      MetadataFunctionGroup metadataFunctionGroup,
      List<TypeUsage> argTypes,
      SemanticResolver sr,
      out DbExpression converted)
    {
      converted = (DbExpression) null;
      bool isAmbiguous = false;
      EdmFunction function = SemanticResolver.ResolveFunctionOverloads(metadataFunctionGroup.FunctionMetadata, (IList<TypeUsage>) argTypes, true, out isAmbiguous);
      if (isAmbiguous)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.AmbiguousFunctionArguments, (Exception) null);
      if (function == null)
        CqlErrorHelper.ReportFunctionOverloadError(methodExpr, metadataFunctionGroup.FunctionMetadata[0], argTypes);
      FunctionAggregateInfo aggregateInfo;
      List<DbExpression> args;
      using (sr.EnterFunctionAggregate(methodExpr, methodExpr.ErrCtx, out aggregateInfo))
        args = SemanticAnalyzer.ConvertFunctionArguments(methodExpr.Args, sr, out List<TypeUsage> _);
      SemanticAnalyzer.ConvertUntypedNullsInArguments<FunctionParameter>(args, (IList<FunctionParameter>) function.Parameters, (Func<FunctionParameter, TypeUsage>) (parameter => TypeHelpers.GetElementTypeUsage(parameter.TypeUsage)));
      DbFunctionAggregate functionAggregate = methodExpr.DistinctKind != DistinctKind.Distinct ? function.Aggregate((IEnumerable<DbExpression>) args) : function.AggregateDistinct((IEnumerable<DbExpression>) args);
      aggregateInfo.AttachToAstNode(sr.GenerateInternalName("groupAgg" + function.Name), (DbAggregate) functionAggregate);
      aggregateInfo.EvaluatingScopeRegion.GroupAggregateInfos.Add((GroupAggregateInfo) aggregateInfo);
      converted = (DbExpression) aggregateInfo.AggregateStubExpression;
      return true;
    }

    private static DbExpression CreateConstructorCallExpression(
      MethodExpr methodExpr,
      TypeUsage type,
      List<DbExpression> args,
      List<DbRelatedEntityRef> relshipExprList,
      SemanticResolver sr)
    {
      int index = 0;
      int count = args.Count;
      StructuralType edmType = (StructuralType) type.EdmType;
      foreach (EdmMember structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers((EdmType) edmType))
      {
        TypeUsage modelTypeUsage = Helper.GetModelTypeUsage(structuralMember);
        if (count <= index)
          throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.NumberOfTypeCtorIsLessThenFormalSpec((object) structuralMember.Name), (Exception) null);
        if (args[index] == null)
        {
          if (structuralMember is EdmProperty edmProperty4 && !edmProperty4.Nullable)
            throw EntitySqlException.Create(methodExpr.Args[index].ErrCtx, System.Data.Entity.Resources.Strings.InvalidNullLiteralForNonNullableMember((object) structuralMember.Name, (object) edmType.FullName), (Exception) null);
          args[index] = (DbExpression) modelTypeUsage.Null();
        }
        bool flag = TypeSemantics.IsPromotableTo(args[index].ResultType, modelTypeUsage);
        if (ParserOptions.CompilationMode.RestrictedViewGenerationMode == sr.ParserOptions.ParserCompilationMode || ParserOptions.CompilationMode.UserViewGenerationMode == sr.ParserOptions.ParserCompilationMode)
        {
          if (!flag && !TypeSemantics.IsPromotableTo(modelTypeUsage, args[index].ResultType))
            throw EntitySqlException.Create(methodExpr.Args[index].ErrCtx, System.Data.Entity.Resources.Strings.InvalidCtorArgumentType((object) args[index].ResultType.EdmType.FullName, (object) structuralMember.Name, (object) modelTypeUsage.EdmType.FullName), (Exception) null);
          if (Helper.IsPrimitiveType(modelTypeUsage.EdmType) && !TypeSemantics.IsSubTypeOf(args[index].ResultType, modelTypeUsage))
            args[index] = (DbExpression) args[index].CastTo(modelTypeUsage);
        }
        else if (!flag)
          throw EntitySqlException.Create(methodExpr.Args[index].ErrCtx, System.Data.Entity.Resources.Strings.InvalidCtorArgumentType((object) args[index].ResultType.EdmType.FullName, (object) structuralMember.Name, (object) modelTypeUsage.EdmType.FullName), (Exception) null);
        ++index;
      }
      if (index != count)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.NumberOfTypeCtorIsMoreThenFormalSpec((object) edmType.FullName), (Exception) null);
      return relshipExprList == null || relshipExprList.Count <= 0 ? (DbExpression) TypeHelpers.GetReadOnlyType(type).New((IEnumerable<DbExpression>) args) : (DbExpression) DbExpressionBuilder.CreateNewEntityWithRelationshipsExpression((EntityType) type.EdmType, (IList<DbExpression>) args, (IList<DbRelatedEntityRef>) relshipExprList);
    }

    private static DbFunctionExpression CreateModelFunctionCallExpression(
      MethodExpr methodExpr,
      MetadataFunctionGroup metadataFunctionGroup,
      SemanticResolver sr)
    {
      bool isAmbiguous = false;
      if (methodExpr.DistinctKind != DistinctKind.None)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidDistinctArgumentInNonAggFunction, (Exception) null);
      List<TypeUsage> argTypes;
      List<DbExpression> args = SemanticAnalyzer.ConvertFunctionArguments(methodExpr.Args, sr, out argTypes);
      EdmFunction function = SemanticResolver.ResolveFunctionOverloads(metadataFunctionGroup.FunctionMetadata, (IList<TypeUsage>) argTypes, false, out isAmbiguous);
      if (isAmbiguous)
        throw EntitySqlException.Create(methodExpr.ErrCtx, System.Data.Entity.Resources.Strings.AmbiguousFunctionArguments, (Exception) null);
      if (function == null)
        CqlErrorHelper.ReportFunctionOverloadError(methodExpr, metadataFunctionGroup.FunctionMetadata[0], argTypes);
      SemanticAnalyzer.ConvertUntypedNullsInArguments<FunctionParameter>(args, (IList<FunctionParameter>) function.Parameters, (Func<FunctionParameter, TypeUsage>) (parameter => parameter.TypeUsage));
      return function.Invoke((IEnumerable<DbExpression>) args);
    }

    private static List<DbExpression> ConvertFunctionArguments(
      NodeList<System.Data.Entity.Core.Common.EntitySql.AST.Node> astExprList,
      SemanticResolver sr,
      out List<TypeUsage> argTypes)
    {
      List<DbExpression> source = new List<DbExpression>();
      if (astExprList != null)
      {
        for (int index = 0; index < astExprList.Count; ++index)
          source.Add(SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(astExprList[index], sr));
      }
      argTypes = source.Select<DbExpression, TypeUsage>((Func<DbExpression, TypeUsage>) (a => a?.ResultType)).ToList<TypeUsage>();
      return source;
    }

    private static void ConvertUntypedNullsInArguments<TParameterMetadata>(
      List<DbExpression> args,
      IList<TParameterMetadata> parametersMetadata,
      Func<TParameterMetadata, TypeUsage> getParameterTypeUsage)
    {
      for (int index = 0; index < args.Count; ++index)
      {
        if (args[index] == null)
          args[index] = (DbExpression) getParameterTypeUsage(parametersMetadata[index]).Null();
      }
    }

    private static ExpressionResolution ConvertParameter(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      QueryParameter queryParameter = (QueryParameter) expr;
      DbParameterReferenceExpression referenceExpression;
      if (sr.Parameters == null || !sr.Parameters.TryGetValue(queryParameter.Name, out referenceExpression))
        throw EntitySqlException.Create(queryParameter.ErrCtx, System.Data.Entity.Resources.Strings.ParameterWasNotDefined((object) queryParameter.Name), (Exception) null);
      return (ExpressionResolution) new ValueExpression((DbExpression) referenceExpression);
    }

    private static DbRelatedEntityRef ConvertRelatedEntityRef(
      RelshipNavigationExpr relshipExpr,
      EntityType driverEntityType,
      SemanticResolver sr)
    {
      EdmType edmType = SemanticAnalyzer.ConvertTypeName(relshipExpr.TypeName, sr).EdmType;
      if (!(edmType is RelationshipType relationshipType))
        throw EntitySqlException.Create(relshipExpr.TypeName.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipTypeExpected((object) edmType.FullName), (Exception) null);
      DbExpression targetEntity = SemanticAnalyzer.ConvertValueExpression(relshipExpr.RefExpr, sr);
      RefType refType = targetEntity.ResultType.EdmType as RefType;
      if (refType == null)
        throw EntitySqlException.Create(relshipExpr.RefExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelatedEndExprTypeMustBeReference, (Exception) null);
      RelationshipEndMember toEnd;
      if (relshipExpr.ToEndIdentifier != null)
      {
        toEnd = (RelationshipEndMember) relationshipType.Members.FirstOrDefault<EdmMember>((Func<EdmMember, bool>) (m => m.Name.Equals(relshipExpr.ToEndIdentifier.Name, StringComparison.OrdinalIgnoreCase)));
        if (toEnd == null)
          throw EntitySqlException.Create(relshipExpr.ToEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.InvalidRelationshipMember((object) relshipExpr.ToEndIdentifier.Name, (object) relationshipType.FullName), (Exception) null);
        if (toEnd.RelationshipMultiplicity != RelationshipMultiplicity.One && toEnd.RelationshipMultiplicity != RelationshipMultiplicity.ZeroOrOne)
          throw EntitySqlException.Create(relshipExpr.ToEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.InvalidWithRelationshipTargetEndMultiplicity((object) toEnd.Name, (object) toEnd.RelationshipMultiplicity.ToString()), (Exception) null);
        if (!TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) refType, toEnd.TypeUsage.EdmType))
          throw EntitySqlException.Create(relshipExpr.RefExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelatedEndExprTypeMustBePromotoableToToEnd((object) refType.FullName, (object) toEnd.TypeUsage.EdmType.FullName), (Exception) null);
      }
      else
      {
        RelationshipEndMember[] array = relationshipType.Members.Select<EdmMember, RelationshipEndMember>((Func<EdmMember, RelationshipEndMember>) (m => (RelationshipEndMember) m)).Where<RelationshipEndMember>((Func<RelationshipEndMember, bool>) (e =>
        {
          if (!TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) refType, e.TypeUsage.EdmType))
            return false;
          return e.RelationshipMultiplicity == RelationshipMultiplicity.One || e.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne;
        })).ToArray<RelationshipEndMember>();
        switch (array.Length)
        {
          case 0:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidImplicitRelationshipToEnd((object) relationshipType.FullName), (Exception) null);
          case 1:
            toEnd = array[0];
            break;
          default:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipToEndIsAmbiguos, (Exception) null);
        }
      }
      RelationshipEndMember sourceEnd;
      if (relshipExpr.FromEndIdentifier != null)
      {
        sourceEnd = (RelationshipEndMember) relationshipType.Members.FirstOrDefault<EdmMember>((Func<EdmMember, bool>) (m => m.Name.Equals(relshipExpr.FromEndIdentifier.Name, StringComparison.OrdinalIgnoreCase)));
        if (sourceEnd == null)
          throw EntitySqlException.Create(relshipExpr.FromEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.InvalidRelationshipMember((object) relshipExpr.FromEndIdentifier.Name, (object) relationshipType.FullName), (Exception) null);
        if (!TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) driverEntityType.GetReferenceType(), sourceEnd.TypeUsage.EdmType))
          throw EntitySqlException.Create(relshipExpr.FromEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.SourceTypeMustBePromotoableToFromEndRelationType((object) driverEntityType.FullName, (object) sourceEnd.TypeUsage.EdmType.FullName), (Exception) null);
        if (sourceEnd.EdmEquals((MetadataItem) toEnd))
          throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipFromEndIsAmbiguos, (Exception) null);
      }
      else
      {
        RelationshipEndMember[] array = relationshipType.Members.Select<EdmMember, RelationshipEndMember>((Func<EdmMember, RelationshipEndMember>) (m => (RelationshipEndMember) m)).Where<RelationshipEndMember>((Func<RelationshipEndMember, bool>) (e => TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) driverEntityType.GetReferenceType(), e.TypeUsage.EdmType) && !e.EdmEquals((MetadataItem) toEnd))).ToArray<RelationshipEndMember>();
        switch (array.Length)
        {
          case 0:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidImplicitRelationshipFromEnd((object) relationshipType.FullName), (Exception) null);
          case 1:
            sourceEnd = array[0];
            break;
          default:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipFromEndIsAmbiguos, (Exception) null);
        }
      }
      return DbExpressionBuilder.CreateRelatedEntityRef(sourceEnd, toEnd, targetEntity);
    }

    private static ExpressionResolution ConvertRelshipNavigationExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      RelshipNavigationExpr relshipExpr = (RelshipNavigationExpr) astExpr;
      EdmType edmType = SemanticAnalyzer.ConvertTypeName(relshipExpr.TypeName, sr).EdmType;
      if (!(edmType is RelationshipType relationshipType))
        throw EntitySqlException.Create(relshipExpr.TypeName.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipTypeExpected((object) edmType.FullName), (Exception) null);
      DbExpression navigateFrom = SemanticAnalyzer.ConvertValueExpression(relshipExpr.RefExpr, sr);
      RefType sourceRefType = navigateFrom.ResultType.EdmType as RefType;
      if (sourceRefType == null)
      {
        if (!(navigateFrom.ResultType.EdmType is EntityType))
          throw EntitySqlException.Create(relshipExpr.RefExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelatedEndExprTypeMustBeReference, (Exception) null);
        navigateFrom = (DbExpression) navigateFrom.GetEntityRef();
        sourceRefType = (RefType) navigateFrom.ResultType.EdmType;
      }
      RelationshipEndMember toEnd;
      if (relshipExpr.ToEndIdentifier != null)
      {
        toEnd = (RelationshipEndMember) relationshipType.Members.FirstOrDefault<EdmMember>((Func<EdmMember, bool>) (m => m.Name.Equals(relshipExpr.ToEndIdentifier.Name, StringComparison.OrdinalIgnoreCase)));
        if (toEnd == null)
          throw EntitySqlException.Create(relshipExpr.ToEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.InvalidRelationshipMember((object) relshipExpr.ToEndIdentifier.Name, (object) relationshipType.FullName), (Exception) null);
      }
      else
        toEnd = (RelationshipEndMember) null;
      RelationshipEndMember fromEnd;
      if (relshipExpr.FromEndIdentifier != null)
      {
        fromEnd = (RelationshipEndMember) relationshipType.Members.FirstOrDefault<EdmMember>((Func<EdmMember, bool>) (m => m.Name.Equals(relshipExpr.FromEndIdentifier.Name, StringComparison.OrdinalIgnoreCase)));
        if (fromEnd == null)
          throw EntitySqlException.Create(relshipExpr.FromEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.InvalidRelationshipMember((object) relshipExpr.FromEndIdentifier.Name, (object) relationshipType.FullName), (Exception) null);
        if (!TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) sourceRefType, fromEnd.TypeUsage.EdmType))
          throw EntitySqlException.Create(relshipExpr.FromEndIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.SourceTypeMustBePromotoableToFromEndRelationType((object) sourceRefType.FullName, (object) fromEnd.TypeUsage.EdmType.FullName), (Exception) null);
        if (toEnd != null && fromEnd.EdmEquals((MetadataItem) toEnd))
          throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipFromEndIsAmbiguos, (Exception) null);
      }
      else
      {
        RelationshipEndMember[] array = relationshipType.Members.Select<EdmMember, RelationshipEndMember>((Func<EdmMember, RelationshipEndMember>) (m => (RelationshipEndMember) m)).Where<RelationshipEndMember>((Func<RelationshipEndMember, bool>) (e =>
        {
          if (!TypeSemantics.IsStructurallyEqualOrPromotableTo((EdmType) sourceRefType, e.TypeUsage.EdmType))
            return false;
          return toEnd == null || !e.EdmEquals((MetadataItem) toEnd);
        })).ToArray<RelationshipEndMember>();
        switch (array.Length)
        {
          case 0:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidImplicitRelationshipFromEnd((object) relationshipType.FullName), (Exception) null);
          case 1:
            fromEnd = array[0];
            break;
          default:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipFromEndIsAmbiguos, (Exception) null);
        }
      }
      if (toEnd == null)
      {
        RelationshipEndMember[] array = relationshipType.Members.Select<EdmMember, RelationshipEndMember>((Func<EdmMember, RelationshipEndMember>) (m => (RelationshipEndMember) m)).Where<RelationshipEndMember>((Func<RelationshipEndMember, bool>) (e => !e.EdmEquals((MetadataItem) fromEnd))).ToArray<RelationshipEndMember>();
        switch (array.Length)
        {
          case 0:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidImplicitRelationshipToEnd((object) relationshipType.FullName), (Exception) null);
          case 1:
            toEnd = array[0];
            break;
          default:
            throw EntitySqlException.Create(relshipExpr.ErrCtx, System.Data.Entity.Resources.Strings.RelationshipToEndIsAmbiguos, (Exception) null);
        }
      }
      return (ExpressionResolution) new ValueExpression((DbExpression) navigateFrom.Navigate(fromEnd, toEnd));
    }

    private static ExpressionResolution ConvertRefExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      RefExpr refExpr = (RefExpr) astExpr;
      DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(refExpr.ArgExpr, sr);
      return TypeSemantics.IsEntityType(dbExpression.ResultType) ? (ExpressionResolution) new ValueExpression((DbExpression) dbExpression.GetEntityRef()) : throw EntitySqlException.Create(refExpr.ArgExpr.ErrCtx, System.Data.Entity.Resources.Strings.RefArgIsNotOfEntityType((object) dbExpression.ResultType.EdmType.FullName), (Exception) null);
    }

    private static ExpressionResolution ConvertDeRefExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      DerefExpr derefExpr = (DerefExpr) astExpr;
      DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(derefExpr.ArgExpr, sr);
      return TypeSemantics.IsReferenceType(dbExpression.ResultType) ? (ExpressionResolution) new ValueExpression((DbExpression) dbExpression.Deref()) : throw EntitySqlException.Create(derefExpr.ArgExpr.ErrCtx, System.Data.Entity.Resources.Strings.DeRefArgIsNotOfRefType((object) dbExpression.ResultType.EdmType.FullName), (Exception) null);
    }

    private static ExpressionResolution ConvertCreateRefExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      CreateRefExpr createRefExpr = (CreateRefExpr) astExpr;
      if (!(SemanticAnalyzer.ConvertValueExpression(createRefExpr.EntitySet, sr) is DbScanExpression dbScanExpression))
        throw EntitySqlException.Create(createRefExpr.EntitySet.ErrCtx, System.Data.Entity.Resources.Strings.ExprIsNotValidEntitySetForCreateRef, (Exception) null);
      if (!(dbScanExpression.Target is EntitySet target))
        throw EntitySqlException.Create(createRefExpr.EntitySet.ErrCtx, System.Data.Entity.Resources.Strings.ExprIsNotValidEntitySetForCreateRef, (Exception) null);
      DbExpression keyRow = SemanticAnalyzer.ConvertValueExpression(createRefExpr.Keys, sr);
      if (!(keyRow.ResultType.EdmType is RowType edmType))
        throw EntitySqlException.Create(createRefExpr.Keys.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCreateRefKeyType, (Exception) null);
      RowType keyRowType = TypeHelpers.CreateKeyRowType((EntityTypeBase) target.ElementType);
      if (keyRowType.Members.Count != edmType.Members.Count)
        throw EntitySqlException.Create(createRefExpr.Keys.ErrCtx, System.Data.Entity.Resources.Strings.ImcompatibleCreateRefKeyType, (Exception) null);
      if (!TypeSemantics.IsStructurallyEqualOrPromotableTo(keyRow.ResultType, TypeUsage.Create((EdmType) keyRowType)))
        throw EntitySqlException.Create(createRefExpr.Keys.ErrCtx, System.Data.Entity.Resources.Strings.ImcompatibleCreateRefKeyElementType, (Exception) null);
      DbExpression dbExpression;
      if (createRefExpr.TypeIdentifier != null)
      {
        TypeUsage type = SemanticAnalyzer.ConvertTypeName(createRefExpr.TypeIdentifier, sr);
        if (!TypeSemantics.IsEntityType(type))
          throw EntitySqlException.Create(createRefExpr.TypeIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.CreateRefTypeIdentifierMustSpecifyAnEntityType((object) type.EdmType.FullName, (object) type.EdmType.BuiltInTypeKind.ToString()), (Exception) null);
        if (!TypeSemantics.IsValidPolymorphicCast((EdmType) target.ElementType, type.EdmType))
          throw EntitySqlException.Create(createRefExpr.TypeIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.CreateRefTypeIdentifierMustBeASubOrSuperType((object) target.ElementType.FullName, (object) type.EdmType.FullName), (Exception) null);
        dbExpression = (DbExpression) target.RefFromKey(keyRow, (EntityType) type.EdmType);
      }
      else
        dbExpression = (DbExpression) target.RefFromKey(keyRow);
      return (ExpressionResolution) new ValueExpression(dbExpression);
    }

    private static ExpressionResolution ConvertKeyExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      KeyExpr keyExpr = (KeyExpr) astExpr;
      DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(keyExpr.ArgExpr, sr);
      if (TypeSemantics.IsEntityType(dbExpression.ResultType))
        dbExpression = (DbExpression) dbExpression.GetEntityRef();
      else if (!TypeSemantics.IsReferenceType(dbExpression.ResultType))
        throw EntitySqlException.Create(keyExpr.ArgExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidKeyArgument((object) dbExpression.ResultType.EdmType.FullName), (Exception) null);
      return (ExpressionResolution) new ValueExpression((DbExpression) dbExpression.GetRefKey());
    }

    private static ExpressionResolution ConvertBuiltIn(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr)
    {
      BuiltInExpr astBltInExpr = (BuiltInExpr) astExpr;
      return (ExpressionResolution) new ValueExpression((SemanticAnalyzer._builtInExprConverter[astBltInExpr.Kind] ?? throw new EntitySqlException(System.Data.Entity.Resources.Strings.UnknownBuiltInAstExpressionType))(astBltInExpr, sr));
    }

    private static Pair<DbExpression, DbExpression> ConvertArithmeticArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertValueExpressionsWithUntypedNulls(astBuiltInExpr.Arg1, astBuiltInExpr.Arg2, astBuiltInExpr.ErrCtx, (Func<string>) (() => System.Data.Entity.Resources.Strings.InvalidNullArithmetic), sr);
      if (!TypeSemantics.IsNumericType(pair.Left.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionMustBeNumericType, (Exception) null);
      if (pair.Right != null)
      {
        if (!TypeSemantics.IsNumericType(pair.Right.ResultType))
          throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionMustBeNumericType, (Exception) null);
        if (TypeHelpers.GetCommonTypeUsage(pair.Left.ResultType, pair.Right.ResultType) == null)
          throw EntitySqlException.Create(astBuiltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ArgumentTypesAreIncompatible((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      }
      return pair;
    }

    private static Pair<DbExpression, DbExpression> ConvertPlusOperands(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertValueExpressionsWithUntypedNulls(astBuiltInExpr.Arg1, astBuiltInExpr.Arg2, astBuiltInExpr.ErrCtx, (Func<string>) (() => System.Data.Entity.Resources.Strings.InvalidNullArithmetic), sr);
      if (!TypeSemantics.IsNumericType(pair.Left.ResultType) && !TypeSemantics.IsPrimitiveType(pair.Left.ResultType, PrimitiveTypeKind.String))
        throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.PlusLeftExpressionInvalidType, (Exception) null);
      if (!TypeSemantics.IsNumericType(pair.Right.ResultType) && !TypeSemantics.IsPrimitiveType(pair.Right.ResultType, PrimitiveTypeKind.String))
        throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.PlusRightExpressionInvalidType, (Exception) null);
      if (TypeHelpers.GetCommonTypeUsage(pair.Left.ResultType, pair.Right.ResultType) == null)
        throw EntitySqlException.Create(astBuiltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ArgumentTypesAreIncompatible((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      return pair;
    }

    private static Pair<DbExpression, DbExpression> ConvertLogicalArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      DbExpression left = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(astBuiltInExpr.Arg1, sr) ?? (DbExpression) TypeResolver.BooleanType.Null();
      DbExpression right = (DbExpression) null;
      if (astBuiltInExpr.Arg2 != null)
        right = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(astBuiltInExpr.Arg2, sr) ?? (DbExpression) TypeResolver.BooleanType.Null();
      if (!SemanticAnalyzer.IsBooleanType(left.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeBoolean, (Exception) null);
      return right == null || SemanticAnalyzer.IsBooleanType(right.ResultType) ? new Pair<DbExpression, DbExpression>(left, right) : throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeBoolean, (Exception) null);
    }

    private static Pair<DbExpression, DbExpression> ConvertEqualCompArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertValueExpressionsWithUntypedNulls(astBuiltInExpr.Arg1, astBuiltInExpr.Arg2, astBuiltInExpr.ErrCtx, (Func<string>) (() => System.Data.Entity.Resources.Strings.InvalidNullComparison), sr);
      if (!TypeSemantics.IsEqualComparableTo(pair.Left.ResultType, pair.Right.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ArgumentTypesAreIncompatible((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      return pair;
    }

    private static Pair<DbExpression, DbExpression> ConvertOrderCompArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertValueExpressionsWithUntypedNulls(astBuiltInExpr.Arg1, astBuiltInExpr.Arg2, astBuiltInExpr.ErrCtx, (Func<string>) (() => System.Data.Entity.Resources.Strings.InvalidNullComparison), sr);
      if (!TypeSemantics.IsOrderComparableTo(pair.Left.ResultType, pair.Right.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ArgumentTypesAreIncompatible((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      return pair;
    }

    private static Pair<DbExpression, DbExpression> ConvertSetArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      DbExpression left = SemanticAnalyzer.ConvertValueExpression(astBuiltInExpr.Arg1, sr);
      DbExpression right = (DbExpression) null;
      if (astBuiltInExpr.Arg2 != null)
      {
        if (!TypeSemantics.IsCollectionType(left.ResultType))
          throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.LeftSetExpressionArgsMustBeCollection, (Exception) null);
        right = SemanticAnalyzer.ConvertValueExpression(astBuiltInExpr.Arg2, sr);
        TypeUsage typeUsage = TypeSemantics.IsCollectionType(right.ResultType) ? TypeHelpers.GetElementTypeUsage(left.ResultType) : throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.RightSetExpressionArgsMustBeCollection, (Exception) null);
        TypeUsage elementTypeUsage = TypeHelpers.GetElementTypeUsage(right.ResultType);
        if (!TypeSemantics.TryGetCommonType(typeUsage, elementTypeUsage, out TypeUsage _))
          CqlErrorHelper.ReportIncompatibleCommonType(astBuiltInExpr.ErrCtx, typeUsage, elementTypeUsage);
        if (astBuiltInExpr.Kind != BuiltInKind.UnionAll)
        {
          if (!TypeHelpers.IsSetComparableOpType(TypeHelpers.GetElementTypeUsage(left.ResultType)))
            throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.PlaceholderSetArgTypeIsNotEqualComparable((object) System.Data.Entity.Resources.Strings.LocalizedLeft, (object) astBuiltInExpr.Kind.ToString().ToUpperInvariant(), (object) TypeHelpers.GetElementTypeUsage(left.ResultType).EdmType.FullName), (Exception) null);
          if (!TypeHelpers.IsSetComparableOpType(TypeHelpers.GetElementTypeUsage(right.ResultType)))
            throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.PlaceholderSetArgTypeIsNotEqualComparable((object) System.Data.Entity.Resources.Strings.LocalizedRight, (object) astBuiltInExpr.Kind.ToString().ToUpperInvariant(), (object) TypeHelpers.GetElementTypeUsage(right.ResultType).EdmType.FullName), (Exception) null);
        }
        else
        {
          if (Helper.IsAssociationType(typeUsage.EdmType))
            throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidAssociationTypeForUnion((object) typeUsage.EdmType.FullName), (Exception) null);
          if (Helper.IsAssociationType(elementTypeUsage.EdmType))
            throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.InvalidAssociationTypeForUnion((object) elementTypeUsage.EdmType.FullName), (Exception) null);
        }
      }
      else
      {
        if (!TypeSemantics.IsCollectionType(left.ResultType))
          throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidUnarySetOpArgument((object) astBuiltInExpr.Name), (Exception) null);
        if (astBuiltInExpr.Kind == BuiltInKind.Distinct && !TypeHelpers.IsValidDistinctOpType(TypeHelpers.GetElementTypeUsage(left.ResultType)))
          throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeEqualComparable, (Exception) null);
      }
      return new Pair<DbExpression, DbExpression>(left, right);
    }

    private static Pair<DbExpression, DbExpression> ConvertInExprArgs(
      BuiltInExpr astBuiltInExpr,
      SemanticResolver sr)
    {
      DbExpression right = SemanticAnalyzer.ConvertValueExpression(astBuiltInExpr.Arg2, sr);
      if (!TypeSemantics.IsCollectionType(right.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.RightSetExpressionArgsMustBeCollection, (Exception) null);
      DbExpression left = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(astBuiltInExpr.Arg1, sr);
      if (left == null)
      {
        TypeUsage elementTypeUsage = TypeHelpers.GetElementTypeUsage(right.ResultType);
        SemanticAnalyzer.ValidateTypeForNullExpression(elementTypeUsage, astBuiltInExpr.Arg1.ErrCtx);
        left = (DbExpression) elementTypeUsage.Null();
      }
      if (TypeSemantics.IsCollectionType(left.ResultType))
        throw EntitySqlException.Create(astBuiltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustNotBeCollection, (Exception) null);
      TypeUsage commonTypeUsage = TypeHelpers.GetCommonTypeUsage(left.ResultType, TypeHelpers.GetElementTypeUsage(right.ResultType));
      if (commonTypeUsage == null || !TypeHelpers.IsValidInOpType(commonTypeUsage))
        throw EntitySqlException.Create(astBuiltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidInExprArgs((object) left.ResultType.EdmType.FullName, (object) right.ResultType.EdmType.FullName), (Exception) null);
      return new Pair<DbExpression, DbExpression>(left, right);
    }

    private static void ValidateTypeForNullExpression(TypeUsage type, ErrorContext errCtx)
    {
      if (TypeSemantics.IsCollectionType(type))
      {
        string collectionOfNulls = System.Data.Entity.Resources.Strings.NullLiteralCannotBePromotedToCollectionOfNulls;
        throw EntitySqlException.Create(errCtx, collectionOfNulls, (Exception) null);
      }
    }

    private static TypeUsage ConvertTypeName(System.Data.Entity.Core.Common.EntitySql.AST.Node typeName, SemanticResolver sr)
    {
      string[] names = (string[]) null;
      NodeList<System.Data.Entity.Core.Common.EntitySql.AST.Node> typeSpecArgs = (NodeList<System.Data.Entity.Core.Common.EntitySql.AST.Node>) null;
      if (typeName is MethodExpr methodExpr)
      {
        typeName = methodExpr.Expr;
        typeName.ErrCtx.ErrorContextInfo = methodExpr.ErrCtx.ErrorContextInfo;
        typeName.ErrCtx.UseContextInfoAsResourceIdentifier = methodExpr.ErrCtx.UseContextInfoAsResourceIdentifier;
        typeSpecArgs = methodExpr.Args;
      }
      if (typeName is Identifier identifier)
        names = new string[1]{ identifier.Name };
      if (typeName is DotExpr dotExpr)
        dotExpr.IsMultipartIdentifier(out names);
      if (names == null)
        throw EntitySqlException.Create(typeName.ErrCtx, System.Data.Entity.Resources.Strings.InvalidMetadataMemberName, (Exception) null);
      MetadataMember metadataMember = sr.ResolveMetadataMemberName(names, typeName.ErrCtx);
      switch (metadataMember.MetadataMemberClass)
      {
        case MetadataMemberClass.Type:
          TypeUsage parameterizedType = ((MetadataType) metadataMember).TypeUsage;
          if (typeSpecArgs != null)
            parameterizedType = SemanticAnalyzer.ConvertTypeSpecArgs(parameterizedType, typeSpecArgs, typeName.ErrCtx);
          return parameterizedType;
        case MetadataMemberClass.Namespace:
          throw EntitySqlException.Create(typeName.ErrCtx, System.Data.Entity.Resources.Strings.TypeNameNotFound((object) metadataMember.Name), (Exception) null);
        default:
          throw EntitySqlException.Create(typeName.ErrCtx, System.Data.Entity.Resources.Strings.InvalidMetadataMemberClassResolution((object) metadataMember.Name, (object) metadataMember.MetadataMemberClassName, (object) MetadataType.TypeClassName), (Exception) null);
      }
    }

    private static TypeUsage ConvertTypeSpecArgs(
      TypeUsage parameterizedType,
      NodeList<System.Data.Entity.Core.Common.EntitySql.AST.Node> typeSpecArgs,
      ErrorContext errCtx)
    {
      foreach (System.Data.Entity.Core.Common.EntitySql.AST.Node typeSpecArg in (IEnumerable<System.Data.Entity.Core.Common.EntitySql.AST.Node>) typeSpecArgs)
      {
        if (!(typeSpecArg is Literal))
          throw EntitySqlException.Create(typeSpecArg.ErrCtx, System.Data.Entity.Resources.Strings.TypeArgumentMustBeLiteral, (Exception) null);
      }
      if (!(parameterizedType.EdmType is PrimitiveType edmType) || edmType.PrimitiveTypeKind != PrimitiveTypeKind.Decimal)
      {
        string errorMessage = System.Data.Entity.Resources.Strings.TypeDoesNotSupportSpec((object) edmType.FullName);
        throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
      }
      if (typeSpecArgs.Count > 2)
      {
        string errorMessage = System.Data.Entity.Resources.Strings.TypeArgumentCountMismatch((object) edmType.FullName, (object) 2);
        throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
      }
      byte byteValue1;
      SemanticAnalyzer.ConvertTypeFacetValue(edmType, (Literal) typeSpecArgs[0], "Precision", out byteValue1);
      byte byteValue2 = 0;
      if (typeSpecArgs.Count == 2)
        SemanticAnalyzer.ConvertTypeFacetValue(edmType, (Literal) typeSpecArgs[1], "Scale", out byteValue2);
      if ((int) byteValue1 < (int) byteValue2)
        throw EntitySqlException.Create(typeSpecArgs[0].ErrCtx, System.Data.Entity.Resources.Strings.PrecisionMustBeGreaterThanScale((object) byteValue1, (object) byteValue2), (Exception) null);
      return TypeUsage.CreateDecimalTypeUsage(edmType, byteValue1, byteValue2);
    }

    private static void ConvertTypeFacetValue(
      PrimitiveType type,
      Literal value,
      string facetName,
      out byte byteValue)
    {
      FacetDescription facet = Helper.GetFacet((IEnumerable<FacetDescription>) type.ProviderManifest.GetFacetDescriptions((EdmType) type), facetName);
      if (facet == null)
        throw EntitySqlException.Create(value.ErrCtx, System.Data.Entity.Resources.Strings.TypeDoesNotSupportFacet((object) type.FullName, (object) facetName), (Exception) null);
      if (!value.IsNumber || !byte.TryParse(value.OriginalValue, out byteValue))
        throw EntitySqlException.Create(value.ErrCtx, System.Data.Entity.Resources.Strings.TypeArgumentIsNotValid, (Exception) null);
      int? nullable = facet.MaxValue;
      if (nullable.HasValue)
      {
        int num1 = (int) byteValue;
        nullable = facet.MaxValue;
        int num2 = nullable.Value;
        if (num1 > num2)
          throw EntitySqlException.Create(value.ErrCtx, System.Data.Entity.Resources.Strings.TypeArgumentExceedsMax((object) facetName), (Exception) null);
      }
      nullable = facet.MinValue;
      if (!nullable.HasValue)
        return;
      int num3 = (int) byteValue;
      nullable = facet.MinValue;
      int num4 = nullable.Value;
      if (num3 < num4)
        throw EntitySqlException.Create(value.ErrCtx, System.Data.Entity.Resources.Strings.TypeArgumentBelowMin((object) facetName), (Exception) null);
    }

    private static TypeUsage ConvertTypeDefinition(
      System.Data.Entity.Core.Common.EntitySql.AST.Node typeDefinitionExpr,
      SemanticResolver sr)
    {
      CollectionTypeDefinition collectionTypeDefinition = typeDefinitionExpr as CollectionTypeDefinition;
      RefTypeDefinition refTypeDefinition = typeDefinitionExpr as RefTypeDefinition;
      RowTypeDefinition rowTypeDefinition = typeDefinitionExpr as RowTypeDefinition;
      TypeUsage typeUsage;
      if (collectionTypeDefinition != null)
        typeUsage = TypeHelpers.CreateCollectionTypeUsage(SemanticAnalyzer.ConvertTypeDefinition(collectionTypeDefinition.ElementTypeDef, sr));
      else if (refTypeDefinition != null)
      {
        TypeUsage type = SemanticAnalyzer.ConvertTypeName(refTypeDefinition.RefTypeIdentifier, sr);
        typeUsage = TypeSemantics.IsEntityType(type) ? TypeHelpers.CreateReferenceTypeUsage((EntityType) type.EdmType) : throw EntitySqlException.Create(refTypeDefinition.RefTypeIdentifier.ErrCtx, System.Data.Entity.Resources.Strings.RefTypeIdentifierMustSpecifyAnEntityType((object) type.EdmType.FullName, (object) type.EdmType.BuiltInTypeKind.ToString()), (Exception) null);
      }
      else
        typeUsage = rowTypeDefinition == null ? SemanticAnalyzer.ConvertTypeName(typeDefinitionExpr, sr) : TypeHelpers.CreateRowTypeUsage(rowTypeDefinition.Properties.Select<PropDefinition, KeyValuePair<string, TypeUsage>>((Func<PropDefinition, KeyValuePair<string, TypeUsage>>) (p => new KeyValuePair<string, TypeUsage>(p.Name.Name, SemanticAnalyzer.ConvertTypeDefinition(p.Type, sr)))));
      return typeUsage;
    }

    private static ExpressionResolution ConvertRowConstructor(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      RowConstructorExpr rowConstructorExpr = (RowConstructorExpr) expr;
      Dictionary<string, TypeUsage> dictionary = new Dictionary<string, TypeUsage>((IEqualityComparer<string>) sr.NameComparer);
      List<DbExpression> dbExpressionList = new List<DbExpression>(rowConstructorExpr.AliasedExprList.Count);
      for (int index = 0; index < rowConstructorExpr.AliasedExprList.Count; ++index)
      {
        AliasedExpr aliasedExpr = rowConstructorExpr.AliasedExprList[index];
        DbExpression convertedExpression = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(aliasedExpr.Expr, sr);
        if (convertedExpression == null)
          throw EntitySqlException.Create(aliasedExpr.Expr.ErrCtx, System.Data.Entity.Resources.Strings.RowCtorElementCannotBeNull, (Exception) null);
        string str = sr.InferAliasName(aliasedExpr, convertedExpression);
        if (dictionary.ContainsKey(str))
        {
          if (aliasedExpr.Alias != null)
            CqlErrorHelper.ReportAliasAlreadyUsedError(str, aliasedExpr.Alias.ErrCtx, System.Data.Entity.Resources.Strings.InRowCtor);
          else
            str = sr.GenerateInternalName("autoRowCol");
        }
        dictionary.Add(str, convertedExpression.ResultType);
        dbExpressionList.Add(convertedExpression);
      }
      return (ExpressionResolution) new ValueExpression((DbExpression) TypeHelpers.CreateRowTypeUsage((IEnumerable<KeyValuePair<string, TypeUsage>>) dictionary).New((IEnumerable<DbExpression>) dbExpressionList));
    }

    private static ExpressionResolution ConvertMultisetConstructor(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      MultisetConstructorExpr multisetConstructorExpr = (MultisetConstructorExpr) expr;
      if (multisetConstructorExpr.ExprList == null)
        throw EntitySqlException.Create(expr.ErrCtx, System.Data.Entity.Resources.Strings.CannotCreateEmptyMultiset, (Exception) null);
      DbExpression[] array1 = multisetConstructorExpr.ExprList.Select<System.Data.Entity.Core.Common.EntitySql.AST.Node, DbExpression>((Func<System.Data.Entity.Core.Common.EntitySql.AST.Node, DbExpression>) (e => SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(e, sr))).ToArray<DbExpression>();
      TypeUsage[] array2 = ((IEnumerable<DbExpression>) array1).Where<DbExpression>((Func<DbExpression, bool>) (e => e != null)).Select<DbExpression, TypeUsage>((Func<DbExpression, TypeUsage>) (e => e.ResultType)).ToArray<TypeUsage>();
      TypeUsage readOnlyType = TypeHelpers.GetReadOnlyType((array2.Length != 0 ? TypeHelpers.GetCommonTypeUsage((IEnumerable<TypeUsage>) array2) : throw EntitySqlException.Create(expr.ErrCtx, System.Data.Entity.Resources.Strings.CannotCreateMultisetofNulls, (Exception) null)) ?? throw EntitySqlException.Create(expr.ErrCtx, System.Data.Entity.Resources.Strings.MultisetElemsAreNotTypeCompatible, (Exception) null));
      for (int index = 0; index < array1.Length; ++index)
      {
        if (array1[index] == null)
        {
          SemanticAnalyzer.ValidateTypeForNullExpression(readOnlyType, multisetConstructorExpr.ExprList[index].ErrCtx);
          array1[index] = (DbExpression) readOnlyType.Null();
        }
      }
      return (ExpressionResolution) new ValueExpression((DbExpression) TypeHelpers.CreateCollectionTypeUsage(readOnlyType).New(array1));
    }

    private static ExpressionResolution ConvertCaseExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      CaseExpr caseExpr = (CaseExpr) expr;
      List<DbExpression> dbExpressionList = new List<DbExpression>(caseExpr.WhenThenExprList.Count);
      List<DbExpression> source = new List<DbExpression>(caseExpr.WhenThenExprList.Count);
      for (int index = 0; index < caseExpr.WhenThenExprList.Count; ++index)
      {
        WhenThenExpr whenThenExpr = caseExpr.WhenThenExprList[index];
        DbExpression dbExpression1 = SemanticAnalyzer.ConvertValueExpression(whenThenExpr.WhenExpr, sr);
        if (!SemanticAnalyzer.IsBooleanType(dbExpression1.ResultType))
          throw EntitySqlException.Create(whenThenExpr.WhenExpr.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeBoolean, (Exception) null);
        dbExpressionList.Add(dbExpression1);
        DbExpression dbExpression2 = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(whenThenExpr.ThenExpr, sr);
        source.Add(dbExpression2);
      }
      DbExpression elseExpression = caseExpr.ElseExpr != null ? SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(caseExpr.ElseExpr, sr) : (DbExpression) null;
      List<TypeUsage> list = source.Where<DbExpression>((Func<DbExpression, bool>) (e => e != null)).Select<DbExpression, TypeUsage>((Func<DbExpression, TypeUsage>) (e => e.ResultType)).ToList<TypeUsage>();
      if (elseExpression != null)
        list.Add(elseExpression.ResultType);
      TypeUsage typeUsage = list.Count != 0 ? TypeHelpers.GetCommonTypeUsage((IEnumerable<TypeUsage>) list) : throw EntitySqlException.Create(caseExpr.ElseExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCaseWhenThenNullType, (Exception) null);
      if (typeUsage == null)
        throw EntitySqlException.Create(caseExpr.WhenThenExprList[0].ThenExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCaseResultTypes, (Exception) null);
      for (int index = 0; index < source.Count; ++index)
      {
        if (source[index] == null)
        {
          SemanticAnalyzer.ValidateTypeForNullExpression(typeUsage, caseExpr.WhenThenExprList[index].ThenExpr.ErrCtx);
          source[index] = (DbExpression) typeUsage.Null();
        }
      }
      if (elseExpression == null)
      {
        if (caseExpr.ElseExpr == null && TypeSemantics.IsCollectionType(typeUsage))
        {
          elseExpression = (DbExpression) typeUsage.NewEmptyCollection();
        }
        else
        {
          SemanticAnalyzer.ValidateTypeForNullExpression(typeUsage, (caseExpr.ElseExpr ?? (System.Data.Entity.Core.Common.EntitySql.AST.Node) caseExpr).ErrCtx);
          elseExpression = (DbExpression) typeUsage.Null();
        }
      }
      return (ExpressionResolution) new ValueExpression((DbExpression) DbExpressionBuilder.Case((IEnumerable<DbExpression>) dbExpressionList, (IEnumerable<DbExpression>) source, elseExpression));
    }

    private static ExpressionResolution ConvertQueryExpr(
      System.Data.Entity.Core.Common.EntitySql.AST.Node expr,
      SemanticResolver sr)
    {
      QueryExpr queryExpr = (QueryExpr) expr;
      DbExpression dbExpression = (DbExpression) null;
      bool flag = ParserOptions.CompilationMode.RestrictedViewGenerationMode == sr.ParserOptions.ParserCompilationMode;
      if (queryExpr.HavingClause != null && queryExpr.GroupByClause == null)
        throw EntitySqlException.Create(queryExpr.ErrCtx, System.Data.Entity.Resources.Strings.HavingRequiresGroupClause, (Exception) null);
      if (queryExpr.SelectClause.TopExpr != null)
      {
        if (queryExpr.OrderByClause != null && queryExpr.OrderByClause.LimitSubClause != null)
          throw EntitySqlException.Create(queryExpr.SelectClause.TopExpr.ErrCtx, System.Data.Entity.Resources.Strings.TopAndLimitCannotCoexist, (Exception) null);
        if (queryExpr.OrderByClause != null && queryExpr.OrderByClause.SkipSubClause != null)
          throw EntitySqlException.Create(queryExpr.SelectClause.TopExpr.ErrCtx, System.Data.Entity.Resources.Strings.TopAndSkipCannotCoexist, (Exception) null);
      }
      using (sr.EnterScopeRegion())
      {
        DbExpressionBinding source = SemanticAnalyzer.ProcessWhereClause(SemanticAnalyzer.ProcessFromClause(queryExpr.FromClause, sr), queryExpr.WhereClause, sr);
        bool queryProjectionProcessed = false;
        if (!flag)
          source = SemanticAnalyzer.ProcessOrderByClause(SemanticAnalyzer.ProcessHavingClause(SemanticAnalyzer.ProcessGroupByClause(source, queryExpr, sr), queryExpr.HavingClause, sr), queryExpr, out queryProjectionProcessed, sr);
        dbExpression = SemanticAnalyzer.ProcessSelectClause(source, queryExpr, queryProjectionProcessed, sr);
      }
      return (ExpressionResolution) new ValueExpression(dbExpression);
    }

    private static DbExpression ProcessSelectClause(
      DbExpressionBinding source,
      QueryExpr queryExpr,
      bool queryProjectionProcessed,
      SemanticResolver sr)
    {
      SelectClause selectClause = queryExpr.SelectClause;
      DbExpression dbExpression1;
      if (queryProjectionProcessed)
      {
        dbExpression1 = source.Expression;
      }
      else
      {
        List<KeyValuePair<string, DbExpression>> projectionItems = SemanticAnalyzer.ConvertSelectClauseItems(queryExpr, sr);
        dbExpression1 = SemanticAnalyzer.CreateProjectExpression(source, selectClause, projectionItems);
      }
      if (selectClause.TopExpr != null || queryExpr.OrderByClause != null && queryExpr.OrderByClause.LimitSubClause != null)
      {
        System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr;
        string exprName;
        if (selectClause.TopExpr != null)
        {
          astExpr = selectClause.TopExpr;
          exprName = "TOP";
        }
        else
        {
          astExpr = queryExpr.OrderByClause.LimitSubClause;
          exprName = "LIMIT";
        }
        DbExpression dbExpression2 = SemanticAnalyzer.ConvertValueExpression(astExpr, sr);
        SemanticAnalyzer.ValidateExpressionIsCommandParamOrNonNegativeIntegerConstant(dbExpression2, astExpr.ErrCtx, exprName);
        dbExpression1 = (DbExpression) dbExpression1.Limit(dbExpression2);
      }
      return dbExpression1;
    }

    private static List<KeyValuePair<string, DbExpression>> ConvertSelectClauseItems(
      QueryExpr queryExpr,
      SemanticResolver sr)
    {
      SelectClause selectClause = queryExpr.SelectClause;
      if (selectClause.SelectKind == SelectKind.Value)
      {
        if (selectClause.Items.Count != 1)
          throw EntitySqlException.Create(selectClause.ErrCtx, System.Data.Entity.Resources.Strings.InvalidSelectValueList, (Exception) null);
        if (selectClause.Items[0].Alias != null && queryExpr.OrderByClause == null)
          throw EntitySqlException.Create(selectClause.Items[0].ErrCtx, System.Data.Entity.Resources.Strings.InvalidSelectValueAliasedExpression, (Exception) null);
      }
      HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) sr.NameComparer);
      List<KeyValuePair<string, DbExpression>> keyValuePairList = new List<KeyValuePair<string, DbExpression>>(selectClause.Items.Count);
      for (int index = 0; index < selectClause.Items.Count; ++index)
      {
        AliasedExpr aliasedExpr = selectClause.Items[index];
        DbExpression convertedExpression = SemanticAnalyzer.ConvertValueExpression(aliasedExpr.Expr, sr);
        string str = sr.InferAliasName(aliasedExpr, convertedExpression);
        if (stringSet.Contains(str))
        {
          if (aliasedExpr.Alias != null)
            CqlErrorHelper.ReportAliasAlreadyUsedError(str, aliasedExpr.Alias.ErrCtx, System.Data.Entity.Resources.Strings.InSelectProjectionList);
          else
            str = sr.GenerateInternalName("autoProject");
        }
        stringSet.Add(str);
        keyValuePairList.Add(new KeyValuePair<string, DbExpression>(str, convertedExpression));
      }
      return keyValuePairList;
    }

    private static DbExpression CreateProjectExpression(
      DbExpressionBinding source,
      SelectClause selectClause,
      List<KeyValuePair<string, DbExpression>> projectionItems)
    {
      DbExpression dbExpression = selectClause.SelectKind != SelectKind.Value ? (DbExpression) source.Project((DbExpression) DbExpressionBuilder.NewRow((IEnumerable<KeyValuePair<string, DbExpression>>) projectionItems)) : (DbExpression) source.Project(projectionItems[0].Value);
      if (selectClause.DistinctKind == DistinctKind.Distinct)
      {
        SemanticAnalyzer.ValidateDistinctProjection(dbExpression.ResultType, selectClause);
        dbExpression = (DbExpression) dbExpression.Distinct();
      }
      return dbExpression;
    }

    private static void ValidateDistinctProjection(
      TypeUsage projectExpressionResultType,
      SelectClause selectClause)
    {
      SemanticAnalyzer.ValidateDistinctProjection(projectExpressionResultType, selectClause.Items[0].Expr.ErrCtx, selectClause.SelectKind == SelectKind.Row ? new List<ErrorContext>(selectClause.Items.Select<AliasedExpr, ErrorContext>((Func<AliasedExpr, ErrorContext>) (item => item.Expr.ErrCtx))) : (List<ErrorContext>) null);
    }

    private static void ValidateDistinctProjection(
      TypeUsage projectExpressionResultType,
      ErrorContext defaultErrCtx,
      List<ErrorContext> projectionItemErrCtxs)
    {
      TypeUsage elementTypeUsage = TypeHelpers.GetElementTypeUsage(projectExpressionResultType);
      if (!TypeHelpers.IsValidDistinctOpType(elementTypeUsage))
      {
        ErrorContext errCtx = defaultErrCtx;
        if (projectionItemErrCtxs != null && TypeSemantics.IsRowType(elementTypeUsage))
        {
          RowType edmType = elementTypeUsage.EdmType as RowType;
          for (int index = 0; index < edmType.Members.Count; ++index)
          {
            if (!TypeHelpers.IsValidDistinctOpType(edmType.Members[index].TypeUsage))
            {
              errCtx = projectionItemErrCtxs[index];
              break;
            }
          }
        }
        string beEqualComparable = System.Data.Entity.Resources.Strings.SelectDistinctMustBeEqualComparable;
        throw EntitySqlException.Create(errCtx, beEqualComparable, (Exception) null);
      }
    }

    private static void ValidateExpressionIsCommandParamOrNonNegativeIntegerConstant(
      DbExpression expr,
      ErrorContext errCtx,
      string exprName)
    {
      if (expr.ExpressionKind != DbExpressionKind.Constant && expr.ExpressionKind != DbExpressionKind.ParameterReference)
      {
        string errorMessage = System.Data.Entity.Resources.Strings.PlaceholderExpressionMustBeConstant((object) exprName);
        throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
      }
      if (!TypeSemantics.IsPromotableTo(expr.ResultType, TypeResolver.Int64Type))
      {
        string errorMessage = System.Data.Entity.Resources.Strings.PlaceholderExpressionMustBeCompatibleWithEdm64((object) exprName, (object) expr.ResultType.EdmType.FullName);
        throw EntitySqlException.Create(errCtx, errorMessage, (Exception) null);
      }
      if (expr is DbConstantExpression constantExpression && System.Convert.ToInt64(constantExpression.Value, (IFormatProvider) CultureInfo.InvariantCulture) < 0L)
      {
        string zero = System.Data.Entity.Resources.Strings.PlaceholderExpressionMustBeGreaterThanOrEqualToZero((object) exprName);
        throw EntitySqlException.Create(errCtx, zero, (Exception) null);
      }
    }

    private static DbExpressionBinding ProcessFromClause(
      FromClause fromClause,
      SemanticResolver sr)
    {
      DbExpressionBinding fromBinding = (DbExpressionBinding) null;
      List<SourceScopeEntry> ts = new List<SourceScopeEntry>();
      for (int index = 0; index < fromClause.FromClauseItems.Count; ++index)
      {
        List<SourceScopeEntry> scopeEntries;
        DbExpressionBinding apply = SemanticAnalyzer.ProcessFromClauseItem(fromClause.FromClauseItems[index], sr, out scopeEntries);
        ts.AddRange((IEnumerable<SourceScopeEntry>) scopeEntries);
        if (fromBinding == null)
        {
          fromBinding = apply;
        }
        else
        {
          fromBinding = fromBinding.CrossApply(apply).BindAs(sr.GenerateInternalName("lcapply"));
          ts.Each<SourceScopeEntry, SourceScopeEntry>((Func<SourceScopeEntry, SourceScopeEntry>) (scopeEntry => scopeEntry.AddParentVar(fromBinding.Variable)));
        }
      }
      return fromBinding;
    }

    private static DbExpressionBinding ProcessFromClauseItem(
      FromClauseItem fromClauseItem,
      SemanticResolver sr,
      out List<SourceScopeEntry> scopeEntries)
    {
      DbExpressionBinding expressionBinding;
      switch (fromClauseItem.FromClauseItemKind)
      {
        case FromClauseItemKind.AliasedFromClause:
          expressionBinding = SemanticAnalyzer.ProcessAliasedFromClauseItem((AliasedExpr) fromClauseItem.FromExpr, sr, out scopeEntries);
          break;
        case FromClauseItemKind.JoinFromClause:
          expressionBinding = SemanticAnalyzer.ProcessJoinClauseItem((JoinClauseItem) fromClauseItem.FromExpr, sr, out scopeEntries);
          break;
        default:
          expressionBinding = SemanticAnalyzer.ProcessApplyClauseItem((ApplyClauseItem) fromClauseItem.FromExpr, sr, out scopeEntries);
          break;
      }
      return expressionBinding;
    }

    private static DbExpressionBinding ProcessAliasedFromClauseItem(
      AliasedExpr aliasedExpr,
      SemanticResolver sr,
      out List<SourceScopeEntry> scopeEntries)
    {
      DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(aliasedExpr.Expr, sr);
      if (!TypeSemantics.IsCollectionType(dbExpression.ResultType))
        throw EntitySqlException.Create(aliasedExpr.Expr.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionMustBeCollection, (Exception) null);
      string str = sr.InferAliasName(aliasedExpr, dbExpression);
      if (sr.CurrentScope.Contains(str))
      {
        if (aliasedExpr.Alias != null)
          CqlErrorHelper.ReportAliasAlreadyUsedError(str, aliasedExpr.Alias.ErrCtx, System.Data.Entity.Resources.Strings.InFromClause);
        else
          str = sr.GenerateInternalName("autoFrom");
      }
      DbExpressionBinding expressionBinding = dbExpression.BindAs(str);
      SourceScopeEntry sourceScopeEntry = new SourceScopeEntry(expressionBinding.Variable);
      sr.CurrentScope.Add(expressionBinding.Variable.VariableName, (ScopeEntry) sourceScopeEntry);
      scopeEntries = new List<SourceScopeEntry>();
      scopeEntries.Add(sourceScopeEntry);
      return expressionBinding;
    }

    private static DbExpressionBinding ProcessJoinClauseItem(
      JoinClauseItem joinClause,
      SemanticResolver sr,
      out List<SourceScopeEntry> scopeEntries)
    {
      DbExpressionBinding joinBinding = (DbExpressionBinding) null;
      if (joinClause.OnExpr == null)
      {
        if (JoinKind.Inner == joinClause.JoinKind)
          throw EntitySqlException.Create(joinClause.ErrCtx, System.Data.Entity.Resources.Strings.InnerJoinMustHaveOnPredicate, (Exception) null);
      }
      else if (joinClause.JoinKind == JoinKind.Cross)
        throw EntitySqlException.Create(joinClause.OnExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidPredicateForCrossJoin, (Exception) null);
      List<SourceScopeEntry> scopeEntries1;
      DbExpressionBinding input1 = SemanticAnalyzer.ProcessFromClauseItem(joinClause.LeftExpr, sr, out scopeEntries1);
      scopeEntries1.Each<SourceScopeEntry, bool>((Func<SourceScopeEntry, bool>) (scopeEntry => scopeEntry.IsJoinClauseLeftExpr = true));
      List<SourceScopeEntry> scopeEntries2;
      DbExpressionBinding input2 = SemanticAnalyzer.ProcessFromClauseItem(joinClause.RightExpr, sr, out scopeEntries2);
      scopeEntries1.Each<SourceScopeEntry, bool>((Func<SourceScopeEntry, bool>) (scopeEntry => scopeEntry.IsJoinClauseLeftExpr = false));
      if (joinClause.JoinKind == JoinKind.RightOuter)
      {
        joinClause.JoinKind = JoinKind.LeftOuter;
        DbExpressionBinding expressionBinding = input1;
        input1 = input2;
        input2 = expressionBinding;
      }
      DbExpressionKind joinKind = SemanticAnalyzer.MapJoinKind(joinClause.JoinKind);
      DbExpression joinCondition = (DbExpression) null;
      if (joinClause.OnExpr == null)
      {
        if (DbExpressionKind.CrossJoin != joinKind)
          joinCondition = (DbExpression) DbExpressionBuilder.True;
      }
      else
        joinCondition = SemanticAnalyzer.ConvertValueExpression(joinClause.OnExpr, sr);
      joinBinding = DbExpressionBuilder.CreateJoinExpressionByKind(joinKind, joinCondition, input1, input2).BindAs(sr.GenerateInternalName("join"));
      scopeEntries = scopeEntries1;
      scopeEntries.AddRange((IEnumerable<SourceScopeEntry>) scopeEntries2);
      scopeEntries.Each<SourceScopeEntry, SourceScopeEntry>((Func<SourceScopeEntry, SourceScopeEntry>) (scopeEntry => scopeEntry.AddParentVar(joinBinding.Variable)));
      return joinBinding;
    }

    private static DbExpressionKind MapJoinKind(JoinKind joinKind) => SemanticAnalyzer._joinMap[(int) joinKind];

    private static DbExpressionBinding ProcessApplyClauseItem(
      ApplyClauseItem applyClause,
      SemanticResolver sr,
      out List<SourceScopeEntry> scopeEntries)
    {
      DbExpressionBinding applyBinding = (DbExpressionBinding) null;
      List<SourceScopeEntry> scopeEntries1;
      DbExpressionBinding input = SemanticAnalyzer.ProcessFromClauseItem(applyClause.LeftExpr, sr, out scopeEntries1);
      List<SourceScopeEntry> scopeEntries2;
      DbExpressionBinding apply = SemanticAnalyzer.ProcessFromClauseItem(applyClause.RightExpr, sr, out scopeEntries2);
      applyBinding = DbExpressionBuilder.CreateApplyExpressionByKind(SemanticAnalyzer.MapApplyKind(applyClause.ApplyKind), input, apply).BindAs(sr.GenerateInternalName("apply"));
      scopeEntries = scopeEntries1;
      scopeEntries.AddRange((IEnumerable<SourceScopeEntry>) scopeEntries2);
      scopeEntries.Each<SourceScopeEntry, SourceScopeEntry>((Func<SourceScopeEntry, SourceScopeEntry>) (scopeEntry => scopeEntry.AddParentVar(applyBinding.Variable)));
      return applyBinding;
    }

    private static DbExpressionKind MapApplyKind(ApplyKind applyKind) => SemanticAnalyzer._applyMap[(int) applyKind];

    private static DbExpressionBinding ProcessWhereClause(
      DbExpressionBinding source,
      System.Data.Entity.Core.Common.EntitySql.AST.Node whereClause,
      SemanticResolver sr)
    {
      return whereClause == null ? source : SemanticAnalyzer.ProcessWhereHavingClausePredicate(source, whereClause, whereClause.ErrCtx, "where", sr);
    }

    private static DbExpressionBinding ProcessHavingClause(
      DbExpressionBinding source,
      HavingClause havingClause,
      SemanticResolver sr)
    {
      return havingClause == null ? source : SemanticAnalyzer.ProcessWhereHavingClausePredicate(source, havingClause.HavingPredicate, havingClause.ErrCtx, "having", sr);
    }

    private static DbExpressionBinding ProcessWhereHavingClausePredicate(
      DbExpressionBinding source,
      System.Data.Entity.Core.Common.EntitySql.AST.Node predicate,
      ErrorContext errCtx,
      string bindingNameTemplate,
      SemanticResolver sr)
    {
      DbExpressionBinding whereBinding = (DbExpressionBinding) null;
      DbExpression predicate1 = SemanticAnalyzer.ConvertValueExpression(predicate, sr);
      if (!SemanticAnalyzer.IsBooleanType(predicate1.ResultType))
      {
        string typeMustBeBoolean = System.Data.Entity.Resources.Strings.ExpressionTypeMustBeBoolean;
        throw EntitySqlException.Create(errCtx, typeMustBeBoolean, (Exception) null);
      }
      whereBinding = source.Filter(predicate1).BindAs(sr.GenerateInternalName(bindingNameTemplate));
      sr.CurrentScopeRegion.ApplyToScopeEntries((Action<ScopeEntry>) (scopeEntry =>
      {
        if (scopeEntry.EntryKind != ScopeEntryKind.SourceVar)
          return;
        ((SourceScopeEntry) scopeEntry).ReplaceParentVar(whereBinding.Variable);
      }));
      return whereBinding;
    }

    private static DbExpressionBinding ProcessGroupByClause(
      DbExpressionBinding source,
      QueryExpr queryExpr,
      SemanticResolver sr)
    {
      GroupByClause groupByClause = queryExpr.GroupByClause;
      int capacity = groupByClause != null ? groupByClause.GroupItems.Count : 0;
      bool flag1 = capacity == 0;
      if (flag1 && !queryExpr.HasMethodCall)
        return source;
      DbGroupExpressionBinding groupInputBinding = source.Expression.GroupBindAs(sr.GenerateInternalName("geb"), sr.GenerateInternalName("group"));
      DbGroupAggregate groupAggregate = groupInputBinding.GroupAggregate;
      DbVariableReferenceExpression referenceExpression = groupAggregate.ResultType.Variable(sr.GenerateInternalName("groupAggregate"));
      DbExpressionBinding groupAggregateBinding = referenceExpression.BindAs(sr.GenerateInternalName("groupPartitionItem"));
      sr.CurrentScopeRegion.EnterGroupOperation(groupAggregateBinding);
      sr.CurrentScopeRegion.ApplyToScopeEntries((Action<ScopeEntry>) (scopeEntry => ((SourceScopeEntry) scopeEntry).AdjustToGroupVar(groupInputBinding.Variable, groupInputBinding.GroupVariable, groupAggregateBinding.Variable)));
      HashSet<string> stringSet = new HashSet<string>((IEqualityComparer<string>) sr.NameComparer);
      List<SemanticAnalyzer.GroupKeyInfo> source1 = new List<SemanticAnalyzer.GroupKeyInfo>(capacity);
      if (!flag1)
      {
        for (int index = 0; index < capacity; ++index)
        {
          AliasedExpr groupItem = groupByClause.GroupItems[index];
          sr.CurrentScopeRegion.WasResolutionCorrelated = false;
          DbExpression dbExpression;
          using (sr.EnterGroupKeyDefinition(GroupAggregateKind.GroupKey, groupItem.ErrCtx, out GroupKeyAggregateInfo _))
            dbExpression = SemanticAnalyzer.ConvertValueExpression(groupItem.Expr, sr);
          if (!sr.CurrentScopeRegion.WasResolutionCorrelated)
            throw EntitySqlException.Create(groupItem.Expr.ErrCtx, System.Data.Entity.Resources.Strings.KeyMustBeCorrelated((object) "GROUP BY"), (Exception) null);
          if (!TypeHelpers.IsValidGroupKeyType(dbExpression.ResultType))
            throw EntitySqlException.Create(groupItem.Expr.ErrCtx, System.Data.Entity.Resources.Strings.GroupingKeysMustBeEqualComparable, (Exception) null);
          DbExpression groupVarBasedKeyExpr;
          using (sr.EnterGroupKeyDefinition(GroupAggregateKind.Function, groupItem.ErrCtx, out GroupKeyAggregateInfo _))
            groupVarBasedKeyExpr = SemanticAnalyzer.ConvertValueExpression(groupItem.Expr, sr);
          DbExpression groupAggBasedKeyExpr;
          using (sr.EnterGroupKeyDefinition(GroupAggregateKind.Partition, groupItem.ErrCtx, out GroupKeyAggregateInfo _))
            groupAggBasedKeyExpr = SemanticAnalyzer.ConvertValueExpression(groupItem.Expr, sr);
          string str = sr.InferAliasName(groupItem, dbExpression);
          if (stringSet.Contains(str))
          {
            if (groupItem.Alias != null)
              CqlErrorHelper.ReportAliasAlreadyUsedError(str, groupItem.Alias.ErrCtx, System.Data.Entity.Resources.Strings.InGroupClause);
            else
              str = sr.GenerateInternalName("autoGroup");
          }
          stringSet.Add(str);
          SemanticAnalyzer.GroupKeyInfo groupKeyInfo = new SemanticAnalyzer.GroupKeyInfo(str, dbExpression, groupVarBasedKeyExpr, groupAggBasedKeyExpr);
          source1.Add(groupKeyInfo);
          string[] names;
          if (groupItem.Alias == null && groupItem.Expr is DotExpr expr3 && expr3.IsMultipartIdentifier(out names))
          {
            groupKeyInfo.AlternativeName = names;
            string fullName = TypeResolver.GetFullName(names);
            if (stringSet.Contains(fullName))
              CqlErrorHelper.ReportAliasAlreadyUsedError(fullName, expr3.ErrCtx, System.Data.Entity.Resources.Strings.InGroupClause);
            stringSet.Add(fullName);
          }
        }
      }
      int currentScopeIndex = sr.CurrentScopeIndex;
      sr.EnterScope();
      foreach (SemanticAnalyzer.GroupKeyInfo groupKeyInfo in source1)
      {
        sr.CurrentScope.Add(groupKeyInfo.Name, (ScopeEntry) new GroupKeyDefinitionScopeEntry(groupKeyInfo.VarBasedKeyExpr, groupKeyInfo.GroupVarBasedKeyExpr, groupKeyInfo.GroupAggBasedKeyExpr, (string[]) null));
        if (groupKeyInfo.AlternativeName != null)
        {
          string fullName = TypeResolver.GetFullName(groupKeyInfo.AlternativeName);
          sr.CurrentScope.Add(fullName, (ScopeEntry) new GroupKeyDefinitionScopeEntry(groupKeyInfo.VarBasedKeyExpr, groupKeyInfo.GroupVarBasedKeyExpr, groupKeyInfo.GroupAggBasedKeyExpr, groupKeyInfo.AlternativeName));
        }
      }
      if (queryExpr.HavingClause != null && queryExpr.HavingClause.HasMethodCall)
        SemanticAnalyzer.ConvertValueExpression(queryExpr.HavingClause.HavingPredicate, sr);
      Dictionary<string, DbExpression> dictionary = (Dictionary<string, DbExpression>) null;
      if (queryExpr.OrderByClause != null || queryExpr.SelectClause.HasMethodCall)
      {
        dictionary = new Dictionary<string, DbExpression>(queryExpr.SelectClause.Items.Count, (IEqualityComparer<string>) sr.NameComparer);
        for (int index = 0; index < queryExpr.SelectClause.Items.Count; ++index)
        {
          AliasedExpr aliasedExpr = queryExpr.SelectClause.Items[index];
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(aliasedExpr.Expr, sr);
          DbExpression convertedExpression = dbExpression.ExpressionKind == DbExpressionKind.Null ? dbExpression : (DbExpression) dbExpression.ResultType.Null();
          string str = sr.InferAliasName(aliasedExpr, convertedExpression);
          if (dictionary.ContainsKey(str))
          {
            if (aliasedExpr.Alias != null)
              CqlErrorHelper.ReportAliasAlreadyUsedError(str, aliasedExpr.Alias.ErrCtx, System.Data.Entity.Resources.Strings.InSelectProjectionList);
            else
              str = sr.GenerateInternalName("autoProject");
          }
          dictionary.Add(str, convertedExpression);
        }
      }
      if (queryExpr.OrderByClause != null && queryExpr.OrderByClause.HasMethodCall)
      {
        sr.EnterScope();
        foreach (KeyValuePair<string, DbExpression> keyValuePair in dictionary)
          sr.CurrentScope.Add(keyValuePair.Key, (ScopeEntry) new ProjectionItemDefinitionScopeEntry(keyValuePair.Value));
        for (int index = 0; index < queryExpr.OrderByClause.OrderByClauseItem.Count; ++index)
        {
          OrderByClauseItem orderByClauseItem = queryExpr.OrderByClause.OrderByClauseItem[index];
          sr.CurrentScopeRegion.WasResolutionCorrelated = false;
          SemanticAnalyzer.ConvertValueExpression(orderByClauseItem.OrderExpr, sr);
          if (!sr.CurrentScopeRegion.WasResolutionCorrelated)
            throw EntitySqlException.Create(orderByClauseItem.ErrCtx, System.Data.Entity.Resources.Strings.KeyMustBeCorrelated((object) "ORDER BY"), (Exception) null);
        }
        sr.LeaveScope();
      }
      if (flag1 && sr.CurrentScopeRegion.GroupAggregateInfos.Count == 0)
      {
        sr.RollbackToScope(currentScopeIndex);
        sr.CurrentScopeRegion.ApplyToScopeEntries((Action<ScopeEntry>) (scopeEntry => ((SourceScopeEntry) scopeEntry).RollbackAdjustmentToGroupVar(source.Variable)));
        sr.CurrentScopeRegion.RollbackGroupOperation();
        return source;
      }
      List<KeyValuePair<string, DbAggregate>> keyValuePairList1 = new List<KeyValuePair<string, DbAggregate>>(sr.CurrentScopeRegion.GroupAggregateInfos.Count);
      bool flag2 = false;
      foreach (GroupAggregateInfo groupAggregateInfo in sr.CurrentScopeRegion.GroupAggregateInfos)
      {
        switch (groupAggregateInfo.AggregateKind)
        {
          case GroupAggregateKind.Function:
            keyValuePairList1.Add(new KeyValuePair<string, DbAggregate>(groupAggregateInfo.AggregateName, ((FunctionAggregateInfo) groupAggregateInfo).AggregateDefinition));
            continue;
          case GroupAggregateKind.Partition:
            flag2 = true;
            continue;
          default:
            continue;
        }
      }
      if (flag2)
        keyValuePairList1.Add(new KeyValuePair<string, DbAggregate>(referenceExpression.VariableName, (DbAggregate) groupAggregate));
      DbExpressionBinding groupBinding = groupInputBinding.GroupBy(source1.Select<SemanticAnalyzer.GroupKeyInfo, KeyValuePair<string, DbExpression>>((Func<SemanticAnalyzer.GroupKeyInfo, KeyValuePair<string, DbExpression>>) (keyInfo => new KeyValuePair<string, DbExpression>(keyInfo.Name, keyInfo.VarBasedKeyExpr))), (IEnumerable<KeyValuePair<string, DbAggregate>>) keyValuePairList1).BindAs(sr.GenerateInternalName("group"));
      if (flag2)
      {
        List<KeyValuePair<string, DbExpression>> keyValuePairList2 = SemanticAnalyzer.ProcessGroupPartitionDefinitions(sr.CurrentScopeRegion.GroupAggregateInfos, referenceExpression, groupBinding);
        if (keyValuePairList2 != null)
        {
          keyValuePairList2.AddRange(source1.Select<SemanticAnalyzer.GroupKeyInfo, KeyValuePair<string, DbExpression>>((Func<SemanticAnalyzer.GroupKeyInfo, KeyValuePair<string, DbExpression>>) (keyInfo => new KeyValuePair<string, DbExpression>(keyInfo.Name, (DbExpression) groupBinding.Variable.Property(keyInfo.Name)))));
          keyValuePairList2.AddRange(sr.CurrentScopeRegion.GroupAggregateInfos.Where<GroupAggregateInfo>((Func<GroupAggregateInfo, bool>) (groupAggregateInfo => groupAggregateInfo.AggregateKind == GroupAggregateKind.Function)).Select<GroupAggregateInfo, KeyValuePair<string, DbExpression>>((Func<GroupAggregateInfo, KeyValuePair<string, DbExpression>>) (groupAggregateInfo => new KeyValuePair<string, DbExpression>(groupAggregateInfo.AggregateName, (DbExpression) groupBinding.Variable.Property(groupAggregateInfo.AggregateName)))));
          DbExpression projection = (DbExpression) DbExpressionBuilder.NewRow((IEnumerable<KeyValuePair<string, DbExpression>>) keyValuePairList2);
          groupBinding = groupBinding.Project(projection).BindAs(sr.GenerateInternalName("groupPartitionDefs"));
        }
      }
      sr.RollbackToScope(currentScopeIndex);
      sr.CurrentScopeRegion.ApplyToScopeEntries((Func<ScopeEntry, ScopeEntry>) (scopeEntry => (ScopeEntry) new InvalidGroupInputRefScopeEntry()));
      sr.EnterScope();
      foreach (SemanticAnalyzer.GroupKeyInfo groupKeyInfo in source1)
      {
        sr.CurrentScope.Add(groupKeyInfo.VarRef.VariableName, (ScopeEntry) new SourceScopeEntry(groupKeyInfo.VarRef).AddParentVar(groupBinding.Variable));
        if (groupKeyInfo.AlternativeName != null)
        {
          string fullName = TypeResolver.GetFullName(groupKeyInfo.AlternativeName);
          sr.CurrentScope.Add(fullName, (ScopeEntry) new SourceScopeEntry(groupKeyInfo.VarRef, groupKeyInfo.AlternativeName).AddParentVar(groupBinding.Variable));
        }
      }
      foreach (GroupAggregateInfo groupAggregateInfo in sr.CurrentScopeRegion.GroupAggregateInfos)
      {
        DbVariableReferenceExpression varRef = groupAggregateInfo.AggregateStubExpression.ResultType.Variable(groupAggregateInfo.AggregateName);
        if (!sr.CurrentScope.Contains(varRef.VariableName))
        {
          sr.CurrentScope.Add(varRef.VariableName, (ScopeEntry) new SourceScopeEntry(varRef).AddParentVar(groupBinding.Variable));
          sr.CurrentScopeRegion.RegisterGroupAggregateName(varRef.VariableName);
        }
        groupAggregateInfo.AggregateStubExpression = (DbNullExpression) null;
      }
      return groupBinding;
    }

    private static List<KeyValuePair<string, DbExpression>> ProcessGroupPartitionDefinitions(
      List<GroupAggregateInfo> groupAggregateInfos,
      DbVariableReferenceExpression groupAggregateVarRef,
      DbExpressionBinding groupBinding)
    {
      ReadOnlyCollection<DbVariableReferenceExpression> variables = new ReadOnlyCollection<DbVariableReferenceExpression>((IList<DbVariableReferenceExpression>) new DbVariableReferenceExpression[1]
      {
        groupAggregateVarRef
      });
      List<KeyValuePair<string, DbExpression>> keyValuePairList = new List<KeyValuePair<string, DbExpression>>();
      bool flag = false;
      foreach (GroupAggregateInfo groupAggregateInfo in groupAggregateInfos)
      {
        if (groupAggregateInfo.AggregateKind == GroupAggregateKind.Partition)
        {
          GroupPartitionInfo groupPartitionInfo = (GroupPartitionInfo) groupAggregateInfo;
          DbExpression aggregateDefinition = groupPartitionInfo.AggregateDefinition;
          if (SemanticAnalyzer.IsTrivialInputProjection(groupAggregateVarRef, aggregateDefinition))
          {
            groupAggregateInfo.AggregateName = groupAggregateVarRef.VariableName;
            flag = true;
          }
          else
          {
            DbLambda lambda = new DbLambda(variables, groupPartitionInfo.AggregateDefinition);
            keyValuePairList.Add(new KeyValuePair<string, DbExpression>(groupAggregateInfo.AggregateName, (DbExpression) lambda.Invoke((DbExpression) groupBinding.Variable.Property(groupAggregateVarRef.VariableName))));
          }
        }
      }
      if (flag)
      {
        if (keyValuePairList.Count > 0)
          keyValuePairList.Add(new KeyValuePair<string, DbExpression>(groupAggregateVarRef.VariableName, (DbExpression) groupBinding.Variable.Property(groupAggregateVarRef.VariableName)));
        else
          keyValuePairList = (List<KeyValuePair<string, DbExpression>>) null;
      }
      return keyValuePairList;
    }

    private static bool IsTrivialInputProjection(
      DbVariableReferenceExpression lambdaVariable,
      DbExpression lambdaBody)
    {
      if (lambdaBody.ExpressionKind != DbExpressionKind.Project)
        return false;
      DbProjectExpression projectExpression = (DbProjectExpression) lambdaBody;
      if (projectExpression.Input.Expression != lambdaVariable)
        return false;
      if (projectExpression.Projection.ExpressionKind == DbExpressionKind.VariableReference)
        return (DbVariableReferenceExpression) projectExpression.Projection == projectExpression.Input.Variable;
      if (projectExpression.Projection.ExpressionKind != DbExpressionKind.NewInstance || !TypeSemantics.IsRowType(projectExpression.Projection.ResultType) || !TypeSemantics.IsEqual(projectExpression.Projection.ResultType, projectExpression.Input.Variable.ResultType))
        return false;
      IBaseList<EdmMember> structuralMembers = TypeHelpers.GetAllStructuralMembers(projectExpression.Input.Variable.ResultType);
      DbNewInstanceExpression projection = (DbNewInstanceExpression) projectExpression.Projection;
      for (int index = 0; index < projection.Arguments.Count; ++index)
      {
        if (projection.Arguments[index].ExpressionKind != DbExpressionKind.Property)
          return false;
        DbPropertyExpression propertyExpression = (DbPropertyExpression) projection.Arguments[index];
        if (propertyExpression.Instance != projectExpression.Input.Variable || propertyExpression.Property != structuralMembers[index])
          return false;
      }
      return true;
    }

    private static DbExpressionBinding ProcessOrderByClause(
      DbExpressionBinding source,
      QueryExpr queryExpr,
      out bool queryProjectionProcessed,
      SemanticResolver sr)
    {
      queryProjectionProcessed = false;
      if (queryExpr.OrderByClause == null)
        return source;
      DbExpressionBinding sortBinding = (DbExpressionBinding) null;
      OrderByClause orderByClause = queryExpr.OrderByClause;
      SelectClause selectClause = queryExpr.SelectClause;
      DbExpression dbExpression = (DbExpression) null;
      if (orderByClause.SkipSubClause != null)
      {
        dbExpression = SemanticAnalyzer.ConvertValueExpression(orderByClause.SkipSubClause, sr);
        SemanticAnalyzer.ValidateExpressionIsCommandParamOrNonNegativeIntegerConstant(dbExpression, orderByClause.SkipSubClause.ErrCtx, "SKIP");
      }
      List<KeyValuePair<string, DbExpression>> keyValuePairList = SemanticAnalyzer.ConvertSelectClauseItems(queryExpr, sr);
      if (selectClause.DistinctKind == DistinctKind.Distinct)
        sr.CurrentScopeRegion.RollbackAllScopes();
      int currentScopeIndex = sr.CurrentScopeIndex;
      sr.EnterScope();
      keyValuePairList.Each<KeyValuePair<string, DbExpression>, Scope>((Func<KeyValuePair<string, DbExpression>, Scope>) (projectionItem => sr.CurrentScope.Add(projectionItem.Key, (ScopeEntry) new ProjectionItemDefinitionScopeEntry(projectionItem.Value))));
      if (selectClause.DistinctKind == DistinctKind.Distinct)
      {
        source = SemanticAnalyzer.CreateProjectExpression(source, selectClause, keyValuePairList).BindAs(sr.GenerateInternalName("distinct"));
        if (selectClause.SelectKind == SelectKind.Value)
        {
          sr.CurrentScope.Replace(keyValuePairList[0].Key, (ScopeEntry) new SourceScopeEntry(source.Variable));
        }
        else
        {
          foreach (KeyValuePair<string, DbExpression> keyValuePair in keyValuePairList)
          {
            DbVariableReferenceExpression varRef = keyValuePair.Value.ResultType.Variable(keyValuePair.Key);
            sr.CurrentScope.Replace(varRef.VariableName, (ScopeEntry) new SourceScopeEntry(varRef).AddParentVar(source.Variable));
          }
        }
        queryProjectionProcessed = true;
      }
      List<DbSortClause> dbSortClauseList = new List<DbSortClause>(orderByClause.OrderByClauseItem.Count);
      for (int index = 0; index < orderByClause.OrderByClauseItem.Count; ++index)
      {
        OrderByClauseItem orderByClauseItem = orderByClause.OrderByClauseItem[index];
        sr.CurrentScopeRegion.WasResolutionCorrelated = false;
        DbExpression key = SemanticAnalyzer.ConvertValueExpression(orderByClauseItem.OrderExpr, sr);
        if (!sr.CurrentScopeRegion.WasResolutionCorrelated)
          throw EntitySqlException.Create(orderByClauseItem.ErrCtx, System.Data.Entity.Resources.Strings.KeyMustBeCorrelated((object) "ORDER BY"), (Exception) null);
        if (!TypeHelpers.IsValidSortOpKeyType(key.ResultType))
          throw EntitySqlException.Create(orderByClauseItem.OrderExpr.ErrCtx, System.Data.Entity.Resources.Strings.OrderByKeyIsNotOrderComparable, (Exception) null);
        bool flag = orderByClauseItem.OrderKind == OrderKind.None || orderByClauseItem.OrderKind == OrderKind.Asc;
        string collation = (string) null;
        if (orderByClauseItem.Collation != null)
        {
          if (!SemanticAnalyzer.IsStringType(key.ResultType))
            throw EntitySqlException.Create(orderByClauseItem.OrderExpr.ErrCtx, System.Data.Entity.Resources.Strings.InvalidKeyTypeForCollation((object) key.ResultType.EdmType.FullName), (Exception) null);
          collation = orderByClauseItem.Collation.Name;
        }
        if (string.IsNullOrEmpty(collation))
          dbSortClauseList.Add(flag ? key.ToSortClause() : key.ToSortClauseDescending());
        else
          dbSortClauseList.Add(flag ? key.ToSortClause(collation) : key.ToSortClauseDescending(collation));
      }
      sr.RollbackToScope(currentScopeIndex);
      sortBinding = (dbExpression == null ? (DbExpression) source.Sort((IEnumerable<DbSortClause>) dbSortClauseList) : (DbExpression) source.Skip((IEnumerable<DbSortClause>) dbSortClauseList, dbExpression)).BindAs(sr.GenerateInternalName("sort"));
      if (!queryProjectionProcessed)
        sr.CurrentScopeRegion.ApplyToScopeEntries((Action<ScopeEntry>) (scopeEntry =>
        {
          if (scopeEntry.EntryKind != ScopeEntryKind.SourceVar)
            return;
          ((SourceScopeEntry) scopeEntry).ReplaceParentVar(sortBinding.Variable);
        }));
      return sortBinding;
    }

    private static DbExpression ConvertSimpleInExpression(
      DbExpression left,
      DbExpression right)
    {
      DbNewInstanceExpression instanceExpression = (DbNewInstanceExpression) right;
      return instanceExpression.Arguments.Count == 0 ? (DbExpression) DbExpressionBuilder.False : Helpers.BuildBalancedTreeInPlace<DbExpression>((IList<DbExpression>) new List<DbExpression>((IEnumerable<DbExpression>) instanceExpression.Arguments.Select<DbExpression, DbComparisonExpression>((Func<DbExpression, DbComparisonExpression>) (arg => left.Equal(arg)))), (Func<DbExpression, DbExpression, DbExpression>) ((prev, next) => (DbExpression) prev.Or(next)));
    }

    private static bool IsStringType(TypeUsage type) => TypeSemantics.IsPrimitiveType(type, PrimitiveTypeKind.String);

    private static bool IsBooleanType(TypeUsage type) => TypeSemantics.IsPrimitiveType(type, PrimitiveTypeKind.Boolean);

    private static bool IsSubOrSuperType(TypeUsage type1, TypeUsage type2) => TypeSemantics.IsStructurallyEqual(type1, type2) || type1.IsSubtypeOf(type2) || type2.IsSubtypeOf(type1);

    private static Dictionary<Type, SemanticAnalyzer.AstExprConverter> CreateAstExprConverters() => new Dictionary<Type, SemanticAnalyzer.AstExprConverter>(17)
    {
      {
        typeof (Literal),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertLiteral)
      },
      {
        typeof (QueryParameter),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertParameter)
      },
      {
        typeof (Identifier),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertIdentifier)
      },
      {
        typeof (DotExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertDotExpr)
      },
      {
        typeof (BuiltInExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertBuiltIn)
      },
      {
        typeof (QueryExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertQueryExpr)
      },
      {
        typeof (ParenExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertParenExpr)
      },
      {
        typeof (RowConstructorExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertRowConstructor)
      },
      {
        typeof (MultisetConstructorExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertMultisetConstructor)
      },
      {
        typeof (CaseExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertCaseExpr)
      },
      {
        typeof (RelshipNavigationExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertRelshipNavigationExpr)
      },
      {
        typeof (RefExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertRefExpr)
      },
      {
        typeof (DerefExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertDeRefExpr)
      },
      {
        typeof (MethodExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertMethodExpr)
      },
      {
        typeof (CreateRefExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertCreateRefExpr)
      },
      {
        typeof (KeyExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertKeyExpr)
      },
      {
        typeof (GroupPartitionExpr),
        new SemanticAnalyzer.AstExprConverter(SemanticAnalyzer.ConvertGroupPartitionExpr)
      }
    };

    private static Dictionary<BuiltInKind, SemanticAnalyzer.BuiltInExprConverter> CreateBuiltInExprConverter() => new Dictionary<BuiltInKind, SemanticAnalyzer.BuiltInExprConverter>(4)
    {
      {
        BuiltInKind.Plus,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertPlusOperands(bltInExpr, sr);
          if (TypeSemantics.IsNumericType(pair.Left.ResultType))
            return (DbExpression) pair.Left.Plus(pair.Right);
          MetadataFunctionGroup functionGroup;
          if (!sr.TypeResolver.TryGetFunctionFromMetadata("Edm", "Concat", out functionGroup))
            throw EntitySqlException.Create(bltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ConcatBuiltinNotSupported, (Exception) null);
          List<TypeUsage> typeUsageList = new List<TypeUsage>(2);
          typeUsageList.Add(pair.Left.ResultType);
          typeUsageList.Add(pair.Right.ResultType);
          bool isAmbiguous = false;
          EdmFunction function = SemanticResolver.ResolveFunctionOverloads(functionGroup.FunctionMetadata, (IList<TypeUsage>) typeUsageList, false, out isAmbiguous);
          if (function == null | isAmbiguous)
            throw EntitySqlException.Create(bltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.ConcatBuiltinNotSupported, (Exception) null);
          return (DbExpression) function.Invoke(pair.Left, pair.Right);
        })
      },
      {
        BuiltInKind.Minus,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Minus(pair.Right);
        })
      },
      {
        BuiltInKind.Multiply,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Multiply(pair.Right);
        })
      },
      {
        BuiltInKind.Divide,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Divide(pair.Right);
        })
      },
      {
        BuiltInKind.Modulus,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Modulo(pair.Right);
        })
      },
      {
        BuiltInKind.UnaryMinus,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression left = SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr).Left;
          if (TypeSemantics.IsUnsignedNumericType(left.ResultType))
          {
            TypeUsage promotableType = (TypeUsage) null;
            if (!TypeHelpers.TryGetClosestPromotableType(left.ResultType, out promotableType))
              throw new EntitySqlException(System.Data.Entity.Resources.Strings.InvalidUnsignedTypeForUnaryMinusOperation((object) left.ResultType.EdmType.FullName));
          }
          return (DbExpression) left.UnaryMinus();
        })
      },
      {
        BuiltInKind.UnaryPlus,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => SemanticAnalyzer.ConvertArithmeticArgs(bltInExpr, sr).Left)
      },
      {
        BuiltInKind.And,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertLogicalArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.And(pair.Right);
        })
      },
      {
        BuiltInKind.Or,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertLogicalArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Or(pair.Right);
        })
      },
      {
        BuiltInKind.Not,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => (DbExpression) SemanticAnalyzer.ConvertLogicalArgs(bltInExpr, sr).Left.Not())
      },
      {
        BuiltInKind.Equal,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertEqualCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Equal(pair.Right);
        })
      },
      {
        BuiltInKind.NotEqual,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertEqualCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Equal(pair.Right).Not();
        })
      },
      {
        BuiltInKind.GreaterEqual,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertOrderCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.GreaterThanOrEqual(pair.Right);
        })
      },
      {
        BuiltInKind.GreaterThan,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertOrderCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.GreaterThan(pair.Right);
        })
      },
      {
        BuiltInKind.LessEqual,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertOrderCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.LessThanOrEqual(pair.Right);
        })
      },
      {
        BuiltInKind.LessThan,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertOrderCompArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.LessThan(pair.Right);
        })
      },
      {
        BuiltInKind.Union,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.UnionAll(pair.Right).Distinct();
        })
      },
      {
        BuiltInKind.UnionAll,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.UnionAll(pair.Right);
        })
      },
      {
        BuiltInKind.Intersect,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Intersect(pair.Right);
        })
      },
      {
        BuiltInKind.Overlaps,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Intersect(pair.Right).IsEmpty().Not();
        })
      },
      {
        BuiltInKind.AnyElement,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => (DbExpression) SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr).Left.Element())
      },
      {
        BuiltInKind.Element,
        (SemanticAnalyzer.BuiltInExprConverter) ((_param1, _param2) =>
        {
          throw new NotSupportedException(System.Data.Entity.Resources.Strings.ElementOperatorIsNotSupported);
        })
      },
      {
        BuiltInKind.Except,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr);
          return (DbExpression) pair.Left.Except(pair.Right);
        })
      },
      {
        BuiltInKind.Exists,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => (DbExpression) SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr).Left.IsEmpty().Not())
      },
      {
        BuiltInKind.Flatten,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression input1 = SemanticAnalyzer.ConvertValueExpression(bltInExpr.Arg1, sr);
          if (!TypeSemantics.IsCollectionType(input1.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidFlattenArgument, (Exception) null);
          DbExpressionBinding input2 = TypeSemantics.IsCollectionType(TypeHelpers.GetElementTypeUsage(input1.ResultType)) ? input1.BindAs(sr.GenerateInternalName("l_flatten")) : throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidFlattenArgument, (Exception) null);
          DbExpressionBinding apply = input2.Variable.BindAs(sr.GenerateInternalName("r_flatten"));
          DbExpressionBinding input3 = input2.CrossApply(apply).BindAs(sr.GenerateInternalName("flatten"));
          return (DbExpression) input3.Project((DbExpression) input3.Variable.Property(apply.VariableName));
        })
      },
      {
        BuiltInKind.In,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertInExprArgs(bltInExpr, sr);
          if (pair.Right.ExpressionKind == DbExpressionKind.NewInstance)
            return SemanticAnalyzer.ConvertSimpleInExpression(pair.Left, pair.Right);
          DbExpressionBinding input = pair.Right.BindAs(sr.GenerateInternalName("in-filter"));
          DbExpression left = pair.Left;
          DbExpression variable = (DbExpression) input.Variable;
          DbExpression right = (DbExpression) input.Filter((DbExpression) left.Equal(variable)).IsEmpty().Not();
          return (DbExpression) DbExpressionBuilder.Case((IEnumerable<DbExpression>) new List<DbExpression>(1)
          {
            (DbExpression) left.IsNull()
          }, (IEnumerable<DbExpression>) new List<DbExpression>(1)
          {
            (DbExpression) TypeResolver.BooleanType.Null()
          }, (DbExpression) DbExpressionBuilder.False).Or(right);
        })
      },
      {
        BuiltInKind.NotIn,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertInExprArgs(bltInExpr, sr);
          if (pair.Right.ExpressionKind == DbExpressionKind.NewInstance)
            return (DbExpression) SemanticAnalyzer.ConvertSimpleInExpression(pair.Left, pair.Right).Not();
          DbExpressionBinding input = pair.Right.BindAs(sr.GenerateInternalName("in-filter"));
          DbExpression left = pair.Left;
          DbExpression variable = (DbExpression) input.Variable;
          DbExpression right = (DbExpression) input.Filter((DbExpression) left.Equal(variable)).IsEmpty();
          return (DbExpression) DbExpressionBuilder.Case((IEnumerable<DbExpression>) new List<DbExpression>(1)
          {
            (DbExpression) left.IsNull()
          }, (IEnumerable<DbExpression>) new List<DbExpression>(1)
          {
            (DbExpression) TypeResolver.BooleanType.Null()
          }, (DbExpression) DbExpressionBuilder.True).And(right);
        })
      },
      {
        BuiltInKind.Distinct,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => (DbExpression) SemanticAnalyzer.ConvertSetArgs(bltInExpr, sr).Left.Distinct())
      },
      {
        BuiltInKind.IsNull,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr);
          if (dbExpression != null && !TypeHelpers.IsValidIsNullOpType(dbExpression.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.IsNullInvalidType, (Exception) null);
          return dbExpression == null ? (DbExpression) DbExpressionBuilder.True : (DbExpression) dbExpression.IsNull();
        })
      },
      {
        BuiltInKind.IsNotNull,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr);
          if (dbExpression != null && !TypeHelpers.IsValidIsNullOpType(dbExpression.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.IsNullInvalidType, (Exception) null);
          return dbExpression == null ? (DbExpression) DbExpressionBuilder.False : (DbExpression) dbExpression.IsNull().Not();
        })
      },
      {
        BuiltInKind.IsOf,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression1 = SemanticAnalyzer.ConvertValueExpression(bltInExpr.Arg1, sr);
          TypeUsage typeUsage = SemanticAnalyzer.ConvertTypeName(bltInExpr.Arg2, sr);
          int num = (bool) ((Literal) bltInExpr.Arg3).Value ? 1 : 0;
          bool flag1 = (bool) ((Literal) bltInExpr.Arg4).Value;
          bool flag2 = sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.RestrictedViewGenerationMode;
          if (!flag2 && !TypeSemantics.IsEntityType(dbExpression1.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeEntityType((object) System.Data.Entity.Resources.Strings.CtxIsOf, (object) dbExpression1.ResultType.EdmType.BuiltInTypeKind.ToString(), (object) dbExpression1.ResultType.EdmType.FullName), (Exception) null);
          if (flag2 && !TypeSemantics.IsNominalType(dbExpression1.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeNominalType((object) System.Data.Entity.Resources.Strings.CtxIsOf, (object) dbExpression1.ResultType.EdmType.BuiltInTypeKind.ToString(), (object) dbExpression1.ResultType.EdmType.FullName), (Exception) null);
          if (!flag2 && !TypeSemantics.IsEntityType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeEntityType((object) System.Data.Entity.Resources.Strings.CtxIsOf, (object) typeUsage.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage.EdmType.FullName), (Exception) null);
          if (flag2 && !TypeSemantics.IsNominalType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeNominalType((object) System.Data.Entity.Resources.Strings.CtxIsOf, (object) typeUsage.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage.EdmType.FullName), (Exception) null);
          if (!TypeSemantics.IsPolymorphicType(dbExpression1.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeInheritableType, (Exception) null);
          if (!TypeSemantics.IsPolymorphicType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeInheritableType, (Exception) null);
          if (!SemanticAnalyzer.IsSubOrSuperType(dbExpression1.ResultType, typeUsage))
            throw EntitySqlException.Create(bltInExpr.ErrCtx, System.Data.Entity.Resources.Strings.NotASuperOrSubType((object) dbExpression1.ResultType.EdmType.FullName, (object) typeUsage.EdmType.FullName), (Exception) null);
          TypeUsage readOnlyType = TypeHelpers.GetReadOnlyType(typeUsage);
          DbExpression dbExpression2 = num == 0 ? (DbExpression) dbExpression1.IsOf(readOnlyType) : (DbExpression) dbExpression1.IsOfOnly(readOnlyType);
          if (flag1)
            dbExpression2 = (DbExpression) dbExpression2.Not();
          return dbExpression2;
        })
      },
      {
        BuiltInKind.Treat,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr);
          TypeUsage typeUsage = SemanticAnalyzer.ConvertTypeName(bltInExpr.Arg2, sr);
          bool flag = sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.RestrictedViewGenerationMode;
          if (!flag && !TypeSemantics.IsEntityType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeEntityType((object) System.Data.Entity.Resources.Strings.CtxTreat, (object) typeUsage.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage.EdmType.FullName), (Exception) null);
          if (flag && !TypeSemantics.IsNominalType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeNominalType((object) System.Data.Entity.Resources.Strings.CtxTreat, (object) typeUsage.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage.EdmType.FullName), (Exception) null);
          if (dbExpression == null)
          {
            dbExpression = (DbExpression) typeUsage.Null();
          }
          else
          {
            if (!flag && !TypeSemantics.IsEntityType(dbExpression.ResultType))
              throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeEntityType((object) System.Data.Entity.Resources.Strings.CtxTreat, (object) dbExpression.ResultType.EdmType.BuiltInTypeKind.ToString(), (object) dbExpression.ResultType.EdmType.FullName), (Exception) null);
            if (flag && !TypeSemantics.IsNominalType(dbExpression.ResultType))
              throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionTypeMustBeNominalType((object) System.Data.Entity.Resources.Strings.CtxTreat, (object) dbExpression.ResultType.EdmType.BuiltInTypeKind.ToString(), (object) dbExpression.ResultType.EdmType.FullName), (Exception) null);
          }
          if (!TypeSemantics.IsPolymorphicType(dbExpression.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeInheritableType, (Exception) null);
          if (!TypeSemantics.IsPolymorphicType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeInheritableType, (Exception) null);
          if (!SemanticAnalyzer.IsSubOrSuperType(dbExpression.ResultType, typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.NotASuperOrSubType((object) dbExpression.ResultType.EdmType.FullName, (object) typeUsage.EdmType.FullName), (Exception) null);
          return (DbExpression) dbExpression.TreatAs(TypeHelpers.GetReadOnlyType(typeUsage));
        })
      },
      {
        BuiltInKind.Cast,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr);
          TypeUsage typeUsage = SemanticAnalyzer.ConvertTypeName(bltInExpr.Arg2, sr);
          if (!TypeSemantics.IsScalarType(typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCastType, (Exception) null);
          if (dbExpression == null)
            return (DbExpression) typeUsage.Null();
          if (!TypeSemantics.IsScalarType(dbExpression.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCastExpressionType, (Exception) null);
          if (!TypeSemantics.IsCastAllowed(dbExpression.ResultType, typeUsage))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.InvalidCast((object) dbExpression.ResultType.EdmType.FullName, (object) typeUsage.EdmType.FullName), (Exception) null);
          return (DbExpression) dbExpression.CastTo(TypeHelpers.GetReadOnlyType(typeUsage));
        })
      },
      {
        BuiltInKind.OfType,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression = SemanticAnalyzer.ConvertValueExpression(bltInExpr.Arg1, sr);
          TypeUsage typeUsage1 = SemanticAnalyzer.ConvertTypeName(bltInExpr.Arg2, sr);
          int num = (bool) ((Literal) bltInExpr.Arg3).Value ? 1 : 0;
          bool flag = sr.ParserOptions.ParserCompilationMode == ParserOptions.CompilationMode.RestrictedViewGenerationMode;
          TypeUsage typeUsage2 = TypeSemantics.IsCollectionType(dbExpression.ResultType) ? TypeHelpers.GetElementTypeUsage(dbExpression.ResultType) : throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.ExpressionMustBeCollection, (Exception) null);
          if (!flag && !TypeSemantics.IsEntityType(typeUsage2))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.OfTypeExpressionElementTypeMustBeEntityType((object) typeUsage2.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage2), (Exception) null);
          if (flag && !TypeSemantics.IsNominalType(typeUsage2))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.OfTypeExpressionElementTypeMustBeNominalType((object) typeUsage2.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage2), (Exception) null);
          if (!flag && !TypeSemantics.IsEntityType(typeUsage1))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeEntityType((object) System.Data.Entity.Resources.Strings.CtxOfType, (object) typeUsage1.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage1.EdmType.FullName), (Exception) null);
          if (flag && !TypeSemantics.IsNominalType(typeUsage1))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.TypeMustBeNominalType((object) System.Data.Entity.Resources.Strings.CtxOfType, (object) typeUsage1.EdmType.BuiltInTypeKind.ToString(), (object) typeUsage1.EdmType.FullName), (Exception) null);
          if (num != 0 && typeUsage1.EdmType.Abstract)
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.OfTypeOnlyTypeArgumentCannotBeAbstract((object) typeUsage1.EdmType.FullName), (Exception) null);
          if (!SemanticAnalyzer.IsSubOrSuperType(typeUsage2, typeUsage1))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.NotASuperOrSubType((object) typeUsage2.EdmType.FullName, (object) typeUsage1.EdmType.FullName), (Exception) null);
          return num == 0 ? (DbExpression) dbExpression.OfType(TypeHelpers.GetReadOnlyType(typeUsage1)) : (DbExpression) dbExpression.OfTypeOnly(TypeHelpers.GetReadOnlyType(typeUsage1));
        })
      },
      {
        BuiltInKind.Like,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) =>
        {
          DbExpression dbExpression1 = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr);
          if (dbExpression1 == null)
            dbExpression1 = (DbExpression) TypeResolver.StringType.Null();
          else if (!SemanticAnalyzer.IsStringType(dbExpression1.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.LikeArgMustBeStringType, (Exception) null);
          DbExpression pattern = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg2, sr);
          if (pattern == null)
            pattern = (DbExpression) TypeResolver.StringType.Null();
          else if (!SemanticAnalyzer.IsStringType(pattern.ResultType))
            throw EntitySqlException.Create(bltInExpr.Arg2.ErrCtx, System.Data.Entity.Resources.Strings.LikeArgMustBeStringType, (Exception) null);
          DbExpression dbExpression2;
          if (3 == bltInExpr.ArgCount)
          {
            DbExpression escape = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg3, sr);
            if (escape == null)
              escape = (DbExpression) TypeResolver.StringType.Null();
            else if (!SemanticAnalyzer.IsStringType(escape.ResultType))
              throw EntitySqlException.Create(bltInExpr.Arg3.ErrCtx, System.Data.Entity.Resources.Strings.LikeArgMustBeStringType, (Exception) null);
            dbExpression2 = (DbExpression) dbExpression1.Like(pattern, escape);
          }
          else
            dbExpression2 = (DbExpression) dbExpression1.Like(pattern);
          return dbExpression2;
        })
      },
      {
        BuiltInKind.Between,
        new SemanticAnalyzer.BuiltInExprConverter(SemanticAnalyzer.ConvertBetweenExpr)
      },
      {
        BuiltInKind.NotBetween,
        (SemanticAnalyzer.BuiltInExprConverter) ((bltInExpr, sr) => (DbExpression) SemanticAnalyzer.ConvertBetweenExpr(bltInExpr, sr).Not())
      }
    };

    private static DbExpression ConvertBetweenExpr(
      BuiltInExpr bltInExpr,
      SemanticResolver sr)
    {
      Pair<DbExpression, DbExpression> pair = SemanticAnalyzer.ConvertValueExpressionsWithUntypedNulls(bltInExpr.Arg2, bltInExpr.Arg3, bltInExpr.Arg1.ErrCtx, (Func<string>) (() => System.Data.Entity.Resources.Strings.BetweenLimitsCannotBeUntypedNulls), sr);
      TypeUsage commonTypeUsage = TypeHelpers.GetCommonTypeUsage(pair.Left.ResultType, pair.Right.ResultType);
      if (commonTypeUsage == null)
        throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.BetweenLimitsTypesAreNotCompatible((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      if (!TypeSemantics.IsOrderComparableTo(pair.Left.ResultType, pair.Right.ResultType))
        throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.BetweenLimitsTypesAreNotOrderComparable((object) pair.Left.ResultType.EdmType.FullName, (object) pair.Right.ResultType.EdmType.FullName), (Exception) null);
      DbExpression left = SemanticAnalyzer.ConvertValueExpressionAllowUntypedNulls(bltInExpr.Arg1, sr) ?? (DbExpression) commonTypeUsage.Null();
      if (!TypeSemantics.IsOrderComparableTo(left.ResultType, commonTypeUsage))
        throw EntitySqlException.Create(bltInExpr.Arg1.ErrCtx, System.Data.Entity.Resources.Strings.BetweenValueIsNotOrderComparable((object) left.ResultType.EdmType.FullName, (object) commonTypeUsage.EdmType.FullName), (Exception) null);
      return (DbExpression) left.GreaterThanOrEqual(pair.Left).And((DbExpression) left.LessThanOrEqual(pair.Right));
    }

    private delegate ParseResult StatementConverter(
      Statement astExpr,
      SemanticResolver sr);

    private sealed class InlineFunctionInfoImpl : InlineFunctionInfo
    {
      private DbLambda _convertedDefinition;
      private bool _convertingDefinition;

      internal InlineFunctionInfoImpl(
        System.Data.Entity.Core.Common.EntitySql.AST.FunctionDefinition functionDef,
        List<DbVariableReferenceExpression> parameters)
        : base(functionDef, parameters)
      {
      }

      internal override DbLambda GetLambda(SemanticResolver sr)
      {
        if (this._convertedDefinition == null)
        {
          if (this._convertingDefinition)
            throw EntitySqlException.Create(this.FunctionDefAst.ErrCtx, System.Data.Entity.Resources.Strings.Cqt_UDF_FunctionDefinitionWithCircularReference((object) this.FunctionDefAst.Name), (Exception) null);
          SemanticResolver sr1 = sr.CloneForInlineFunctionConversion();
          this._convertingDefinition = true;
          this._convertedDefinition = SemanticAnalyzer.ConvertInlineFunctionDefinition((InlineFunctionInfo) this, sr1);
          this._convertingDefinition = false;
        }
        return this._convertedDefinition;
      }
    }

    private sealed class GroupKeyInfo
    {
      internal readonly string Name;
      private string[] _alternativeName;
      internal readonly DbVariableReferenceExpression VarRef;
      internal readonly DbExpression VarBasedKeyExpr;
      internal readonly DbExpression GroupVarBasedKeyExpr;
      internal readonly DbExpression GroupAggBasedKeyExpr;

      internal GroupKeyInfo(
        string name,
        DbExpression varBasedKeyExpr,
        DbExpression groupVarBasedKeyExpr,
        DbExpression groupAggBasedKeyExpr)
      {
        this.Name = name;
        this.VarRef = varBasedKeyExpr.ResultType.Variable(name);
        this.VarBasedKeyExpr = varBasedKeyExpr;
        this.GroupVarBasedKeyExpr = groupVarBasedKeyExpr;
        this.GroupAggBasedKeyExpr = groupAggBasedKeyExpr;
      }

      internal string[] AlternativeName
      {
        get => this._alternativeName;
        set => this._alternativeName = value;
      }
    }

    private delegate ExpressionResolution AstExprConverter(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      SemanticResolver sr);

    private delegate DbExpression BuiltInExprConverter(
      BuiltInExpr astBltInExpr,
      SemanticResolver sr);
  }
}
