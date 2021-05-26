// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SymbolPair
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

namespace System.Data.SQLite.EF6
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
