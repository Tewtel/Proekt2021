// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.AugmentedNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class AugmentedNode
  {
    private readonly int m_id;
    private readonly System.Data.Entity.Core.Query.InternalTrees.Node m_node;
    protected AugmentedNode m_parent;
    private readonly List<AugmentedNode> m_children;
    private readonly List<JoinEdge> m_joinEdges = new List<JoinEdge>();

    internal AugmentedNode(int id, System.Data.Entity.Core.Query.InternalTrees.Node node)
      : this(id, node, new List<AugmentedNode>())
    {
    }

    internal AugmentedNode(int id, System.Data.Entity.Core.Query.InternalTrees.Node node, List<AugmentedNode> children)
    {
      this.m_id = id;
      this.m_node = node;
      this.m_children = children;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(children != null, "null children (gasp!)");
      foreach (AugmentedNode child in this.m_children)
        child.m_parent = this;
    }

    internal int Id => this.m_id;

    internal System.Data.Entity.Core.Query.InternalTrees.Node Node => this.m_node;

    internal AugmentedNode Parent => this.m_parent;

    internal List<AugmentedNode> Children => this.m_children;

    internal List<JoinEdge> JoinEdges => this.m_joinEdges;
  }
}
