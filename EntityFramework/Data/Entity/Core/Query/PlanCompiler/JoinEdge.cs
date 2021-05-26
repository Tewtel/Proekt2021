// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.JoinEdge
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class JoinEdge
  {
    private readonly AugmentedTableNode m_left;
    private readonly AugmentedTableNode m_right;
    private readonly AugmentedJoinNode m_joinNode;
    private readonly List<ColumnVar> m_leftVars;
    private readonly List<ColumnVar> m_rightVars;

    private JoinEdge(
      AugmentedTableNode left,
      AugmentedTableNode right,
      AugmentedJoinNode joinNode,
      JoinKind joinKind,
      List<ColumnVar> leftVars,
      List<ColumnVar> rightVars)
    {
      this.m_left = left;
      this.m_right = right;
      this.JoinKind = joinKind;
      this.m_joinNode = joinNode;
      this.m_leftVars = leftVars;
      this.m_rightVars = rightVars;
      int num = this.m_leftVars.Count == this.m_rightVars.Count ? 1 : 0;
      int count = this.m_leftVars.Count;
      string str1 = count.ToString();
      count = this.m_rightVars.Count;
      string str2 = count.ToString();
      string message = "Count mismatch: " + str1 + "," + str2;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(num != 0, message);
    }

    internal AugmentedTableNode Left => this.m_left;

    internal AugmentedTableNode Right => this.m_right;

    internal AugmentedJoinNode JoinNode => this.m_joinNode;

    internal JoinKind JoinKind { get; set; }

    internal List<ColumnVar> LeftVars => this.m_leftVars;

    internal List<ColumnVar> RightVars => this.m_rightVars;

    internal bool IsEliminated => this.Left.IsEliminated || this.Right.IsEliminated;

    internal bool RestrictedElimination
    {
      get
      {
        if (this.m_joinNode == null)
          return false;
        return this.m_joinNode.OtherPredicate != null || this.m_left.LastVisibleId < this.m_joinNode.Id || this.m_right.LastVisibleId < this.m_joinNode.Id;
      }
    }

    internal static JoinEdge CreateJoinEdge(
      AugmentedTableNode left,
      AugmentedTableNode right,
      AugmentedJoinNode joinNode,
      ColumnVar leftVar,
      ColumnVar rightVar)
    {
      List<ColumnVar> leftVars = new List<ColumnVar>();
      List<ColumnVar> rightVars = new List<ColumnVar>();
      leftVars.Add(leftVar);
      rightVars.Add(rightVar);
      OpType opType = joinNode.Node.Op.OpType;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(opType == OpType.LeftOuterJoin || opType == OpType.InnerJoin, "Unexpected join type for join edge: " + opType.ToString());
      JoinKind joinKind = opType == OpType.LeftOuterJoin ? JoinKind.LeftOuter : JoinKind.Inner;
      return new JoinEdge(left, right, joinNode, joinKind, leftVars, rightVars);
    }

    internal static JoinEdge CreateTransitiveJoinEdge(
      AugmentedTableNode left,
      AugmentedTableNode right,
      JoinKind joinKind,
      List<ColumnVar> leftVars,
      List<ColumnVar> rightVars)
    {
      return new JoinEdge(left, right, (AugmentedJoinNode) null, joinKind, leftVars, rightVars);
    }

    internal bool AddCondition(AugmentedJoinNode joinNode, ColumnVar leftVar, ColumnVar rightVar)
    {
      if (joinNode != this.m_joinNode)
        return false;
      this.m_leftVars.Add(leftVar);
      this.m_rightVars.Add(rightVar);
      return true;
    }
  }
}
