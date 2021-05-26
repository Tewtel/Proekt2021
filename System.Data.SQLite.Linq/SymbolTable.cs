// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SymbolTable
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections.Generic;

namespace System.Data.SQLite.Linq
{
  internal sealed class SymbolTable
  {
    private List<Dictionary<string, Symbol>> symbols = new List<Dictionary<string, Symbol>>();

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
