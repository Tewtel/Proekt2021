// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.DebugCheck
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Diagnostics;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal class DebugCheck
  {
    [Conditional("DEBUG")]
    public static void NotNull<T>(T value) where T : class
    {
    }

    [Conditional("DEBUG")]
    public static void NotNull<T>(T? value) where T : struct
    {
    }

    [Conditional("DEBUG")]
    public static void NotEmpty(string value)
    {
    }
  }
}
