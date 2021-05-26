// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.AggregatePushdownUtil
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal static class AggregatePushdownUtil
  {
    internal static bool IsVarRefOverGivenVar(System.Data.Entity.Core.Query.InternalTrees.Node node, Var var) => node.Op.OpType == OpType.VarRef && ((VarRefOp) node.Op).Var == var;
  }
}
