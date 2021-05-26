// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ObjectNavigationPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  internal class ObjectNavigationPropertyMapping : ObjectMemberMapping
  {
    internal ObjectNavigationPropertyMapping(
      NavigationProperty edmNavigationProperty,
      NavigationProperty clrNavigationProperty)
      : base((EdmMember) edmNavigationProperty, (EdmMember) clrNavigationProperty)
    {
    }

    internal override MemberMappingKind MemberMappingKind => MemberMappingKind.NavigationPropertyMapping;
  }
}
