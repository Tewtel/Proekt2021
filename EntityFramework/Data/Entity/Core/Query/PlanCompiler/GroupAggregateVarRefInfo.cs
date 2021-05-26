// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.GroupAggregateVarRefInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class GroupAggregateVarRefInfo
  {
    private readonly System.Data.Entity.Core.Query.InternalTrees.Node _computation;
    private readonly GroupAggregateVarInfo _groupAggregateVarInfo;
    private readonly bool _isUnnested;

    internal GroupAggregateVarRefInfo(
      GroupAggregateVarInfo groupAggregateVarInfo,
      System.Data.Entity.Core.Query.InternalTrees.Node computation,
      bool isUnnested)
    {
      this._groupAggregateVarInfo = groupAggregateVarInfo;
      this._computation = computation;
      this._isUnnested = isUnnested;
    }

    internal System.Data.Entity.Core.Query.InternalTrees.Node Computation => this._computation;

    internal GroupAggregateVarInfo GroupAggregateVarInfo => this._groupAggregateVarInfo;

    internal bool IsUnnested => this._isUnnested;
  }
}
