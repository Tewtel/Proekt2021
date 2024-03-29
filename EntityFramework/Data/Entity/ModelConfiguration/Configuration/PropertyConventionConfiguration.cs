﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.PropertyConventionConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a lightweight convention based on
  /// the properties in a model.
  /// </summary>
  public class PropertyConventionConfiguration
  {
    private readonly ConventionsConfiguration _conventionsConfiguration;
    private readonly IEnumerable<Func<PropertyInfo, bool>> _predicates;

    internal PropertyConventionConfiguration(ConventionsConfiguration conventionsConfiguration)
      : this(conventionsConfiguration, Enumerable.Empty<Func<PropertyInfo, bool>>())
    {
    }

    private PropertyConventionConfiguration(
      ConventionsConfiguration conventionsConfiguration,
      IEnumerable<Func<PropertyInfo, bool>> predicates)
    {
      this._conventionsConfiguration = conventionsConfiguration;
      this._predicates = predicates;
    }

    internal ConventionsConfiguration ConventionsConfiguration => this._conventionsConfiguration;

    internal IEnumerable<Func<PropertyInfo, bool>> Predicates => this._predicates;

    /// <summary>
    /// Filters the properties that this convention applies to based on a predicate.
    /// </summary>
    /// <param name="predicate"> A function to test each property for a condition. </param>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.PropertyConventionConfiguration" /> instance so that multiple calls can be chained.
    /// </returns>
    public PropertyConventionConfiguration Where(
      Func<PropertyInfo, bool> predicate)
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<PropertyInfo, bool>>(predicate, nameof (predicate));
      return new PropertyConventionConfiguration(this._conventionsConfiguration, this._predicates.Append<Func<PropertyInfo, bool>>(predicate));
    }

    /// <summary>
    /// Filters the properties that this convention applies to based on a predicate
    /// while capturing a value to use later during configuration.
    /// </summary>
    /// <typeparam name="T"> Type of the captured value. </typeparam>
    /// <param name="capturingPredicate">
    /// A function to capture a value for each property. If the value is null, the
    /// property will be filtered out.
    /// </param>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.PropertyConventionWithHavingConfiguration`1" /> instance so that multiple calls can be chained.
    /// </returns>
    public PropertyConventionWithHavingConfiguration<T> Having<T>(
      Func<PropertyInfo, T> capturingPredicate)
      where T : class
    {
      System.Data.Entity.Utilities.Check.NotNull<Func<PropertyInfo, T>>(capturingPredicate, nameof (capturingPredicate));
      return new PropertyConventionWithHavingConfiguration<T>(this._conventionsConfiguration, this._predicates, capturingPredicate);
    }

    /// <summary>
    /// Allows configuration of the properties that this convention applies to.
    /// </summary>
    /// <param name="propertyConfigurationAction">
    /// An action that performs configuration against a
    /// <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ConventionPrimitivePropertyConfiguration" />
    /// .
    /// </param>
    public void Configure(
      Action<ConventionPrimitivePropertyConfiguration> propertyConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ConventionPrimitivePropertyConfiguration>>(propertyConfigurationAction, nameof (propertyConfigurationAction));
      this._conventionsConfiguration.Add((IConvention) new PropertyConvention(this._predicates, propertyConfigurationAction));
    }

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}
