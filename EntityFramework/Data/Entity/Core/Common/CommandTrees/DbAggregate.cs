// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbAggregate
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.Internal;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  /// <summary>Implements the basic functionality required by aggregates in a GroupBy clause. </summary>
  public abstract class DbAggregate
  {
    private readonly DbExpressionList _args;
    private readonly TypeUsage _type;

    internal DbAggregate(TypeUsage resultType, DbExpressionList arguments)
    {
      this._type = resultType;
      this._args = arguments;
    }

    /// <summary>
    /// Gets the result type of this <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" />.
    /// </summary>
    /// <returns>
    /// The result type of this <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" />.
    /// </returns>
    public TypeUsage ResultType => this._type;

    /// <summary>
    /// Gets the list of expressions that define the arguments to this
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" />
    /// .
    /// </summary>
    /// <returns>
    /// The list of expressions that define the arguments to this
    /// <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbAggregate" />
    /// .
    /// </returns>
    public IList<DbExpression> Arguments => (IList<DbExpression>) this._args;
  }
}
