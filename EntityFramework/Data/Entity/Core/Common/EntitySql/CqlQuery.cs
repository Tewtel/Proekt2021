// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.CqlQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal static class CqlQuery
  {
    internal static ParseResult Compile(
      string commandText,
      Perspective perspective,
      ParserOptions parserOptions,
      IEnumerable<DbParameterReferenceExpression> parameters)
    {
      return CqlQuery.CompileCommon<ParseResult>(commandText, parserOptions, (Func<System.Data.Entity.Core.Common.EntitySql.AST.Node, ParserOptions, ParseResult>) ((astCommand, validatedParserOptions) => CqlQuery.AnalyzeCommandSemantics(astCommand, perspective, validatedParserOptions, parameters)));
    }

    internal static DbLambda CompileQueryCommandLambda(
      string queryCommandText,
      Perspective perspective,
      ParserOptions parserOptions,
      IEnumerable<DbParameterReferenceExpression> parameters,
      IEnumerable<DbVariableReferenceExpression> variables)
    {
      return CqlQuery.CompileCommon<DbLambda>(queryCommandText, parserOptions, (Func<System.Data.Entity.Core.Common.EntitySql.AST.Node, ParserOptions, DbLambda>) ((astCommand, validatedParserOptions) => CqlQuery.AnalyzeQueryExpressionSemantics(astCommand, perspective, validatedParserOptions, parameters, variables)));
    }

    private static System.Data.Entity.Core.Common.EntitySql.AST.Node Parse(
      string commandText,
      ParserOptions parserOptions)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(commandText, nameof (commandText));
      return new CqlParser(parserOptions, true).Parse(commandText) ?? throw EntitySqlException.Create(commandText, Strings.InvalidEmptyQuery, 0, (string) null, false, (Exception) null);
    }

    private static TResult CompileCommon<TResult>(
      string commandText,
      ParserOptions parserOptions,
      Func<System.Data.Entity.Core.Common.EntitySql.AST.Node, ParserOptions, TResult> compilationFunction)
      where TResult : class
    {
      parserOptions = parserOptions ?? new ParserOptions();
      return compilationFunction(CqlQuery.Parse(commandText, parserOptions), parserOptions);
    }

    private static ParseResult AnalyzeCommandSemantics(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      Perspective perspective,
      ParserOptions parserOptions,
      IEnumerable<DbParameterReferenceExpression> parameters)
    {
      return CqlQuery.AnalyzeSemanticsCommon<ParseResult>(astExpr, perspective, parserOptions, parameters, (IEnumerable<DbVariableReferenceExpression>) null, (Func<SemanticAnalyzer, System.Data.Entity.Core.Common.EntitySql.AST.Node, ParseResult>) ((analyzer, astExpression) => analyzer.AnalyzeCommand(astExpression)));
    }

    private static DbLambda AnalyzeQueryExpressionSemantics(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astQueryCommand,
      Perspective perspective,
      ParserOptions parserOptions,
      IEnumerable<DbParameterReferenceExpression> parameters,
      IEnumerable<DbVariableReferenceExpression> variables)
    {
      return CqlQuery.AnalyzeSemanticsCommon<DbLambda>(astQueryCommand, perspective, parserOptions, parameters, variables, (Func<SemanticAnalyzer, System.Data.Entity.Core.Common.EntitySql.AST.Node, DbLambda>) ((analyzer, astExpr) => analyzer.AnalyzeQueryCommand(astExpr)));
    }

    private static TResult AnalyzeSemanticsCommon<TResult>(
      System.Data.Entity.Core.Common.EntitySql.AST.Node astExpr,
      Perspective perspective,
      ParserOptions parserOptions,
      IEnumerable<DbParameterReferenceExpression> parameters,
      IEnumerable<DbVariableReferenceExpression> variables,
      Func<SemanticAnalyzer, System.Data.Entity.Core.Common.EntitySql.AST.Node, TResult> analysisFunction)
      where TResult : class
    {
      TResult result = default (TResult);
      try
      {
        SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(SemanticResolver.Create(perspective, parserOptions, parameters, variables));
        return analysisFunction(semanticAnalyzer, astExpr);
      }
      catch (MetadataException ex)
      {
        throw new EntitySqlException(Strings.GeneralExceptionAsQueryInnerException((object) "Metadata"), (Exception) ex);
      }
      catch (MappingException ex)
      {
        throw new EntitySqlException(Strings.GeneralExceptionAsQueryInnerException((object) "Mapping"), (Exception) ex);
      }
    }
  }
}
