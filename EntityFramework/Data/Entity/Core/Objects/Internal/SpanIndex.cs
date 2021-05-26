// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.SpanIndex
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class SpanIndex
  {
    private Dictionary<RowType, Dictionary<int, AssociationEndMember>> _spanMap;
    private Dictionary<RowType, TypeUsage> _rowMap;

    internal void AddSpannedRowType(RowType spannedRowType, TypeUsage originalRowType)
    {
      if (this._rowMap == null)
        this._rowMap = new Dictionary<RowType, TypeUsage>((IEqualityComparer<RowType>) SpanIndex.RowTypeEqualityComparer.Instance);
      this._rowMap[spannedRowType] = originalRowType;
    }

    internal TypeUsage GetSpannedRowType(RowType spannedRowType)
    {
      TypeUsage typeUsage;
      return this._rowMap != null && this._rowMap.TryGetValue(spannedRowType, out typeUsage) ? typeUsage : (TypeUsage) null;
    }

    internal bool HasSpanMap(RowType spanRowType) => this._spanMap != null && this._spanMap.ContainsKey(spanRowType);

    internal void AddSpanMap(RowType rowType, Dictionary<int, AssociationEndMember> columnMap)
    {
      if (this._spanMap == null)
        this._spanMap = new Dictionary<RowType, Dictionary<int, AssociationEndMember>>((IEqualityComparer<RowType>) SpanIndex.RowTypeEqualityComparer.Instance);
      this._spanMap[rowType] = columnMap;
    }

    internal Dictionary<int, AssociationEndMember> GetSpanMap(
      RowType rowType)
    {
      Dictionary<int, AssociationEndMember> dictionary = (Dictionary<int, AssociationEndMember>) null;
      return this._spanMap != null && this._spanMap.TryGetValue(rowType, out dictionary) ? dictionary : (Dictionary<int, AssociationEndMember>) null;
    }

    private sealed class RowTypeEqualityComparer : IEqualityComparer<RowType>
    {
      internal static readonly SpanIndex.RowTypeEqualityComparer Instance = new SpanIndex.RowTypeEqualityComparer();

      private RowTypeEqualityComparer()
      {
      }

      public bool Equals(RowType x, RowType y) => x != null && y != null && x.EdmEquals((MetadataItem) y);

      public int GetHashCode(RowType obj) => obj.Identity.GetHashCode();
    }
  }
}
