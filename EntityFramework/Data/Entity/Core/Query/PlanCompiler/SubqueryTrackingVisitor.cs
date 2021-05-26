// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.SubqueryTrackingVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal abstract class SubqueryTrackingVisitor : BasicOpVisitorOfNode
  {
    protected readonly System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler m_compilerState;
    protected readonly Stack<System.Data.Entity.Core.Query.InternalTrees.Node> m_ancestors = new Stack<System.Data.Entity.Core.Query.InternalTrees.Node>();
    private readonly Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, List<System.Data.Entity.Core.Query.InternalTrees.Node>> m_nodeSubqueries = new Dictionary<System.Data.Entity.Core.Query.InternalTrees.Node, List<System.Data.Entity.Core.Query.InternalTrees.Node>>();

    protected Command m_command => this.m_compilerState.Command;

    protected SubqueryTrackingVisitor(System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler planCompilerState) => this.m_compilerState = planCompilerState;

    protected void AddSubqueryToRelOpNode(System.Data.Entity.Core.Query.InternalTrees.Node relOpNode, System.Data.Entity.Core.Query.InternalTrees.Node subquery)
    {
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList;
      if (!this.m_nodeSubqueries.TryGetValue(relOpNode, out nodeList))
      {
        nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        this.m_nodeSubqueries[relOpNode] = nodeList;
      }
      nodeList.Add(subquery);
    }

    protected System.Data.Entity.Core.Query.InternalTrees.Node AddSubqueryToParentRelOp(
      Var outputVar,
      System.Data.Entity.Core.Query.InternalTrees.Node subquery)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node relOpAncestor = this.FindRelOpAncestor();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(relOpAncestor != null, "no ancestors found?");
      this.AddSubqueryToRelOpNode(relOpAncestor, subquery);
      subquery = this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(outputVar));
      return subquery;
    }

    protected System.Data.Entity.Core.Query.InternalTrees.Node FindRelOpAncestor()
    {
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node ancestor in this.m_ancestors)
      {
        if (ancestor.Op.IsRelOp)
          return ancestor;
        if (ancestor.Op.IsPhysicalOp)
          return (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      }
      return (System.Data.Entity.Core.Query.InternalTrees.Node) null;
    }

    protected override void VisitChildren(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.m_ancestors.Push(n);
      for (int index = 0; index < n.Children.Count; ++index)
        n.Children[index] = this.VisitNode(n.Children[index]);
      this.m_ancestors.Pop();
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node AugmentWithSubqueries(
      System.Data.Entity.Core.Query.InternalTrees.Node input,
      List<System.Data.Entity.Core.Query.InternalTrees.Node> subqueries,
      bool inputFirst)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node;
      int num;
      if (inputFirst)
      {
        node = input;
        num = 0;
      }
      else
      {
        node = subqueries[0];
        num = 1;
      }
      for (int index = num; index < subqueries.Count; ++index)
        node = this.m_command.CreateNode((Op) this.m_command.CreateOuterApplyOp(), node, subqueries[index]);
      if (!inputFirst)
        node = this.m_command.CreateNode((Op) this.m_command.CreateCrossApplyOp(), node, input);
      this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.JoinElimination);
      return node;
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitRelOpDefault(
      RelOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> subqueries;
      if (this.m_nodeSubqueries.TryGetValue(n, out subqueries) && subqueries.Count > 0)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(n.Op.OpType == OpType.Project || n.Op.OpType == OpType.Filter || n.Op.OpType == OpType.GroupBy || n.Op.OpType == OpType.GroupByInto, "VisitRelOpDefault: Unexpected op?" + n.Op.OpType.ToString());
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.AugmentWithSubqueries(n.Child0, subqueries, true);
        n.Child0 = node;
      }
      return n;
    }

    protected bool ProcessJoinOp(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> subqueries;
      if (!this.m_nodeSubqueries.TryGetValue(n, out subqueries))
        return false;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(n.Op.OpType == OpType.InnerJoin || n.Op.OpType == OpType.LeftOuterJoin || n.Op.OpType == OpType.FullOuterJoin, "unexpected op?");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(n.HasChild2, "missing second child to JoinOp?");
      System.Data.Entity.Core.Query.InternalTrees.Node child2 = n.Child2;
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.AugmentWithSubqueries(this.m_command.CreateNode((Op) this.m_command.CreateSingleRowTableOp()), subqueries, true);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), node1, child2);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.CreateNode((Op) this.m_command.CreateExistsOp(), node2);
      n.Child2 = node3;
      return true;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(UnnestOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> subqueries;
      return this.m_nodeSubqueries.TryGetValue(n, out subqueries) ? this.AugmentWithSubqueries(n, subqueries, false) : n;
    }
  }
}
