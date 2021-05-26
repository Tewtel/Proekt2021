// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.MemberMaps
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class MemberMaps
  {
    private readonly MemberProjectionIndex m_projectedSlotMap;
    private readonly MemberDomainMap m_queryDomainMap;
    private readonly MemberDomainMap m_updateDomainMap;
    private readonly ViewTarget m_viewTarget;

    internal MemberMaps(
      ViewTarget viewTarget,
      MemberProjectionIndex projectedSlotMap,
      MemberDomainMap queryDomainMap,
      MemberDomainMap updateDomainMap)
    {
      this.m_projectedSlotMap = projectedSlotMap;
      this.m_queryDomainMap = queryDomainMap;
      this.m_updateDomainMap = updateDomainMap;
      this.m_viewTarget = viewTarget;
    }

    internal MemberProjectionIndex ProjectedSlotMap => this.m_projectedSlotMap;

    internal MemberDomainMap QueryDomainMap => this.m_queryDomainMap;

    internal MemberDomainMap UpdateDomainMap => this.m_updateDomainMap;

    internal MemberDomainMap RightDomainMap => this.m_viewTarget != ViewTarget.QueryView ? this.m_queryDomainMap : this.m_updateDomainMap;

    internal MemberDomainMap LeftDomainMap => this.m_viewTarget != ViewTarget.QueryView ? this.m_updateDomainMap : this.m_queryDomainMap;
  }
}
