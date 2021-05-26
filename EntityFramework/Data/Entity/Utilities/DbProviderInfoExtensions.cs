// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbProviderInfoExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Utilities
{
  internal static class DbProviderInfoExtensions
  {
    public static bool IsSqlCe(this DbProviderInfo providerInfo) => !string.IsNullOrWhiteSpace(providerInfo.ProviderInvariantName) && providerInfo.ProviderInvariantName.StartsWith("System.Data.SqlServerCe", StringComparison.OrdinalIgnoreCase);
  }
}
