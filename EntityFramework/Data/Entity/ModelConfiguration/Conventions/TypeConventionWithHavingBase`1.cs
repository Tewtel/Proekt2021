﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.TypeConventionWithHavingBase`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration.Types;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  internal abstract class TypeConventionWithHavingBase<T> : TypeConventionBase where T : class
  {
    private readonly Func<Type, T> _capturingPredicate;

    public TypeConventionWithHavingBase(
      IEnumerable<Func<Type, bool>> predicates,
      Func<Type, T> capturingPredicate)
      : base(predicates)
    {
      this._capturingPredicate = capturingPredicate;
    }

    internal Func<Type, T> CapturingPredicate => this._capturingPredicate;

    protected override void ApplyCore(Type memberInfo, System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      T obj = this._capturingPredicate(memberInfo);
      if ((object) obj == null)
        return;
      this.InvokeAction(memberInfo, modelConfiguration, obj);
    }

    protected abstract void InvokeAction(
      Type memberInfo,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration configuration,
      T value);

    protected override sealed void ApplyCore(
      Type memberInfo,
      Func<EntityTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      T obj = this._capturingPredicate(memberInfo);
      if ((object) obj == null)
        return;
      this.InvokeAction(memberInfo, configuration, modelConfiguration, obj);
    }

    protected abstract void InvokeAction(
      Type memberInfo,
      Func<EntityTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      T value);

    protected override void ApplyCore(
      Type memberInfo,
      Func<ComplexTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      T obj = this._capturingPredicate(memberInfo);
      if ((object) obj == null)
        return;
      this.InvokeAction(memberInfo, configuration, modelConfiguration, obj);
    }

    protected abstract void InvokeAction(
      Type memberInfo,
      Func<ComplexTypeConfiguration> configuration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration,
      T value);
  }
}
