﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.TypeConvention`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  internal class TypeConvention<T> : TypeConventionBase where T : class
  {
    private static readonly Func<Type, bool> _ofTypePredicate = (Func<Type, bool>) (t => typeof (T).IsAssignableFrom(t));
    private readonly Action<ConventionTypeConfiguration<T>> _entityConfigurationAction;

    public TypeConvention(
      IEnumerable<Func<Type, bool>> predicates,
      Action<ConventionTypeConfiguration<T>> entityConfigurationAction)
      : base(predicates.Prepend<Func<Type, bool>>(TypeConvention<T>._ofTypePredicate))
    {
      this._entityConfigurationAction = entityConfigurationAction;
    }

    internal Action<ConventionTypeConfiguration<T>> EntityConfigurationAction => this._entityConfigurationAction;

    internal static Func<Type, bool> OfTypePredicate => TypeConvention<T>._ofTypePredicate;

    protected override void ApplyCore(Type memberInfo, System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => this._entityConfigurationAction(new ConventionTypeConfiguration<T>(memberInfo, modelConfiguration));

    protected override void ApplyCore(
      Type memberInfo,
      Func<EntityTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration<T>(memberInfo, configuration, modelConfiguration));
    }

    protected override void ApplyCore(
      Type memberInfo,
      Func<ComplexTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._entityConfigurationAction(new ConventionTypeConfiguration<T>(memberInfo, configuration, modelConfiguration));
    }
  }
}
