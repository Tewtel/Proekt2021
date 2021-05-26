﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting.TileProcessor`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting
{
  internal abstract class TileProcessor<T_Tile>
  {
    internal abstract bool IsEmpty(T_Tile tile);

    internal abstract T_Tile Union(T_Tile a, T_Tile b);

    internal abstract T_Tile Join(T_Tile a, T_Tile b);

    internal abstract T_Tile AntiSemiJoin(T_Tile a, T_Tile b);

    internal abstract T_Tile GetArg1(T_Tile tile);

    internal abstract T_Tile GetArg2(T_Tile tile);

    internal abstract TileOpKind GetOpKind(T_Tile tile);
  }
}
