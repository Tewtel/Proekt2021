﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbSqlQuery
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Internal;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Represents a SQL query for entities that is created from a <see cref="T:System.Data.Entity.DbContext" />
  /// and is executed using the connection from that context.
  /// Instances of this class are obtained from the <see cref="T:System.Data.Entity.DbSet" /> instance for the
  /// entity type. The query is not executed when this object is created; it is executed
  /// each time it is enumerated, for example by using foreach.
  /// SQL queries for non-entities are created using <see cref="M:System.Data.Entity.Database.SqlQuery(System.Type,System.String,System.Object[])" />.
  /// See <see cref="T:System.Data.Entity.Infrastructure.DbSqlQuery`1" /> for a generic version of this class.
  /// </summary>
  public class DbSqlQuery : DbRawSqlQuery
  {
    internal DbSqlQuery(InternalSqlQuery internalQuery)
      : base(internalQuery)
    {
    }

    /// <summary>
    /// Creates an instance of a <see cref="T:System.Data.Entity.Infrastructure.DbSqlQuery" /> when called from the constructor of a derived
    /// type that will be used as a test double for <see cref="M:System.Data.Entity.DbSet.SqlQuery(System.String,System.Object[])" />. Methods and properties
    /// that will be used by the test double must be implemented by the test double except AsNoTracking
    /// and AsStreaming where the default implementation is a no-op.
    /// </summary>
    protected DbSqlQuery()
      : this((InternalSqlQuery) null)
    {
    }

    /// <summary>
    /// Returns a new query where the results of the query will not be tracked by the associated
    /// <see cref="T:System.Data.Entity.DbContext" />.
    /// </summary>
    /// <returns> A new query with NoTracking applied. </returns>
    public virtual DbSqlQuery AsNoTracking() => this.InternalQuery != null ? new DbSqlQuery(this.InternalQuery.AsNoTracking()) : this;

    /// <summary>
    /// Returns a new query that will stream the results instead of buffering.
    /// </summary>
    /// <returns> A new query with AsStreaming applied. </returns>
    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public virtual DbSqlQuery AsStreaming() => this.InternalQuery != null ? new DbSqlQuery(this.InternalQuery.AsStreaming()) : this;

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
