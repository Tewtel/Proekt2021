// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalSqlSetQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Internal.Linq;

namespace System.Data.Entity.Internal
{
  internal class InternalSqlSetQuery : InternalSqlQuery
  {
    private readonly IInternalSet _set;
    private readonly bool _isNoTracking;

    internal InternalSqlSetQuery(
      IInternalSet set,
      string sql,
      bool isNoTracking,
      object[] parameters)
      : this(set, sql, isNoTracking, new bool?(), parameters)
    {
    }

    private InternalSqlSetQuery(
      IInternalSet set,
      string sql,
      bool isNoTracking,
      bool? streaming,
      object[] parameters)
      : base(sql, streaming, parameters)
    {
      this._set = set;
      this._isNoTracking = isNoTracking;
    }

    public override InternalSqlQuery AsNoTracking() => !this._isNoTracking ? (InternalSqlQuery) new InternalSqlSetQuery(this._set, this.Sql, true, this.Streaming, this.Parameters) : (InternalSqlQuery) this;

    public bool IsNoTracking => this._isNoTracking;

    public override InternalSqlQuery AsStreaming() => !this.Streaming.HasValue || !this.Streaming.Value ? (InternalSqlQuery) new InternalSqlSetQuery(this._set, this.Sql, this._isNoTracking, new bool?(true), this.Parameters) : (InternalSqlQuery) this;

    public override IEnumerator GetEnumerator() => this._set.ExecuteSqlQuery(this.Sql, this._isNoTracking, this.Streaming, this.Parameters);

    public override IDbAsyncEnumerator GetAsyncEnumerator() => this._set.ExecuteSqlQueryAsync(this.Sql, this._isNoTracking, this.Streaming, this.Parameters);
  }
}
