// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.Check
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.SqlServer.Resources;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal class Check
  {
    public static T NotNull<T>(T value, string parameterName) where T : class => (object) value != null ? value : throw new ArgumentNullException(parameterName);

    public static T? NotNull<T>(T? value, string parameterName) where T : struct => value.HasValue ? value : throw new ArgumentNullException(parameterName);

    public static string NotEmpty(string value, string parameterName) => !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(Strings.ArgumentIsNullOrWhitespace((object) parameterName));
  }
}
