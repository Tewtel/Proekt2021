// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbExecutionStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Resources;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Provides the base implementation of the retry mechanism for unreliable operations and transient conditions that uses
  /// exponentially increasing delays between retries.
  /// </summary>
  /// <remarks>
  /// A new instance will be created each time an operation is executed.
  /// The following formula is used to calculate the delay after <c>retryCount</c> number of attempts:
  /// <code>min(random(1, 1.1) * (2 ^ retryCount - 1), maxDelay)</code>
  /// The <c>retryCount</c> starts at 0.
  /// The random factor distributes uniformly the retry attempts from multiple simultaneous operations failing simultaneously.
  /// </remarks>
  public abstract class DbExecutionStrategy : IDbExecutionStrategy
  {
    private readonly List<Exception> _exceptionsEncountered = new List<Exception>();
    private readonly Random _random = new Random();
    private readonly int _maxRetryCount;
    private readonly TimeSpan _maxDelay;
    private const string ContextName = "ExecutionStrategySuspended";
    private const int DefaultMaxRetryCount = 5;
    private const double DefaultRandomFactor = 1.1;
    private const double DefaultExponentialBase = 2.0;
    private static readonly TimeSpan DefaultCoefficient = TimeSpan.FromSeconds(1.0);
    private static readonly TimeSpan DefaultMaxDelay = TimeSpan.FromSeconds(30.0);

    /// <summary>
    /// Creates a new instance of <see cref="T:System.Data.Entity.Infrastructure.DbExecutionStrategy" />.
    /// </summary>
    /// <remarks>
    /// The default retry limit is 5, which means that the total amount of time spent between retries is 26 seconds plus the random factor.
    /// </remarks>
    protected DbExecutionStrategy()
      : this(5, DbExecutionStrategy.DefaultMaxDelay)
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="T:System.Data.Entity.Infrastructure.DbExecutionStrategy" /> with the specified limits for number of retries and the delay between retries.
    /// </summary>
    /// <param name="maxRetryCount"> The maximum number of retry attempts. </param>
    /// <param name="maxDelay"> The maximum delay in milliseconds between retries. </param>
    protected DbExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
    {
      if (maxRetryCount < 0)
        throw new ArgumentOutOfRangeException(nameof (maxRetryCount));
      if (maxDelay.TotalMilliseconds < 0.0)
        throw new ArgumentOutOfRangeException(nameof (maxDelay));
      this._maxRetryCount = maxRetryCount;
      this._maxDelay = maxDelay;
    }

    /// <summary>
    /// Returns <c>true</c> to indicate that <see cref="T:System.Data.Entity.Infrastructure.DbExecutionStrategy" /> might retry the execution after a failure.
    /// </summary>
    public bool RetriesOnFailure => !DbExecutionStrategy.Suspended;

    /// <summary>
    ///     Indicates whether the strategy is suspended. The strategy is typically suspending while executing to avoid
    ///     recursive execution from nested operations.
    /// </summary>
    protected internal static bool Suspended
    {
      get => ((bool?) CallContext.LogicalGetData("ExecutionStrategySuspended")).GetValueOrDefault();
      set => CallContext.LogicalSetData("ExecutionStrategySuspended", (object) value);
    }

    /// <summary>
    /// Repetitively executes the specified operation while it satisfies the current retry policy.
    /// </summary>
    /// <param name="operation">A delegate representing an executable operation that doesn't return any results.</param>
    /// <exception cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException">if the retry delay strategy determines the operation shouldn't be retried anymore</exception>
    /// <exception cref="T:System.InvalidOperationException">if an existing transaction is detected and the execution strategy doesn't support it</exception>
    /// <exception cref="T:System.InvalidOperationException">if this instance was already used to execute an operation</exception>
    public void Execute(Action operation)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action>(operation, nameof (operation));
      this.Execute<object>((Func<object>) (() =>
      {
        operation();
        return (object) null;
      }));
    }

    /// <summary>
    /// Repetitively executes the specified operation while it satisfies the current retry policy.
    /// </summary>
    /// <typeparam name="TResult">The type of result expected from the executable operation.</typeparam>
    /// <param name="operation">
    /// A delegate representing an executable operation that returns the result of type <typeparamref name="TResult" />.
    /// </param>
    /// <returns>The result from the operation.</returns>
    /// <exception cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException">if the retry delay strategy determines the operation shouldn't be retried anymore</exception>
    /// <exception cref="T:System.InvalidOperationException">if an existing transaction is detected and the execution strategy doesn't support it</exception>
    /// <exception cref="T:System.InvalidOperationException">if this instance was already used to execute an operation</exception>
    public TResult Execute<TResult>(Func<TResult> operation)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<TResult>>(operation, nameof (operation));
      if (this.RetriesOnFailure)
      {
        this.EnsurePreexecutionState();
        TimeSpan? nextDelay;
        while (true)
        {
          try
          {
            DbExecutionStrategy.Suspended = true;
            return operation();
          }
          catch (Exception ex)
          {
            if (!DbExecutionStrategy.UnwrapAndHandleException<bool>(ex, new Func<Exception, bool>(this.ShouldRetryOn)))
            {
              throw;
            }
            else
            {
              nextDelay = this.GetNextDelay(ex);
              if (!nextDelay.HasValue)
                throw new RetryLimitExceededException(Strings.ExecutionStrategy_RetryLimitExceeded((object) this._maxRetryCount, (object) this.GetType().Name), ex);
            }
          }
          finally
          {
            DbExecutionStrategy.Suspended = false;
          }
          TimeSpan? nullable = nextDelay;
          TimeSpan zero = TimeSpan.Zero;
          if ((nullable.HasValue ? (nullable.GetValueOrDefault() < zero ? 1 : 0) : 0) == 0)
            Thread.Sleep(nextDelay.Value);
          else
            break;
        }
        throw new InvalidOperationException(Strings.ExecutionStrategy_NegativeDelay((object) nextDelay));
      }
      return operation();
    }

    /// <summary>
    /// Repetitively executes the specified asynchronous operation while it satisfies the current retry policy.
    /// </summary>
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
    /// <exception cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException">if the retry delay strategy determines the operation shouldn't be retried anymore</exception>
    /// <exception cref="T:System.InvalidOperationException">if an existing transaction is detected and the execution strategy doesn't support it</exception>
    /// <exception cref="T:System.InvalidOperationException">if this instance was already used to execute an operation</exception>
    public Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<Task>>(operation, nameof (operation));
      if (this.RetriesOnFailure)
        this.EnsurePreexecutionState();
      cancellationToken.ThrowIfCancellationRequested();
      return (Task) this.ProtectedExecuteAsync<bool>((Func<Task<bool>>) (async () =>
      {
        await operation().WithCurrentCulture();
        return true;
      }), cancellationToken);
    }

    /// <summary>
    /// Repeatedly executes the specified asynchronous operation while it satisfies the current retry policy.
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
    /// <exception cref="T:System.Data.Entity.Infrastructure.RetryLimitExceededException">if the retry delay strategy determines the operation shouldn't be retried anymore</exception>
    /// <exception cref="T:System.InvalidOperationException">if an existing transaction is detected and the execution strategy doesn't support it</exception>
    /// <exception cref="T:System.InvalidOperationException">if this instance was already used to execute an operation</exception>
    public Task<TResult> ExecuteAsync<TResult>(
      Func<Task<TResult>> operation,
      CancellationToken cancellationToken)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<Task<TResult>>>(operation, nameof (operation));
      if (this.RetriesOnFailure)
        this.EnsurePreexecutionState();
      cancellationToken.ThrowIfCancellationRequested();
      return this.ProtectedExecuteAsync<TResult>(operation, cancellationToken);
    }

    private async Task<TResult> ProtectedExecuteAsync<TResult>(
      Func<Task<TResult>> operation,
      CancellationToken cancellationToken)
    {
      DbExecutionStrategy executionStrategy = this;
      // ISSUE: explicit non-virtual call
      if (!__nonvirtual (executionStrategy.RetriesOnFailure))
        return await operation().WithCurrentCulture<TResult>();
      TimeSpan? nextDelay;
      while (true)
      {
        try
        {
          DbExecutionStrategy.Suspended = true;
          return await operation().WithCurrentCulture<TResult>();
        }
        catch (Exception ex)
        {
          if (!DbExecutionStrategy.UnwrapAndHandleException<bool>(ex, new Func<Exception, bool>(executionStrategy.ShouldRetryOn)))
          {
            throw;
          }
          else
          {
            nextDelay = executionStrategy.GetNextDelay(ex);
            if (!nextDelay.HasValue)
              throw new RetryLimitExceededException(Strings.ExecutionStrategy_RetryLimitExceeded((object) executionStrategy._maxRetryCount, (object) executionStrategy.GetType().Name), ex);
          }
        }
        finally
        {
          DbExecutionStrategy.Suspended = false;
        }
        TimeSpan? nullable = nextDelay;
        TimeSpan zero = TimeSpan.Zero;
        if ((nullable.HasValue ? (nullable.GetValueOrDefault() < zero ? 1 : 0) : 0) == 0)
          await Task.Delay(nextDelay.Value, cancellationToken).WithCurrentCulture();
        else
          break;
      }
      throw new InvalidOperationException(Strings.ExecutionStrategy_NegativeDelay((object) nextDelay));
    }

    private void EnsurePreexecutionState()
    {
      if (Transaction.Current != (Transaction) null)
        throw new InvalidOperationException(Strings.ExecutionStrategy_ExistingTransaction((object) this.GetType().Name));
      this._exceptionsEncountered.Clear();
    }

    /// <summary>
    /// Determines whether the operation should be retried and the delay before the next attempt.
    /// </summary>
    /// <param name="lastException">The exception thrown during the last execution attempt.</param>
    /// <returns>
    /// Returns the delay indicating how long to wait for before the next execution attempt if the operation should be retried;
    /// <c>null</c> otherwise
    /// </returns>
    protected internal virtual TimeSpan? GetNextDelay(Exception lastException)
    {
      this._exceptionsEncountered.Add(lastException);
      int num1 = this._exceptionsEncountered.Count - 1;
      if (num1 >= this._maxRetryCount)
        return new TimeSpan?();
      double num2 = (Math.Pow(2.0, (double) num1) - 1.0) * (1.0 + this._random.NextDouble() * 0.1);
      TimeSpan timeSpan = DbExecutionStrategy.DefaultCoefficient;
      double val1 = timeSpan.TotalMilliseconds * num2;
      timeSpan = this._maxDelay;
      double totalMilliseconds = timeSpan.TotalMilliseconds;
      return new TimeSpan?(TimeSpan.FromMilliseconds(Math.Min(val1, totalMilliseconds)));
    }

    /// <summary>
    /// Recursively gets InnerException from <paramref name="exception" /> as long as it's an
    /// <see cref="T:System.Data.Entity.Core.EntityException" />, <see cref="T:System.Data.Entity.Infrastructure.DbUpdateException" /> or <see cref="T:System.Data.Entity.Core.UpdateException" />
    /// and passes it to <paramref name="exceptionHandler" />
    /// </summary>
    /// <typeparam name="T">The type of the unwrapped exception.</typeparam>
    /// <param name="exception"> The exception to be unwrapped. </param>
    /// <param name="exceptionHandler"> A delegate that will be called with the unwrapped exception. </param>
    /// <returns>
    /// The result from <paramref name="exceptionHandler" />.
    /// </returns>
    public static T UnwrapAndHandleException<T>(
      Exception exception,
      Func<Exception, T> exceptionHandler)
    {
      switch (exception)
      {
        case EntityException entityException:
          return DbExecutionStrategy.UnwrapAndHandleException<T>(entityException.InnerException, exceptionHandler);
        case DbUpdateException dbUpdateException:
          return DbExecutionStrategy.UnwrapAndHandleException<T>(dbUpdateException.InnerException, exceptionHandler);
        case UpdateException updateException:
          return DbExecutionStrategy.UnwrapAndHandleException<T>(updateException.InnerException, exceptionHandler);
        default:
          return exceptionHandler(exception);
      }
    }

    /// <summary>
    /// Determines whether the specified exception represents a transient failure that can be compensated by a retry.
    /// </summary>
    /// <param name="exception">The exception object to be verified.</param>
    /// <returns>
    /// <c>true</c> if the specified exception is considered as transient, otherwise <c>false</c>.
    /// </returns>
    protected internal abstract bool ShouldRetryOn(Exception exception);
  }
}
