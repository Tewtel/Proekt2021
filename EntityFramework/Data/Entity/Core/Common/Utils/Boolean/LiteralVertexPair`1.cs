// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.LiteralVertexPair`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class LiteralVertexPair<T_Identifier>
  {
    internal readonly Vertex Vertex;
    internal readonly System.Data.Entity.Core.Common.Utils.Boolean.Literal<T_Identifier> Literal;

    internal LiteralVertexPair(Vertex vertex, System.Data.Entity.Core.Common.Utils.Boolean.Literal<T_Identifier> literal)
    {
      this.Vertex = vertex;
      this.Literal = literal;
    }
  }
}
