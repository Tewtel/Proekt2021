// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbExecutionStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// A strategy that is used to execute a command or query against the database, possibly with logic to retry when a failure occurs.
  /// </summary>
  public interface IDbExecutionStrategy
  {
    /// <summary>
    /// Indicates whether this <see cref="T:System.Data.Entity.Infrastructure.IDbExecutionStrategy" /> might retry the execution after a failure.
    /// </summary>
    bool RetriesOnFailure { get; }

    /// <summary>Executes the specified operation.</summary>
    /// <param name="operation">A delegate representing an executable operation that doesn't return any results.</param>
    void Execute(Action operation);

    /// <summary>
    /// Executes the specified operation and returns the result.
    /// </summary>
    /// <typeparam name="TResult">
    /// The return type of <paramref name="operation" />.
    /// </typeparam>
    /// <param name="operation">
    /// A delegate representing an executable operation that returns the result of type <typeparamref name="TResult" />.
    /// </param>
    /// <returns>The result from the operation.</returns>
    TResult Execute<TResult>(Func<TResult> operation);

    /// <summary>Executes the specified asynchronous operation.</summary>
    /// <param name="operation">A function that returns a started task.</param>
    /// <param name="cancellationToken">
    /// A cancellation token used to cancel the retry operation, but not operations that are already in flight
    /// or that already completed successfully.
    /// </param>
    /// <returns>
    /// A task that will run to completion if the original task completes successfully (either the
    /// first time or after retrying transient failures). If the task fails with a non-transient error or
    /// the retry limit is reached, the returned task will become faulted and the exception must be observed.
    /// </returns>
    Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken);

    /// <summary>
    /// Executes the specified asynchronous operation and returns the result.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result type of the <see cref="T:System.Threading.Tasks.Task`1" /> returned by <paramref name="operation" />.
    /// </typeparam>
    /// <param name="operation">
    /// A function that returns a started task of type <typeparamref name="TResult" />.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token used to cancel the retry operation, but not operations that are already in flight
    /// or that already completed successfully.
    /// </param>
    /// <returns>
    /// A task that will run to completion if the original task completes successfully (either the
    /// first time or after retrying transient failures). If the task fails with a non-transient error or
    /// the retry limit is reached, the returned task will become faulted and the exception must be observed.
    /// </returns>
    Task<TResult> ExecuteAsync<TResult>(
      Func<Task<TResult>> operation,
      CancellationToken cancellationToken);
  }
}
