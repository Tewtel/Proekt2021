// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.PrimaryKeyIndexConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>Configures a primary key index.</summary>
  public class PrimaryKeyIndexConfiguration
  {
    private readonly System.Data.Entity.ModelConfiguration.Configuration.Properties.Index.IndexConfiguration _configuration;

    internal PrimaryKeyIndexConfiguration(System.Data.Entity.ModelConfiguration.Configuration.Properties.Index.IndexConfiguration configuration) => this._configuration = configuration;

    /// <summary>Configures the index to be clustered.</summary>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public PrimaryKeyIndexConfiguration IsClustered() => this.IsClustered(true);

    /// <summary>
    /// Configures whether or not the index will be clustered.
    /// </summary>
    /// <param name="clustered"> Value indicating if the index should be clustered or not. </param>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public PrimaryKeyIndexConfiguration IsClustered(bool clustered)
    {
      this._configuration.IsClustered = new bool?(clustered);
      return this;
    }

    /// <summary>Configures the index to have a specific name.</summary>
    /// <param name="name"> Value indicating what the index name should be.</param>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public PrimaryKeyIndexConfiguration HasName(string name)
    {
      this._configuration.Name = name;
      return this;
    }
  }
}
