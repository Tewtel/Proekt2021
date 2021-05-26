// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Resources.Error
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.CodeDom.Compiler;

namespace System.Data.Entity.SqlServer.Resources
{
  [GeneratedCode("Resources.SqlServer.tt", "1.0.0.0")]
  internal static class Error
  {
    internal static Exception InvalidDatabaseName(object p0) => (Exception) new ArgumentException(Strings.InvalidDatabaseName(p0));

    internal static Exception SqlServerMigrationSqlGenerator_UnknownOperation(
      object p0,
      object p1)
    {
      return (Exception) new InvalidOperationException(Strings.SqlServerMigrationSqlGenerator_UnknownOperation(p0, p1));
    }

    internal static Exception ArgumentOutOfRange(string paramName) => (Exception) new ArgumentOutOfRangeException(paramName);

    internal static Exception NotImplemented() => (Exception) new NotImplementedException();

    internal static Exception NotSupported() => (Exception) new NotSupportedException();
  }
}
