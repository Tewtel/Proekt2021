﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.ExceptionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core;
using System.Security;
using System.Threading;

namespace System.Data.Entity.Utilities
{
  internal static class ExceptionExtensions
  {
    public static bool IsCatchableExceptionType(this Exception e)
    {
      Type type = e.GetType();
      return type != typeof (StackOverflowException) && type != typeof (OutOfMemoryException) && (type != typeof (ThreadAbortException) && type != typeof (NullReferenceException)) && type != typeof (AccessViolationException) && !typeof (SecurityException).IsAssignableFrom(type);
    }

    public static bool IsCatchableEntityExceptionType(this Exception e)
    {
      Type type = e.GetType();
      return e.IsCatchableExceptionType() && type != typeof (EntityCommandExecutionException) && type != typeof (EntityCommandCompilationException) && type != typeof (EntitySqlException);
    }

    public static bool RequiresContext(this Exception e) => e.IsCatchableExceptionType() && !(e is UpdateException) && !(e is ProviderIncompatibleException);
  }
}
