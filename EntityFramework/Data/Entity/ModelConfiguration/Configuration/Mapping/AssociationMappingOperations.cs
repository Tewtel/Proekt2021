﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.AssociationMappingOperations
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  internal static class AssociationMappingOperations
  {
    private static void MoveAssociationSetMappingDependents(
      AssociationSetMapping associationSetMapping,
      EndPropertyMapping dependentMapping,
      EntitySet toSet,
      bool useExistingColumns)
    {
      EntityType toTable = toSet.ElementType;
      dependentMapping.PropertyMappings.Each<ScalarPropertyMapping>((Action<ScalarPropertyMapping>) (pm =>
      {
        EdmProperty oldColumn = pm.Column;
        pm.Column = TableOperations.MoveColumnAndAnyConstraints(associationSetMapping.Table, toTable, oldColumn, useExistingColumns);
        associationSetMapping.Conditions.Where<ConditionPropertyMapping>((Func<ConditionPropertyMapping, bool>) (cc => cc.Column == oldColumn)).Each<ConditionPropertyMapping, EdmProperty>((Func<ConditionPropertyMapping, EdmProperty>) (cc => cc.Column = pm.Column));
      }));
      associationSetMapping.StoreEntitySet = toSet;
    }

    public static void MoveAllDeclaredAssociationSetMappings(
      DbDatabaseMapping databaseMapping,
      EntityType entityType,
      EntityType fromTable,
      EntityType toTable,
      bool useExistingColumns)
    {
      foreach (AssociationSetMapping associationSetMapping in databaseMapping.EntityContainerMappings.SelectMany<EntityContainerMapping, AssociationSetMapping>((Func<EntityContainerMapping, IEnumerable<AssociationSetMapping>>) (asm => asm.AssociationSetMappings)).Where<AssociationSetMapping>((Func<AssociationSetMapping, bool>) (a =>
      {
        if (a.Table != fromTable)
          return false;
        return a.AssociationSet.ElementType.SourceEnd.GetEntityType() == entityType || a.AssociationSet.ElementType.TargetEnd.GetEntityType() == entityType;
      })).ToArray<AssociationSetMapping>())
      {
        AssociationEndMember dependentEnd;
        if (!associationSetMapping.AssociationSet.ElementType.TryGuessPrincipalAndDependentEnds(out AssociationEndMember _, out dependentEnd))
          dependentEnd = associationSetMapping.AssociationSet.ElementType.TargetEnd;
        if (dependentEnd.GetEntityType() == entityType)
        {
          EndPropertyMapping dependentMapping = dependentEnd == associationSetMapping.TargetEndMapping.AssociationEnd ? associationSetMapping.SourceEndMapping : associationSetMapping.TargetEndMapping;
          AssociationMappingOperations.MoveAssociationSetMappingDependents(associationSetMapping, dependentMapping, databaseMapping.Database.GetEntitySet(toTable), useExistingColumns);
          (dependentMapping == associationSetMapping.TargetEndMapping ? associationSetMapping.SourceEndMapping : associationSetMapping.TargetEndMapping).PropertyMappings.Each<ScalarPropertyMapping>((Action<ScalarPropertyMapping>) (pm =>
          {
            if (pm.Column.DeclaringType == toTable)
              return;
            pm.Column = toTable.Properties.Single<EdmProperty>((Func<EdmProperty, bool>) (p => string.Equals(p.GetPreferredName(), pm.Column.GetPreferredName(), StringComparison.Ordinal)));
          }));
        }
      }
    }
  }
}
