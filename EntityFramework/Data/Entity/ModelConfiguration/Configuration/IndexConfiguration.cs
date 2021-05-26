// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.IndexConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>Configures an index.</summary>
  public class IndexConfiguration
  {
    private readonly System.Data.Entity.ModelConfiguration.Configuration.Properties.Index.IndexConfiguration _configuration;

    internal IndexConfiguration(System.Data.Entity.ModelConfiguration.Configuration.Properties.Index.IndexConfiguration configuration) => this._configuration = configuration;

    /// <summary>Configures the index to be unique.</summary>
    /// <returns> The same IndexConfiguration instance so that multiple calls can be chained. </returns>
    public IndexConfiguration IsUnique() => this.IsUnique(true);

    /// <summary>Configures whether the index will be unique.</summary>
    /// <param name="unique"> Value indicating if the index should be unique or not. </param>
    /// <returns> The same IndexConfiguration instance so that multiple calls can be chained. </returns>
    public IndexConfiguration IsUnique(bool unique)
    {
      this._configuration.IsUnique = new bool?(unique);
      return this;
    }

    /// <summary>Configures the index to be clustered.</summary>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public IndexConfiguration IsClustered() => this.IsClustered(true);

    /// <summary>
    /// Configures whether or not the index will be clustered.
    /// </summary>
    /// <param name="clustered"> Value indicating if the index should be clustered or not. </param>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public IndexConfiguration IsClustered(bool clustered)
    {
      this._configuration.IsClustered = new bool?(clustered);
      return this;
    }

    /// <summary>Configures the index to have a specific name.</summary>
    /// <param name="name"> Value indicating what the index name should be.</param>
    /// <returns> The same IndexConfigurationBase instance so that multiple calls can be chained. </returns>
    public IndexConfiguration HasName(string name)
    {
      this._configuration.Name = name;
      return this;
    }
  }
}
