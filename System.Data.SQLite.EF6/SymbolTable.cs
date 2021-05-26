// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SymbolTable
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;

namespace System.Data.SQLite.EF6
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
