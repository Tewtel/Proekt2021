﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityClient.Internal.IEntityAdapter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.EntityClient.Internal
{
  internal interface IEntityAdapter
  {
    DbConnection Connection { get; set; }

    bool AcceptChangesDuringUpdate { get; set; }

    int? CommandTimeout { get; set; }

    int Update();

    Task<int> UpdateAsync(CancellationToken cancellationToken);
  }
}
