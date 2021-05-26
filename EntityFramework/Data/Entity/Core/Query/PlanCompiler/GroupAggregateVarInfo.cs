// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.GroupAggregateVarInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class GroupAggregateVarInfo
  {
    private readonly System.Data.Entity.Core.Query.InternalTrees.Node _definingGroupByNode;
    private HashSet<KeyValuePair<System.Data.Entity.Core.Query.InternalTrees.Node, List<System.Data.Entity.Core.Query.InternalTrees.Node>>> _candidateAggregateNodes;
    private readonly Var _groupAggregateVar;

    internal GroupAggregateVarInfo(System.Data.Entity.Core.Query.InternalTrees.Node defingingGroupNode, Var groupAggregateVar)
    {
      this._definingGroupByNode = defingingGroupNode;
      this._groupAggregateVar = groupAggregateVar;
    }

    internal HashSet<KeyValuePair<System.Data.Entity.Core.Query.InternalTrees.Node, List<System.Data.Entity.Core.Query.InternalTrees.Node>>> CandidateAggregateNodes
    {
      get
      {
        if (this._candidateAggregateNodes == null)
          this._candidateAggregateNodes = new HashSet<KeyValuePair<System.Data.Entity.Core.Query.InternalTrees.Node, List<System.Data.Entity.Core.Query.InternalTrees.Node>>>();
        return this._candidateAggregateNodes;
      }
    }

    internal bool HasCandidateAggregateNodes => this._candidateAggregateNodes != null && (uint) this._candidateAggregateNodes.Count > 0U;

    internal System.Data.Entity.Core.Query.InternalTrees.Node DefiningGroupNode => this._definingGroupByNode;

    internal Var GroupAggregateVar => this._groupAggregateVar;
  }
}
