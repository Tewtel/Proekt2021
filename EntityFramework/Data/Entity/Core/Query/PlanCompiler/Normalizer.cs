// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.Normalizer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class Normalizer : SubqueryTrackingVisitor
  {
    private Normalizer(System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler planCompilerState)
      : base(planCompilerState)
    {
    }

    internal static void Process(System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler planCompilerState) => new Normalizer(planCompilerState).Process();

    private void Process() => this.m_command.Root = this.VisitNode(this.m_command.Root);

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(ExistsOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      n.Child0 = this.BuildDummyProjectForExists(n.Child0);
      return n;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildDummyProjectForExists(
      System.Data.Entity.Core.Query.InternalTrees.Node child)
    {
      return this.m_command.BuildProject(child, this.m_command.CreateNode((Op) this.m_command.CreateInternalConstantOp(this.m_command.IntegerType, (object) 1)), out Var _);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildUnnest(System.Data.Entity.Core.Query.InternalTrees.Node collectionNode)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(collectionNode.Op.IsScalarOp, "non-scalar usage of Un-nest?");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(collectionNode.Op.Type), "non-collection usage for Un-nest?");
      Var computedVar;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefNode = this.m_command.CreateVarDefNode(collectionNode, out computedVar);
      return this.m_command.CreateNode((Op) this.m_command.CreateUnnestOp(computedVar), varDefNode);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitCollectionFunction(
      FunctionOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(TypeSemantics.IsCollectionType(op.Type), "non-TVF function?");
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.BuildUnnest(n);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreatePhysicalProjectOp((node1.Op as UnnestOp).Table.Columns[0]), node1);
      return this.m_command.CreateNode((Op) this.m_command.CreateCollectOp(n.Op.Type), node2);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitCollectionAggregateFunction(
      FunctionOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      TypeUsage type = (TypeUsage) null;
      System.Data.Entity.Core.Query.InternalTrees.Node child0 = n.Child0;
      if (OpType.SoftCast == child0.Op.OpType)
      {
        type = TypeHelpers.GetEdmType<CollectionType>(child0.Op.Type).TypeUsage;
        child0 = child0.Child0;
        while (OpType.SoftCast == child0.Op.OpType)
          child0 = child0.Child0;
      }
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.BuildUnnest(child0);
      Var column = (node1.Op as UnnestOp).Table.Columns[0];
      AggregateOp aggregateOp = this.m_command.CreateAggregateOp(op.Function, false);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(column));
      if (type != null)
        node2 = this.m_command.CreateNode((Op) this.m_command.CreateSoftCastOp(type), node2);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.CreateNode((Op) aggregateOp, node2);
      VarVec varVec1 = this.m_command.CreateVarVec();
      System.Data.Entity.Core.Query.InternalTrees.Node node4 = this.m_command.CreateNode((Op) this.m_command.CreateVarDefListOp());
      VarVec varVec2 = this.m_command.CreateVarVec();
      Var computedVar;
      System.Data.Entity.Core.Query.InternalTrees.Node varDefListNode = this.m_command.CreateVarDefListNode(node3, out computedVar);
      varVec2.Set(computedVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node5 = this.m_command.CreateNode((Op) this.m_command.CreateGroupByOp(varVec1, varVec2), node1, node4, varDefListNode);
      return this.AddSubqueryToParentRelOp(computedVar, node5);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      FunctionOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      System.Data.Entity.Core.Query.InternalTrees.Node node = !TypeSemantics.IsCollectionType(op.Type) ? (!PlanCompilerUtil.IsCollectionAggregateFunction(op, n) ? n : this.VisitCollectionAggregateFunction(op, n)) : this.VisitCollectionFunction(op, n);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(node != null, "failure to construct a functionOp?");
      return node;
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitJoinOp(
      JoinBaseOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (this.ProcessJoinOp(n))
        n.Child2.Child0 = this.BuildDummyProjectForExists(n.Child2.Child0);
      return n;
    }
  }
}
