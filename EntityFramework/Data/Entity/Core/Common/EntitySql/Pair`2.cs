// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.Pair`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class Pair<L, R>
  {
    internal L Left;
    internal R Right;

    internal Pair(L left, R right)
    {
      this.Left = left;
      this.Right = right;
    }

    internal KeyValuePair<L, R> GetKVP() => new KeyValuePair<L, R>(this.Left, this.Right);
  }
}
