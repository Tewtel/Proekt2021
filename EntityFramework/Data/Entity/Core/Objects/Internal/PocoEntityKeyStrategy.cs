// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.PocoEntityKeyStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class PocoEntityKeyStrategy : IEntityKeyStrategy
  {
    private EntityKey _key;

    public EntityKey GetEntityKey() => this._key;

    public void SetEntityKey(EntityKey key) => this._key = key;

    public EntityKey GetEntityKeyFromEntity() => (EntityKey) null;
  }
}
