﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbBinaryExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Implements the basic functionality required by expressions that accept two expression operands.</summary>
  public abstract class DbBinaryExpression : DbExpression
  {
    private readonly DbExpression _left;
    private readonly DbExpression _right;

    internal DbBinaryExpression()
    {
    }

    internal DbBinaryExpression(
      DbExpressionKind kind,
      TypeUsage type,
      DbExpression left,
      DbExpression right)
      : base(kind, type)
    {
      this._left = left;
      this._right = right;
    }

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the left argument.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the left argument.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of the
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbBinaryExpression" />
    /// ,or its result type is not equal or promotable to the required type for the left argument.
    /// </exception>
    public virtual DbExpression Left => this._left;

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the right argument.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the right argument.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of the
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbBinaryExpression" />
    /// ,or its result type is not equal or promotable to the required type for the right argument.
    /// </exception>
    public virtual DbExpression Right => this._right;
  }
}
