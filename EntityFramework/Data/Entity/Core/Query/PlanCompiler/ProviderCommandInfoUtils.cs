// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ProviderCommandInfoUtils
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal static class ProviderCommandInfoUtils
  {
    internal static ProviderCommandInfo Create(Command command, System.Data.Entity.Core.Query.InternalTrees.Node node)
    {
      PhysicalProjectOp op = node.Op as PhysicalProjectOp;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op != null, "Expected root Op to be a physical Project");
      DbCommandTree commandTree = CTreeGenerator.Generate(command, node);
      DbQueryCommandTree queryCommandTree = commandTree as DbQueryCommandTree;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(queryCommandTree != null, "null query command tree");
      CollectionType edmType = TypeHelpers.GetEdmType<CollectionType>(queryCommandTree.Query.ResultType);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsRowType(edmType.TypeUsage), "command rowtype is not a record");
      ProviderCommandInfoUtils.BuildOutputVarMap(op, edmType.TypeUsage);
      return new ProviderCommandInfo(commandTree);
    }

    private static Dictionary<Var, EdmProperty> BuildOutputVarMap(
      PhysicalProjectOp projectOp,
      TypeUsage outputType)
    {
      Dictionary<Var, EdmProperty> dictionary = new Dictionary<Var, EdmProperty>();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsRowType(outputType), "PhysicalProjectOp result type is not a RowType?");
      IEnumerator<EdmProperty> enumerator1 = (IEnumerator<EdmProperty>) TypeHelpers.GetEdmType<RowType>(outputType).Properties.GetEnumerator();
      IEnumerator<Var> enumerator2 = (IEnumerator<Var>) projectOp.Outputs.GetEnumerator();
      while (true)
      {
        int num = enumerator1.MoveNext() ? 1 : 0;
        if (num == (enumerator2.MoveNext() ? 1 : 0))
        {
          if (num != 0)
            dictionary[enumerator2.Current] = enumerator1.Current;
          else
            goto label_5;
        }
        else
          break;
      }
      throw EntityUtil.InternalError(EntityUtil.InternalErrorCode.ColumnCountMismatch, 1, (object) null);
label_5:
      return dictionary;
    }
  }
}
