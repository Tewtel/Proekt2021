﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.TypeConventionWithHaving`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Types;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  internal class TypeConventionWithHaving<T> : TypeConventionWithHavingBase<T>
    where T : class
  {
    private readonly Action<ConventionTypeConfiguration, T> _entityConfigurationAction;

    public TypeConventionWithHaving(
      IEnumerable<Func<Type, bool>> predicates,
      Func<Type, T> capturingPredicate,
      Action<ConventionTypeConfiguration, T> entityConfigurationAction)
      : base(predicates, capturingPredicate)
    {
      this._entityConfigurationAction = entityConfigurationAction;
    }

    internal Action<ConventionTypeConfiguration, T> EntityConfigurationAction => this._entityConfigurationAction;

    protected override void InvokeAction(
      Type memberInfo,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      T value)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, modelConfiguration), value);
    }

    protected override void InvokeAction(
      Type memberInfo,
      Func<EntityTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      T value)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, configuration, modelConfiguration), value);
    }

    protected override void InvokeAction(
      Type memberInfo,
      Func<ComplexTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      T value)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration(memberInfo, configuration, modelConfiguration), value);
    }
  }
}
