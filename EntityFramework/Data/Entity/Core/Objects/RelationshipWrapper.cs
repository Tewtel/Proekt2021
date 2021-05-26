// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.RelationshipWrapper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Objects
{
  internal sealed class RelationshipWrapper : IEquatable<RelationshipWrapper>
  {
    internal readonly AssociationSet AssociationSet;
    internal readonly EntityKey Key0;
    internal readonly EntityKey Key1;

    internal RelationshipWrapper(AssociationSet extent, EntityKey key)
    {
      this.AssociationSet = extent;
      this.Key0 = key;
      this.Key1 = key;
    }

    internal RelationshipWrapper(RelationshipWrapper wrapper, int ordinal, EntityKey key)
    {
      this.AssociationSet = wrapper.AssociationSet;
      this.Key0 = ordinal == 0 ? key : wrapper.Key0;
      this.Key1 = ordinal == 0 ? wrapper.Key1 : key;
    }

    internal RelationshipWrapper(
      AssociationSet extent,
      KeyValuePair<string, EntityKey> roleAndKey1,
      KeyValuePair<string, EntityKey> roleAndKey2)
      : this(extent, roleAndKey1.Key, roleAndKey1.Value, roleAndKey2.Key, roleAndKey2.Value)
    {
    }

    internal RelationshipWrapper(
      AssociationSet extent,
      string role0,
      EntityKey key0,
      string role1,
      EntityKey key1)
    {
      this.AssociationSet = extent;
      if (extent.ElementType.AssociationEndMembers[0].Name == role0)
      {
        this.Key0 = key0;
        this.Key1 = key1;
      }
      else
      {
        this.Key0 = key1;
        this.Key1 = key0;
      }
    }

    internal ReadOnlyMetadataCollection<AssociationEndMember> AssociationEndMembers => this.AssociationSet.ElementType.AssociationEndMembers;

    internal AssociationEndMember GetAssociationEndMember(EntityKey key) => this.AssociationEndMembers[this.Key0 != key ? 1 : 0];

    internal EntityKey GetOtherEntityKey(EntityKey key)
    {
      if (this.Key0 == key)
        return this.Key1;
      return !(this.Key1 == key) ? (EntityKey) null : this.Key0;
    }

    internal EntityKey GetEntityKey(int ordinal)
    {
      if (ordinal == 0)
        return this.Key0;
      if (ordinal == 1)
        return this.Key1;
      throw new ArgumentOutOfRangeException(nameof (ordinal));
    }

    public override int GetHashCode() => this.AssociationSet.Name.GetHashCode() ^ this.Key0.GetHashCode() + this.Key1.GetHashCode();

    public override bool Equals(object obj) => this.Equals(obj as RelationshipWrapper);

    public bool Equals(RelationshipWrapper wrapper)
    {
      if (this == wrapper)
        return true;
      return wrapper != null && this.AssociationSet == wrapper.AssociationSet && this.Key0.Equals(wrapper.Key0) && this.Key1.Equals(wrapper.Key1);
    }
  }
}
