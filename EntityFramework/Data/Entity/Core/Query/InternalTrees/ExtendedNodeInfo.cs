// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ExtendedNodeInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class ExtendedNodeInfo : NodeInfo
  {
    private readonly VarVec m_localDefinitions;
    private readonly VarVec m_definitions;
    private readonly KeyVec m_keys;
    private readonly VarVec m_nonNullableDefinitions;
    private readonly VarVec m_nonNullableVisibleDefinitions;
    private RowCount m_minRows;
    private RowCount m_maxRows;

    internal ExtendedNodeInfo(Command cmd)
      : base(cmd)
    {
      this.m_localDefinitions = cmd.CreateVarVec();
      this.m_definitions = cmd.CreateVarVec();
      this.m_nonNullableDefinitions = cmd.CreateVarVec();
      this.m_nonNullableVisibleDefinitions = cmd.CreateVarVec();
      this.m_keys = new KeyVec(cmd);
      this.m_minRows = RowCount.Zero;
      this.m_maxRows = RowCount.Unbounded;
    }

    internal override void Clear()
    {
      base.Clear();
      this.m_definitions.Clear();
      this.m_localDefinitions.Clear();
      this.m_nonNullableDefinitions.Clear();
      this.m_nonNullableVisibleDefinitions.Clear();
      this.m_keys.Clear();
      this.m_minRows = RowCount.Zero;
      this.m_maxRows = RowCount.Unbounded;
    }

    internal override void ComputeHashValue(Command cmd, Node n)
    {
      base.ComputeHashValue(cmd, n);
      this.m_hashValue = this.m_hashValue << 4 ^ NodeInfo.GetHashValue(this.Definitions);
      this.m_hashValue = this.m_hashValue << 4 ^ NodeInfo.GetHashValue(this.Keys.KeyVars);
    }

    internal VarVec LocalDefinitions => this.m_localDefinitions;

    internal VarVec Definitions => this.m_definitions;

    internal KeyVec Keys => this.m_keys;

    internal VarVec NonNullableDefinitions => this.m_nonNullableDefinitions;

    internal VarVec NonNullableVisibleDefinitions => this.m_nonNullableVisibleDefinitions;

    internal RowCount MinRows
    {
      get => this.m_minRows;
      set => this.m_minRows = value;
    }

    internal RowCount MaxRows
    {
      get => this.m_maxRows;
      set => this.m_maxRows = value;
    }

    internal void SetRowCount(RowCount minRows, RowCount maxRows)
    {
      this.m_minRows = minRows;
      this.m_maxRows = maxRows;
    }

    internal void InitRowCountFrom(ExtendedNodeInfo source)
    {
      this.m_minRows = source.m_minRows;
      this.m_maxRows = source.m_maxRows;
    }

    [Conditional("DEBUG")]
    private void ValidateRowCount()
    {
    }
  }
}
