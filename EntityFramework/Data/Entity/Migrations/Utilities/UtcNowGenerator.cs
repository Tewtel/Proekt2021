// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Utilities.UtcNowGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Globalization;
using System.Threading;

namespace System.Data.Entity.Migrations.Utilities
{
  internal static class UtcNowGenerator
  {
    public const string MigrationIdFormat = "yyyyMMddHHmmssf";
    private static readonly ThreadLocal<DateTime> _lastNow = new ThreadLocal<DateTime>((Func<DateTime>) (() => DateTime.UtcNow));

    public static DateTime UtcNow()
    {
      DateTime dateTime1 = DateTime.UtcNow;
      DateTime dateTime2 = UtcNowGenerator._lastNow.Value;
      if (dateTime1 <= dateTime2 || dateTime1.ToString("yyyyMMddHHmmssf", (IFormatProvider) CultureInfo.InvariantCulture).Equals(dateTime2.ToString("yyyyMMddHHmmssf", (IFormatProvider) CultureInfo.InvariantCulture), StringComparison.Ordinal))
        dateTime1 = dateTime2.AddMilliseconds(100.0);
      UtcNowGenerator._lastNow.Value = dateTime1;
      return dateTime1;
    }

    public static string UtcNowAsMigrationIdTimestamp() => UtcNowGenerator.UtcNow().ToString("yyyyMMddHHmmssf", (IFormatProvider) CultureInfo.InvariantCulture);
  }
}
