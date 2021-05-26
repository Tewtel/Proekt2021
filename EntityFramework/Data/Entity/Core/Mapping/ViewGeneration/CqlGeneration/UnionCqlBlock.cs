// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration.UnionCqlBlock
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Structures;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration
{
  internal sealed class UnionCqlBlock : CqlBlock
  {
    internal UnionCqlBlock(
      SlotInfo[] slotInfos,
      List<CqlBlock> children,
      CqlIdentifiers identifiers,
      int blockAliasNum)
      : base(slotInfos, children, BoolExpression.True, identifiers, blockAliasNum)
    {
    }

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      bool isTopLevel,
      int indentLevel)
    {
      bool flag = true;
      foreach (CqlBlock child in this.Children)
      {
        if (!flag)
        {
          StringUtil.IndentNewLine(builder, indentLevel + 1);
          builder.Append(OpCellTreeNode.OpToEsql(CellTreeOpType.Union));
        }
        flag = false;
        builder.Append(" (");
        StringBuilder builder1 = builder;
        int num = isTopLevel ? 1 : 0;
        int indentLevel1 = indentLevel + 1;
        child.AsEsql(builder1, num != 0, indentLevel1);
        builder.Append(')');
      }
      return builder;
    }

    internal override DbExpression AsCqt(bool isTopLevel)
    {
      DbExpression left = this.Children[0].AsCqt(isTopLevel);
      for (int index = 1; index < this.Children.Count; ++index)
        left = (DbExpression) left.UnionAll(this.Children[index].AsCqt(isTopLevel));
      return left;
    }
  }
}
