// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.BasicOpVisitorOfNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class BasicOpVisitorOfNode : BasicOpVisitorOfT<Node>
  {
    protected override void VisitChildren(Node n)
    {
      for (int index = 0; index < n.Children.Count; ++index)
        n.Children[index] = this.VisitNode(n.Children[index]);
    }

    protected override void VisitChildrenReverse(Node n)
    {
      for (int index = n.Children.Count - 1; index >= 0; --index)
        n.Children[index] = this.VisitNode(n.Children[index]);
    }

    protected override Node VisitDefault(Node n)
    {
      this.VisitChildren(n);
      return n;
    }

    protected override Node VisitAncillaryOpDefault(AncillaryOp op, Node n) => this.VisitDefault(n);

    protected override Node VisitPhysicalOpDefault(PhysicalOp op, Node n) => this.VisitDefault(n);

    protected override Node VisitRelOpDefault(RelOp op, Node n) => this.VisitDefault(n);

    protected override Node VisitScalarOpDefault(ScalarOp op, Node n) => this.VisitDefault(n);
  }
}
