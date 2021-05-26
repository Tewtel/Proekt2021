// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.NodeCounter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class NodeCounter : BasicOpVisitorOfT<int>
  {
    internal static int Count(Node subTree) => new NodeCounter().VisitNode(subTree);

    protected override int VisitDefault(Node n)
    {
      int num = 1;
      foreach (Node child in n.Children)
        num += this.VisitNode(child);
      return num;
    }
  }
}
