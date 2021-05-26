// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ScanTableBaseOp
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class ScanTableBaseOp : RelOp
  {
    private readonly Table m_table;

    protected ScanTableBaseOp(OpType opType, Table table)
      : base(opType)
    {
      this.m_table = table;
    }

    protected ScanTableBaseOp(OpType opType)
      : base(opType)
    {
    }

    internal Table Table => this.m_table;
  }
}
