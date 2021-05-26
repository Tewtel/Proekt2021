﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityProxyMemberInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class EntityProxyMemberInfo
  {
    private readonly EdmMember _member;
    private readonly int _propertyIndex;

    internal EntityProxyMemberInfo(EdmMember member, int propertyIndex)
    {
      this._member = member;
      this._propertyIndex = propertyIndex;
    }

    internal EdmMember EdmMember => this._member;

    internal int PropertyIndex => this._propertyIndex;
  }
}
