// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.SortKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class SortKey
  {
    private readonly bool m_asc;
    private readonly string m_collation;

    internal SortKey(Var v, bool asc, string collation)
    {
      this.Var = v;
      this.m_asc = asc;
      this.m_collation = collation;
    }

    internal Var Var { get; set; }

    internal bool AscendingSort => this.m_asc;

    internal string Collation => this.m_collation;
  }
}
