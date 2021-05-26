// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.Provider.EdmProviderManifestHierarchyIdFunctions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm.Provider
{
  internal static class EdmProviderManifestHierarchyIdFunctions
  {
    internal static void AddFunctions(EdmProviderManifestFunctionBuilder functions)
    {
      functions.AddFunction(PrimitiveTypeKind.HierarchyId, "HierarchyIdGetRoot");
      functions.AddFunction(PrimitiveTypeKind.HierarchyId, "HierarchyIdParse", PrimitiveTypeKind.String, "input");
      functions.AddFunction(PrimitiveTypeKind.HierarchyId, "GetAncestor", PrimitiveTypeKind.HierarchyId, "hierarchyIdValue", PrimitiveTypeKind.Int32, "n");
      functions.AddFunction(PrimitiveTypeKind.HierarchyId, "GetDescendant", PrimitiveTypeKind.HierarchyId, "hierarchyIdValue", PrimitiveTypeKind.HierarchyId, "child1", PrimitiveTypeKind.HierarchyId, "child2");
      functions.AddFunction(PrimitiveTypeKind.Int16, "GetLevel", PrimitiveTypeKind.HierarchyId, "hierarchyIdValue");
      functions.AddFunction(PrimitiveTypeKind.Boolean, "IsDescendantOf", PrimitiveTypeKind.HierarchyId, "hierarchyIdValue", PrimitiveTypeKind.HierarchyId, "parent");
      functions.AddFunction(PrimitiveTypeKind.HierarchyId, "GetReparentedValue", PrimitiveTypeKind.HierarchyId, "hierarchyIdValue", PrimitiveTypeKind.HierarchyId, "oldRoot", PrimitiveTypeKind.HierarchyId, "newRoot");
    }
  }
}
