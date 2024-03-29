﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.ForeignKeyFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class ForeignKeyFactory
  {
    private const string s_NullPart = "EntityHasNullForeignKey";
    private const string s_NullForeignKey = "EntityHasNullForeignKey.EntityHasNullForeignKey";

    public static bool IsConceptualNullKey(EntityKey key) => !(key == (EntityKey) null) && string.Equals(key.EntityContainerName, "EntityHasNullForeignKey") && string.Equals(key.EntitySetName, "EntityHasNullForeignKey");

    public static bool IsConceptualNullKeyChanged(EntityKey conceptualNullKey, EntityKey realKey) => realKey == (EntityKey) null || !EntityKey.InternalEquals(conceptualNullKey, realKey, false);

    public static EntityKey CreateConceptualNullKey(EntityKey originalKey) => new EntityKey("EntityHasNullForeignKey.EntityHasNullForeignKey", (IEnumerable<EntityKeyMember>) originalKey.EntityKeyValues);

    public static EntityKey CreateKeyFromForeignKeyValues(
      EntityEntry dependentEntry,
      RelatedEnd relatedEnd)
    {
      ReferentialConstraint constraint = ((AssociationType) relatedEnd.RelationMetadata).ReferentialConstraints.First<ReferentialConstraint>();
      return ForeignKeyFactory.CreateKeyFromForeignKeyValues(dependentEntry, constraint, relatedEnd.GetTargetEntitySetFromRelationshipSet(), false);
    }

    public static EntityKey CreateKeyFromForeignKeyValues(
      EntityEntry dependentEntry,
      ReferentialConstraint constraint,
      EntitySet principalEntitySet,
      bool useOriginalValues)
    {
      ReadOnlyMetadataCollection<EdmProperty> toProperties = constraint.ToProperties;
      int count = toProperties.Count;
      if (count == 1)
      {
        object singletonKeyValue = useOriginalValues ? dependentEntry.GetOriginalEntityValue(toProperties.First<EdmProperty>().Name) : dependentEntry.GetCurrentEntityValue(toProperties.First<EdmProperty>().Name);
        return singletonKeyValue != DBNull.Value ? new EntityKey((EntitySetBase) principalEntitySet, singletonKeyValue) : (EntityKey) null;
      }
      string[] keyMemberNames = principalEntitySet.ElementType.KeyMemberNames;
      object[] compositeKeyValues = new object[count];
      ReadOnlyMetadataCollection<EdmProperty> fromProperties = constraint.FromProperties;
      for (int index1 = 0; index1 < count; ++index1)
      {
        object obj = useOriginalValues ? dependentEntry.GetOriginalEntityValue(toProperties[index1].Name) : dependentEntry.GetCurrentEntityValue(toProperties[index1].Name);
        if (obj == DBNull.Value)
          return (EntityKey) null;
        int index2 = Array.IndexOf<string>(keyMemberNames, fromProperties[index1].Name);
        compositeKeyValues[index2] = obj;
      }
      return new EntityKey((EntitySetBase) principalEntitySet, compositeKeyValues);
    }
  }
}
