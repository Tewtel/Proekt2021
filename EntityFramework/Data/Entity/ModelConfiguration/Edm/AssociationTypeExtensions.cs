// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.AssociationTypeExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class AssociationTypeExtensions
  {
    private const string IsIndependentAnnotation = "IsIndependent";
    private const string IsPrincipalConfiguredAnnotation = "IsPrincipalConfigured";

    public static void MarkIndependent(this AssociationType associationType) => associationType.GetMetadataProperties().SetAnnotation("IsIndependent", (object) true);

    public static bool IsIndependent(this AssociationType associationType)
    {
      object annotation = associationType.Annotations.GetAnnotation(nameof (IsIndependent));
      return annotation != null && (bool) annotation;
    }

    public static void MarkPrincipalConfigured(this AssociationType associationType) => associationType.GetMetadataProperties().SetAnnotation("IsPrincipalConfigured", (object) true);

    public static bool IsPrincipalConfigured(this AssociationType associationType)
    {
      object annotation = associationType.Annotations.GetAnnotation(nameof (IsPrincipalConfigured));
      return annotation != null && (bool) annotation;
    }

    public static AssociationEndMember GetOtherEnd(
      this AssociationType associationType,
      AssociationEndMember associationEnd)
    {
      return associationEnd != associationType.SourceEnd ? associationType.SourceEnd : associationType.TargetEnd;
    }

    public static object GetConfiguration(this AssociationType associationType) => associationType.Annotations.GetConfiguration();

    public static void SetConfiguration(this AssociationType associationType, object configuration) => associationType.GetMetadataProperties().SetConfiguration(configuration);

    public static bool IsRequiredToMany(this AssociationType associationType) => associationType.SourceEnd.IsRequired() && associationType.TargetEnd.IsMany();

    public static bool IsRequiredToRequired(this AssociationType associationType) => associationType.SourceEnd.IsRequired() && associationType.TargetEnd.IsRequired();

    public static bool IsManyToRequired(this AssociationType associationType) => associationType.SourceEnd.IsMany() && associationType.TargetEnd.IsRequired();

    public static bool IsManyToMany(this AssociationType associationType) => associationType.SourceEnd.IsMany() && associationType.TargetEnd.IsMany();

    public static bool IsOneToOne(this AssociationType associationType) => !associationType.SourceEnd.IsMany() && !associationType.TargetEnd.IsMany();

    public static bool IsSelfReferencing(this AssociationType associationType)
    {
      AssociationEndMember sourceEnd = associationType.SourceEnd;
      AssociationEndMember targetEnd = associationType.TargetEnd;
      return sourceEnd.GetEntityType().GetRootType() == targetEnd.GetEntityType().GetRootType();
    }

    public static bool IsRequiredToNonRequired(this AssociationType associationType)
    {
      if (associationType.SourceEnd.IsRequired() && !associationType.TargetEnd.IsRequired())
        return true;
      return associationType.TargetEnd.IsRequired() && !associationType.SourceEnd.IsRequired();
    }

    public static bool TryGuessPrincipalAndDependentEnds(
      this AssociationType associationType,
      out AssociationEndMember principalEnd,
      out AssociationEndMember dependentEnd)
    {
      principalEnd = dependentEnd = (AssociationEndMember) null;
      AssociationEndMember sourceEnd = associationType.SourceEnd;
      AssociationEndMember targetEnd = associationType.TargetEnd;
      if (sourceEnd.RelationshipMultiplicity != targetEnd.RelationshipMultiplicity)
      {
        principalEnd = sourceEnd.IsRequired() || sourceEnd.IsOptional() && targetEnd.IsMany() ? sourceEnd : targetEnd;
        dependentEnd = principalEnd == sourceEnd ? targetEnd : sourceEnd;
      }
      return principalEnd != null;
    }
  }
}
