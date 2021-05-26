// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Common;
using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls to <see cref="T:System.Data.Common.DbTransaction" /> that don't return any results.
  /// </summary>
  public class DbTransactionInterceptionContext : MutableInterceptionContext
  {
    private DbConnection _connection;

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> with no state.
    /// </summary>
    public DbTransactionInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> by copying immutable state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public DbTransactionInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      if (copyFrom is DbTransactionInterceptionContext interceptionContext)
        this._connection = interceptionContext.Connection;
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
    }

    /// <summary>The connection on which the transaction was started</summary>
    public DbConnection Connection => this._connection;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Common.DbConnection" />.
    /// </summary>
    /// <param name="connection">The connection on which the transaction was started.</param>
    /// <returns>A new interception context that also contains the connection on which the transaction was started.</returns>
    public DbTransactionInterceptionContext WithConnection(
      DbConnection connection)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbConnection>(connection, nameof (connection));
      DbTransactionInterceptionContext interceptionContext = this.TypedClone();
      interceptionContext._connection = connection;
      return interceptionContext;
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context together with the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public DbTransactionInterceptionContext AsAsync() => (DbTransactionInterceptionContext) base.AsAsync();

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbTransactionInterceptionContext WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (DbTransactionInterceptionContext) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.DbTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public DbTransactionInterceptionContext WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (DbTransactionInterceptionContext) base.WithObjectContext(context);
    }

    private DbTransactionInterceptionContext TypedClone() => (DbTransactionInterceptionContext) this.Clone();

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new DbTransactionInterceptionContext((DbInterceptionContext) this);

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
