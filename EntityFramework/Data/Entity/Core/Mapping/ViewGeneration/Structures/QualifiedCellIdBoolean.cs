// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.QualifiedCellIdBoolean
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class QualifiedCellIdBoolean : CellIdBoolean
  {
    private readonly CqlBlock m_block;

    internal QualifiedCellIdBoolean(
      CqlBlock block,
      CqlIdentifiers identifiers,
      int originalCellNum)
      : base(identifiers, originalCellNum)
    {
      this.m_block = block;
    }

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      string blockAlias,
      bool skipIsNotNull)
    {
      return base.AsEsql(builder, this.m_block.CqlAlias, skipIsNotNull);
    }

    internal override DbExpression AsCqt(DbExpression row, bool skipIsNotNull) => base.AsCqt(this.m_block.GetInput(row), skipIsNotNull);
  }
}
