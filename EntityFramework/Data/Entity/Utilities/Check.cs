// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.Check
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Utilities
{
  internal class Check
  {
    public static T NotNull<T>(T value, string parameterName) where T : class => (object) value != null ? value : throw new ArgumentNullException(parameterName);

    public static T? NotNull<T>(T? value, string parameterName) where T : struct => value.HasValue ? value : throw new ArgumentNullException(parameterName);

    public static string NotEmpty(string value, string parameterName) => !string.IsNullOrWhiteSpace(value) ? value : throw new ArgumentException(Strings.ArgumentIsNullOrWhitespace((object) parameterName));
  }
}
