﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbRawSqlQuery`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Internal;
using System.Data.Entity.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Represents a SQL query for non-entities that is created from a <see cref="T:System.Data.Entity.DbContext" />
  /// and is executed using the connection from that context.
  /// Instances of this class are obtained from the <see cref="P:System.Data.Entity.DbContext.Database" /> instance.
  /// The query is not executed when this object is created; it is executed
  /// each time it is enumerated, for example by using <c>foreach</c>.
  /// SQL queries for entities are created using <see cref="M:System.Data.Entity.DbSet`1.SqlQuery(System.String,System.Object[])" />.
  /// See <see cref="T:System.Data.Entity.Infrastructure.DbRawSqlQuery" /> for a non-generic version of this class.
  /// </summary>
  /// <typeparam name="TElement">The type of elements returned by the query.</typeparam>
  public class DbRawSqlQuery<TElement> : 
    IEnumerable<TElement>,
    IEnumerable,
    IListSource,
    IDbAsyncEnumerable<TElement>,
    IDbAsyncEnumerable
  {
    private readonly InternalSqlQuery _internalQuery;

    internal DbRawSqlQuery(InternalSqlQuery internalQuery) => this._internalQuery = internalQuery;

    /// <summary>
    /// Returns a new query that will stream the results instead of buffering.
    /// </summary>
    /// <returns> A new query with AsStreaming applied. </returns>
    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. Calling this method will have no effect.")]
    public virtual DbRawSqlQuery<TElement> AsStreaming() => this._internalQuery != null ? new DbRawSqlQuery<TElement>(this._internalQuery.AsStreaming()) : this;

    /// <summary>
    /// Returns an <see cref="T:System.Collections.Generic.IEnumerator`1" /> which when enumerated will execute the SQL query against the database.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.Generic.IEnumerator`1" /> object that can be used to iterate through the elements.
    /// </returns>
    public virtual IEnumerator<TElement> GetEnumerator() => (IEnumerator<TElement>) this.GetInternalQueryWithCheck(nameof (GetEnumerator)).GetEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Collections.IEnumerator" /> which when enumerated will execute the SQL query against the database.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the elements.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerable`1" /> which when enumerated will execute the SQL query against the database.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerable`1" /> object that can be used to iterate through the elements.
    /// </returns>
    IDbAsyncEnumerator<TElement> IDbAsyncEnumerable<TElement>.GetAsyncEnumerator() => (IDbAsyncEnumerator<TElement>) this.GetInternalQueryWithCheck("IDbAsyncEnumerable<TElement>.GetAsyncEnumerator").GetAsyncEnumerator();

    /// <summary>
    /// Returns an <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerable" /> which when enumerated will execute the SQL query against the database.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Data.Entity.Infrastructure.IDbAsyncEnumerable" /> object that can be used to iterate through the elements.
    /// </returns>
    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this._internalQuery.GetAsyncEnumerator();

    /// <summary>
    /// Asynchronously enumerates the query results and performs the specified action on each element.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="action"> The action to be executed. </param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public Task ForEachAsync(Action<TElement> action)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<TElement>>(action, nameof (action));
      return this.ForEachAsync<TElement>(action, CancellationToken.None);
    }

    /// <summary>
    /// Asynchronously enumerates the query results and performs the specified action on each element.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="action"> The action to be executed. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns> A task that represents the asynchronous operation. </returns>
    public Task ForEachAsync(Action<TElement> action, CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<TElement>>(action, nameof (action));
      return this.ForEachAsync<TElement>(action, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.List`1" /> from the query by enumerating it asynchronously.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.List`1" /> that contains elements from the input sequence.
    /// </returns>
    public Task<List<TElement>> ToListAsync() => IDbAsyncEnumerableExtensions.ToListAsync<TElement>(this);

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.List`1" /> from the query by enumerating it asynchronously.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.List`1" /> that contains elements from the input sequence.
    /// </returns>
    public Task<List<TElement>> ToListAsync(CancellationToken cancellationToken) => IDbAsyncEnumerableExtensions.ToListAsync<TElement>(this, cancellationToken);

    /// <summary>
    /// Creates an array from the query by enumerating it asynchronously.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an array that contains elements from the input sequence.
    /// </returns>
    public Task<TElement[]> ToArrayAsync() => this.ToArrayAsync<TElement>();

    /// <summary>
    /// Creates an array from the query by enumerating it asynchronously.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains an array that contains elements from the input sequence.
    /// </returns>
    public Task<TElement[]> ToArrayAsync(CancellationToken cancellationToken) => this.ToArrayAsync<TElement>(cancellationToken);

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains selected keys and values.
    /// </returns>
    public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey>(
      Func<TElement, TKey> keySelector)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      return this.ToDictionaryAsync<TElement, TKey>(keySelector);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains selected keys and values.
    /// </returns>
    public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey>(
      Func<TElement, TKey> keySelector,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      return this.ToDictionaryAsync<TElement, TKey>(keySelector, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function and a comparer.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="comparer">
    /// An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains selected keys and values.
    /// </returns>
    public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey>(
      Func<TElement, TKey> keySelector,
      IEqualityComparer<TKey> comparer)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      return this.ToDictionaryAsync<TElement, TKey>(keySelector, comparer);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function and a comparer.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="comparer">
    /// An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains selected keys and values.
    /// </returns>
    public Task<Dictionary<TKey, TElement>> ToDictionaryAsync<TKey>(
      Func<TElement, TKey> keySelector,
      IEqualityComparer<TKey> comparer,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      return this.ToDictionaryAsync<TElement, TKey>(keySelector, comparer, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector and an element selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the value returned by <paramref name="elementSelector" />.
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="elementSelector"> A transform function to produce a result element value from each element. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type
    /// <typeparamref name="TResult" /> selected from the query.
    /// </returns>
    public Task<Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(
      Func<TElement, TKey> keySelector,
      Func<TElement, TResult> elementSelector)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TResult>>(elementSelector, nameof (elementSelector));
      return this.ToDictionaryAsync<TElement, TKey, TResult>(keySelector, elementSelector);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector and an element selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the value returned by <paramref name="elementSelector" />.
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="elementSelector"> A transform function to produce a result element value from each element. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type
    /// <typeparamref name="TResult" /> selected from the query.
    /// </returns>
    public Task<Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(
      Func<TElement, TKey> keySelector,
      Func<TElement, TResult> elementSelector,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TResult>>(elementSelector, nameof (elementSelector));
      return this.ToDictionaryAsync<TElement, TKey, TResult>(keySelector, elementSelector, cancellationToken);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function, a comparer, and an element selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the value returned by <paramref name="elementSelector" />.
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="elementSelector"> A transform function to produce a result element value from each element. </param>
    /// <param name="comparer">
    /// An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type
    /// <typeparamref name="TResult" /> selected from the input sequence.
    /// </returns>
    public Task<Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(
      Func<TElement, TKey> keySelector,
      Func<TElement, TResult> elementSelector,
      IEqualityComparer<TKey> comparer)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TResult>>(elementSelector, nameof (elementSelector));
      return this.ToDictionaryAsync<TElement, TKey, TResult>(keySelector, elementSelector, comparer);
    }

    /// <summary>
    /// Creates a <see cref="T:System.Collections.Generic.Dictionary`2" /> from the query by enumerating it asynchronously
    /// according to a specified key selector function, a comparer, and an element selector function.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <typeparam name="TKey">
    /// The type of the key returned by <paramref name="keySelector" /> .
    /// </typeparam>
    /// <typeparam name="TResult">
    /// The type of the value returned by <paramref name="elementSelector" />.
    /// </typeparam>
    /// <param name="keySelector"> A function to extract a key from each element. </param>
    /// <param name="elementSelector"> A transform function to produce a result element value from each element. </param>
    /// <param name="comparer">
    /// An <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> to compare keys.
    /// </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="T:System.Collections.Generic.Dictionary`2" /> that contains values of type
    /// <typeparamref name="TResult" /> selected from the input sequence.
    /// </returns>
    public Task<Dictionary<TKey, TResult>> ToDictionaryAsync<TKey, TResult>(
      Func<TElement, TKey> keySelector,
      Func<TElement, TResult> elementSelector,
      IEqualityComparer<TKey> comparer,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TKey>>(keySelector, nameof (keySelector));
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, TResult>>(elementSelector, nameof (elementSelector));
      return this.ToDictionaryAsync<TElement, TKey, TResult>(keySelector, elementSelector, comparer, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the first element of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the first element in the query result.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> FirstAsync() => this.FirstAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the first element of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the first element in the query result.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> FirstAsync(CancellationToken cancellationToken) => this.FirstAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the first element of the query that satisfies a specified condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the first element in the query result that satisfies a specified condition.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> FirstAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.FirstAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns the first element of the query that satisfies a specified condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the first element in the query result that satisfies a specified condition.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> FirstAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.FirstAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the first element of the query, or a default value if the query result contains no elements.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>default</c> ( <typeparamref name="TElement" /> ) if query result is empty;
    /// otherwise, the first element in the query result.
    /// </returns>
    public Task<TElement> FirstOrDefaultAsync() => this.FirstOrDefaultAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the first element of the query, or a default value if the query result contains no elements.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>default</c> ( <typeparamref name="TElement" /> ) if query result is empty;
    /// otherwise, the first element in the query result.
    /// </returns>
    public Task<TElement> FirstOrDefaultAsync(CancellationToken cancellationToken) => this.FirstOrDefaultAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the first element of the query that satisfies a specified condition
    /// or a default value if no such element is found.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>default</c> ( <typeparamref name="TElement" /> ) if query result is empty
    /// or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element
    /// in the query result that passes the test specified by <paramref name="predicate" /> .
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    public Task<TElement> FirstOrDefaultAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.FirstOrDefaultAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns the first element of the query that satisfies a specified condition
    /// or a default value if no such element is found.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>default</c> ( <typeparamref name="TElement" /> ) if query result is empty
    /// or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element
    /// in the query result that passes the test specified by <paramref name="predicate" /> .
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    public Task<TElement> FirstOrDefaultAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.FirstOrDefaultAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the only element of the query, and throws an exception
    /// if there is not exactly one element in the sequence.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result has more than one element.</exception>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> SingleAsync() => this.SingleAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the only element of the query, and throws an exception
    /// if there is not exactly one element in the sequence.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result has more than one element.</exception>
    /// <exception cref="T:System.InvalidOperationException">The query result is empty.</exception>
    public Task<TElement> SingleAsync(CancellationToken cancellationToken) => this.SingleAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the only element of the query that satisfies a specified condition,
    /// and throws an exception if more than one such element exists.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result that satisfies the condition in
    /// <paramref name="predicate" />.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// No element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// More than one element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    public Task<TElement> SingleAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.SingleAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns the only element of the query that satisfies a specified condition,
    /// and throws an exception if more than one such element exists.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result that satisfies the condition in
    /// <paramref name="predicate" />.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// No element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// More than one element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    public Task<TElement> SingleAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.SingleAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
    /// this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result, or <c>default</c> (<typeparamref name="TElement" />)
    /// if the sequence contains no elements.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result has more than one element.</exception>
    public Task<TElement> SingleOrDefaultAsync() => this.SingleOrDefaultAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the only element of a sequence, or a default value if the sequence is empty;
    /// this method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result, or <c>default</c> (<typeparamref name="TElement" />)
    /// if the sequence contains no elements.
    /// </returns>
    /// <exception cref="T:System.InvalidOperationException">The query result has more than one element.</exception>
    public Task<TElement> SingleOrDefaultAsync(CancellationToken cancellationToken) => this.SingleOrDefaultAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the only element of the query that satisfies a specified condition or
    /// a default value if no such element exists; this method throws an exception if more than one element
    /// satisfies the condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result that satisfies the condition in
    /// <paramref name="predicate" />, or <c>default</c> ( <typeparamref name="TElement" /> ) if no such element is found.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// More than one element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    public Task<TElement> SingleOrDefaultAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.SingleOrDefaultAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns the only element of the query that satisfies a specified condition or
    /// a default value if no such element exists; this method throws an exception if more than one element
    /// satisfies the condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the single element of the query result that satisfies the condition in
    /// <paramref name="predicate" />, or <c>default</c> ( <typeparamref name="TElement" /> ) if no such element is found.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// More than one element satisfies the condition in
    /// <paramref name="predicate" />
    /// .
    /// </exception>
    public Task<TElement> SingleOrDefaultAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.SingleOrDefaultAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously determines whether the query contains a specified element by using the default equality comparer.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="value"> The object to locate in the query result. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the query result contains the specified value; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> ContainsAsync(TElement value) => this.ContainsAsync<TElement>(value);

    /// <summary>
    /// Asynchronously determines whether the query contains a specified element by using the default equality comparer.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="value"> The object to locate in the query result. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the query result contains the specified value; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> ContainsAsync(TElement value, CancellationToken cancellationToken) => this.ContainsAsync<TElement>(value, cancellationToken);

    /// <summary>
    /// Asynchronously determines whether the query contains any elements.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the query result contains any elements; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AnyAsync() => this.AnyAsync<TElement>();

    /// <summary>
    /// Asynchronously determines whether the query contains any elements.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if the query result contains any elements; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AnyAsync(CancellationToken cancellationToken) => this.AnyAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously determines whether any element of the query satisfies a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if any elements in the query result pass the test in the specified predicate; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AnyAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.AnyAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously determines whether any element of the query satisfies a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if any elements in the query result pass the test in the specified predicate; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> AnyAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.AnyAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously determines whether all the elements of the query satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if every element of the query result passes the test in the specified predicate; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    public Task<bool> AllAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.AllAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously determines whether all the elements of the query satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains <c>true</c> if every element of the query result passes the test in the specified predicate; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="predicate" />
    /// is
    /// <c>null</c>
    /// .
    /// </exception>
    public Task<bool> AllAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.AllAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the number of elements in the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result is larger than
    /// <see cref="F:System.Int32.MaxValue" />
    /// .
    /// </exception>
    public Task<int> CountAsync() => this.CountAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the number of elements in the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result is larger than
    /// <see cref="F:System.Int32.MaxValue" />
    /// .
    /// </exception>
    public Task<int> CountAsync(CancellationToken cancellationToken) => this.CountAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the number of elements in the query that satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result that satisfy the condition in the predicate function.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result that satisfy the condition in the predicate function
    /// is larger than
    /// <see cref="F:System.Int32.MaxValue" />
    /// .
    /// </exception>
    public Task<int> CountAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.CountAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns the number of elements in the query that satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result that satisfy the condition in the predicate function.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result that satisfy the condition in the predicate function
    /// is larger than
    /// <see cref="F:System.Int32.MaxValue" />
    /// .
    /// </exception>
    public Task<int> CountAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.CountAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns an <see cref="T:System.Int64" /> that represents the total number of elements in the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result is larger than
    /// <see cref="F:System.Int64.MaxValue" />
    /// .
    /// </exception>
    public Task<long> LongCountAsync() => this.LongCountAsync<TElement>();

    /// <summary>
    /// Asynchronously returns an <see cref="T:System.Int64" /> that represents the total number of elements in the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result is larger than
    /// <see cref="F:System.Int64.MaxValue" />
    /// .
    /// </exception>
    public Task<long> LongCountAsync(CancellationToken cancellationToken) => this.LongCountAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns an <see cref="T:System.Int64" /> that represents the number of elements in the query
    /// that satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result that satisfy the condition in the predicate function.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result that satisfy the condition in the predicate function
    /// is larger than
    /// <see cref="F:System.Int64.MaxValue" />
    /// .
    /// </exception>
    public Task<long> LongCountAsync(Func<TElement, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.LongCountAsync<TElement>(predicate);
    }

    /// <summary>
    /// Asynchronously returns an <see cref="T:System.Int64" /> that represents the number of elements in the query
    /// that satisfy a condition.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="predicate"> A function to test each element for a condition. </param>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the number of elements in the query result that satisfy the condition in the predicate function.
    /// </returns>
    /// <exception cref="T:System.OverflowException">
    /// The number of elements in the query result that satisfy the condition in the predicate function
    /// is larger than
    /// <see cref="F:System.Int64.MaxValue" />
    /// .
    /// </exception>
    public Task<long> LongCountAsync(
      Func<TElement, bool> predicate,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TElement, bool>>(predicate, nameof (predicate));
      return this.LongCountAsync<TElement>(predicate, cancellationToken);
    }

    /// <summary>
    /// Asynchronously returns the minimum value of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the minimum value in the query result.
    /// </returns>
    public Task<TElement> MinAsync() => this.MinAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the minimum value of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the minimum value in the query result.
    /// </returns>
    public Task<TElement> MinAsync(CancellationToken cancellationToken) => this.MinAsync<TElement>(cancellationToken);

    /// <summary>
    /// Asynchronously returns the maximum value of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the maximum value in the query result.
    /// </returns>
    public Task<TElement> MaxAsync() => this.MaxAsync<TElement>();

    /// <summary>
    /// Asynchronously returns the maximum value of the query.
    /// </summary>
    /// <remarks>
    /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
    /// that any asynchronous operations have completed before calling another method on this context.
    /// </remarks>
    /// <param name="cancellationToken">
    /// A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains the maximum value in the query result.
    /// </returns>
    public Task<TElement> MaxAsync(CancellationToken cancellationToken) => this.MaxAsync<TElement>(cancellationToken);

    /// <summary>
    /// Returns a <see cref="T:System.String" /> that contains the SQL string that was set
    /// when the query was created.  The parameters are not included.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String" /> that represents this instance.
    /// </returns>
    public override string ToString() => this._internalQuery != null ? this._internalQuery.ToString() : base.ToString();

    internal InternalSqlQuery InternalQuery => this._internalQuery;

    private InternalSqlQuery GetInternalQueryWithCheck(string memberName) => this._internalQuery != null ? this._internalQuery : throw new NotImplementedException(Strings.TestDoubleNotImplemented((object) memberName, (object) this.GetType().Name, (object) typeof (DbSqlQuery<>).Name));

    /// <summary>
    /// Returns <c>false</c>.
    /// </summary>
    /// <returns>
    /// <c>false</c> .
    /// </returns>
    bool IListSource.ContainsListCollection => false;

    /// <summary>
    /// Throws an exception indicating that binding directly to a store query is not supported.
    /// </summary>
    /// <returns> Never returns; always throws. </returns>
    IList IListSource.GetList() => throw Error.DbQuery_BindingToDbQueryNotSupported();

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
