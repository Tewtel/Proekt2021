// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.TypeUtils
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal static class TypeUtils
  {
    internal static bool IsStructuredType(TypeUsage type) => TypeSemantics.IsReferenceType(type) || TypeSemantics.IsRowType(type) || (TypeSemantics.IsEntityType(type) || TypeSemantics.IsRelationshipType(type)) || TypeSemantics.IsComplexType(type);

    internal static bool IsCollectionType(TypeUsage type) => TypeSemantics.IsCollectionType(type);

    internal static bool IsEnumerationType(TypeUsage type) => TypeSemantics.IsEnumerationType(type);

    internal static TypeUsage CreateCollectionType(TypeUsage elementType) => TypeHelpers.CreateCollectionTypeUsage(elementType);
  }
}
