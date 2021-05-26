// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ScopeManager
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class ScopeManager
  {
    private readonly IEqualityComparer<string> _keyComparer;
    private readonly List<Scope> _scopes = new List<Scope>();

    internal ScopeManager(IEqualityComparer<string> keyComparer) => this._keyComparer = keyComparer;

    internal void EnterScope() => this._scopes.Add(new Scope(this._keyComparer));

    internal void LeaveScope() => this._scopes.RemoveAt(this.CurrentScopeIndex);

    internal int CurrentScopeIndex => this._scopes.Count - 1;

    internal Scope CurrentScope => this._scopes[this.CurrentScopeIndex];

    internal Scope GetScopeByIndex(int scopeIndex) => 0 <= scopeIndex && scopeIndex <= this.CurrentScopeIndex ? this._scopes[scopeIndex] : throw new EntitySqlException(Strings.InvalidScopeIndex);

    internal void RollbackToScope(int scopeIndex)
    {
      if (scopeIndex > this.CurrentScopeIndex || scopeIndex < 0 || this.CurrentScopeIndex < 0)
        throw new EntitySqlException(Strings.InvalidSavePoint);
      if (this.CurrentScopeIndex - scopeIndex <= 0)
        return;
      this._scopes.RemoveRange(scopeIndex + 1, this.CurrentScopeIndex - scopeIndex);
    }

    internal bool IsInCurrentScope(string key) => this.CurrentScope.Contains(key);
  }
}
