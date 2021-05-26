// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ConstraintManager
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class ConstraintManager
  {
    private readonly Dictionary<EntityContainer, EntityContainer> m_entityContainerMap;
    private readonly Dictionary<ExtentPair, List<ForeignKeyConstraint>> m_parentChildRelationships;

    internal bool IsParentChildRelationship(
      EntitySetBase table1,
      EntitySetBase table2,
      out List<ForeignKeyConstraint> constraints)
    {
      this.LoadRelationships(table1.EntityContainer);
      this.LoadRelationships(table2.EntityContainer);
      return this.m_parentChildRelationships.TryGetValue(new ExtentPair(table1, table2), out constraints);
    }

    internal void LoadRelationships(EntityContainer entityContainer)
    {
      if (this.m_entityContainerMap.ContainsKey(entityContainer))
        return;
      foreach (EntitySetBase baseEntitySet in entityContainer.BaseEntitySets)
      {
        if (baseEntitySet is RelationshipSet relationshipSet1)
        {
          RelationshipType elementType = relationshipSet1.ElementType;
          if (elementType is AssociationType associationType3 && ConstraintManager.IsBinary(elementType))
          {
            foreach (ReferentialConstraint referentialConstraint in associationType3.ReferentialConstraints)
            {
              ForeignKeyConstraint foreignKeyConstraint = new ForeignKeyConstraint(relationshipSet1, referentialConstraint);
              List<ForeignKeyConstraint> foreignKeyConstraintList;
              if (!this.m_parentChildRelationships.TryGetValue(foreignKeyConstraint.Pair, out foreignKeyConstraintList))
              {
                foreignKeyConstraintList = new List<ForeignKeyConstraint>();
                this.m_parentChildRelationships[foreignKeyConstraint.Pair] = foreignKeyConstraintList;
              }
              foreignKeyConstraintList.Add(foreignKeyConstraint);
            }
          }
        }
      }
      this.m_entityContainerMap[entityContainer] = entityContainer;
    }

    internal ConstraintManager()
    {
      this.m_entityContainerMap = new Dictionary<EntityContainer, EntityContainer>();
      this.m_parentChildRelationships = new Dictionary<ExtentPair, List<ForeignKeyConstraint>>();
    }

    private static bool IsBinary(RelationshipType relationshipType)
    {
      int num = 0;
      foreach (EdmMember member in relationshipType.Members)
      {
        if (member is RelationshipEndMember)
        {
          ++num;
          if (num > 2)
            return false;
        }
      }
      return num == 2;
    }
  }
}
