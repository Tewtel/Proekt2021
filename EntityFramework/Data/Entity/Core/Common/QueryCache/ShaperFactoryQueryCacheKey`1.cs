// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.ShaperFactoryQueryCacheKey`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal class ShaperFactoryQueryCacheKey<T> : QueryCacheKey
  {
    private readonly string _columnMapKey;
    private readonly MergeOption _mergeOption;
    private readonly bool _isValueLayer;
    private readonly bool _streaming;

    internal ShaperFactoryQueryCacheKey(
      string columnMapKey,
      MergeOption mergeOption,
      bool streaming,
      bool isValueLayer)
    {
      this._columnMapKey = columnMapKey;
      this._mergeOption = mergeOption;
      this._isValueLayer = isValueLayer;
      this._streaming = streaming;
    }

    public override bool Equals(object obj) => obj is ShaperFactoryQueryCacheKey<T> factoryQueryCacheKey && this._columnMapKey.Equals(factoryQueryCacheKey._columnMapKey, QueryCacheKey._stringComparison) && (this._mergeOption == factoryQueryCacheKey._mergeOption && this._isValueLayer == factoryQueryCacheKey._isValueLayer) && this._streaming == factoryQueryCacheKey._streaming;

    public override int GetHashCode() => this._columnMapKey.GetHashCode();
  }
}
