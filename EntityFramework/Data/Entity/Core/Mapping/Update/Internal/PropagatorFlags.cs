﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.PropagatorFlags
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  [Flags]
  internal enum PropagatorFlags : byte
  {
    NoFlags = 0,
    Preserve = 1,
    ConcurrencyValue = 2,
    Unknown = 8,
    Key = 16, // 0x10
    ForeignKey = 32, // 0x20
  }
}
