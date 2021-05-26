// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.InternalSqlQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal abstract class InternalSqlQuery : IEnumerable, IDbAsyncEnumerable
  {
    private readonly string _sql;
    private readonly object[] _parameters;
    private readonly bool? _streaming;

    internal InternalSqlQuery(string sql, bool? streaming, object[] parameters)
    {
      this._sql = sql;
      this._parameters = parameters;
      this._streaming = streaming;
    }

    public string Sql => this._sql;

    internal bool? Streaming => this._streaming;

    public object[] Parameters => this._parameters;

    public abstract InternalSqlQuery AsNoTracking();

    public abstract InternalSqlQuery AsStreaming();

    public abstract IEnumerator GetEnumerator();

    public abstract IDbAsyncEnumerator GetAsyncEnumerator();

    public override string ToString() => this.Sql;
  }
}
