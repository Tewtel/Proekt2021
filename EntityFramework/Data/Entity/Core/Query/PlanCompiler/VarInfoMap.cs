// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.VarInfoMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class VarInfoMap
  {
    private readonly Dictionary<Var, VarInfo> m_map;

    internal VarInfoMap() => this.m_map = new Dictionary<Var, VarInfo>();

    internal VarInfo CreateStructuredVarInfo(
      Var v,
      RowType newType,
      List<Var> newVars,
      List<EdmProperty> newProperties,
      bool newVarsIncludeNullSentinelVar)
    {
      VarInfo varInfo = (VarInfo) new StructuredVarInfo(newType, newVars, newProperties, newVarsIncludeNullSentinelVar);
      this.m_map.Add(v, varInfo);
      return varInfo;
    }

    internal VarInfo CreateStructuredVarInfo(
      Var v,
      RowType newType,
      List<Var> newVars,
      List<EdmProperty> newProperties)
    {
      return this.CreateStructuredVarInfo(v, newType, newVars, newProperties, false);
    }

    internal VarInfo CreateCollectionVarInfo(Var v, Var newVar)
    {
      VarInfo varInfo = (VarInfo) new CollectionVarInfo(newVar);
      this.m_map.Add(v, varInfo);
      return varInfo;
    }

    internal VarInfo CreatePrimitiveTypeVarInfo(Var v, Var newVar)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsScalarType(v.Type), "The current variable should be of primitive or enum type.");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsScalarType(newVar.Type), "The new variable should be of primitive or enum type.");
      VarInfo varInfo = (VarInfo) new PrimitiveTypeVarInfo(newVar);
      this.m_map.Add(v, varInfo);
      return varInfo;
    }

    internal bool TryGetVarInfo(Var v, out VarInfo varInfo) => this.m_map.TryGetValue(v, out varInfo);
  }
}
