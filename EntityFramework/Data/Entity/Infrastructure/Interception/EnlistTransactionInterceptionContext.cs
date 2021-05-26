// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Transactions;

namespace System.Data.Entity.Infrastructure.Interception
{
  /// <summary>
  /// Represents contextual information associated with calls to <see cref="M:System.Data.Common.DbConnection.EnlistTransaction(System.Transactions.Transaction)" />
  /// implementations.
  /// </summary>
  /// <remarks>
  /// Instances of this class are publicly immutable for contextual information. To add
  /// contextual information use one of the With... or As... methods to create a new
  /// interception context containing the new information.
  /// </remarks>
  public class EnlistTransactionInterceptionContext : DbConnectionInterceptionContext
  {
    private Transaction _transaction;

    /// <summary>
    /// Constructs a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> with no state.
    /// </summary>
    public EnlistTransactionInterceptionContext()
    {
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> by copying immutable state from the given
    /// interception context. Also see <see cref="M:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext.Clone" />
    /// </summary>
    /// <param name="copyFrom">The context from which to copy state.</param>
    public EnlistTransactionInterceptionContext(DbInterceptionContext copyFrom)
      : base(copyFrom)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbInterceptionContext>(copyFrom, nameof (copyFrom));
      if (!(copyFrom is EnlistTransactionInterceptionContext interceptionContext))
        return;
      this._transaction = interceptionContext._transaction;
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context together with the <see cref="P:System.Data.Entity.Infrastructure.Interception.DbInterceptionContext.IsAsync" /> flag set to true.
    /// </summary>
    /// <returns>A new interception context associated with the async flag set.</returns>
    public EnlistTransactionInterceptionContext AsAsync() => (EnlistTransactionInterceptionContext) base.AsAsync();

    /// <summary>
    /// The <see cref="P:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext.Transaction" /> that will be used or has been used to enlist a connection.
    /// </summary>
    public Transaction Transaction => this._transaction;

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context together with the given <see cref="P:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext.Transaction" />.
    /// </summary>
    /// <param name="transaction">The transaction to be used in the <see cref="M:System.Data.Common.DbConnection.EnlistTransaction(System.Transactions.Transaction)" /> invocation.</param>
    /// <returns>A new interception context associated with the given isolation level.</returns>
    public EnlistTransactionInterceptionContext WithTransaction(
      Transaction transaction)
    {
      EnlistTransactionInterceptionContext interceptionContext = this.TypedClone();
      interceptionContext._transaction = transaction;
      return interceptionContext;
    }

    private EnlistTransactionInterceptionContext TypedClone() => (EnlistTransactionInterceptionContext) this.Clone();

    /// <inheritdoc />
    protected override DbInterceptionContext Clone() => (DbInterceptionContext) new EnlistTransactionInterceptionContext((DbInterceptionContext) this);

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public EnlistTransactionInterceptionContext WithDbContext(
      DbContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      return (EnlistTransactionInterceptionContext) base.WithDbContext(context);
    }

    /// <summary>
    /// Creates a new <see cref="T:System.Data.Entity.Infrastructure.Interception.EnlistTransactionInterceptionContext" /> that contains all the contextual information in this
    /// interception context with the addition of the given <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />.
    /// </summary>
    /// <param name="context">The context to associate.</param>
    /// <returns>A new interception context associated with the given context.</returns>
    public EnlistTransactionInterceptionContext WithObjectContext(
      ObjectContext context)
    {
      System.Data.Entity.Utilities.Check.NotNull<ObjectContext>(context, nameof (context));
      return (EnlistTransactionInterceptionContext) base.WithObjectContext(context);
    }

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
