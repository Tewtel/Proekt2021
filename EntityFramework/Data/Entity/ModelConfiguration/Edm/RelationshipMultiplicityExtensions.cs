// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.RelationshipMultiplicityExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class RelationshipMultiplicityExtensions
  {
    public static bool IsMany(this RelationshipMultiplicity associationEndKind) => associationEndKind == RelationshipMultiplicity.Many;

    public static bool IsOptional(this RelationshipMultiplicity associationEndKind) => associationEndKind == RelationshipMultiplicity.ZeroOrOne;

    public static bool IsRequired(this RelationshipMultiplicity associationEndKind) => associationEndKind == RelationshipMultiplicity.One;
  }
}
