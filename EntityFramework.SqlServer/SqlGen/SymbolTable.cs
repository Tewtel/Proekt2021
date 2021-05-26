// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SymbolTable
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal sealed class SymbolTable
  {
    private readonly List<Dictionary<string, Symbol>> symbols = new List<Dictionary<string, Symbol>>();

    internal void EnterScope() => this.symbols.Add(new Dictionary<string, Symbol>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase));

    internal void ExitScope() => this.symbols.RemoveAt(this.symbols.Count - 1);

    internal void Add(string name, Symbol value) => this.symbols[this.symbols.Count - 1][name] = value;

    internal Symbol Lookup(string name)
    {
      for (int index = this.symbols.Count - 1; index >= 0; --index)
      {
        if (this.symbols[index].ContainsKey(name))
          return this.symbols[index][name];
      }
      return (Symbol) null;
    }
  }
}
