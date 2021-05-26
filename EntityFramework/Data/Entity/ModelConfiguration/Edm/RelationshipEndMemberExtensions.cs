// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.RelationshipEndMemberExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class RelationshipEndMemberExtensions
  {
    public static bool IsMany(this RelationshipEndMember associationEnd) => associationEnd.RelationshipMultiplicity.IsMany();

    public static bool IsOptional(this RelationshipEndMember associationEnd) => associationEnd.RelationshipMultiplicity.IsOptional();

    public static bool IsRequired(this RelationshipEndMember associationEnd) => associationEnd.RelationshipMultiplicity.IsRequired();
  }
}
