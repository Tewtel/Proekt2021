// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.SymbolPair
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

namespace System.Data.SQLite.Linq
{
  internal class SymbolPair : ISqlFragment
  {
    public Symbol Source;
    public Symbol Column;

    public SymbolPair(Symbol source, Symbol column)
    {
      this.Source = source;
      this.Column = column;
    }

    public void WriteSql(SqlWriter writer, SqlGenerator sqlGenerator)
    {
    }
  }
}
