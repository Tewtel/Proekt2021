// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.IEntityStateManager
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core
{
  internal interface IEntityStateManager
  {
    IEnumerable<IEntityStateEntry> GetEntityStateEntries(
      EntityState state);

    IEnumerable<IEntityStateEntry> FindRelationshipsByKey(EntityKey key);

    IEntityStateEntry GetEntityStateEntry(EntityKey key);

    bool TryGetEntityStateEntry(EntityKey key, out IEntityStateEntry stateEntry);

    bool TryGetReferenceKey(
      EntityKey dependentKey,
      AssociationEndMember principalRole,
      out EntityKey principalKey);
  }
}
