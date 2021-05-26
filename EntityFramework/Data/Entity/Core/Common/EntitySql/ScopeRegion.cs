// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ScopeRegion
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class ScopeRegion
  {
    private readonly ScopeManager _scopeManager;
    private readonly int _firstScopeIndex;
    private readonly int _scopeRegionIndex;
    private DbExpressionBinding _groupAggregateBinding;
    private readonly List<GroupAggregateInfo> _groupAggregateInfos = new List<GroupAggregateInfo>();
    private readonly HashSet<string> _groupAggregateNames = new HashSet<string>();

    internal ScopeRegion(ScopeManager scopeManager, int firstScopeIndex, int scopeRegionIndex)
    {
      this._scopeManager = scopeManager;
      this._firstScopeIndex = firstScopeIndex;
      this._scopeRegionIndex = scopeRegionIndex;
    }

    internal int FirstScopeIndex => this._firstScopeIndex;

    internal int ScopeRegionIndex => this._scopeRegionIndex;

    internal bool ContainsScope(int scopeIndex) => scopeIndex >= this._firstScopeIndex;

    internal void EnterGroupOperation(DbExpressionBinding groupAggregateBinding) => this._groupAggregateBinding = groupAggregateBinding;

    internal void RollbackGroupOperation() => this._groupAggregateBinding = (DbExpressionBinding) null;

    internal bool IsAggregating => this._groupAggregateBinding != null;

    internal DbExpressionBinding GroupAggregateBinding => this._groupAggregateBinding;

    internal List<GroupAggregateInfo> GroupAggregateInfos => this._groupAggregateInfos;

    internal void RegisterGroupAggregateName(string groupAggregateName) => this._groupAggregateNames.Add(groupAggregateName);

    internal bool ContainsGroupAggregate(string groupAggregateName) => this._groupAggregateNames.Contains(groupAggregateName);

    internal bool WasResolutionCorrelated { get; set; }

    internal void ApplyToScopeEntries(Action<ScopeEntry> action)
    {
      for (int firstScopeIndex = this.FirstScopeIndex; firstScopeIndex <= this._scopeManager.CurrentScopeIndex; ++firstScopeIndex)
      {
        foreach (KeyValuePair<string, ScopeEntry> keyValuePair in this._scopeManager.GetScopeByIndex(firstScopeIndex))
          action(keyValuePair.Value);
      }
    }

    internal void ApplyToScopeEntries(Func<ScopeEntry, ScopeEntry> action)
    {
      for (int firstScopeIndex = this.FirstScopeIndex; firstScopeIndex <= this._scopeManager.CurrentScopeIndex; ++firstScopeIndex)
      {
        Scope scope = this._scopeManager.GetScopeByIndex(firstScopeIndex);
        List<KeyValuePair<string, ScopeEntry>> ts = (List<KeyValuePair<string, ScopeEntry>>) null;
        foreach (KeyValuePair<string, ScopeEntry> keyValuePair in scope)
        {
          ScopeEntry scopeEntry = action(keyValuePair.Value);
          if (keyValuePair.Value != scopeEntry)
          {
            if (ts == null)
              ts = new List<KeyValuePair<string, ScopeEntry>>();
            ts.Add(new KeyValuePair<string, ScopeEntry>(keyValuePair.Key, scopeEntry));
          }
        }
        if (ts != null)
          ts.Each<KeyValuePair<string, ScopeEntry>>((Action<KeyValuePair<string, ScopeEntry>>) (updatedScopeEntry => scope.Replace(updatedScopeEntry.Key, updatedScopeEntry.Value)));
      }
    }

    internal void RollbackAllScopes() => this._scopeManager.RollbackToScope(this.FirstScopeIndex - 1);
  }
}
