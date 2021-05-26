// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.DistinctOpRules
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal static class DistinctOpRules
  {
    internal static readonly SimpleRule Rule_DistinctOpOfKeys = new SimpleRule(OpType.Distinct, new System.Data.Entity.Core.Query.InternalTrees.Rule.ProcessNodeDelegate(DistinctOpRules.ProcessDistinctOpOfKeys));
    internal static readonly System.Data.Entity.Core.Query.InternalTrees.Rule[] Rules = new System.Data.Entity.Core.Query.InternalTrees.Rule[1]
    {
      (System.Data.Entity.Core.Query.InternalTrees.Rule) DistinctOpRules.Rule_DistinctOpOfKeys
    };

    private static bool ProcessDistinctOpOfKeys(
      RuleProcessingContext context,
      System.Data.Entity.Core.Query.InternalTrees.Node n,
      out System.Data.Entity.Core.Query.InternalTrees.Node newNode)
    {
      Command command = context.Command;
      ExtendedNodeInfo extendedNodeInfo = command.GetExtendedNodeInfo(n.Child0);
      DistinctOp op = (DistinctOp) n.Op;
      if (!extendedNodeInfo.Keys.NoKeys && op.Keys.Subsumes(extendedNodeInfo.Keys.KeyVars))
      {
        ProjectOp projectOp = command.CreateProjectOp(op.Keys);
        VarDefListOp varDefListOp = command.CreateVarDefListOp();
        System.Data.Entity.Core.Query.InternalTrees.Node node = command.CreateNode((Op) varDefListOp);
        newNode = command.CreateNode((Op) projectOp, n.Child0, node);
        return true;
      }
      newNode = n;
      return false;
    }
  }
}
