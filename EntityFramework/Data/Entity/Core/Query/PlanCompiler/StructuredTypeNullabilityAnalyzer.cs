﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.StructuredTypeNullabilityAnalyzer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class StructuredTypeNullabilityAnalyzer : ColumnMapVisitor<HashSet<string>>
  {
    internal static StructuredTypeNullabilityAnalyzer Instance = new StructuredTypeNullabilityAnalyzer();

    internal override void Visit(
      VarRefColumnMap columnMap,
      HashSet<string> typesNeedingNullSentinel)
    {
      StructuredTypeNullabilityAnalyzer.AddTypeNeedingNullSentinel(typesNeedingNullSentinel, columnMap.Type);
      base.Visit(columnMap, typesNeedingNullSentinel);
    }

    private static void AddTypeNeedingNullSentinel(
      HashSet<string> typesNeedingNullSentinel,
      TypeUsage typeUsage)
    {
      if (TypeSemantics.IsCollectionType(typeUsage))
      {
        StructuredTypeNullabilityAnalyzer.AddTypeNeedingNullSentinel(typesNeedingNullSentinel, TypeHelpers.GetElementTypeUsage(typeUsage));
      }
      else
      {
        if (TypeSemantics.IsRowType(typeUsage) || TypeSemantics.IsComplexType(typeUsage))
          StructuredTypeNullabilityAnalyzer.MarkAsNeedingNullSentinel(typesNeedingNullSentinel, typeUsage);
        foreach (EdmMember structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers(typeUsage))
          StructuredTypeNullabilityAnalyzer.AddTypeNeedingNullSentinel(typesNeedingNullSentinel, structuralMember.TypeUsage);
      }
    }

    internal static void MarkAsNeedingNullSentinel(
      HashSet<string> typesNeedingNullSentinel,
      TypeUsage typeUsage)
    {
      typesNeedingNullSentinel.Add(typeUsage.EdmType.Identity);
    }
  }
}
