// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskHelpersExtensions
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

namespace System.Threading.Tasks
{
  internal static class TaskHelpersExtensions
  {
    internal static async Task<object> CastToObject(this Task task)
    {
      await task;
      object obj;
      return obj;
    }

    internal static async Task<object> CastToObject<T>(this Task<T> task) => (object) await task;

    internal static void ThrowIfFaulted(this Task task) => task.GetAwaiter().GetResult();

    internal static bool TryGetResult<TResult>(this Task<TResult> task, out TResult result)
    {
      if (task.Status == TaskStatus.RanToCompletion)
      {
        result = task.Result;
        return true;
      }
      result = default (TResult);
      return false;
    }
  }
}
