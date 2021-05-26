// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbArithmeticExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// Represents an arithmetic operation applied to numeric arguments.
  /// Addition, subtraction, multiplication, division, modulo, and negation are arithmetic operations.
  /// This class cannot be inherited.
  /// </summary>
  public sealed class DbArithmeticExpression : DbExpression
  {
    private readonly DbExpressionList _args;

    internal DbArithmeticExpression(
      DbExpressionKind kind,
      TypeUsage numericResultType,
      DbExpressionList args)
      : base(kind, numericResultType)
    {
      this._args = args;
    }

    /// <summary>
    /// Gets the list of <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> elements that define the current arguments.
    /// </summary>
    /// <returns>
    /// A fixed-size list of <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> elements.
    /// </returns>
    public IList<DbExpression> Arguments => (IList<DbExpression>) this._args;

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
