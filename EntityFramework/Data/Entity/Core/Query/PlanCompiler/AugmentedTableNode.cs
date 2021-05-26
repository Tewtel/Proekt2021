// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.AugmentedTableNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal sealed class AugmentedTableNode : AugmentedNode
  {
    private readonly Table m_table;
    private AugmentedTableNode m_replacementTable;
    private int m_newLocationId;

    internal AugmentedTableNode(int id, System.Data.Entity.Core.Query.InternalTrees.Node node)
      : base(id, node)
    {
      this.m_table = ((ScanTableBaseOp) node.Op).Table;
      this.LastVisibleId = id;
      this.m_replacementTable = this;
      this.m_newLocationId = id;
    }

    internal Table Table => this.m_table;

    internal int LastVisibleId { get; set; }

    internal bool IsEliminated => this.m_replacementTable != this;

    internal AugmentedTableNode ReplacementTable
    {
      get => this.m_replacementTable;
      set => this.m_replacementTable = value;
    }

    internal int NewLocationId
    {
      get => this.m_newLocationId;
      set => this.m_newLocationId = value;
    }

    internal bool IsMoved => this.m_newLocationId != this.Id;

    internal VarVec NullableColumns { get; set; }
  }
}
