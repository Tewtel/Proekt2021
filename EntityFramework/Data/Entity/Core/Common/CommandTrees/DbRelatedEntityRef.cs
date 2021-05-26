// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.DbRelatedEntityRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.CommandTrees
{
  internal sealed class DbRelatedEntityRef
  {
    private readonly RelationshipEndMember _sourceEnd;
    private readonly RelationshipEndMember _targetEnd;
    private readonly DbExpression _targetEntityRef;

    internal DbRelatedEntityRef(
      RelationshipEndMember sourceEnd,
      RelationshipEndMember targetEnd,
      DbExpression targetEntityRef)
    {
      if (sourceEnd.DeclaringType != targetEnd.DeclaringType)
        throw new ArgumentException(Strings.Cqt_RelatedEntityRef_TargetEndFromDifferentRelationship, nameof (targetEnd));
      if (sourceEnd == targetEnd)
        throw new ArgumentException(Strings.Cqt_RelatedEntityRef_TargetEndSameAsSourceEnd, nameof (targetEnd));
      if (targetEnd.RelationshipMultiplicity != RelationshipMultiplicity.One && targetEnd.RelationshipMultiplicity != RelationshipMultiplicity.ZeroOrOne)
        throw new ArgumentException(Strings.Cqt_RelatedEntityRef_TargetEndMustBeAtMostOne, nameof (targetEnd));
      if (!TypeSemantics.IsReferenceType(targetEntityRef.ResultType))
        throw new ArgumentException(Strings.Cqt_RelatedEntityRef_TargetEntityNotRef, nameof (targetEntityRef));
      EntityTypeBase elementType1 = TypeHelpers.GetEdmType<RefType>(targetEnd.TypeUsage).ElementType;
      EntityTypeBase elementType2 = TypeHelpers.GetEdmType<RefType>(targetEntityRef.ResultType).ElementType;
      if (!elementType1.EdmEquals((MetadataItem) elementType2) && !TypeSemantics.IsSubTypeOf((EdmType) elementType2, (EdmType) elementType1))
        throw new ArgumentException(Strings.Cqt_RelatedEntityRef_TargetEntityNotCompatible, nameof (targetEntityRef));
      this._targetEntityRef = targetEntityRef;
      this._targetEnd = targetEnd;
      this._sourceEnd = sourceEnd;
    }

    internal RelationshipEndMember SourceEnd => this._sourceEnd;

    internal RelationshipEndMember TargetEnd => this._targetEnd;

    internal DbExpression TargetEntityReference => this._targetEntityRef;
  }
}
