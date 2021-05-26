// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.PropertyConventionBase
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  internal abstract class PropertyConventionBase : 
    IConfigurationConvention<PropertyInfo, System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration>,
    IConvention
  {
    private readonly IEnumerable<Func<PropertyInfo, bool>> _predicates;

    public PropertyConventionBase(IEnumerable<Func<PropertyInfo, bool>> predicates) => this._predicates = predicates;

    internal IEnumerable<Func<PropertyInfo, bool>> Predicates => this._predicates;

    public void Apply(
      PropertyInfo memberInfo,
      Func<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      if (!this._predicates.All<Func<PropertyInfo, bool>>((Func<Func<PropertyInfo, bool>, bool>) (p => p(memberInfo))))
        return;
      this.ApplyCore(memberInfo, configuration, modelConfiguration);
    }

    protected abstract void ApplyCore(
      PropertyInfo memberInfo,
      Func<System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration);
  }
}
