// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DefaultModelCacheKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Internal
{
  internal sealed class DefaultModelCacheKey : IDbModelCacheKey
  {
    private readonly Type _contextType;
    private readonly string _providerName;
    private readonly Type _providerType;
    private readonly string _customKey;

    public DefaultModelCacheKey(
      Type contextType,
      string providerName,
      Type providerType,
      string customKey)
    {
      this._contextType = contextType;
      this._providerName = providerName;
      this._providerType = providerType;
      this._customKey = customKey;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return obj is DefaultModelCacheKey other && this.Equals(other);
    }

    public override int GetHashCode() => this._contextType.GetHashCode() * 397 ^ this._providerName.GetHashCode() ^ this._providerType.GetHashCode() ^ (!string.IsNullOrWhiteSpace(this._customKey) ? this._customKey.GetHashCode() : 0);

    private bool Equals(DefaultModelCacheKey other) => this._contextType == other._contextType && string.Equals(this._providerName, other._providerName) && object.Equals((object) this._providerType, (object) other._providerType) && string.Equals(this._customKey, other._customKey);
  }
}
