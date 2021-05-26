// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalSqlNonSetQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal class InternalSqlNonSetQuery : InternalSqlQuery
  {
    private readonly InternalContext _internalContext;
    private readonly Type _elementType;

    internal InternalSqlNonSetQuery(
      InternalContext internalContext,
      Type elementType,
      string sql,
      object[] parameters)
      : this(internalContext, elementType, sql, new bool?(), parameters)
    {
    }

    private InternalSqlNonSetQuery(
      InternalContext internalContext,
      Type elementType,
      string sql,
      bool? streaming,
      object[] parameters)
      : base(sql, streaming, parameters)
    {
      this._internalContext = internalContext;
      this._elementType = elementType;
    }

    public override InternalSqlQuery AsNoTracking() => (InternalSqlQuery) this;

    public override InternalSqlQuery AsStreaming() => !this.Streaming.HasValue || !this.Streaming.Value ? (InternalSqlQuery) new InternalSqlNonSetQuery(this._internalContext, this._elementType, this.Sql, new bool?(true), this.Parameters) : (InternalSqlQuery) this;

    public override IEnumerator GetEnumerator() => this._internalContext.ExecuteSqlQuery(this._elementType, this.Sql, this.Streaming, this.Parameters);

    public override IDbAsyncEnumerator GetAsyncEnumerator() => this._internalContext.ExecuteSqlQueryAsync(this._elementType, this.Sql, this.Streaming, this.Parameters);
  }
}
