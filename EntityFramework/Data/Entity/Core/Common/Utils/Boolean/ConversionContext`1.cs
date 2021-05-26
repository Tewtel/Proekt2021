// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.ConversionContext`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal abstract class ConversionContext<T_Identifier>
  {
    internal readonly Solver Solver = new Solver();

    internal abstract Vertex TranslateTermToVertex(TermExpr<T_Identifier> term);

    internal abstract IEnumerable<LiteralVertexPair<T_Identifier>> GetSuccessors(
      Vertex vertex);
  }
}
