﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ObjectPropertyMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping
{
  internal class ObjectPropertyMapping : ObjectMemberMapping
  {
    internal ObjectPropertyMapping(EdmProperty edmProperty, EdmProperty clrProperty)
      : base((EdmMember) edmProperty, (EdmMember) clrProperty)
    {
    }

    internal EdmProperty ClrProperty => (EdmProperty) this.ClrMember;

    internal override MemberMappingKind MemberMappingKind => MemberMappingKind.ScalarPropertyMapping;
  }
}
