// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SubTreeId
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class SubTreeId
  {
    public Node m_subTreeRoot;
    private readonly int m_hashCode;
    private readonly Node m_parent;
    private readonly int m_childIndex;

    internal SubTreeId(RuleProcessingContext context, Node node, Node parent, int childIndex)
    {
      this.m_subTreeRoot = node;
      this.m_parent = parent;
      this.m_childIndex = childIndex;
      this.m_hashCode = context.GetHashCode(node);
    }

    public override int GetHashCode() => this.m_hashCode;

    public override bool Equals(object obj)
    {
      if (!(obj is SubTreeId subTreeId) || this.m_hashCode != subTreeId.m_hashCode)
        return false;
      if (subTreeId.m_subTreeRoot == this.m_subTreeRoot)
        return true;
      return subTreeId.m_parent == this.m_parent && subTreeId.m_childIndex == this.m_childIndex;
    }
  }
}
