// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbModel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Internal;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Represents an Entity Data Model (EDM) created by the <see cref="T:System.Data.Entity.DbModelBuilder" />.
  /// The Compile method can be used to go from this EDM representation to a <see cref="T:System.Data.Entity.Infrastructure.DbCompiledModel" />
  /// which is a compiled snapshot of the model suitable for caching and creation of
  /// <see cref="T:System.Data.Entity.DbContext" /> or <see cref="T:System.Data.Objects.ObjectContext" /> instances.
  /// </summary>
  public class DbModel : IEdmModelAdapter
  {
    private readonly DbDatabaseMapping _databaseMapping;
    private readonly DbModelBuilder _cachedModelBuilder;

    internal DbModel(DbDatabaseMapping databaseMapping, DbModelBuilder modelBuilder)
    {
      this._databaseMapping = databaseMapping;
      this._cachedModelBuilder = modelBuilder;
    }

    internal DbModel(DbProviderInfo providerInfo, DbProviderManifest providerManifest) => this._databaseMapping = new DbDatabaseMapping().Initialize(EdmModel.CreateConceptualModel(), EdmModel.CreateStoreModel(providerInfo, providerManifest));

    internal DbModel(EdmModel conceptualModel, EdmModel storeModel) => this._databaseMapping = new DbDatabaseMapping()
    {
      Model = conceptualModel,
      Database = storeModel
    };

    /// <summary>Gets the provider information.</summary>
    public DbProviderInfo ProviderInfo => this.StoreModel.ProviderInfo;

    /// <summary>Gets the provider manifest.</summary>
    public DbProviderManifest ProviderManifest => this.StoreModel.ProviderManifest;

    /// <summary>Gets the conceptual model.</summary>
    public EdmModel ConceptualModel => this._databaseMapping.Model;

    /// <summary>Gets the store model.</summary>
    public EdmModel StoreModel => this._databaseMapping.Database;

    /// <summary>Gets the mapping model.</summary>
    public EntityContainerMapping ConceptualToStoreMapping => this._databaseMapping.EntityContainerMappings.SingleOrDefault<EntityContainerMapping>();

    internal DbModelBuilder CachedModelBuilder => this._cachedModelBuilder;

    internal DbDatabaseMapping DatabaseMapping => this._databaseMapping;

    /// <summary>
    /// Creates a <see cref="T:System.Data.Entity.Infrastructure.DbCompiledModel" /> for this mode which is a compiled snapshot
    /// suitable for caching and creation of <see cref="T:System.Data.Entity.DbContext" /> instances.
    /// </summary>
    /// <returns> The compiled model. </returns>
    public DbCompiledModel Compile() => new DbCompiledModel(CodeFirstCachedMetadataWorkspace.Create(this.DatabaseMapping), this.CachedModelBuilder);
  }
}
