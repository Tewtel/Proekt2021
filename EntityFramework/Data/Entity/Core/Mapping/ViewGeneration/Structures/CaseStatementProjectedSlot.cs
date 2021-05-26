// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.CaseStatementProjectedSlot
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal sealed class CaseStatementProjectedSlot : ProjectedSlot
  {
    private readonly CaseStatement m_caseStatement;
    private readonly IEnumerable<WithRelationship> m_withRelationships;

    internal CaseStatementProjectedSlot(
      CaseStatement statement,
      IEnumerable<WithRelationship> withRelationships)
    {
      this.m_caseStatement = statement;
      this.m_withRelationships = withRelationships;
    }

    internal override ProjectedSlot DeepQualify(CqlBlock block) => (ProjectedSlot) new CaseStatementProjectedSlot(this.m_caseStatement.DeepQualify(block), (IEnumerable<WithRelationship>) null);

    internal override StringBuilder AsEsql(
      StringBuilder builder,
      MemberPath outputMember,
      string blockAlias,
      int indentLevel)
    {
      this.m_caseStatement.AsEsql(builder, this.m_withRelationships, blockAlias, indentLevel);
      return builder;
    }

    internal override DbExpression AsCqt(DbExpression row, MemberPath outputMember) => this.m_caseStatement.AsCqt(row, this.m_withRelationships);

    internal override void ToCompactString(StringBuilder builder) => this.m_caseStatement.ToCompactString(builder);
  }
}
