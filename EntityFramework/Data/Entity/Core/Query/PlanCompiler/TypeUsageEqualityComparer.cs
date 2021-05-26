// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.TypeUsageEqualityComparer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal sealed class TypeUsageEqualityComparer : IEqualityComparer<TypeUsage>
  {
    internal static readonly TypeUsageEqualityComparer Instance = new TypeUsageEqualityComparer();

    private TypeUsageEqualityComparer()
    {
    }

    public bool Equals(TypeUsage x, TypeUsage y) => x != null && y != null && TypeUsageEqualityComparer.Equals(x.EdmType, y.EdmType);

    public int GetHashCode(TypeUsage obj) => obj.EdmType.Identity.GetHashCode();

    internal static bool Equals(EdmType x, EdmType y) => x.Identity.Equals(y.Identity);
  }
}
