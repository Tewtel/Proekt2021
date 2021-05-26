// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.ChangeNode
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  internal class ChangeNode
  {
    private readonly TypeUsage m_elementType;
    private readonly List<PropagatorResult> m_inserted = new List<PropagatorResult>();
    private readonly List<PropagatorResult> m_deleted = new List<PropagatorResult>();

    internal ChangeNode(TypeUsage elementType) => this.m_elementType = elementType;

    internal TypeUsage ElementType => this.m_elementType;

    internal List<PropagatorResult> Inserted => this.m_inserted;

    internal List<PropagatorResult> Deleted => this.m_deleted;

    internal PropagatorResult Placeholder { get; set; }
  }
}
