// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.RelProperty
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal sealed class RelProperty
  {
    private readonly RelationshipType m_relationshipType;
    private readonly RelationshipEndMember m_fromEnd;
    private readonly RelationshipEndMember m_toEnd;

    internal RelProperty(
      RelationshipType relationshipType,
      RelationshipEndMember fromEnd,
      RelationshipEndMember toEnd)
    {
      this.m_relationshipType = relationshipType;
      this.m_fromEnd = fromEnd;
      this.m_toEnd = toEnd;
    }

    public RelationshipType Relationship => this.m_relationshipType;

    public RelationshipEndMember FromEnd => this.m_fromEnd;

    public RelationshipEndMember ToEnd => this.m_toEnd;

    public override bool Equals(object obj) => obj is RelProperty relProperty && this.Relationship.EdmEquals((MetadataItem) relProperty.Relationship) && this.FromEnd.EdmEquals((MetadataItem) relProperty.FromEnd) && this.ToEnd.EdmEquals((MetadataItem) relProperty.ToEnd);

    public override int GetHashCode() => this.ToEnd.Identity.GetHashCode();

    [DebuggerNonUserCode]
    public override string ToString() => this.m_relationshipType?.ToString() + ":" + this.m_fromEnd?.ToString() + ":" + this.m_toEnd?.ToString();
  }
}
