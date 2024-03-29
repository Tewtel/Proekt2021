﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows derived configuration classes for entities and complex types to be registered with a
  /// <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  /// <remarks>
  /// Derived configuration classes are created by deriving from <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration" />
  /// or <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration" /> and using a type to be included in the model as the generic
  /// parameter.
  /// Configuration can be performed without creating derived configuration classes via the Entity and ComplexType
  /// methods on <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </remarks>
  public class ConfigurationRegistrar
  {
    private readonly System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration _modelConfiguration;

    internal ConfigurationRegistrar(System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => this._modelConfiguration = modelConfiguration;

    /// <summary>
    /// Discovers all types that inherit from <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration" /> or
    /// <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration" /> in the given assembly and adds an instance
    /// of each discovered type to this registrar.
    /// </summary>
    /// <remarks>
    /// Note that only types that are abstract or generic type definitions are skipped. Every
    /// type that is discovered and added must provide a parameterless constructor.
    /// </remarks>
    /// <param name="assembly">The assembly containing model configurations to add.</param>
    /// <returns>The same ConfigurationRegistrar instance so that multiple calls can be chained.</returns>
    public virtual ConfigurationRegistrar AddFromAssembly(Assembly assembly)
    {
      System.Data.Entity.Utilities.Check.NotNull<Assembly>(assembly, nameof (assembly));
      new ConfigurationTypesFinder().AddConfigurationTypesToModel(assembly.GetAccessibleTypes(), this._modelConfiguration);
      return this;
    }

    /// <summary>
    /// Adds an <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration" /> to the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// Only one <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.EntityTypeConfiguration" /> can be added for each type in a model.
    /// </summary>
    /// <typeparam name="TEntityType"> The entity type being configured. </typeparam>
    /// <param name="entityTypeConfiguration"> The entity type configuration to be added. </param>
    /// <returns> The same ConfigurationRegistrar instance so that multiple calls can be chained. </returns>
    public virtual ConfigurationRegistrar Add<TEntityType>(
      EntityTypeConfiguration<TEntityType> entityTypeConfiguration)
      where TEntityType : class
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityTypeConfiguration<TEntityType>>(entityTypeConfiguration, nameof (entityTypeConfiguration));
      this._modelConfiguration.Add((EntityTypeConfiguration) entityTypeConfiguration.Configuration);
      return this;
    }

    /// <summary>
    /// Adds an <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration" /> to the <see cref="T:System.Data.Entity.DbModelBuilder" />.
    /// Only one <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.Types.ComplexTypeConfiguration" /> can be added for each type in a model.
    /// </summary>
    /// <typeparam name="TComplexType"> The complex type being configured. </typeparam>
    /// <param name="complexTypeConfiguration"> The complex type configuration to be added </param>
    /// <returns> The same ConfigurationRegistrar instance so that multiple calls can be chained. </returns>
    public virtual ConfigurationRegistrar Add<TComplexType>(
      ComplexTypeConfiguration<TComplexType> complexTypeConfiguration)
      where TComplexType : class
    {
      System.Data.Entity.Utilities.Check.NotNull<ComplexTypeConfiguration<TComplexType>>(complexTypeConfiguration, nameof (complexTypeConfiguration));
      this._modelConfiguration.Add((ComplexTypeConfiguration) complexTypeConfiguration.Configuration);
      return this;
    }

    internal virtual IEnumerable<Type> GetConfiguredTypes() => (IEnumerable<Type>) this._modelConfiguration.ConfiguredTypes.ToList<Type>();

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
