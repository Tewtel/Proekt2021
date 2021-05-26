// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.RuleProcessor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class RuleProcessor
  {
    private readonly Dictionary<SubTreeId, SubTreeId> m_processedNodeMap;

    internal RuleProcessor() => this.m_processedNodeMap = new Dictionary<SubTreeId, SubTreeId>();

    private static bool ApplyRulesToNode(
      RuleProcessingContext context,
      ReadOnlyCollection<ReadOnlyCollection<Rule>> rules,
      Node currentNode,
      out Node newNode)
    {
      newNode = currentNode;
      context.PreProcess(currentNode);
      foreach (Rule rule in rules[(int) currentNode.Op.OpType])
      {
        if (rule.Match(currentNode) && rule.Apply(context, currentNode, out newNode))
        {
          context.PostProcess(newNode, rule);
          return true;
        }
      }
      context.PostProcess(currentNode, (Rule) null);
      return false;
    }

    private Node ApplyRulesToSubtree(
      RuleProcessingContext context,
      ReadOnlyCollection<ReadOnlyCollection<Rule>> rules,
      Node subTreeRoot,
      Node parent,
      int childIndexInParent)
    {
      int num = 0;
      Dictionary<SubTreeId, SubTreeId> dictionary = new Dictionary<SubTreeId, SubTreeId>();
      SubTreeId key;
      while (true)
      {
        ++num;
        context.PreProcessSubTree(subTreeRoot);
        key = new SubTreeId(context, subTreeRoot, parent, childIndexInParent);
        if (!this.m_processedNodeMap.ContainsKey(key))
        {
          if (!dictionary.ContainsKey(key))
          {
            dictionary[key] = key;
            for (int index = 0; index < subTreeRoot.Children.Count; ++index)
            {
              Node child = subTreeRoot.Children[index];
              if (RuleProcessor.ShouldApplyRules(child, subTreeRoot))
                subTreeRoot.Children[index] = this.ApplyRulesToSubtree(context, rules, child, subTreeRoot, index);
            }
            Node newNode;
            if (RuleProcessor.ApplyRulesToNode(context, rules, subTreeRoot, out newNode))
            {
              context.PostProcessSubTree(subTreeRoot);
              subTreeRoot = newNode;
            }
            else
              goto label_10;
          }
          else
            break;
        }
        else
          goto label_12;
      }
      this.m_processedNodeMap[key] = key;
      goto label_12;
label_10:
      this.m_processedNodeMap[key] = key;
label_12:
      context.PostProcessSubTree(subTreeRoot);
      return subTreeRoot;
    }

    private static bool ShouldApplyRules(Node node, Node parent) => parent.Op.OpType != OpType.In || (uint) node.Op.OpType > 0U;

    internal Node ApplyRulesToSubtree(
      RuleProcessingContext context,
      ReadOnlyCollection<ReadOnlyCollection<Rule>> rules,
      Node subTreeRoot)
    {
      return this.ApplyRulesToSubtree(context, rules, subTreeRoot, (Node) null, 0);
    }
  }
}
