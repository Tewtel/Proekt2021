// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ModificationStoredProceduresConfiguration`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a stored procedure that is used to modify entities.
  /// </summary>
  /// <typeparam name="TEntityType">The type of the entity that the stored procedure can be used to modify.</typeparam>
  public class ModificationStoredProceduresConfiguration<TEntityType> where TEntityType : class
  {
    private readonly ModificationStoredProceduresConfiguration _configuration = new ModificationStoredProceduresConfiguration();

    internal ModificationStoredProceduresConfiguration()
    {
    }

    internal ModificationStoredProceduresConfiguration Configuration => this._configuration;

    /// <summary>Configures stored procedure used to insert entities.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="modificationStoredProcedureConfigurationAction">A lambda expression that performs configuration for the stored procedure.</param>
    public ModificationStoredProceduresConfiguration<TEntityType> Insert(
      Action<InsertModificationStoredProcedureConfiguration<TEntityType>> modificationStoredProcedureConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<InsertModificationStoredProcedureConfiguration<TEntityType>>>(modificationStoredProcedureConfigurationAction, nameof (modificationStoredProcedureConfigurationAction));
      InsertModificationStoredProcedureConfiguration<TEntityType> procedureConfiguration = new InsertModificationStoredProcedureConfiguration<TEntityType>();
      modificationStoredProcedureConfigurationAction(procedureConfiguration);
      this._configuration.Insert(procedureConfiguration.Configuration);
      return this;
    }

    /// <summary>Configures stored procedure used to update entities.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="modificationStoredProcedureConfigurationAction">A lambda expression that performs configuration for the stored procedure.</param>
    public ModificationStoredProceduresConfiguration<TEntityType> Update(
      Action<UpdateModificationStoredProcedureConfiguration<TEntityType>> modificationStoredProcedureConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<UpdateModificationStoredProcedureConfiguration<TEntityType>>>(modificationStoredProcedureConfigurationAction, nameof (modificationStoredProcedureConfigurationAction));
      UpdateModificationStoredProcedureConfiguration<TEntityType> procedureConfiguration = new UpdateModificationStoredProcedureConfiguration<TEntityType>();
      modificationStoredProcedureConfigurationAction(procedureConfiguration);
      this._configuration.Update(procedureConfiguration.Configuration);
      return this;
    }

    /// <summary>Configures stored procedure used to delete entities.</summary>
    /// <returns> The same configuration instance so that multiple calls can be chained. </returns>
    /// <param name="modificationStoredProcedureConfigurationAction">A lambda expression that performs configuration for the stored procedure.</param>
    public ModificationStoredProceduresConfiguration<TEntityType> Delete(
      Action<DeleteModificationStoredProcedureConfiguration<TEntityType>> modificationStoredProcedureConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<DeleteModificationStoredProcedureConfiguration<TEntityType>>>(modificationStoredProcedureConfigurationAction, nameof (modificationStoredProcedureConfigurationAction));
      DeleteModificationStoredProcedureConfiguration<TEntityType> procedureConfiguration = new DeleteModificationStoredProcedureConfiguration<TEntityType>();
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
