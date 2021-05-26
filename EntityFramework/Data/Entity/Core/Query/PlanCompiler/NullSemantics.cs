// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.NullSemantics
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;
using System.Linq;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class NullSemantics : BasicOpVisitorOfNode
  {
    private Command _command;
    private bool _modified;
    private bool _negated;
    private NullSemantics.VariableNullabilityTable _variableNullabilityTable = new NullSemantics.VariableNullabilityTable(32);

    private NullSemantics(Command command) => this._command = command;

    public static bool Process(Command command)
    {
      NullSemantics nullSemantics = new NullSemantics(command);
      command.Root = nullSemantics.VisitNode(command.Root);
      return nullSemantics._modified;
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitDefault(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      bool negated = this._negated;
      switch (n.Op.OpType)
      {
        case OpType.EQ:
          this._negated = false;
          n = this.HandleEQ(n, negated);
          break;
        case OpType.NE:
          n = this.HandleNE(n);
          break;
        case OpType.And:
          n = base.VisitDefault(n);
          break;
        case OpType.Or:
          n = this.HandleOr(n);
          break;
        case OpType.Not:
          this._negated = !this._negated;
          n = base.VisitDefault(n);
          break;
        default:
          this._negated = false;
          n = base.VisitDefault(n);
          break;
      }
      this._negated = negated;
      return n;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node HandleOr(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node = n.Child0.Op.OpType == OpType.IsNull ? n.Child0 : (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      if (node == null || node.Child0.Op.OpType != OpType.VarRef)
        return base.VisitDefault(n);
      Var var = ((VarRefOp) node.Child0.Op).Var;
      bool flag = this._variableNullabilityTable[var];
      this._variableNullabilityTable[var] = false;
      n.Child1 = this.VisitNode(n.Child1);
      this._variableNullabilityTable[var] = flag;
      return n;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node HandleEQ(System.Data.Entity.Core.Query.InternalTrees.Node n, bool negated)
    {
      this._modified = ((this._modified ? 1 : 0) | (n.Child0 != (n.Child0 = this.VisitNode(n.Child0)) || n.Child1 != (n.Child1 = this.VisitNode(n.Child1)) ? 1 : (n != (n = this.ImplementEquality(n, negated)) ? 1 : 0))) != 0;
      return n;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node HandleNE(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      ComparisonOp op = (ComparisonOp) n.Op;
      n = this._command.CreateNode((Op) this._command.CreateConditionalOp(OpType.Not), this._command.CreateNode((Op) this._command.CreateComparisonOp(OpType.EQ, op.UseDatabaseNullSemantics), n.Child0, n.Child1));
      this._modified = true;
      return base.VisitDefault(n);
    }

    private bool IsNullableVarRef(System.Data.Entity.Core.Query.InternalTrees.Node n) => n.Op.OpType == OpType.VarRef && this._variableNullabilityTable[((VarRefOp) n.Op).Var];

    private System.Data.Entity.Core.Query.InternalTrees.Node ImplementEquality(
      System.Data.Entity.Core.Query.InternalTrees.Node n,
      bool negated)
    {
      if (((ComparisonOp) n.Op).UseDatabaseNullSemantics)
        return n;
      System.Data.Entity.Core.Query.InternalTrees.Node child0 = n.Child0;
      System.Data.Entity.Core.Query.InternalTrees.Node child1 = n.Child1;
      switch (child0.Op.OpType)
      {
        case OpType.Constant:
        case OpType.InternalConstant:
        case OpType.NullSentinel:
          switch (child1.Op.OpType)
          {
            case OpType.Constant:
            case OpType.InternalConstant:
            case OpType.NullSentinel:
              return n;
            case OpType.Null:
              return this.False();
            default:
              return !negated ? n : this.And(n, this.Not(this.IsNull(this.Clone(child1))));
          }
        case OpType.Null:
          switch (child1.Op.OpType)
          {
            case OpType.Constant:
            case OpType.InternalConstant:
            case OpType.NullSentinel:
              return this.False();
            case OpType.Null:
              return this.True();
            default:
              return this.IsNull(child1);
          }
        default:
          switch (child1.Op.OpType)
          {
            case OpType.Constant:
            case OpType.InternalConstant:
            case OpType.NullSentinel:
              return !negated || !this.IsNullableVarRef(n) ? n : this.And(n, this.Not(this.IsNull(this.Clone(child0))));
            case OpType.Null:
              return this.IsNull(child0);
            default:
              return !negated ? this.Or(n, this.And(this.IsNull(this.Clone(child0)), this.IsNull(this.Clone(child1)))) : this.And(n, this.NotXor(this.Clone(child0), this.Clone(child1)));
          }
      }
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node Clone(System.Data.Entity.Core.Query.InternalTrees.Node x) => OpCopier.Copy(this._command, x);

    private System.Data.Entity.Core.Query.InternalTrees.Node False() => this._command.CreateNode((Op) this._command.CreateFalseOp());

    private System.Data.Entity.Core.Query.InternalTrees.Node True() => this._command.CreateNode((Op) this._command.CreateTrueOp());

    private System.Data.Entity.Core.Query.InternalTrees.Node IsNull(System.Data.Entity.Core.Query.InternalTrees.Node x) => this._command.CreateNode((Op) this._command.CreateConditionalOp(OpType.IsNull), x);

    private System.Data.Entity.Core.Query.InternalTrees.Node Not(System.Data.Entity.Core.Query.InternalTrees.Node x) => this._command.CreateNode((Op) this._command.CreateConditionalOp(OpType.Not), x);

    private System.Data.Entity.Core.Query.InternalTrees.Node And(System.Data.Entity.Core.Query.InternalTrees.Node x, System.Data.Entity.Core.Query.InternalTrees.Node y) => this._command.CreateNode((Op) this._command.CreateConditionalOp(OpType.And), x, y);

    private System.Data.Entity.Core.Query.InternalTrees.Node Or(System.Data.Entity.Core.Query.InternalTrees.Node x, System.Data.Entity.Core.Query.InternalTrees.Node y) => this._command.CreateNode((Op) this._command.CreateConditionalOp(OpType.Or), x, y);

    private System.Data.Entity.Core.Query.InternalTrees.Node Boolean(bool value) => this._command.CreateNode((Op) this._command.CreateConstantOp(this._command.BooleanType, (object) value));

    private System.Data.Entity.Core.Query.InternalTrees.Node NotXor(System.Data.Entity.Core.Query.InternalTrees.Node x, System.Data.Entity.Core.Query.InternalTrees.Node y) => this._command.CreateNode((Op) this._command.CreateComparisonOp(OpType.EQ), this._command.CreateNode((Op) this._command.CreateCaseOp(this._command.BooleanType), this.IsNull(x), this.Boolean(true), this.Boolean(false)), this._command.CreateNode((Op) this._command.CreateCaseOp(this._command.BooleanType), this.IsNull(y), this.Boolean(true), this.Boolean(false)));

    private struct VariableNullabilityTable
    {
      private bool[] _entries;

      public VariableNullabilityTable(int capacity) => this._entries = Enumerable.Repeat<bool>(true, capacity).ToArray<bool>();

      public bool this[Var variable]
      {
        get => variable.Id >= this._entries.Length || this._entries[variable.Id];
        set
        {
          this.EnsureCapacity(variable.Id + 1);
          this._entries[variable.Id] = value;
        }
      }

      private void EnsureCapacity(int minimum)
      {
        if (this._entries.Length >= minimum)
          return;
        int count = this._entries.Length * 2;
        if (count < minimum)
          count = minimum;
        bool[] array = Enumerable.Repeat<bool>(true, count).ToArray<bool>();
        Array.Copy((Array) this._entries, 0, (Array) array, 0, this._entries.Length);
        this._entries = array;
      }
    }
  }
}
