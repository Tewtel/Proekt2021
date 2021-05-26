// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityWithKeyStrategy
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects.DataClasses;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class EntityWithKeyStrategy : IEntityKeyStrategy
  {
    private readonly IEntityWithKey _entity;

    public EntityWithKeyStrategy(IEntityWithKey entity) => this._entity = entity;

    public EntityKey GetEntityKey() => this._entity.EntityKey;

    public void SetEntityKey(EntityKey key) => this._entity.EntityKey = key;

    public EntityKey GetEntityKeyFromEntity() => this._entity.EntityKey;
  }
}
