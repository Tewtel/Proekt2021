// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.SingleRowOpRules
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal static class SingleRowOpRules
  {
    internal static readonly PatternMatchRule Rule_SingleRowOpOverAnything = new PatternMatchRule(new System.Data.Entity.Core.Query.InternalTrees.Node((Op) SingleRowOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[1]
    {
      new System.Data.Entity.Core.Query.InternalTrees.Node((Op) LeafOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[0])
    }), new System.Data.Entity.Core.Query.InternalTrees.Rule.ProcessNodeDelegate(SingleRowOpRules.ProcessSingleRowOpOverAnything));
    internal static readonly PatternMatchRule Rule_SingleRowOpOverProject = new PatternMatchRule(new System.Data.Entity.Core.Query.InternalTrees.Node((Op) SingleRowOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[1]
    {
      new System.Data.Entity.Core.Query.InternalTrees.Node((Op) ProjectOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[2]
      {
        new System.Data.Entity.Core.Query.InternalTrees.Node((Op) LeafOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[0]),
        new System.Data.Entity.Core.Query.InternalTrees.Node((Op) LeafOp.Pattern, new System.Data.Entity.Core.Query.InternalTrees.Node[0])
      })
    }), new System.Data.Entity.Core.Query.InternalTrees.Rule.ProcessNodeDelegate(SingleRowOpRules.ProcessSingleRowOpOverProject));
    internal static readonly System.Data.Entity.Core.Query.InternalTrees.Rule[] Rules = new System.Data.Entity.Core.Query.InternalTrees.Rule[2]
    {
      (System.Data.Entity.Core.Query.InternalTrees.Rule) SingleRowOpRules.Rule_SingleRowOpOverAnything,
      (System.Data.Entity.Core.Query.InternalTrees.Rule) SingleRowOpRules.Rule_SingleRowOpOverProject
    };

    private static bool ProcessSingleRowOpOverAnything(
      RuleProcessingContext context,
      System.Data.Entity.Core.Query.InternalTrees.Node singleRowNode,
      out System.Data.Entity.Core.Query.InternalTrees.Node newNode)
    {
      newNode = singleRowNode;
      ExtendedNodeInfo extendedNodeInfo = context.Command.GetExtendedNodeInfo(singleRowNode.Child0);
      if (extendedNodeInfo.MaxRows <= RowCount.One)
      {
        newNode = singleRowNode.Child0;
        return true;
      }
      if (singleRowNode.Child0.Op.OpType != OpType.Filter || !new Predicate(context.Command, singleRowNode.Child0.Child1).SatisfiesKey(extendedNodeInfo.Keys.KeyVars, extendedNodeInfo.Definitions))
        return false;
      extendedNodeInfo.MaxRows = RowCount.One;
      newNode = singleRowNode.Child0;
      return true;
    }

    private static bool ProcessSingleRowOpOverProject(
      RuleProcessingContext context,
      System.Data.Entity.Core.Query.InternalTrees.Node singleRowNode,
      out System.Data.Entity.Core.Query.InternalTrees.Node newNode)
    {
      newNode = singleRowNode;
      System.Data.Entity.Core.Query.InternalTrees.Node child0_1 = singleRowNode.Child0;
      System.Data.Entity.Core.Query.InternalTrees.Node child0_2 = child0_1.Child0;
      singleRowNode.Child0 = child0_2;
      context.Command.RecomputeNodeInfo(singleRowNode);
      child0_1.Child0 = singleRowNode;
      newNode = child0_1;
      return true;
    }
  }
}
