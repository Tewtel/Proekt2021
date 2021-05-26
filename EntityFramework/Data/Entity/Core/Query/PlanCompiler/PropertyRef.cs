// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.PropertyRef
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal abstract class PropertyRef
  {
    internal virtual PropertyRef CreateNestedPropertyRef(PropertyRef p) => (PropertyRef) new NestedPropertyRef(p, this);

    internal PropertyRef CreateNestedPropertyRef(EdmMember p) => this.CreateNestedPropertyRef((PropertyRef) new SimplePropertyRef(p));

    internal PropertyRef CreateNestedPropertyRef(RelProperty p) => this.CreateNestedPropertyRef((PropertyRef) new RelPropertyRef(p));

    public override string ToString() => "";
  }
}
