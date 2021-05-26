﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.History.LegacyHistoryContext
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;

namespace System.Data.Entity.Migrations.History
{
  internal sealed class LegacyHistoryContext : DbContext
  {
    public LegacyHistoryContext(DbConnection existingConnection)
      : base(existingConnection, false)
    {
      this.InternalContext.InitializerDisabled = true;
    }

    public IDbSet<LegacyHistoryRow> History { get; set; }
  }
}
