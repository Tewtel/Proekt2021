// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Node
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class Node
  {
    private readonly int m_id;
    private readonly List<Node> m_children;
    private NodeInfo m_nodeInfo;

    internal Node(int nodeId, Op op, List<Node> children)
    {
      this.m_id = nodeId;
      this.Op = op;
      this.m_children = children;
    }

    internal Node(Op op, params Node[] children)
      : this(-1, op, new List<Node>((IEnumerable<Node>) children))
    {
    }

    internal List<Node> Children => this.m_children;

    internal Op Op { get; set; }

    internal Node Child0
    {
      get => this.m_children[0];
      set => this.m_children[0] = value;
    }

    internal bool HasChild0 => this.m_children.Count > 0;

    internal Node Child1
    {
      get => this.m_children[1];
      set => this.m_children[1] = value;
    }

    internal bool HasChild1 => this.m_children.Count > 1;

    internal Node Child2
    {
      get => this.m_children[2];
      set => this.m_children[2] = value;
    }

    internal Node Child3 => this.m_children[3];

    internal bool HasChild2 => this.m_children.Count > 2;

    internal bool HasChild3 => this.m_children.Count > 3;

    internal bool IsEquivalent(Node other)
    {
      if (this.Children.Count != other.Children.Count)
        return false;
      bool? nullable = new bool?(this.Op.IsEquivalent(other.Op));
      bool flag = true;
      if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
        return false;
      for (int index = 0; index < this.Children.Count; ++index)
      {
        if (!this.Children[index].IsEquivalent(other.Children[index]))
          return false;
      }
      return true;
    }

    internal bool IsNodeInfoInitialized => this.m_nodeInfo != null;

    internal NodeInfo GetNodeInfo(Command command)
    {
      if (this.m_nodeInfo == null)
        this.InitializeNodeInfo(command);
      return this.m_nodeInfo;
    }

    internal ExtendedNodeInfo GetExtendedNodeInfo(Command command)
    {
      if (this.m_nodeInfo == null)
        this.InitializeNodeInfo(command);
      return this.m_nodeInfo as ExtendedNodeInfo;
    }

    private void InitializeNodeInfo(Command command)
    {
      this.m_nodeInfo = this.Op.IsRelOp || this.Op.IsPhysicalOp ? (NodeInfo) new ExtendedNodeInfo(command) : new NodeInfo(command);
      command.RecomputeNodeInfo(this);
    }
  }
}
