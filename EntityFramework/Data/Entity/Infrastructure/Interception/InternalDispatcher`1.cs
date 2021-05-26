// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.InternalDispatcher`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.Interception
{
  internal class InternalDispatcher<TInterceptor> where TInterceptor : class, IDbInterceptor
  {
    private volatile List<TInterceptor> _interceptors = new List<TInterceptor>();
    private readonly object _lock = new object();

    public void Add(IDbInterceptor interceptor)
    {
      if (!(interceptor is TInterceptor interceptor1))
        return;
      lock (this._lock)
      {
        List<TInterceptor> list = this._interceptors.ToList<TInterceptor>();
        list.Add(interceptor1);
        this._interceptors = list;
      }
    }

    public void Remove(IDbInterceptor interceptor)
    {
      if (!(interceptor is TInterceptor interceptor1))
        return;
      lock (this._lock)
      {
        List<TInterceptor> list = this._interceptors.ToList<TInterceptor>();
        list.Remove(interceptor1);
        this._interceptors = list;
      }
    }

    public TResult Dispatch<TResult>(
      TResult result,
      Func<TResult, TInterceptor, TResult> accumulator)
    {
      return this._interceptors.Count != 0 ? this._interceptors.Aggregate<TInterceptor, TResult>(result, accumulator) : result;
    }

    public void Dispatch(Action<TInterceptor> action)
    {
      if (this._interceptors.Count == 0)
        return;
      this._interceptors.Each<TInterceptor>(action);
    }

    public TResult Dispatch<TInterceptionContext, TResult>(
      TResult result,
      TInterceptionContext interceptionContext,
      Action<TInterceptor, TInterceptionContext> intercept)
      where TInterceptionContext : DbInterceptionContext, IDbMutableInterceptionContext<TResult>
    {
      if (this._interceptors.Count == 0)
        return result;
      interceptionContext.MutableData.SetExecuted(result);
      foreach (TInterceptor interceptor in this._interceptors)
        intercept(interceptor, interceptionContext);
      if (interceptionContext.MutableData.Exception != null)
        throw interceptionContext.MutableData.Exception;
      return interceptionContext.MutableData.Result;
    }

    public void Dispatch<TTarget, TInterceptionContext>(
      TTarget target,
      Action<TTarget, TInterceptionContext> operation,
      TInterceptionContext interceptionContext,
      Action<TInterceptor, TTarget, TInterceptionContext> executing,
      Action<TInterceptor, TTarget, TInterceptionContext> executed)
      where TInterceptionContext : DbInterceptionContext, IDbMutableInterceptionContext
    {
      if (this._interceptors.Count == 0)
      {
        operation(target, interceptionContext);
      }
      else
      {
        foreach (TInterceptor interceptor in this._interceptors)
          executing(interceptor, target, interceptionContext);
        if (!interceptionContext.MutableData.IsExecutionSuppressed)
        {
          try
          {
            operation(target, interceptionContext);
            interceptionContext.MutableData.HasExecuted = true;
          }
          catch (Exception ex)
          {
            interceptionContext.MutableData.SetExceptionThrown(ex);
            foreach (TInterceptor interceptor in this._interceptors)
              executed(interceptor, target, interceptionContext);
            if (interceptionContext.MutableData.Exception == ex)
              throw;
          }
        }
        if (interceptionContext.MutableData.OriginalException == null)
        {
          foreach (TInterceptor interceptor in this._interceptors)
            executed(interceptor, target, interceptionContext);
        }
        if (interceptionContext.MutableData.Exception != null)
          throw interceptionContext.MutableData.Exception;
      }
    }

    public TResult Dispatch<TTarget, TInterceptionContext, TResult>(
      TTarget target,
      Func<TTarget, TInterceptionContext, TResult> operation,
      TInterceptionContext interceptionContext,
      Action<TInterceptor, TTarget, TInterceptionContext> executing,
      Action<TInterceptor, TTarget, TInterceptionContext> executed)
      where TInterceptionContext : DbInterceptionContext, IDbMutableInterceptionContext<TResult>
    {
      if (this._interceptors.Count == 0)
        return operation(target, interceptionContext);
      foreach (TInterceptor interceptor in this._interceptors)
        executing(interceptor, target, interceptionContext);
      if (!interceptionContext.MutableData.IsExecutionSuppressed)
      {
        try
        {
          interceptionContext.MutableData.SetExecuted(operation(target, interceptionContext));
        }
        catch (Exception ex)
        {
          interceptionContext.MutableData.SetExceptionThrown(ex);
          foreach (TInterceptor interceptor in this._interceptors)
            executed(interceptor, target, interceptionContext);
          if (interceptionContext.MutableData.Exception == ex)
            throw;
        }
      }
      if (interceptionContext.MutableData.OriginalException == null)
      {
        foreach (TInterceptor interceptor in this._interceptors)
          executed(interceptor, target, interceptionContext);
      }
      if (interceptionContext.MutableData.Exception != null)
        throw interceptionContext.MutableData.Exception;
      return interceptionContext.MutableData.Result;
    }

    public Task DispatchAsync<TTarget, TInterceptionContext>(
      TTarget target,
      Func<TTarget, TInterceptionContext, CancellationToken, Task> operation,
      TInterceptionContext interceptionContext,
      Action<TInterceptor, TTarget, TInterceptionContext> executing,
      Action<TInterceptor, TTarget, TInterceptionContext> executed,
      CancellationToken cancellationToken)
      where TInterceptionContext : DbInterceptionContext, IDbMutableInterceptionContext
    {
      if (this._interceptors.Count == 0)
        return operation(target, interceptionContext, cancellationToken);
      foreach (TInterceptor interceptor in this._interceptors)
        executing(interceptor, target, interceptionContext);
      Task task = interceptionContext.MutableData.IsExecutionSuppressed ? (Task) Task.FromResult<object>((object) null) : operation(target, interceptionContext, cancellationToken);
      TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
      Action<Task> continuationAction = (Action<Task>) (t =>
      {
        ((TInterceptionContext) interceptionContext).MutableData.TaskStatus = t.Status;
        if (t.IsFaulted)
          ((TInterceptionContext) interceptionContext).MutableData.SetExceptionThrown(t.Exception.InnerException);
        else if (!((TInterceptionContext) interceptionContext).MutableData.IsExecutionSuppressed)
          ((TInterceptionContext) interceptionContext).MutableData.HasExecuted = true;
        try
        {
          foreach (TInterceptor interceptor in this._interceptors)
            executed(interceptor, target, interceptionContext);
        }
        catch (Exception ex)
        {
          ((TInterceptionContext) interceptionContext).MutableData.Exception = ex;
        }
        if (((TInterceptionContext) interceptionContext).MutableData.Exception != null)
          tcs.SetException(((TInterceptionContext) interceptionContext).MutableData.Exception);
        else if (t.IsCanceled)
          tcs.SetCanceled();
        else
          tcs.SetResult((object) null);
      });
      task.ContinueWith(continuationAction, TaskContinuationOptions.ExecuteSynchronously);
      return (Task) tcs.Task;
    }

    public Task<TResult> DispatchAsync<TTarget, TInterceptionContext, TResult>(
      TTarget target,
      Func<TTarget, TInterceptionContext, CancellationToken, Task<TResult>> operation,
      TInterceptionContext interceptionContext,
      Action<TInterceptor, TTarget, TInterceptionContext> executing,
      Action<TInterceptor, TTarget, TInterceptionContext> executed,
      CancellationToken cancellationToken)
      where TInterceptionContext : DbInterceptionContext, IDbMutableInterceptionContext<TResult>
    {
      cancellationToken.ThrowIfCancellationRequested();
      if (this._interceptors.Count == 0)
        return operation(target, interceptionContext, cancellationToken);
      foreach (TInterceptor interceptor in this._interceptors)
        executing(interceptor, target, interceptionContext);
      Task<TResult> task = interceptionContext.MutableData.IsExecutionSuppressed ? Task.FromResult<TResult>(interceptionContext.MutableData.Result) : operation(target, interceptionContext, cancellationToken);
      TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
      Action<Task<TResult>> continuationAction = (Action<Task<TResult>>) (t =>
      {
        ((TInterceptionContext) interceptionContext).MutableData.TaskStatus = t.Status;
        if (t.IsFaulted)
          ((TInterceptionContext) interceptionContext).MutableData.SetExceptionThrown(t.Exception.InnerException);
        else if (!((TInterceptionContext) interceptionContext).MutableData.IsExecutionSuppressed)
          ((TInterceptionContext) interceptionContext).MutableData.SetExecuted(t.IsCanceled || t.IsFaulted ? default (TResult) : t.Result);
        try
        {
          foreach (TInterceptor interceptor in this._interceptors)
            executed(interceptor, target, interceptionContext);
        }
        catch (Exception ex)
        {
          ((TInterceptionContext) interceptionContext).MutableData.Exception = ex;
        }
        if (((TInterceptionContext) interceptionContext).MutableData.Exception != null)
          tcs.SetException(((TInterceptionContext) interceptionContext).MutableData.Exception);
        else if (t.IsCanceled)
          tcs.SetCanceled();
        else
          tcs.SetResult(((TInterceptionContext) interceptionContext).MutableData.Result);
      });
      task.ContinueWith(continuationAction, TaskContinuationOptions.ExecuteSynchronously);
      return tcs.Task;
    }
  }
}
