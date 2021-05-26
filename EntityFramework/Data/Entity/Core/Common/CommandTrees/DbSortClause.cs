﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbSortClause
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>
  /// Specifies a sort key that can be used as part of the sort order in a
  /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbSortExpression" />
  /// . This class cannot be inherited.
  /// </summary>
  public sealed class DbSortClause
  {
    private readonly DbExpression _expr;
    private readonly bool _asc;
    private readonly string _coll;

    internal DbSortClause(DbExpression key, bool asc, string collation)
    {
      this._expr = key;
      this._asc = asc;
      this._coll = collation;
    }

    /// <summary>Gets a Boolean value indicating whether or not this sort key uses an ascending sort order.</summary>
    /// <returns>true if this sort key uses an ascending sort order; otherwise, false.</returns>
    public bool Ascending => this._asc;

    /// <summary>Gets a string value that specifies the collation for this sort key.</summary>
    /// <returns>A string value that specifies the collation for this sort key.</returns>
    public string Collation => this._coll;

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that provides the value for this sort key.
    /// </summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" /> that provides the value for this sort key.
    /// </returns>
    public DbExpression Expression => this._expr;
  }
}
