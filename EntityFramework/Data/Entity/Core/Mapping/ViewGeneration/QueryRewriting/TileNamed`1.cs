// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting.TileNamed`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.ViewGeneration.QueryRewriting
{
  internal class TileNamed<T_Query> : Tile<T_Query> where T_Query : ITileQuery
  {
    public TileNamed(T_Query namedQuery)
      : base(TileOpKind.Named, namedQuery)
    {
    }

    public T_Query NamedQuery => this.Query;

    public override Tile<T_Query> Arg1 => (Tile<T_Query>) null;

    public override Tile<T_Query> Arg2 => (Tile<T_Query>) null;

    public override string Description => this.Query.Description;

    public override string ToString() => this.Query.ToString();

    internal override Tile<T_Query> Replace(Tile<T_Query> oldTile, Tile<T_Query> newTile) => this != oldTile ? (Tile<T_Query>) this : newTile;
  }
}
