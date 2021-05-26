// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting.TileQueryProcessor`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting
{
  internal abstract class TileQueryProcessor<T_Query> where T_Query : ITileQuery
  {
    internal abstract T_Query Intersect(T_Query arg1, T_Query arg2);

    internal abstract T_Query Difference(T_Query arg1, T_Query arg2);

    internal abstract T_Query Union(T_Query arg1, T_Query arg2);

    internal abstract bool IsSatisfiable(T_Query query);

    internal abstract T_Query CreateDerivedViewBySelectingConstantAttributes(T_Query query);
  }
}
