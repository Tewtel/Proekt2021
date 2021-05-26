// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConventionsTypeFinder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  internal class ConventionsTypeFinder
  {
    private readonly ConventionsTypeFilter _conventionsTypeFilter;
    private readonly ConventionsTypeActivator _conventionsTypeActivator;

    public ConventionsTypeFinder()
      : this(new ConventionsTypeFilter(), new ConventionsTypeActivator())
    {
    }

    public ConventionsTypeFinder(
      ConventionsTypeFilter conventionsTypeFilter,
      ConventionsTypeActivator conventionsTypeActivator)
    {
      this._conventionsTypeFilter = conventionsTypeFilter;
      this._conventionsTypeActivator = conventionsTypeActivator;
    }

    public void AddConventions(IEnumerable<Type> types, Action<IConvention> addFunction)
    {
      foreach (Type type in types)
      {
        if (this._conventionsTypeFilter.IsConvention(type))
          addFunction(this._conventionsTypeActivator.Activate(type));
      }
    }
  }
}
