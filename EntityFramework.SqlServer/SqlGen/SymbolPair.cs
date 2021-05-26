// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SymbolPair
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

namespace System.Data.Entity.SqlServer.SqlGen
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
