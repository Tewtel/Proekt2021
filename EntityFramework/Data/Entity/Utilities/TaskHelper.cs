// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.TaskHelper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Threading.Tasks;

namespace System.Data.Entity.Utilities
{
  internal static class TaskHelper
  {
    internal static Task<T> FromException<T>(Exception ex)
    {
      TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
      completionSource.SetException(ex);
      return completionSource.Task;
    }

    internal static Task<T> FromCancellation<T>()
    {
      TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
      completionSource.SetCanceled();
      return completionSource.Task;
    }
  }
}
