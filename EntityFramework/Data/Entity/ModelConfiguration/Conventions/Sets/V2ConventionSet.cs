// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.Sets.V2ConventionSet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.ModelConfiguration.Conventions.Sets
{
  internal static class V2ConventionSet
  {
    private static readonly ConventionSet _conventions;

    static V2ConventionSet()
    {
      List<IConvention> conventionList = new List<IConvention>(V1ConventionSet.Conventions.StoreModelConventions);
      int index = conventionList.FindIndex((Predicate<IConvention>) (c => c.GetType() == typeof (ColumnOrderingConvention)));
      conventionList[index] = (IConvention) new ColumnOrderingConventionStrict();
      V2ConventionSet._conventions = new ConventionSet(V1ConventionSet.Conventions.ConfigurationConventions, V1ConventionSet.Conventions.ConceptualModelConventions, V1ConventionSet.Conventions.ConceptualToStoreMappingConventions, (IEnumerable<IConvention>) conventionList);
    }

    public static ConventionSet Conventions => V2ConventionSet._conventions;
  }
}
