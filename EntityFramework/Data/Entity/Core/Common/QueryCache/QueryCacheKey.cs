// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.QueryCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal abstract class QueryCacheKey
  {
    protected const int EstimatedParameterStringSize = 20;
    private uint _hitCount;
    protected static StringComparison _stringComparison = StringComparison.Ordinal;

    protected QueryCacheKey() => this._hitCount = 1U;

    public abstract override bool Equals(object obj);

    public abstract override int GetHashCode();

    internal uint HitCount
    {
      get => this._hitCount;
      set => this._hitCount = value;
    }

    internal int AgingIndex { get; set; }

    internal void UpdateHit()
    {
      if (uint.MaxValue == this._hitCount)
        return;
      ++this._hitCount;
    }

    protected virtual bool Equals(string s, string t) => string.Equals(s, t, QueryCacheKey._stringComparison);
  }
}
