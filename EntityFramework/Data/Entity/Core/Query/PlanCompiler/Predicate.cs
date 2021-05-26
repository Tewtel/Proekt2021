// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.Predicate
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class Predicate
  {
    private readonly Command m_command;
    private readonly List<System.Data.Entity.Core.Query.InternalTrees.Node> m_parts;

    internal Predicate(Command command)
    {
      this.m_command = command;
      this.m_parts = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
    }

    internal Predicate(Command command, System.Data.Entity.Core.Query.InternalTrees.Node andTree)
      : this(command)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(andTree != null, "null node passed to Predicate() constructor");
      this.InitFromAndTree(andTree);
    }

    internal void AddPart(System.Data.Entity.Core.Query.InternalTrees.Node n) => this.m_parts.Add(n);

    internal System.Data.Entity.Core.Query.InternalTrees.Node BuildAndTree()
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
        node = node != null ? this.m_command.CreateNode((Op) this.m_command.CreateConditionalOp(OpType.And), node, part) : part;
      return node;
    }

    internal Predicate GetSingleTablePredicates(
      VarVec tableDefinitions,
      out Predicate otherPredicates)
    {
      List<Predicate> singleTablePredicates;
      this.GetSingleTablePredicates(new List<VarVec>()
      {
        tableDefinitions
      }, out singleTablePredicates, out otherPredicates);
      return singleTablePredicates[0];
    }

    internal void GetEquiJoinPredicates(
      VarVec leftTableDefinitions,
      VarVec rightTableDefinitions,
      out List<Var> leftTableEquiJoinColumns,
      out List<Var> rightTableEquiJoinColumns,
      out Predicate otherPredicates)
    {
      otherPredicates = new Predicate(this.m_command);
      leftTableEquiJoinColumns = new List<Var>();
      rightTableEquiJoinColumns = new List<Var>();
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
      {
        Var leftVar;
        Var rightVar;
        if (Predicate.IsEquiJoinPredicate(part, leftTableDefinitions, rightTableDefinitions, out leftVar, out rightVar))
        {
          leftTableEquiJoinColumns.Add(leftVar);
          rightTableEquiJoinColumns.Add(rightVar);
        }
        else
          otherPredicates.AddPart(part);
      }
    }

    internal Predicate GetJoinPredicates(
      VarVec leftTableDefinitions,
      VarVec rightTableDefinitions,
      out Predicate otherPredicates)
    {
      Predicate predicate = new Predicate(this.m_command);
      otherPredicates = new Predicate(this.m_command);
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
      {
        if (Predicate.IsEquiJoinPredicate(part, leftTableDefinitions, rightTableDefinitions, out Var _, out Var _))
          predicate.AddPart(part);
        else
          otherPredicates.AddPart(part);
      }
      return predicate;
    }

    internal bool SatisfiesKey(VarVec keyVars, VarVec definitions)
    {
      if (keyVars.Count <= 0)
        return false;
      VarVec varVec = keyVars.Clone();
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
      {
        if (part.Op.OpType == OpType.EQ)
        {
          Var keyVar;
          if (this.IsKeyPredicate(part.Child0, part.Child1, keyVars, definitions, out keyVar))
            varVec.Clear(keyVar);
          else if (this.IsKeyPredicate(part.Child1, part.Child0, keyVars, definitions, out keyVar))
            varVec.Clear(keyVar);
        }
      }
      return varVec.IsEmpty;
    }

    internal bool PreservesNulls(VarVec tableColumns, bool ansiNullSemantics)
    {
      if (!ansiNullSemantics)
        return true;
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
      {
        if (!Predicate.PreservesNulls(part, tableColumns))
          return false;
      }
      return true;
    }

    private void InitFromAndTree(System.Data.Entity.Core.Query.InternalTrees.Node andTree)
    {
      if (andTree.Op.OpType == OpType.And)
      {
        this.InitFromAndTree(andTree.Child0);
        this.InitFromAndTree(andTree.Child1);
      }
      else
        this.m_parts.Add(andTree);
    }

    private void GetSingleTablePredicates(
      List<VarVec> tableDefinitions,
      out List<Predicate> singleTablePredicates,
      out Predicate otherPredicates)
    {
      singleTablePredicates = new List<Predicate>();
      foreach (VarVec tableDefinition in tableDefinitions)
        singleTablePredicates.Add(new Predicate(this.m_command));
      otherPredicates = new Predicate(this.m_command);
      VarVec varVec = this.m_command.CreateVarVec();
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node part in this.m_parts)
      {
        NodeInfo nodeInfo = this.m_command.GetNodeInfo(part);
        bool flag = false;
        for (int index = 0; index < tableDefinitions.Count; ++index)
        {
          VarVec tableDefinition = tableDefinitions[index];
          if (tableDefinition != null)
          {
            varVec.InitFrom(nodeInfo.ExternalReferences);
            varVec.Minus(tableDefinition);
            if (varVec.IsEmpty)
            {
              flag = true;
              singleTablePredicates[index].AddPart(part);
              break;
            }
          }
        }
        if (!flag)
          otherPredicates.AddPart(part);
      }
    }

    private static bool IsEquiJoinPredicate(
      System.Data.Entity.Core.Query.InternalTrees.Node simplePredicateNode,
      out Var leftVar,
      out Var rightVar)
    {
      leftVar = (Var) null;
      rightVar = (Var) null;
      if (simplePredicateNode.Op.OpType != OpType.EQ || !(simplePredicateNode.Child0.Op is VarRefOp op1) || !(simplePredicateNode.Child1.Op is VarRefOp op2))
        return false;
      leftVar = op1.Var;
      rightVar = op2.Var;
      return true;
    }

    private static bool IsEquiJoinPredicate(
      System.Data.Entity.Core.Query.InternalTrees.Node simplePredicateNode,
      VarVec leftTableDefinitions,
      VarVec rightTableDefinitions,
      out Var leftVar,
      out Var rightVar)
    {
      leftVar = (Var) null;
      rightVar = (Var) null;
      Var leftVar1;
      Var rightVar1;
      if (!Predicate.IsEquiJoinPredicate(simplePredicateNode, out leftVar1, out rightVar1))
        return false;
      if (leftTableDefinitions.IsSet(leftVar1) && rightTableDefinitions.IsSet(rightVar1))
      {
        leftVar = leftVar1;
        rightVar = rightVar1;
      }
      else
      {
        if (!leftTableDefinitions.IsSet(rightVar1) || !rightTableDefinitions.IsSet(leftVar1))
          return false;
        leftVar = rightVar1;
        rightVar = leftVar1;
      }
      return true;
    }

    private static bool PreservesNulls(System.Data.Entity.Core.Query.InternalTrees.Node simplePredNode, VarVec tableColumns)
    {
      switch (simplePredNode.Op.OpType)
      {
        case OpType.GT:
        case OpType.GE:
        case OpType.LE:
        case OpType.LT:
        case OpType.EQ:
        case OpType.NE:
          return (!(simplePredNode.Child0.Op is VarRefOp op6) || !tableColumns.IsSet(op6.Var)) && (!(simplePredNode.Child1.Op is VarRefOp op7) || !tableColumns.IsSet(op7.Var));
        case OpType.Like:
          return !(simplePredNode.Child1.Op is ConstantBaseOp op8) || op8.OpType == OpType.Null || (!(simplePredNode.Child0.Op is VarRefOp op9) || !tableColumns.IsSet(op9.Var));
        case OpType.Not:
          return simplePredNode.Child0.Op.OpType != OpType.IsNull || !(simplePredNode.Child0.Child0.Op is VarRefOp op10) || !tableColumns.IsSet(op10.Var);
        default:
          return true;
      }
    }

    private bool IsKeyPredicate(
      System.Data.Entity.Core.Query.InternalTrees.Node left,
      System.Data.Entity.Core.Query.InternalTrees.Node right,
      VarVec keyVars,
      VarVec definitions,
      out Var keyVar)
    {
      keyVar = (Var) null;
      if (left.Op.OpType != OpType.VarRef)
        return false;
      VarRefOp op = (VarRefOp) left.Op;
      keyVar = op.Var;
      if (!keyVars.IsSet(keyVar))
        return false;
      VarVec varVec = this.m_command.GetNodeInfo(right).ExternalReferences.Clone();
      varVec.And(definitions);
      return varVec.IsEmpty;
    }
  }
}
