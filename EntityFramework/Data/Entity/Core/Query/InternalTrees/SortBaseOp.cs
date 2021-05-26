﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SortBaseOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class SortBaseOp : RelOp
  {
    private readonly List<SortKey> m_keys;

    internal SortBaseOp(OpType opType)
      : base(opType)
    {
    }

    internal SortBaseOp(OpType opType, List<SortKey> sortKeys)
      : this(opType)
    {
      this.m_keys = sortKeys;
    }

    internal List<SortKey> Keys => this.m_keys;
  }
}
