// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.CodeFirstCachedMetadataWorkspace
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Internal
{
  internal class CodeFirstCachedMetadataWorkspace : ICachedMetadataWorkspace
  {
    private readonly MetadataWorkspace _metadataWorkspace;
    private readonly IEnumerable<Assembly> _assemblies;
    private readonly DbProviderInfo _providerInfo;
    private readonly string _defaultContainerName;

    private CodeFirstCachedMetadataWorkspace(
      MetadataWorkspace metadataWorkspace,
      IEnumerable<Assembly> assemblies,
      DbProviderInfo providerInfo,
      string defaultContainerName)
    {
      this._metadataWorkspace = metadataWorkspace;
      this._assemblies = assemblies;
      this._providerInfo = providerInfo;
      this._defaultContainerName = defaultContainerName;
    }

    public MetadataWorkspace GetMetadataWorkspace(DbConnection connection)
    {
      if (!string.Equals(this._providerInfo.ProviderInvariantName, connection.GetProviderInvariantName(), StringComparison.Ordinal))
        throw System.Data.Entity.Resources.Error.CodeFirstCachedMetadataWorkspace_SameModelDifferentProvidersNotSupported();
      return this._metadataWorkspace;
    }

    public string DefaultContainerName => this._defaultContainerName;

    public IEnumerable<Assembly> Assemblies => this._assemblies;

    public DbProviderInfo ProviderInfo => this._providerInfo;

    public static CodeFirstCachedMetadataWorkspace Create(
      DbDatabaseMapping databaseMapping)
    {
      EdmModel model = databaseMapping.Model;
      return new CodeFirstCachedMetadataWorkspace(databaseMapping.ToMetadataWorkspace(), (IEnumerable<Assembly>) model.GetClrTypes().Select<Type, Assembly>((Func<Type, Assembly>) (t => t.Assembly())).Distinct<Assembly>().ToArray<Assembly>(), databaseMapping.ProviderInfo, model.Container.Name);
    }

    public static CodeFirstCachedMetadataWorkspace Create(
      StorageMappingItemCollection mappingItemCollection,
      DbProviderInfo providerInfo)
    {
      EdmItemCollection edmItemCollection = mappingItemCollection.EdmItemCollection;
      IEnumerable<Type> first = edmItemCollection.GetItems<EntityType>().Select<EntityType, Type>((Func<EntityType, Type>) (et => EntityTypeExtensions.GetClrType(et)));
      IEnumerable<Type> second = edmItemCollection.GetItems<ComplexType>().Select<ComplexType, Type>((Func<ComplexType, Type>) (ct => ComplexTypeExtensions.GetClrType(ct)));
      return new CodeFirstCachedMetadataWorkspace(mappingItemCollection.Workspace, (IEnumerable<Assembly>) first.Union<Type>(second).Select<Type, Assembly>((Func<Type, Assembly>) (t => t.Assembly())).Distinct<Assembly>().ToArray<Assembly>(), providerInfo, edmItemCollection.GetItems<EntityContainer>().Single<EntityContainer>().Name);
    }
  }
}
