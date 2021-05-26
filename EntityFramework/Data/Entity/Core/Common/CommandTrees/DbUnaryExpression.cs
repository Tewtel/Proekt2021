// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbUnaryExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Implements the basic functionality required by expressions that accept a single expression argument. </summary>
  public abstract class DbUnaryExpression : DbExpression
  {
    private readonly DbExpression _argument;

    internal DbUnaryExpression()
    {
    }

    internal DbUnaryExpression(DbExpressionKind kind, TypeUsage resultType, DbExpression argument)
      : base(kind, resultType)
    {
      this._argument = argument;
    }

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the argument.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that defines the argument.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">The expression is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// The expression is not associated with the command tree of a
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbUnaryExpression" />
    /// , or its result type is not equal or promotable to the required type for the argument.
    /// </exception>
    public virtual DbExpression Argument => this._argument;
  }
}
