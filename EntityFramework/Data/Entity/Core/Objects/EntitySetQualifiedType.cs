// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.EntitySetQualifiedType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Objects
{
  internal struct EntitySetQualifiedType : IEqualityComparer<EntitySetQualifiedType>
  {
    internal static readonly IEqualityComparer<EntitySetQualifiedType> EqualityComparer = (IEqualityComparer<EntitySetQualifiedType>) new EntitySetQualifiedType();
    internal readonly Type ClrType;
    internal readonly EntitySet EntitySet;

    internal EntitySetQualifiedType(Type type, EntitySet set)
    {
      this.ClrType = EntityUtil.GetEntityIdentityType(type);
      this.EntitySet = set;
    }

    public bool Equals(EntitySetQualifiedType x, EntitySetQualifiedType y) => (object) x.ClrType == (object) y.ClrType && x.EntitySet == y.EntitySet;

    public int GetHashCode(EntitySetQualifiedType obj) => obj.ClrType.GetHashCode() + obj.EntitySet.Name.GetHashCode() + obj.EntitySet.EntityContainer.Name.GetHashCode();
  }
}
