// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.QueryCache.EntityClientCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Internal;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;

namespace System.Data.Entity.Core.Common.QueryCache
{
  internal sealed class EntityClientCacheKey : QueryCacheKey
  {
    private readonly CommandType _commandType;
    private readonly string _eSqlStatement;
    private readonly string _parametersToken;
    private readonly int _parameterCount;
    private readonly int _hashCode;

    internal EntityClientCacheKey(EntityCommand entityCommand)
    {
      this._commandType = entityCommand.CommandType;
      this._eSqlStatement = entityCommand.CommandText;
      this._parametersToken = EntityClientCacheKey.GetParametersToken(entityCommand);
      this._parameterCount = entityCommand.Parameters.Count;
      this._hashCode = this._commandType.GetHashCode() ^ this._eSqlStatement.GetHashCode() ^ this._parametersToken.GetHashCode();
    }

    public override bool Equals(object otherObject)
    {
      if (typeof (EntityClientCacheKey) != otherObject.GetType())
        return false;
      EntityClientCacheKey entityClientCacheKey = (EntityClientCacheKey) otherObject;
      return this._commandType == entityClientCacheKey._commandType && this._parameterCount == entityClientCacheKey._parameterCount && this.Equals(entityClientCacheKey._eSqlStatement, this._eSqlStatement) && this.Equals(entityClientCacheKey._parametersToken, this._parametersToken);
    }

    public override int GetHashCode() => this._hashCode;

    private static string GetTypeUsageToken(TypeUsage type) => type != DbTypeMap.AnsiString ? (type != DbTypeMap.AnsiStringFixedLength ? (type != DbTypeMap.String ? (type != DbTypeMap.StringFixedLength ? (type != DbTypeMap.Xml ? (!TypeSemantics.IsEnumerationType(type) ? type.EdmType.Name : type.EdmType.FullName) : "String") : "StringFixedLength") : "String") : "AnsiStringFixedLength") : "AnsiString";

    private static string GetParametersToken(EntityCommand entityCommand)
    {
      if (entityCommand.Parameters == null || entityCommand.Parameters.Count == 0)
        return "@@0";
      Dictionary<string, TypeUsage> parameterTypeUsage = entityCommand.GetParameterTypeUsage();
      if (1 == parameterTypeUsage.Count)
        return "@@1:" + entityCommand.Parameters[0].ParameterName + ":" + EntityClientCacheKey.GetTypeUsageToken(parameterTypeUsage[entityCommand.Parameters[0].ParameterName]);
      StringBuilder stringBuilder = new StringBuilder(entityCommand.Parameters.Count * 20);
      stringBuilder.Append("@@");
      stringBuilder.Append(entityCommand.Parameters.Count);
      stringBuilder.Append(":");
      string str = "";
      foreach (KeyValuePair<string, TypeUsage> keyValuePair in parameterTypeUsage)
      {
        stringBuilder.Append(str);
        stringBuilder.Append(keyValuePair.Key);
        stringBuilder.Append(":");
        stringBuilder.Append(EntityClientCacheKey.GetTypeUsageToken(keyValuePair.Value));
        str = ";";
      }
      return stringBuilder.ToString();
    }

    public override string ToString() => string.Join("|", new string[3]
    {
      Enum.GetName(typeof (CommandType), (object) this._commandType),
      this._eSqlStatement,
      this._parametersToken
    });
  }
}
