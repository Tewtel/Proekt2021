﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.TypeConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Types;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  internal class TypeConvention : TypeConventionBase
  {
    private readonly Action<ConventionTypeConfiguration> _entityConfigurationAction;

    public TypeConvention(
      IEnumerable<Func<Type, bool>> predicates,
      Action<ConventionTypeConfiguration> entityConfigurationAction)
      : base(predicates)
    {
      this._entityConfigurationAction = entityConfigurationAction;
    }

    internal Action<ConventionTypeConfiguration> EntityConfigurationAction => this._entityConfigurationAction;

    protected override void ApplyCore(Type memberInfo, System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, modelConfiguration));

    protected override void ApplyCore(
      Type memberInfo,
      Func<EntityTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, configuration, modelConfiguration));
    }

    protected override void ApplyCore(
      Type memberInfo,
      Func<ComplexTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, configuration, modelConfiguration));
    }
  }
}
