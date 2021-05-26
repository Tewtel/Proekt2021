// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Rule
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class Rule
  {
    private readonly Rule.ProcessNodeDelegate m_nodeDelegate;
    private readonly OpType m_opType;

    protected Rule(OpType opType, Rule.ProcessNodeDelegate nodeProcessDelegate)
    {
      this.m_opType = opType;
      this.m_nodeDelegate = nodeProcessDelegate;
    }

    internal abstract bool Match(Node node);

    internal bool Apply(RuleProcessingContext ruleProcessingContext, Node node, out Node newNode) => this.m_nodeDelegate(ruleProcessingContext, node, out newNode);

    internal OpType RuleOpType => this.m_opType;

    internal delegate bool ProcessNodeDelegate(
      RuleProcessingContext context,
      Node subTree,
      out Node newSubTree);
  }
}
