// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.AST.QueryExpr
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql.AST
{
  internal sealed class QueryExpr : Node
  {
    private readonly SelectClause _selectClause;
    private readonly FromClause _fromClause;
    private readonly Node _whereClause;
    private readonly GroupByClause _groupByClause;
    private readonly HavingClause _havingClause;
    private readonly OrderByClause _orderByClause;

    internal QueryExpr(
      SelectClause selectClause,
      FromClause fromClause,
      Node whereClause,
      GroupByClause groupByClause,
      HavingClause havingClause,
      OrderByClause orderByClause)
    {
      this._selectClause = selectClause;
      this._fromClause = fromClause;
      this._whereClause = whereClause;
      this._groupByClause = groupByClause;
      this._havingClause = havingClause;
      this._orderByClause = orderByClause;
    }

    internal SelectClause SelectClause => this._selectClause;

    internal FromClause FromClause => this._fromClause;

    internal Node WhereClause => this._whereClause;

    internal GroupByClause GroupByClause => this._groupByClause;

    internal HavingClause HavingClause => this._havingClause;

    internal OrderByClause OrderByClause => this._orderByClause;

    internal bool HasMethodCall
    {
      get
      {
        if (this._selectClause.HasMethodCall || this._havingClause != null && this._havingClause.HasMethodCall)
          return true;
        return this._orderByClause != null && this._orderByClause.HasMethodCall;
      }
    }
  }
}
