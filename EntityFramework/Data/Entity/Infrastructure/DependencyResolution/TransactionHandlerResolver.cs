﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.TransactionHandlerResolver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  /// <summary>
  /// An <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.IDbDependencyResolver" /> implementation used for resolving <see cref="T:System.Data.Entity.Infrastructure.TransactionHandler" />
  /// factories.
  /// </summary>
  public class TransactionHandlerResolver : IDbDependencyResolver
  {
    private readonly Func<TransactionHandler> _transactionHandlerFactory;
    private readonly string _providerInvariantName;
    private readonly string _serverName;

    /// <summary>
    /// Initializes a new instance of <see cref="T:System.Data.Entity.Infrastructure.DependencyResolution.TransactionHandlerResolver" />
    /// </summary>
    /// <param name="transactionHandlerFactory">A function that returns a new instance of a transaction handler.</param>
    /// <param name="providerInvariantName">
    /// The ADO.NET provider invariant name indicating the type of ADO.NET connection for which the transaction handler will be used.
    /// <c>null</c> will match anything.
    /// </param>
    /// <param name="serverName">
    /// A string that will be matched against the server name in the connection string. <c>null</c> will match anything.
    /// </param>
    public TransactionHandlerResolver(
      Func<TransactionHandler> transactionHandlerFactory,
      string providerInvariantName,
      string serverName)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TransactionHandler>>(transactionHandlerFactory, nameof (transactionHandlerFactory));
      this._providerInvariantName = providerInvariantName;
      this._serverName = serverName;
      this._transactionHandlerFactory = transactionHandlerFactory;
    }

    /// <summary>
    /// If the given type is <see cref="T:System.Func`1" />, then this method will attempt
    /// to return the service to use, otherwise it will return <c>null</c>. When the given type is
    /// <see cref="T:System.Func`1" />, then the key is expected to be a <see cref="T:System.Data.Entity.Infrastructure.ExecutionStrategyKey" />.
    /// </summary>
    /// <param name="type">The service type to resolve.</param>
    /// <param name="key">A key used to make a determination of the service to return.</param>
    /// <returns>
    /// An <see cref="T:System.Func`1" />, or null.
    /// </returns>
    public object GetService(Type type, object key)
    {
      if (!(type == typeof (Func<TransactionHandler>)))
        return (object) null;
      if (!(key is ExecutionStrategyKey executionStrategyKey))
        throw new ArgumentException(Strings.DbDependencyResolver_InvalidKey((object) typeof (ExecutionStrategyKey).Name, (object) "Func<TransactionHandler>"));
      if (this._providerInvariantName != null && !executionStrategyKey.ProviderInvariantName.Equals(this._providerInvariantName, StringComparison.Ordinal))
        return (object) null;
      return this._serverName != null && !this._serverName.Equals(executionStrategyKey.ServerName, StringComparison.Ordinal) ? (object) null : (object) this._transactionHandlerFactory;
    }

    /// <summary>
    /// If the given type is <see cref="T:System.Func`1" />, then this resolver will attempt
    /// to return the service to use, otherwise it will return an empty enumeration. When the given type is
    /// <see cref="T:System.Func`1" />, then the key is expected to be an <see cref="T:System.Data.Entity.Infrastructure.ExecutionStrategyKey" />.
    /// </summary>
    /// <param name="type">The service type to resolve.</param>
    /// <param name="key">A key used to make a determination of the service to return.</param>
    /// <returns>
    /// An enumerable of <see cref="T:System.Func`1" />, or an empty enumeration.
    /// </returns>
    public IEnumerable<object> GetServices(Type type, object key) => this.GetServiceAsServices(type, key);

    /// <inheritdoc />
    public override bool Equals(object obj) => obj is TransactionHandlerResolver transactionHandlerResolver && this._transactionHandlerFactory == transactionHandlerResolver._transactionHandlerFactory && this._providerInvariantName == transactionHandlerResolver._providerInvariantName && this._serverName == transactionHandlerResolver._serverName;

    /// <inheritdoc />
    public override int GetHashCode()
    {
      int num = this._transactionHandlerFactory.GetHashCode();
      if (this._providerInvariantName != null)
        num = num * 41 + this._providerInvariantName.GetHashCode();
      if (this._serverName != null)
        num = num * 41 + this._serverName.GetHashCode();
      return num;
    }
  }
}
