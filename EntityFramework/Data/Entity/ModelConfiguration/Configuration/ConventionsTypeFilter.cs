// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConventionsTypeFilter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  internal class ConventionsTypeFilter
  {
    public virtual bool IsConvention(Type conventionType) => ConventionsTypeFilter.IsConfigurationConvention(conventionType) || ConventionsTypeFilter.IsConceptualModelConvention(conventionType) || ConventionsTypeFilter.IsConceptualToStoreMappingConvention(conventionType) || ConventionsTypeFilter.IsStoreModelConvention(conventionType);

    public static bool IsConfigurationConvention(Type conventionType) => typeof (IConfigurationConvention).IsAssignableFrom(conventionType) || typeof (Convention).IsAssignableFrom(conventionType) || conventionType.GetGenericTypeImplementations(typeof (IConfigurationConvention<>)).Any<Type>() || conventionType.GetGenericTypeImplementations(typeof (IConfigurationConvention<,>)).Any<Type>();

    public static bool IsConceptualModelConvention(Type conventionType) => conventionType.GetGenericTypeImplementations(typeof (IConceptualModelConvention<>)).Any<Type>();

    public static bool IsStoreModelConvention(Type conventionType) => conventionType.GetGenericTypeImplementations(typeof (IStoreModelConvention<>)).Any<Type>();

    public static bool IsConceptualToStoreMappingConvention(Type conventionType) => typeof (IDbMappingConvention).IsAssignableFrom(conventionType);
  }
}
