// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.VarRemapper
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class VarRemapper : BasicOpVisitor
  {
    private readonly IDictionary<Var, Var> m_varMap;
    protected readonly Command m_command;

    internal VarRemapper(Command command)
      : this(command, (IDictionary<Var, Var>) new Dictionary<Var, Var>())
    {
    }

    internal VarRemapper(Command command, IDictionary<Var, Var> varMap)
    {
      this.m_command = command;
      this.m_varMap = varMap;
    }

    internal void AddMapping(Var oldVar, Var newVar) => this.m_varMap[oldVar] = newVar;

    internal virtual void RemapNode(System.Data.Entity.Core.Query.InternalTrees.Node node)
    {
      if (this.m_varMap.Count == 0)
        return;
      this.VisitNode(node);
    }

    internal virtual void RemapSubtree(System.Data.Entity.Core.Query.InternalTrees.Node subTree)
    {
      if (this.m_varMap.Count == 0)
        return;
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node child in subTree.Children)
        this.RemapSubtree(child);
      this.RemapNode(subTree);
      this.m_command.RecomputeNodeInfo(subTree);
    }

    internal VarList RemapVarList(VarList varList) => Command.CreateVarList(this.MapVars((IEnumerable<Var>) varList));

    internal static VarList RemapVarList(
      Command command,
      IDictionary<Var, Var> varMap,
      VarList varList)
    {
      return new VarRemapper(command, varMap).RemapVarList(varList);
    }

    private Var Map(Var v)
    {
      Var var;
      while (this.m_varMap.TryGetValue(v, out var))
        v = var;
      return v;
    }

    private IEnumerable<Var> MapVars(IEnumerable<Var> vars)
    {
      foreach (Var var in vars)
        yield return this.Map(var);
    }

    private void Map(VarVec vec)
    {
      VarVec varVec = this.m_command.CreateVarVec(this.MapVars((IEnumerable<Var>) vec));
      vec.InitFrom(varVec);
    }

    private void Map(VarList varList)
    {
      VarList varList1 = Command.CreateVarList(this.MapVars((IEnumerable<Var>) varList));
      varList.Clear();
      varList.AddRange((IEnumerable<Var>) varList1);
    }

    private void Map(VarMap varMap)
    {
      VarMap varMap1 = new VarMap();
      foreach (KeyValuePair<Var, Var> var1 in varMap)
      {
        Var var2 = this.Map(var1.Value);
        varMap1.Add(var1.Key, var2);
      }
      varMap.Clear();
      foreach (KeyValuePair<Var, Var> keyValuePair in varMap1)
        varMap.Add(keyValuePair.Key, keyValuePair.Value);
    }

    private void Map(List<SortKey> sortKeys)
    {
      VarVec varVec = this.m_command.CreateVarVec();
      bool flag = false;
      foreach (SortKey sortKey in sortKeys)
      {
        sortKey.Var = this.Map(sortKey.Var);
        if (varVec.IsSet(sortKey.Var))
          flag = true;
        varVec.Set(sortKey.Var);
      }
      if (!flag)
        return;
      List<SortKey> sortKeyList = new List<SortKey>((IEnumerable<SortKey>) sortKeys);
      sortKeys.Clear();
      varVec.Clear();
      foreach (SortKey sortKey in sortKeyList)
      {
        if (!varVec.IsSet(sortKey.Var))
          sortKeys.Add(sortKey);
        varVec.Set(sortKey.Var);
      }
    }

    protected override void VisitDefault(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
    }

    public override void Visit(VarRefOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      Var v = this.Map(op.Var);
      if (v == op.Var)
        return;
      n.Op = (Op) this.m_command.CreateVarRefOp(v);
    }

    protected override void VisitNestOp(NestBaseOp op, System.Data.Entity.Core.Query.InternalTrees.Node n) => throw new NotSupportedException();

    public override void Visit(PhysicalProjectOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitPhysicalOpDefault((PhysicalOp) op, n);
      this.Map(op.Outputs);
      SimpleCollectionColumnMap columnMap = (SimpleCollectionColumnMap) ColumnMapTranslator.Translate((ColumnMap) op.ColumnMap, this.m_varMap);
      n.Op = (Op) this.m_command.CreatePhysicalProjectOp(op.Outputs, columnMap);
    }

    protected override void VisitGroupByOp(GroupByBaseOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      this.Map(op.Outputs);
      this.Map(op.Keys);
    }

    public override void Visit(GroupByIntoOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitGroupByOp((GroupByBaseOp) op, n);
      this.Map(op.Inputs);
    }

    public override void Visit(DistinctOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      this.Map(op.Keys);
    }

    public override void Visit(ProjectOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      this.Map(op.Outputs);
    }

    public override void Visit(UnnestOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      Var v = this.Map(op.Var);
      if (v == op.Var)
        return;
      n.Op = (Op) this.m_command.CreateUnnestOp(v, op.Table);
    }

    protected override void VisitSetOp(SetOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      this.Map(op.VarMap[0]);
      this.Map(op.VarMap[1]);
    }

    protected override void VisitSortOp(SortBaseOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitRelOpDefault((RelOp) op, n);
      this.Map(op.Keys);
    }
  }
}
