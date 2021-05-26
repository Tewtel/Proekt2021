// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.EntitySqlParser
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.EntitySql
{
  /// <summary>Public Entity SQL Parser class.</summary>
  public sealed class EntitySqlParser
  {
    private readonly Perspective _perspective;

    internal EntitySqlParser(Perspective perspective) => this._perspective = perspective;

    /// <summary>Parse the specified query with the specified parameters.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.EntitySql.ParseResult" /> containing
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" />
    /// and information describing inline function definitions if any.
    /// </returns>
    /// <param name="query">The EntitySQL query to be parsed.</param>
    /// <param name="parameters">The optional query parameters.</param>
    public ParseResult Parse(
      string query,
      params DbParameterReferenceExpression[] parameters)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(query, nameof (query));
      if (parameters != null)
      {
        IEnumerable<DbParameterReferenceExpression> enumerableArgument = (IEnumerable<DbParameterReferenceExpression>) parameters;
        EntityUtil.CheckArgumentContainsNull<DbParameterReferenceExpression>(ref enumerableArgument, nameof (parameters));
      }
      return CqlQuery.Compile(query, this._perspective, (ParserOptions) null, (IEnumerable<DbParameterReferenceExpression>) parameters);
    }

    /// <summary>
    /// Parse a specific query with a specific set variables and produce a
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLambda" />
    /// .
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.EntitySql.ParseResult" /> containing
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbCommandTree" />
    /// and information describing inline function definitions if any.
    /// </returns>
    /// <param name="query">The query to be parsed.</param>
    /// <param name="variables">The optional query variables.</param>
    public DbLambda ParseLambda(
      string query,
      params DbVariableReferenceExpression[] variables)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(query, nameof (query));
      if (variables != null)
      {
        IEnumerable<DbVariableReferenceExpression> enumerableArgument = (IEnumerable<DbVariableReferenceExpression>) variables;
        EntityUtil.CheckArgumentContainsNull<DbVariableReferenceExpression>(ref enumerableArgument, nameof (variables));
      }
      return CqlQuery.CompileQueryCommandLambda(query, this._perspective, (ParserOptions) null, (IEnumerable<DbParameterReferenceExpression>) null, (IEnumerable<DbVariableReferenceExpression>) variables);
    }
  }
}
