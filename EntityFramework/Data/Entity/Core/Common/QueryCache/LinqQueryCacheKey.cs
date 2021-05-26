// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.LinqQueryCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal sealed class LinqQueryCacheKey : QueryCacheKey
  {
    private readonly int _hashCode;
    private readonly string _expressionKey;
    private readonly string _parametersToken;
    private readonly int _parameterCount;
    private readonly string _includePathsToken;
    private readonly MergeOption _mergeOption;
    private readonly Type _resultType;
    private readonly bool _streaming;
    private readonly bool _useCSharpNullComparisonBehavior;

    internal LinqQueryCacheKey(
      string expressionKey,
      int parameterCount,
      string parametersToken,
      string includePathsToken,
      MergeOption mergeOption,
      bool streaming,
      bool useCSharpNullComparisonBehavior,
      Type resultType)
    {
      this._expressionKey = expressionKey;
      this._parameterCount = parameterCount;
      this._parametersToken = parametersToken;
      this._includePathsToken = includePathsToken;
      this._mergeOption = mergeOption;
      this._streaming = streaming;
      this._resultType = resultType;
      this._useCSharpNullComparisonBehavior = useCSharpNullComparisonBehavior;
      int num = this._expressionKey.GetHashCode() ^ this._mergeOption.GetHashCode();
      if (this._parametersToken != null)
        num ^= this._parametersToken.GetHashCode();
      if (this._includePathsToken != null)
        num ^= this._includePathsToken.GetHashCode();
      this._hashCode = num ^ this._useCSharpNullComparisonBehavior.GetHashCode();
    }

    public override bool Equals(object otherObject)
    {
      if (typeof (LinqQueryCacheKey) != otherObject.GetType())
        return false;
      LinqQueryCacheKey linqQueryCacheKey = (LinqQueryCacheKey) otherObject;
      return this._parameterCount == linqQueryCacheKey._parameterCount && this._mergeOption == linqQueryCacheKey._mergeOption && (this._streaming == linqQueryCacheKey._streaming && this.Equals(linqQueryCacheKey._expressionKey, this._expressionKey)) && (this.Equals(linqQueryCacheKey._includePathsToken, this._includePathsToken) && this.Equals(linqQueryCacheKey._parametersToken, this._parametersToken) && object.Equals((object) linqQueryCacheKey._resultType, (object) this._resultType)) && object.Equals((object) linqQueryCacheKey._useCSharpNullComparisonBehavior, (object) this._useCSharpNullComparisonBehavior);
    }

    public override int GetHashCode() => this._hashCode;

    public override string ToString() => string.Join("|", new string[5]
    {
      this._expressionKey,
      this._parametersToken,
      this._includePathsToken,
      Enum.GetName(typeof (MergeOption), (object) this._mergeOption),
      this._useCSharpNullComparisonBehavior.ToString()
    });
  }
}
