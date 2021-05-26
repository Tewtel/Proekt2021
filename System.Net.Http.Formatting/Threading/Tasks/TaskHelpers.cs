// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskHelpers
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Runtime.InteropServices;

namespace System.Threading.Tasks
{
  internal static class TaskHelpers
  {
    private static readonly Task _defaultCompleted = (Task) Task.FromResult<TaskHelpers.AsyncVoid>(new TaskHelpers.AsyncVoid());
    private static readonly Task<object> _completedTaskReturningNull = Task.FromResult<object>((object) null);

    internal static Task Canceled() => (Task) TaskHelpers.CancelCache<TaskHelpers.AsyncVoid>.Canceled;

    internal static Task<TResult> Canceled<TResult>() => TaskHelpers.CancelCache<TResult>.Canceled;

    internal static Task Completed() => TaskHelpers._defaultCompleted;

    internal static Task FromError(Exception exception) => (Task) TaskHelpers.FromError<TaskHelpers.AsyncVoid>(exception);

    internal static Task<TResult> FromError<TResult>(Exception exception)
    {
      TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
      completionSource.SetException(exception);
      return completionSource.Task;
    }

    internal static Task<object> NullResult() => TaskHelpers._completedTaskReturningNull;

    [StructLayout(LayoutKind.Sequential, Size = 1)]
    private struct AsyncVoid
    {
    }

    private static class CancelCache<TResult>
    {
      public static readonly Task<TResult> Canceled = TaskHelpers.CancelCache<TResult>.GetCancelledTask();

      private static Task<TResult> GetCancelledTask()
      {
        TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>();
        completionSource.SetCanceled();
        return completionSource.Task;
      }
    }
  }
}
