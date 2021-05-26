﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.IDbSpatialValue
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

namespace System.Data.Entity.SqlServer
{
  internal interface IDbSpatialValue
  {
    bool IsGeography { get; }

    object ProviderValue { get; }

    int? CoordinateSystemId { get; }

    string WellKnownText { get; }

    byte[] WellKnownBinary { get; }

    string GmlString { get; }

    Exception NotSqlCompatible();
  }
}
