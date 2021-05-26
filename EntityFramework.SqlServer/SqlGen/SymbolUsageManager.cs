// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SymbolUsageManager
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SymbolUsageManager
  {
    private readonly Dictionary<Symbol, BoolWrapper> optionalColumnUsage = new Dictionary<Symbol, BoolWrapper>();

    internal bool ContainsKey(Symbol key) => this.optionalColumnUsage.ContainsKey(key);

    internal bool TryGetValue(Symbol key, out bool value)
    {
      BoolWrapper boolWrapper;
      if (this.optionalColumnUsage.TryGetValue(key, out boolWrapper))
      {
        value = boolWrapper.Value;
        return true;
      }
      value = false;
      return false;
    }

    internal void Add(Symbol sourceSymbol, Symbol symbolToAdd)
    {
      BoolWrapper boolWrapper;
      if (sourceSymbol == null || !this.optionalColumnUsage.TryGetValue(sourceSymbol, out boolWrapper))
        boolWrapper = new BoolWrapper();
      this.optionalColumnUsage.Add(symbolToAdd, boolWrapper);
    }

    internal void MarkAsUsed(Symbol key)
    {
      if (!this.optionalColumnUsage.ContainsKey(key))
        return;
      this.optionalColumnUsage[key].Value = true;
    }

    internal bool IsUsed(Symbol key) => this.optionalColumnUsage[key].Value;
  }
}
