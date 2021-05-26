// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.IInternalConnection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal interface IInternalConnection : IDisposable
  {
    DbConnection Connection { get; }

    string ConnectionKey { get; }

    bool ConnectionHasModel { get; }

    DbConnectionStringOrigin ConnectionStringOrigin { get; }

    AppConfig AppConfig { get; set; }

    string ProviderName { get; set; }

    string ConnectionStringName { get; }

    string OriginalConnectionString { get; }

    ObjectContext CreateObjectContextFromConnectionModel();
  }
}
