// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.SortRemover
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class SortRemover : BasicOpVisitorOfNode
  {
    private readonly Command m_command;
    private readonly System.Data.Entity.Core.Query.InternalTrees.Node m_topMostSort;
    private readonly HashSet<System.Data.Entity.Core.Query.InternalTrees.Node> changedNodes = new HashSet<System.Data.Entity.Core.Query.InternalTrees.Node>();

    private SortRemover(Command command, System.Data.Entity.Core.Query.InternalTrees.Node topMostSort)
    {
      this.m_command = command;
      this.m_topMostSort = topMostSort;
    }

    internal static void Process(Command command)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node topMostSort = command.Root.Child0 == null || command.Root.Child0.Op.OpType != OpType.Sort ? (System.Data.Entity.Core.Query.InternalTrees.Node) null : command.Root.Child0;
      SortRemover sortRemover = new SortRemover(command, topMostSort);
      command.Root = sortRemover.VisitNode(command.Root);
    }

    protected override void VisitChildren(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      bool flag = false;
      for (int index = 0; index < n.Children.Count; ++index)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node child = n.Children[index];
        n.Children[index] = this.VisitNode(n.Children[index]);
        if (child != n.Children[index] || this.changedNodes.Contains(child))
          flag = true;
      }
      if (!flag)
        return;
      this.m_command.RecomputeNodeInfo(n);
      this.changedNodes.Add(n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      ConstrainedSortOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      return op.Keys.Count > 0 || n.Children.Count != 3 || (n.Child0 == null || n.Child1 == null) || (n.Child0.Op.OpType != OpType.Sort || n.Child1.Op.OpType != OpType.Null || n.Child0.Children.Count != 1) ? n : this.m_command.CreateNode((Op) this.m_command.CreateConstrainedSortOp(((SortBaseOp) n.Child0.Op).Keys, op.WithTies), n.Child0.Child0, n.Child1, n.Child2);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(SortOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      return n != this.m_topMostSort ? n.Child0 : n;
    }
  }
}
