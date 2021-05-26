// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.AugmentedJoinNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal sealed class AugmentedJoinNode : AugmentedNode
  {
    private readonly List<ColumnVar> m_leftVars;
    private readonly List<ColumnVar> m_rightVars;
    private readonly System.Data.Entity.Core.Query.InternalTrees.Node m_otherPredicate;

    internal AugmentedJoinNode(
      int id,
      System.Data.Entity.Core.Query.InternalTrees.Node node,
      AugmentedNode leftChild,
      AugmentedNode rightChild,
      List<ColumnVar> leftVars,
      List<ColumnVar> rightVars,
      System.Data.Entity.Core.Query.InternalTrees.Node otherPredicate)
      : this(id, node, new List<AugmentedNode>((IEnumerable<AugmentedNode>) new AugmentedNode[2]
      {
        leftChild,
        rightChild
      }))
    {
      this.m_otherPredicate = otherPredicate;
      this.m_rightVars = rightVars;
      this.m_leftVars = leftVars;
    }

    internal AugmentedJoinNode(int id, System.Data.Entity.Core.Query.InternalTrees.Node node, List<AugmentedNode> children)
      : base(id, node, children)
    {
      this.m_leftVars = new List<ColumnVar>();
      this.m_rightVars = new List<ColumnVar>();
    }

    internal System.Data.Entity.Core.Query.InternalTrees.Node OtherPredicate => this.m_otherPredicate;

    internal List<ColumnVar> LeftVars => this.m_leftVars;

    internal List<ColumnVar> RightVars => this.m_rightVars;
  }
}
