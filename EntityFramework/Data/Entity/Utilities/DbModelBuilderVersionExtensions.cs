// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DbModelBuilderVersionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Utilities
{
  internal static class DbModelBuilderVersionExtensions
  {
    public static double GetEdmVersion(this DbModelBuilderVersion modelBuilderVersion)
    {
      switch (modelBuilderVersion)
      {
        case DbModelBuilderVersion.Latest:
        case DbModelBuilderVersion.V5_0:
        case DbModelBuilderVersion.V6_0:
          return 3.0;
        case DbModelBuilderVersion.V4_1:
        case DbModelBuilderVersion.V5_0_Net4:
          return 2.0;
        default:
          throw new ArgumentOutOfRangeException(nameof (modelBuilderVersion));
      }
    }

    public static bool IsEF6OrHigher(this DbModelBuilderVersion modelBuilderVersion) => modelBuilderVersion >= DbModelBuilderVersion.V6_0 || modelBuilderVersion == DbModelBuilderVersion.Latest;
  }
}
