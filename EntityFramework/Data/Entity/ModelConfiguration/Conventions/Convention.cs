// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.Convention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration.Properties;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Conventions.Sets;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>A convention that doesn't override configuration.</summary>
  public class Convention : IConvention
  {
    private readonly ConventionsConfiguration _conventionsConfiguration = new ConventionsConfiguration(new ConventionSet());

    /// <summary>
    /// The derived class can use the default constructor to apply a set rule of that change the model configuration.
    /// </summary>
    public Convention()
    {
    }

    internal Convention(ConventionsConfiguration conventionsConfiguration) => this._conventionsConfiguration = conventionsConfiguration;

    /// <summary>
    /// Begins configuration of a lightweight convention that applies to all mapped types in
    /// the model.
    /// </summary>
    /// <returns> A configuration object for the convention. </returns>
    public TypeConventionConfiguration Types() => new TypeConventionConfiguration(this._conventionsConfiguration);

    /// <summary>
    /// Begins configuration of a lightweight convention that applies to all mapped types in
    /// the model that derive from or implement the specified type.
    /// </summary>
    /// <typeparam name="T"> The type of the entities that this convention will apply to. </typeparam>
    /// <returns> A configuration object for the convention. </returns>
    /// <remarks> This method does not add new types to the model.</remarks>
    public TypeConventionConfiguration<T> Types<T>() where T : class => new TypeConventionConfiguration<T>(this._conventionsConfiguration);

    /// <summary>
    /// Begins configuration of a lightweight convention that applies to all properties
    /// in the model.
    /// </summary>
    /// <returns> A configuration object for the convention. </returns>
    public PropertyConventionConfiguration Properties() => new PropertyConventionConfiguration(this._conventionsConfiguration);

    /// <summary>
    /// Begins configuration of a lightweight convention that applies to all primitive
    /// properties of the specified type in the model.
    /// </summary>
    /// <typeparam name="T"> The type of the properties that the convention will apply to. </typeparam>
    /// <returns> A configuration object for the convention. </returns>
    /// <remarks>
    /// The convention will apply to both nullable and non-nullable properties of the
    /// specified type.
    /// </remarks>
    public PropertyConventionConfiguration Properties<T>()
    {
      if (!typeof (T).IsValidEdmScalarType())
        throw Error.ModelBuilder_PropertyFilterTypeMustBePrimitive((object) typeof (T));
      return new PropertyConventionConfiguration(this._conventionsConfiguration).Where((Func<PropertyInfo, bool>) (p =>
      {
        Type underlyingType;
        p.PropertyType.TryUnwrapNullableType(out underlyingType);
        return underlyingType == typeof (T);
      }));
    }

    internal virtual void ApplyModelConfiguration(System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => this._conventionsConfiguration.ApplyModelConfiguration(modelConfiguration);

    internal virtual void ApplyModelConfiguration(Type type, System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration) => this._conventionsConfiguration.ApplyModelConfiguration(type, modelConfiguration);

    internal virtual void ApplyTypeConfiguration<TStructuralTypeConfiguration>(
      Type type,
      Func<TStructuralTypeConfiguration> structuralTypeConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
      where TStructuralTypeConfiguration : StructuralTypeConfiguration
    {
      this._conventionsConfiguration.ApplyTypeConfiguration<TStructuralTypeConfiguration>(type, structuralTypeConfiguration, modelConfiguration);
    }

    internal virtual void ApplyPropertyConfiguration(
      PropertyInfo propertyInfo,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._conventionsConfiguration.ApplyPropertyConfiguration(propertyInfo, modelConfiguration);
    }

    internal virtual void ApplyPropertyConfiguration(
      PropertyInfo propertyInfo,
      Func<PropertyConfiguration> propertyConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
    {
      this._conventionsConfiguration.ApplyPropertyConfiguration(propertyInfo, propertyConfiguration, modelConfiguration);
    }

    internal virtual void ApplyPropertyTypeConfiguration<TStructuralTypeConfiguration>(
      PropertyInfo propertyInfo,
      Func<TStructuralTypeConfiguration> structuralTypeConfiguration,
      System.Data.Entity.ModelConfiguration.Configuration.ModelConfiguration modelConfiguration)
      where TStructuralTypeConfiguration : StructuralTypeConfiguration
    {
      this._conventionsConfiguration.ApplyPropertyTypeConfiguration<TStructuralTypeConfiguration>(propertyInfo, structuralTypeConfiguration, modelConfiguration);
    }
  }
}
