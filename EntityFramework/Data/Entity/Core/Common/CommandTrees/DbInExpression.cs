// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbInExpression
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// Represents a boolean expression that tests whether a specified item matches any element in a list.
  /// </summary>
  public class DbInExpression : DbExpression
  {
    private readonly DbExpression _item;
    private readonly DbExpressionList _list;

    internal DbInExpression(TypeUsage booleanResultType, DbExpression item, DbExpressionList list)
      : base(DbExpressionKind.In, booleanResultType)
    {
      this._item = item;
      this._list = list;
    }

    /// <summary>
    /// Gets a DbExpression that specifies the item to be matched.
    /// </summary>
    public DbExpression Item => this._item;

    /// <summary>Gets the list of DbExpression to test for a match.</summary>
    public IList<DbExpression> List => (IList<DbExpression>) this._list;

    /// <summary>
    /// The visitor pattern method for expression visitors that do not produce a result value.
    /// </summary>
    /// <param name="visitor"> An instance of DbExpressionVisitor. </param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="visitor" />
    /// is null
    /// </exception>
    public override void Accept(DbExpressionVisitor visitor)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpressionVisitor>(visitor, nameof (visitor));
      visitor.Visit(this);
    }

    /// <summary>
    /// The visitor pattern method for expression visitors that produce a result value of a specific type.
    /// </summary>
    /// <param name="visitor"> An instance of a typed DbExpressionVisitor that produces a result value of type TResultType. </param>
    /// <typeparam name="TResultType">
    /// The type of the result produced by <paramref name="visitor" />
    /// </typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="visitor" />
    /// is null
    /// </exception>
    /// <returns>
    /// An instance of <typeparamref name="TResultType" /> .
    /// </returns>
    public override TResultType Accept<TResultType>(DbExpressionVisitor<TResultType> visitor)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpressionVisitor<TResultType>>(visitor, nameof (visitor));
      return visitor.Visit(this);
    }
  }
}
