// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.GroupAggregateVarInfoManager
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class GroupAggregateVarInfoManager
  {
    private readonly Dictionary<Var, GroupAggregateVarRefInfo> _groupAggregateVarRelatedVarToInfo = new Dictionary<Var, GroupAggregateVarRefInfo>();
    private Dictionary<Var, Dictionary<EdmMember, GroupAggregateVarRefInfo>> _groupAggregateVarRelatedVarPropertyToInfo;
    private readonly HashSet<GroupAggregateVarInfo> _groupAggregateVarInfos = new HashSet<GroupAggregateVarInfo>();

    internal IEnumerable<GroupAggregateVarInfo> GroupAggregateVarInfos => (IEnumerable<GroupAggregateVarInfo>) this._groupAggregateVarInfos;

    internal void Add(
      Var var,
      GroupAggregateVarInfo groupAggregateVarInfo,
      System.Data.Entity.Core.Query.InternalTrees.Node computationTemplate,
      bool isUnnested)
    {
      this._groupAggregateVarRelatedVarToInfo.Add(var, new GroupAggregateVarRefInfo(groupAggregateVarInfo, computationTemplate, isUnnested));
      this._groupAggregateVarInfos.Add(groupAggregateVarInfo);
    }

    internal void Add(
      Var var,
      GroupAggregateVarInfo groupAggregateVarInfo,
      System.Data.Entity.Core.Query.InternalTrees.Node computationTemplate,
      bool isUnnested,
      EdmMember property)
    {
      if (property == null)
      {
        this.Add(var, groupAggregateVarInfo, computationTemplate, isUnnested);
      }
      else
      {
        if (this._groupAggregateVarRelatedVarPropertyToInfo == null)
          this._groupAggregateVarRelatedVarPropertyToInfo = new Dictionary<Var, Dictionary<EdmMember, GroupAggregateVarRefInfo>>();
        Dictionary<EdmMember, GroupAggregateVarRefInfo> dictionary;
        if (!this._groupAggregateVarRelatedVarPropertyToInfo.TryGetValue(var, out dictionary))
        {
          dictionary = new Dictionary<EdmMember, GroupAggregateVarRefInfo>();
          this._groupAggregateVarRelatedVarPropertyToInfo.Add(var, dictionary);
        }
        dictionary.Add(property, new GroupAggregateVarRefInfo(groupAggregateVarInfo, computationTemplate, isUnnested));
        this._groupAggregateVarInfos.Add(groupAggregateVarInfo);
      }
    }

    internal bool TryGetReferencedGroupAggregateVarInfo(
      Var var,
      out GroupAggregateVarRefInfo groupAggregateVarRefInfo)
    {
      return this._groupAggregateVarRelatedVarToInfo.TryGetValue(var, out groupAggregateVarRefInfo);
    }

    internal bool TryGetReferencedGroupAggregateVarInfo(
      Var var,
      EdmMember property,
      out GroupAggregateVarRefInfo groupAggregateVarRefInfo)
    {
      if (property == null)
        return this.TryGetReferencedGroupAggregateVarInfo(var, out groupAggregateVarRefInfo);
      Dictionary<EdmMember, GroupAggregateVarRefInfo> dictionary;
      if (this._groupAggregateVarRelatedVarPropertyToInfo != null && this._groupAggregateVarRelatedVarPropertyToInfo.TryGetValue(var, out dictionary))
        return dictionary.TryGetValue(property, out groupAggregateVarRefInfo);
      groupAggregateVarRefInfo = (GroupAggregateVarRefInfo) null;
      return false;
    }
  }
}
