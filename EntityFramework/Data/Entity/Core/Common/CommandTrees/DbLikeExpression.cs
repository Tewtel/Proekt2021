﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbLikeExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Represents a string comparison against the specified pattern with an optional escape string. This class cannot be inherited.  </summary>
  public sealed class DbLikeExpression : DbExpression
  {
    private readonly DbExpression _argument;
    private readonly DbExpression _pattern;
    private readonly DbExpression _escape;

    internal DbLikeExpression(
      TypeUsage booleanResultType,
      DbExpression input,
      DbExpression pattern,
      DbExpression escape)
      : base(DbExpressionKind.Like, booleanResultType)
    {
      this._argument = input;
      this._pattern = pattern;
      this._escape = escape;
    }

    /// <summary>Gets an expression that specifies the string to compare against the given pattern.</summary>
    /// <returns>An expression that specifies the string to compare against the given pattern.</returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLikeExpression" />
    /// , or its result type is not a string type.
    /// </exception>
    public DbExpression Argument => this._argument;

    /// <summary>Gets an expression that specifies the pattern against which the given string should be compared.</summary>
    /// <returns>An expression that specifies the pattern against which the given string should be compared.</returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLikeExpression" />
    /// , or its result type is not a string type.
    /// </exception>
    public DbExpression Pattern => this._pattern;

    /// <summary>Gets an expression that provides an optional escape string to use for the comparison.</summary>
    /// <returns>An expression that provides an optional escape string to use for the comparison.</returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbLikeExpression" />
    /// , or its result type is not a string type.
    /// </exception>
    public DbExpression Escape => this._escape;

    /// <summary>Implements the visitor pattern for expressions that do not produce a result value.</summary>
    /// <param name="visitor">
    /// An instance of <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpressionVisitor" />.
    /// </param>
    /// <exception cref="T:System.ArgumentNullException"> visitor  is null.</exception>
    public override void Accept(DbExpressionVisitor visitor)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpressionVisitor>(visitor, nameof (visitor));
      visitor.Visit(this);
    }

    /// <summary>Implements the visitor pattern for expressions that produce a result value of a specific type.</summary>
    /// <returns>
    /// A result value of a specific type produced by
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpressionVisitor" />
    /// .
    /// </returns>
    /// <param name="visitor">
    /// An instance of a typed <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpressionVisitor" /> that produces a result value of a specific type.
    /// </param>
    /// <typeparam name="TResultType">The type of the result produced by  visitor .</typeparam>
    /// <exception cref="T:System.ArgumentNullException"> visitor  is null.</exception>
    public override TResultType Accept<TResultType>(DbExpressionVisitor<TResultType> visitor)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpressionVisitor<TResultType>>(visitor, nameof (visitor));
      return visitor.Visit(this);
    }
  }
}
