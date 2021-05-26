// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.DnfSentence`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class DnfSentence<T_Identifier> : Sentence<T_Identifier, DnfClause<T_Identifier>>
  {
    internal DnfSentence(Set<DnfClause<T_Identifier>> clauses)
      : base(clauses, ExprType.Or)
    {
    }
  }
}
