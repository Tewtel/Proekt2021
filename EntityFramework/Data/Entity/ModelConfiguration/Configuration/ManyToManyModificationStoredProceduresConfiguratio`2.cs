// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ManyToManyModificationStoredProceduresConfiguration`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a stored procedure that is used to modify a many to many relationship.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the entity that the relationship is being configured from.</typeparam>
  /// <typeparam name="TTargetEntityType">The type of the entity that the other end of the relationship targets.</typeparam>
  public class ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType>
    where TEntityType : class
    where TTargetEntityType : class
  {
    private readonly ModificationStoredProceduresConfiguration _configuration = new ModificationStoredProceduresConfiguration();

    internal ManyToManyModificationStoredProceduresConfiguration()
    {
    }

    internal ModificationStoredProceduresConfiguration Configuration => this._configuration;

    /// <summary>Configures stored procedure used to insert relationships.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="modificationStoredProcedureConfigurationAction">A lambda expression that performs configuration for the stored procedure.</param>
    public ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType> Insert(
      Action<ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>> modificationStoredProcedureConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>>>(modificationStoredProcedureConfigurationAction, nameof (modificationStoredProcedureConfigurationAction));
      ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> procedureConfiguration = new ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>();
      modificationStoredProcedureConfigurationAction(procedureConfiguration);
      this._configuration.Insert(procedureConfiguration.Configuration);
      return this;
    }

    /// <summary>Configures stored procedure used to delete relationships.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="modificationStoredProcedureConfigurationAction">A lambda expression that performs configuration for the stored procedure.</param>
    public ManyToManyModificationStoredProceduresConfiguration<TEntityType, TTargetEntityType> Delete(
      Action<ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>> modificationStoredProcedureConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>>>(modificationStoredProcedureConfigurationAction, nameof (modificationStoredProcedureConfigurationAction));
      ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType> procedureConfiguration = new ManyToManyModificationStoredProcedureConfiguration<TEntityType, TTargetEntityType>();
      modificationStoredProcedureConfigurationAction(procedureConfiguration);
      this._configuration.Delete(procedureConfiguration.Configuration);
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
