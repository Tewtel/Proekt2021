// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportNormalizedEntityTypeMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  internal sealed class FunctionImportNormalizedEntityTypeMapping
  {
    internal readonly ReadOnlyCollection<FunctionImportEntityTypeMappingCondition> ColumnConditions;
    internal readonly BitArray ImpliedEntityTypes;
    internal readonly BitArray ComplementImpliedEntityTypes;

    internal FunctionImportNormalizedEntityTypeMapping(
      FunctionImportStructuralTypeMappingKB parent,
      List<FunctionImportEntityTypeMappingCondition> columnConditions,
      BitArray impliedEntityTypes)
    {
      this.ColumnConditions = new ReadOnlyCollection<FunctionImportEntityTypeMappingCondition>((IList<FunctionImportEntityTypeMappingCondition>) columnConditions.ToList<FunctionImportEntityTypeMappingCondition>());
      this.ImpliedEntityTypes = impliedEntityTypes;
      this.ComplementImpliedEntityTypes = new BitArray(this.ImpliedEntityTypes).Not();
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Values={0}, Types={1}", (object) StringUtil.ToCommaSeparatedString((IEnumerable) this.ColumnConditions), (object) StringUtil.ToCommaSeparatedString((IEnumerable) this.ImpliedEntityTypes));
  }
}
