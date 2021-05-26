// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ManyToManyNavigationPropertyConfiguration`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Configures a many:many relationship.
  /// This configuration functionality is available via the Code First Fluent API, see <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the parent entity of the navigation property specified in the HasMany call.</typeparam>
  /// <typeparam name="TTargetEntityType">The type of the parent entity of the navigation property specified in the WithMany call.</typeparam>
  public class ManyToManyNavigationPropertyConfiguration<TEntityType, TTargetEntityType>
    where TEntityType : class
    where TTargetEntityType : class
  {
    private readonly NavigationPropertyConfiguration _navigationPropertyConfiguration;

    internal ManyToManyNavigationPropertyConfiguration(
      NavigationPropertyConfiguration navigationPropertyConfiguration)
    {
      this._navigationPropertyConfiguration = navigationPropertyConfiguration;
    }

    /// <summary>
    /// Configures the foreign key column(s) and table used to store the relationship.
    /// </summary>
    /// <param name="configurationAction"> Action that configures the foreign key column(s) and table. </param>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ManyToManyNavigationPropertyConfiguration`2" /> instance so that multiple calls can be chained.</returns>
    public ManyToManyNavigationPropertyConfiguration<TEntityType, TTargetEntityType> Map(
      Action<ManyToManyAssociationMappingConfiguration> configurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ManyToManyAssociationMappingConfiguration>>(configurationAction, nameof (configurationAction));
      ManyToManyAssociationMappingConfiguration mappingConfiguration = new ManyToManyAssociationMappingConfiguration();
      configurationAction(mappingConfiguration);
      this._navigationPropertyConfiguration.AssociationMappingConfiguration = (AssociationMappingConfiguration) mappingConfiguration;
      return this;
    }

    /// <summary>
    /// Configures stored procedures to be used for modifying this relationship.
    /// The default conventions for procedure and parameter names will be used.
    /// </summary>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ManyToManyNavigationPropertyConfiguration`2" /> instance so that multiple calls can be chained.</returns>
    public ManyToManyNavigationPropertyConfiguration<TEntityType, TTargetEntityType> MapToStoredProcedures()
    {
      if (this._navigationPropertyConfiguration.ModificationStoredProceduresConfiguration == null)
        this._navigationPropertyConfiguration.ModificationStoredProceduresConfiguration = new ModificationStoredProceduresConfiguration();
      return this;
    }

    /// <summary>
    /// Configures stored procedures to be used for modifying this relationship.
    /// </summary>
    /// <param name="modificationStoredProcedureMappingConfigurationAction">
    /// Configuration to override the default conventions for procedure and parameter names.
    /// </param>
    /// <returns>The same <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ManyToManyNavigationPropertyConfiguration`2" /> instance so that multiple calls can be chained.</returns>
    public ManyToManyNavigationPropertyConfiguration<TEntityType, TTargetEntityType> MapToStoredProcedures(
      Action<ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType>> modificationStoredProcedureMappingConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType>>>(modificationStoredProcedureMappingConfigurationAction, nameof (modificationStoredProcedureMappingConfigurationAction));
      ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType> proceduresConfiguration = new ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType>();
      modificationStoredProcedureMappingConfigurationAction(proceduresConfiguration);
      if (this._navigationPropertyConfiguration.ModificationStoredProceduresConfiguration == null)
        this._navigationPropertyConfiguration.ModificationStoredProceduresConfiguration = proceduresConfiguration.Configuration;
      else
        this._navigationPropertyConfiguration.ModificationStoredProceduresConfiguration.Merge(proceduresConfiguration.Configuration, true);
      return this;
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
