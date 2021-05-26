// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.EntitySqlQueryCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects;

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal sealed class EntitySqlQueryCacheKey : QueryCacheKey
  {
    private readonly int _hashCode;
    private readonly string _defaultContainer;
    private readonly string _eSqlStatement;
    private readonly string _parametersToken;
    private readonly int _parameterCount;
    private readonly string _includePathsToken;
    private readonly MergeOption _mergeOption;
    private readonly Type _resultType;
    private readonly bool _streaming;

    internal EntitySqlQueryCacheKey(
      string defaultContainerName,
      string eSqlStatement,
      int parameterCount,
      string parametersToken,
      string includePathsToken,
      MergeOption mergeOption,
      bool streaming,
      Type resultType)
    {
      this._defaultContainer = defaultContainerName;
      this._eSqlStatement = eSqlStatement;
      this._parameterCount = parameterCount;
      this._parametersToken = parametersToken;
      this._includePathsToken = includePathsToken;
      this._mergeOption = mergeOption;
      this._streaming = streaming;
      this._resultType = resultType;
      int num = this._eSqlStatement.GetHashCode() ^ this._mergeOption.GetHashCode();
      if (this._parametersToken != null)
        num ^= this._parametersToken.GetHashCode();
      if (this._includePathsToken != null)
        num ^= this._includePathsToken.GetHashCode();
      if (this._defaultContainer != null)
        num ^= this._defaultContainer.GetHashCode();
      this._hashCode = num;
    }

    public override bool Equals(object otherObject)
    {
      if (typeof (EntitySqlQueryCacheKey) != otherObject.GetType())
        return false;
      EntitySqlQueryCacheKey sqlQueryCacheKey = (EntitySqlQueryCacheKey) otherObject;
      return this._parameterCount == sqlQueryCacheKey._parameterCount && this._mergeOption == sqlQueryCacheKey._mergeOption && (this._streaming == sqlQueryCacheKey._streaming && this.Equals(sqlQueryCacheKey._defaultContainer, this._defaultContainer)) && (this.Equals(sqlQueryCacheKey._eSqlStatement, this._eSqlStatement) && this.Equals(sqlQueryCacheKey._includePathsToken, this._includePathsToken) && this.Equals(sqlQueryCacheKey._parametersToken, this._parametersToken)) && object.Equals((object) sqlQueryCacheKey._resultType, (object) this._resultType);
    }

    public override int GetHashCode() => this._hashCode;

    public override string ToString() => string.Join("|", new string[5]
    {
      this._defaultContainer,
      this._eSqlStatement,
      this._parametersToken,
      this._includePathsToken,
      Enum.GetName(typeof (MergeOption), (object) this._mergeOption)
    });
  }
}
